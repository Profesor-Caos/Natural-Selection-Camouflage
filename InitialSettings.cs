using Godot;
using System;

public class InitialSettings : VBoxContainer, ILocalizable
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

	public void ResetDefaults()
	{
		GetNode<SpinBoxSlider>("AA Males").ResetDefault();
        GetNode<SpinBoxSlider>("Aa Males").ResetDefault();
        GetNode<SpinBoxSlider>("aa Males").ResetDefault();
		GetNode<SpinBoxSlider>("AA Females").ResetDefault();
        GetNode<SpinBoxSlider>("Aa Females").ResetDefault();
        GetNode<SpinBoxSlider>("aa Females").ResetDefault();
    }

	public void SubscribeLogger(EventHandler<LogEventArgs> handler)
	{
        GetNode<SpinBoxSlider>("AA Males").LogEvent += handler;
        GetNode<SpinBoxSlider>("Aa Males").LogEvent += handler;
        GetNode<SpinBoxSlider>("aa Males").LogEvent += handler;
        GetNode<SpinBoxSlider>("AA Females").LogEvent += handler;
        GetNode<SpinBoxSlider>("Aa Females").LogEvent += handler;
        GetNode<SpinBoxSlider>("aa Females").LogEvent += handler;
    }

	public void Localize(Language language)
	{
		Label label = GetNode<Label>("Label");
		SpinBoxSlider AAMales = GetNode<SpinBoxSlider>("AA Males");
        SpinBoxSlider AaMales = GetNode<SpinBoxSlider>("Aa Males");
        SpinBoxSlider aaMales = GetNode<SpinBoxSlider>("aa Males");
        SpinBoxSlider AAFemales = GetNode<SpinBoxSlider>("AA Females");
        SpinBoxSlider AaFemales = GetNode<SpinBoxSlider>("Aa Females");
        SpinBoxSlider aaFemales = GetNode<SpinBoxSlider>("aa Females");

		if (language == Language.English)
		{
			label.Text = "Initial Settings";
			AAMales.Label = "Homozygous Dominant Males";
			AaMales.Label = "Heterozygous Males";
			aaMales.Label = "Homozygous Recessive Males";
			AAFemales.Label = "Homozygous Dominant  Females";
			AaFemales.Label = "Heterozygous Females";
			aaFemales.Label = "Homozygous Recessive Females";
        }
        else if (language == Language.Spanish)
        {
            label.Text = "Configuraciones Iniciales";
            AAMales.Label = "Machos Homocigotos Dominantes";
            AaMales.Label = "Machos Heterocigotos";
            aaMales.Label = "Machos Homocigotos Recesivos";
            AAFemales.Label = "Hembras Homocigotas Dominantes";
            AaFemales.Label = "Hembras Heterocigotas";
            aaFemales.Label = "Hembras Homocigotas Recesivas";
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
