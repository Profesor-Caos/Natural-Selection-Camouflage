using Godot;
using NaturalSelectionCamouflage;
using System;

public class Buttons : HBoxContainer
{
    public event EventHandler<LogEventArgs> LogEvent;
    
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

    public void SubscribeLogger(EventHandler<LogEventArgs> handler)
    {
        SpinBoxSlider predationSlider = GetNode<SpinBoxSlider>("PredationSlider");
        predationSlider.LogEvent += handler;
    }

    private void TryInteraction(string signal, string buttonName)
    {
        LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{buttonName} pressed."));

        string message;
        if (InteractionStatus.CheckInteraction(buttonName, out message))
        { 
            if (signal != null)
                EmitSignal(signal);
        }
        else
        {
            AcceptDialog ad = new AcceptDialog();
            ad.DialogText = message;
            this.AddChild(ad);
            ad.PopupCentered();
        }
    }

    private void OnSetupPressed()
	{
		TryInteraction("SetupPressed", "Setup");
	}

	private void OnGoOncePressed()
	{
        TryInteraction("GoOncePressed", "Go Once");
	}

    private void OnGoPressed()
    {
        TryInteraction("GoPressed", "Go");
    }

	private void OnSetLightBackgroundPressed()
	{
        TryInteraction("SetLightBackgroundPressed", "Set Light Background");
	}

    private void OnSetDarkBackgroundPressed()
    {
        TryInteraction("SetDarkBackgroundPressed", "Set Dark Background");
    }

    private void OnSetMixedBackgroundPressed()
    {
        TryInteraction("SetMixedBackgroundPressed", "Set Mixed Background");
    }

    private void OnAddAMutantPressed()
    {
        TryInteraction("AddAMutantPressed", "Add A Mutant");
    }

    private bool _toggling = false;
    private void OnPredationEnabledToggled(bool buttonPressed)
    {
        if (_toggling)
            return;

        LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"PredationEnabled pressed."));

        string message;
        if (InteractionStatus.CheckInteraction("Predation", out message))
            return;

        CheckBox predation = GetNode<CheckBox>("PredationEnabled");
        _toggling = true;
        predation.Pressed = !buttonPressed;
        _toggling = false;

        AcceptDialog ad = new AcceptDialog();
        ad.DialogText = message;
        this.AddChild(ad);
        ad.PopupCentered();
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
