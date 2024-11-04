using Godot;
using System;
using System.Collections.Generic;

public class NavigationPage : HBoxContainer
{
    [Signal]
    public delegate void BackButtonPressed();
    [Signal]
    public delegate void ForwardButtonPressed();

    private List<Control> Pages = new List<Control>();
    private int CurrentPageIndex = 0;

    private void OnBackButtonPressed()
    {
        EmitSignal(nameof(BackButtonPressed));
    }

    private void OnForwardButtonPressed()
    {
        EmitSignal(nameof(ForwardButtonPressed));
    }

    public void AddPage(Control page)
    {
        Pages.Add(page);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        CenterContainer content = GetNode<CenterContainer>("Content");
        PackedScene page1PackedScene = (PackedScene)ResourceLoader.Load("res://Page1.tscn");
        Control page1 = (Control)page1PackedScene.Instance();
        AddPage(page1);

        PackedScene page2PackedScene = (PackedScene)ResourceLoader.Load("res://Page2.tscn");
        Control page2 = (Control)page2PackedScene.Instance();
        AddPage(page2);

        // loop through ints and load pages for each int in range.
        // add pages to list.
        content.AddChild(page2);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
