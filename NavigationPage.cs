using Godot;
using System;
using System.Collections.Generic;

public class NavigationPage : HBoxContainer
{
    [Signal]
    public delegate void NavigationButtonPressed();

    private List<Control> Pages = new List<Control>();
    public int CurrentPageIndex = 0;

    private void OnBackButtonPressed()
    {
        if (CurrentPageIndex == 0)
            return;

        CenterContainer content = GetNode<CenterContainer>("Content");
        content.RemoveChild(Pages[CurrentPageIndex--]);
        content.AddChild(Pages[CurrentPageIndex]);


        EmitSignal(nameof(NavigationButtonPressed));
    }

    private void OnForwardButtonPressed()
    {
        if (CurrentPageIndex == Pages.Count - 1)
            return;

        CenterContainer content = GetNode<CenterContainer>("Content");
        content.RemoveChild(Pages[CurrentPageIndex++]);
        content.AddChild(Pages[CurrentPageIndex]);

        EmitSignal(nameof(NavigationButtonPressed));
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

        PackedScene page3PackedScene = (PackedScene)ResourceLoader.Load("res://Page3.tscn");
        Control page3 = (Control)page3PackedScene.Instance();
        AddPage(page3);

        PackedScene page4PackedScene = (PackedScene)ResourceLoader.Load("res://Page4.tscn");
        Control page4 = (Control)page4PackedScene.Instance();
        AddPage(page4);

        // loop through ints and load pages for each int in range.
        // add pages to list.
        content.AddChild(page1);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
