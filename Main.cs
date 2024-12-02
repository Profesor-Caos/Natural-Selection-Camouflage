using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;

public class Main : HBoxContainer
{
    public Queue<string> Logs = new Queue<string>();
    private static float PROJECT_WIDTH = 1620.0f;
    private static float PROJECT_HEIGHT = 600.0f;

    private int studentID = 1;

    private string URL = "http://127.0.0.1:5000";
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ResizeUI();

        GetNode<Simulation>("Simulation").SubscribeLogger(HandleLogEvent);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ResizeUI();
    }
}
