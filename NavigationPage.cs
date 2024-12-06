using Godot;
using System;
using System.Collections.Generic;

public class NavigationPage : HBoxContainer, ILocalizable
{
    public event EventHandler<LogEventArgs> LogEvent;

    [Signal]
    public delegate void NavigationButtonPressed();

    private List<Control> Pages = new List<Control>();
    private List<string> Text = new List<string>();
    public int CurrentPageIndex = 0;

    public void SubscribeLogger(EventHandler<LogEventArgs> handler)
    {
        this.LogEvent += handler;
    }

    private void OnBackButtonPressed()
    {
        if (CurrentPageIndex == 0)
            return;

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

        CenterContainer content = GetNode<CenterContainer>("Content");
        content.RemoveChild(Pages[CurrentPageIndex++]);
        content.AddChild(Pages[CurrentPageIndex]);

        this.LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Next Button Pressed"));
        EmitSignal(nameof(NavigationButtonPressed));
    }

    public void AddPage(Control page)
    {
        Pages.Add(page);
        Text.Add(string.Empty);
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

        int pageCount = 10;
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
