using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;
using Timer = System.Timers.Timer;

public class Main : HBoxContainer, ILocalizable
{
    private PackedScene _openingScene;
    private OpeningScreen _openingSceneInstance;

    public event EventHandler<AssistanceRequestEventArgs> PopupClosed;

    private DateTime _startTime;

    public Simulation Simulation { get { return this.GetNode<Simulation>("Simulation"); } }

    public Language Language { get; set; }
    public int TestGroup { get; set; }

    public int StudentID { get; set; }

    public Queue<string> Logs = new Queue<string>();
    private static float PROJECT_WIDTH = 1662.0f;
    private static float PROJECT_HEIGHT = 620.0f;

    private string URL = "https://nscbackend.onrender.com";
    private string[] Headers = { "Content-Type: application/json" };

    private Timer _page3Timer;

    private void OnNavigationPageNavigationButtonPressed()
    {
        int pageNumber = GetNode<NavigationPage>("NavigationPage").CurrentPageIndex;
        GetNode<Simulation>("Simulation").SelectedPage = pageNumber;
        InteractionStatus.PageNumber = pageNumber;
        if (pageNumber == 2)
        {
            _page3Timer = new Timer(60 * 1000 * 3); // 3 minutes
            _page3Timer.Elapsed += _page3Timer_Elapsed;
            _page3Timer.AutoReset = false;
            _page3Timer.Enabled = true;
        }
        else if (_page3Timer != null)
        {
            _page3Timer.Dispose();
        }
    }

    private void _page3Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        AcceptDialog ad = new AcceptDialog();
        ad.DialogText = Language == Language.English ? "Don't forget to move on to the next page to continue learning." : "No olvides pasar a la siguiente página para seguir aprendiendo.";
        ad.WindowTitle = Language == Language.English ? "Reminder" : "Recordatorio";
        AddChild(ad);
        ad.PopupCentered();
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

        DateTime now = DateTime.Now;
        string timestamp = now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        TimeSpan difference = now - _startTime;

        var logEntry = new Godot.Collections.Dictionary<string, object>
        {
            { "StudentID", StudentID },
            { "Timestamp", timestamp },
            { "TimePassed", $"{difference.Hours}:{difference.Minutes}:{difference.Seconds}.{difference.Milliseconds:D3}" },
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
        _openingSceneInstance.WaitStarted += (x,y) => this.MouseDefaultCursorShape = CursorShape.Wait;
        _openingSceneInstance.WaitFinished += (x, y) => this.MouseDefaultCursorShape = CursorShape.Arrow;
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
            this.StudentID = _openingSceneInstance.StudentID;
            this._startTime = DateTime.Now;
            this.CreateLog("Student submitted StudentID");
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

        GetNode<NavigationPage>("NavigationPage").SubscribeLogger(HandleLogEvent);
        GetNode<Simulation>("Simulation").SubscribeLogger(HandleLogEvent);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ResizeUI();
    }
}
