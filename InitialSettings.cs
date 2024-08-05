using Godot;
using System;

public class InitialSettings : VBoxContainer
{
    public int AAMales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("AA Males");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("AA Males");
            slider.Value = value;
        }
    }

    public int AaMales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("Aa Males");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("Aa Males");
            slider.Value = value;
        }
    }

    public int aaMales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("aa Males");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("aa Males");
            slider.Value = value;
        }
    }

    public int AAFemales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("AA Females");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("AA Females");
            slider.Value = value;
        }
    }

    public int AaFemales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("Aa Females");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("Aa Females");
            slider.Value = value;
        }
    }

    public int aaFemales
    {
        get
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("aa Females");
            return slider.Value;
        }
        set
        {
            SpinBoxSlider slider = GetNode<SpinBoxSlider>("aa Females");
            slider.Value = value;
        }
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
