using Godot;
using System;
using System.Collections.Generic;

public class LineDrawer : Control
{
    // List of points for the lines
    private List<Vector2> points = new List<Vector2>();

    [Export]
    public Color LineColor = Colors.Gray; // Line color
    [Export]
    public float LineWidth = 2f; // Line thickness

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
