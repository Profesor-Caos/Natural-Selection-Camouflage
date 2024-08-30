using Godot;
using System;

public class Buttons : HBoxContainer
{
	[Signal]
	public delegate void SetupPressed();
	[Signal]
	public delegate void GoOncePressed();
    [Signal]
    public delegate void GoPressed();

    private void OnSetupPressed()
	{
		EmitSignal("SetupPressed");
	}

	private void OnGoOncePressed()
	{
		EmitSignal("GoOncePressed");
	}

    private void OnGoPressed()
    {
        EmitSignal("GoPressed");
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
