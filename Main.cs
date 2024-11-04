using Godot;
using NaturalSelectionCamouflage;
using System;

public class Main : HBoxContainer
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    private void OnNavigationPageNavigationButtonPressed()
    {
        int pageNumber = GetNode<NavigationPage>("NavigationPage").CurrentPageIndex;
        GetNode<Simulation>("Simulation").SelectedPage = pageNumber;
        InteractionStatus.PageNumber = pageNumber;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
