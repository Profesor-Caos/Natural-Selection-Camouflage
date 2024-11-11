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
