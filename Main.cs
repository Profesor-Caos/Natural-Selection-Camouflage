using Godot;
using System;

public class Main : Node
{
	[Export]
	public PackedScene MouseScene;

	private void OnSetupPressed()
	{
		InitialSettings settings = GetNode<InitialSettings>("VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/InitialSettings");

		Mouse mouse = MouseScene.Instance() as Mouse;
		Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
		Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");
		mouse.Position = poly.Position;
		canvas.AddChild(mouse);
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
