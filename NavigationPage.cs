using Godot;
using System;
using System.Collections.Generic;

public class NavigationPage : HBoxContainer, ILocalizable
{
    public event EventHandler<LogEventArgs> LogEvent;

    [Signal]
    public delegate void NavigationButtonPressed();

    private List<Control> Pages = new List<Control>();
    private Dictionary<string, string> Text = new Dictionary<string, string>();
    public int CurrentPageIndex = 0;

    public void SubscribeLogger(EventHandler<LogEventArgs> handler)
    {
        this.LogEvent += handler;
    }

    private void OnBackButtonPressed()
    {
        if (CurrentPageIndex == 0)
            return;

        LogTextChange();

        CenterContainer content = GetNode<CenterContainer>("Content");
        content.RemoveChild(Pages[CurrentPageIndex--]);
        content.AddChild(Pages[CurrentPageIndex]);

        this.LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Back Button Pressed"));
        EmitSignal(nameof(NavigationButtonPressed));
    }

    private void OnForwardButtonPressed()
    {
        if (CurrentPageIndex == Pages.Count - 1)
            return;

        LogTextChange();

        CenterContainer content = GetNode<CenterContainer>("Content");
        content.RemoveChild(Pages[CurrentPageIndex++]);
        content.AddChild(Pages[CurrentPageIndex]);

        this.LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Next Button Pressed"));
        EmitSignal(nameof(NavigationButtonPressed));
    }

    private void LogTextChange()
    {
        Control currentPage = Pages[CurrentPageIndex];
        if (currentPage is ITextResponse textResponse)
        {
            List<string> responses = textResponse.GetResponses();
            for (int i = 0; i < responses.Count; i++)
            {
                string key = currentPage.Name + " text box " + (i + 1);
                string updatedText = responses[i];
                if (Text[key] != updatedText)
                {
                    this.LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{key} text changed to: {updatedText}"));
                    Text[key] = updatedText;
                }
            }
        }
    }

    public void AddPage(Control page)
    {
        Pages.Add(page);
        if (page is ITextResponse textResponse)
        {
            List<string> responses = textResponse.GetResponses();
            for (int i = 0; i < responses.Count; i++)
            {
                Text.Add(page.Name + " text box " + (i + 1), responses[i]);
            }
        }
    }

    public void Localize(Language language)
    {
        foreach (var item in Pages)
        {
            if (item is ILocalizable localizable)
                localizable.Localize(language);
        }

        Button back = GetNode<Button>("BackButton");
        Button next = GetNode<Button>("NextButton");

        if (language == Language.English)
        {
            back.Text = "Back";
            next.Text = "Next";
        }
        else if (language == Language.Spanish)
        {
            back.Text = "Atrás";
            next.Text = "Siguiente";
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CenterContainer content = GetNode<CenterContainer>("Content");

        int pageCount = 11;
        for (int i = 1; i <= pageCount; i++)
        {
            PackedScene pagePackedScene = (PackedScene)ResourceLoader.Load($"res://Page{i}.tscn");
            Control page = (Control)pagePackedScene.Instance();
            AddPage(page);
        }
        content.AddChild(Pages[0]);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
