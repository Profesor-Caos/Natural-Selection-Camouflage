using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;

public class Main : HBoxContainer, ILocalizable
{
    private PackedScene _openingScene;
    private OpeningScreen _openingSceneInstance;

    public event EventHandler<AssistanceRequestEventArgs> PopupClosed;

    public Simulation Simulation { get { return this.GetNode<Simulation>("Simulation"); } }

    public Language Language { get; set; }
    public int TestGroup { get; set; }

    public Queue<string> Logs = new Queue<string>();
    private static float PROJECT_WIDTH = 1662.0f;
    private static float PROJECT_HEIGHT = 620.0f;

    private int studentID = 1;

    private string URL = "https://nscbackend.onrender.com";
    private string EndPoint = "/api/data";
    private string[] Headers = { "Content-Type: application/json" };

    private void OnNavigationPageNavigationButtonPressed()
    {
        int pageNumber = GetNode<NavigationPage>("NavigationPage").CurrentPageIndex;
        GetNode<Simulation>("Simulation").SelectedPage = pageNumber;
        InteractionStatus.PageNumber = pageNumber;
        Random r = new Random();
        int studentID = r.Next(int.MaxValue);
        int ID = r.Next(int.MaxValue); 
        Dictionary<string, object> data = new Dictionary<string, object>()
        {
            {"id", ID },
            {"student_id", studentID}
        };

        var http = CreateRequest();
        http.Request(URL + EndPoint, Headers, false, HTTPClient.Method.Post, JSON.Print(data));
    }

    private void OnRequestCompleted(int result, int responseCode, string[] headers, byte[] body, object request)
    {
        // Handle the server's response
        string responseText = System.Text.Encoding.UTF8.GetString(body);

        (request as HTTPRequest).QueueFree();
        this.RemoveChild(request as HTTPRequest);

        GD.Print("Response Code: ", responseCode);
        GD.Print("Response: ", responseText);
    }

    private void ResizeUI()
    {
        // Get the current window size
        Vector2 windowSize = GetViewport().Size;
        // Scale the root Control node based on window size
        // You can adjust these values to suit your design needs
        float scaleFactor = Mathf.Min(windowSize.x / PROJECT_WIDTH, windowSize.y / PROJECT_HEIGHT);
        RectScale = new Vector2(scaleFactor, scaleFactor);
    }

    public void HandleLogEvent(object sender, LogEventArgs args)
    {
        CreateLog(args.Message);
    }

    private void CreateLog(string logData)
    {
        string url = URL + "/logs";

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");

        var logEntry = new Godot.Collections.Dictionary<string, object>
        {
            { "StudentID", studentID },
            { "Timestamp", timestamp },
            { "PageNumber", GetNode<NavigationPage>("NavigationPage").CurrentPageIndex + 1 },
            { "LogData", logData }
        };

        string jsonData = JSON.Print(logEntry);

        var http = CreateRequest();

        Error err = http.Request(url, Headers, false, HTTPClient.Method.Post, jsonData);

        if (err != Error.Ok)
            GD.PrintErr("Failed to post log to server: ", err);
    }

    private HTTPRequest CreateRequest()
    {
        var http = new HTTPRequest();
        AddChild(http);
        http.Connect("request_completed", this, nameof(OnRequestCompleted), new Godot.Collections.Array(http));
        return http;
    }

    private void ShowOpeningScene()
    {
        foreach (Node child in GetChildren())
        {
            if (child is Control control)
            {
                control.Visible = false;
            }
        }

        _openingSceneInstance = _openingScene.Instance() as OpeningScreen;
        _openingSceneInstance.Finished += _openingSceneInstance_Finished;
        GetParent().CallDeferred("add_child", _openingSceneInstance);
    }

    private void _openingSceneInstance_Finished(object sender, EventArgs e)
    {
        HideOpeningScene();
    }

    private void HideOpeningScene()
    {
        if (_openingSceneInstance != null)
        {
            _openingSceneInstance.QueueFree();
            this.Language = _openingSceneInstance.Language;
            this.TestGroup = _openingSceneInstance.TestGroup;
            InteractionStatus.Language = this.Language;
            InteractionStatus.TestGroup = this.TestGroup;
            InteractionStatus.Main = this;
            InteractionStatus.Initialize();
            Localize(this.Language);
        }

        foreach (Node child in GetChildren())
        {
            if (child is Control control)
            {
                control.Visible = true;
            }
        }
    }

    public void Localize(Language language)
    {
        foreach (Node child in GetChildren())
        {
            if (child is ILocalizable localizable)
            {
                localizable.Localize(language);
            }
        }
    }

    private AssistanceRequest popup;

    public void ShowPopup(AssistanceRequest popup)
    {
        this.popup = popup;
        popup.Connect("popup_hide", this, nameof(OnPopupHide));
        popup.PopupCentered();
    }

    public void OnPopupHide()
    {
        popup.Disconnect("popup_hide", this, nameof(OnPopupHide));
        this.PopupClosed?.Invoke(this, new AssistanceRequestEventArgs(popup.Result, popup.Assistance));
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _openingScene = (PackedScene)ResourceLoader.Load("res://OpeningScreen.tscn");
        ShowOpeningScene();

        ResizeUI();

        GetNode<Simulation>("Simulation").SubscribeLogger(HandleLogEvent);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ResizeUI();
    }
}
