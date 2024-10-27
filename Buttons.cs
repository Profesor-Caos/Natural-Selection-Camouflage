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
    [Signal]
    public delegate void SetLightBackgroundPressed();
    [Signal]
    public delegate void SetDarkBackgroundPressed();
    [Signal]
    public delegate void SetMixedBackgroundPressed();
    [Signal]
    public delegate void AddAMutantPressed();

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

	private void OnSetLightBackgroundPressed()
	{
		EmitSignal("SetLightBackgroundPressed");
	}

    private void OnSetDarkBackgroundPressed()
    {
        EmitSignal("SetDarkBackgroundPressed");
    }

    private void OnSetMixedBackgroundPressed()
    {
        EmitSignal("SetMixedBackgroundPressed");
    }

    private void OnAddAMutantPressed()
    {
        EmitSignal("AddAMutantPressed");
    }

    public void ResetDefaults()
	{
        SpinBoxSlider predationSlider = GetNode<SpinBoxSlider>("PredationSlider");
		predationSlider.ResetDefault();
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
