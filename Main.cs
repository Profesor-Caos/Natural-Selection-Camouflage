using Godot;
using NaturalSelectionCamouflage;
using System.Collections.Generic;

public class Main : HBoxContainer
{
    public Queue<string> Logs = new Queue<string>();
    private static float PROJECT_WIDTH = 1620.0f;
    private static float PROJECT_HEIGHT = 600.0f;

    private void OnNavigationPageNavigationButtonPressed()
    {
        int pageNumber = GetNode<NavigationPage>("NavigationPage").CurrentPageIndex;
        GetNode<Simulation>("Simulation").SelectedPage = pageNumber;
        InteractionStatus.PageNumber = pageNumber;
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

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ResizeUI();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        ResizeUI();
    }
}
