using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;

public class Main : Node
{
	private bool _isGoActive = false;

	[Export]
	public PackedScene MouseScene;

	private void ClearMice()
	{
		// Note that for calling Godot-provided methods with strings,
		// we have to use the original Godot snake_case name.

		// Clear any mice that already exist.
		GetTree().CallGroup("mice", "queue_free");
	}

	private void PopulateMice()
	{
		InitialSettings settings = GetNode("VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/InitialSettings") as InitialSettings;

		Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
		Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");

		for (int i = 0; i < settings.AAMales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Male, Genotype.AA);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);

		}

		for (int i = 0; i < settings.AaMales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Male, Genotype.Aa);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);
		}

		for (int i = 0; i < settings.aaMales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Male, Genotype.aa);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);
		}

		for (int i = 0; i < settings.AAFemales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Female, Genotype.AA);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);
		}

		for (int i = 0; i < settings.AaFemales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Female, Genotype.Aa);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);
		}

		for (int i = 0; i < settings.aaFemales; i++)
		{
			Mouse mouse = MouseScene.Instance() as Mouse;
			mouse.Initialize(Sex.Female, Genotype.aa);
			mouse.Position = Utils.GetRandomPointInPolygon(poly);
			poly.AddChild(mouse);
		}
	}

	private void AdvanceOneStep()
	{
		var mice = GetTree().GetNodesInGroup("mice");
		foreach (Mouse mouse in mice)
		{
			mouse.Behave();
		}

		foreach (Mouse mouse in mice)
		{
			mouse.SeekAPartner();
		}

		List<Mouse> newGeneration = new List<Mouse>();
		foreach (Mouse mouse in mice)
		{
			Mouse[] newMice = mouse.Reproduce(MouseScene);
			if (newMice != null)
				newGeneration.AddRange(newMice);
		}

		Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
		Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");
		newGeneration.ForEach(m => poly.AddChild(m));
	}

	private void CountMice()
	{
		int homoDMales = 0;
		int homoRMales = 0;
		int hetMales = 0;
		int homoDFemales = 0;
		int homoRFemales = 0;
		int hetFemales = 0;
        var mice = GetTree().GetNodesInGroup("mice");
        foreach (Mouse mouse in mice)
        {
            if (mouse.Sex == Sex.Female)
			{
				if (mouse.Genotype == Genotype.AA)
					homoDFemales += 1;
				else if (mouse.Genotype == Genotype.aa) 
					homoRFemales += 1;
				else 
					hetFemales += 1;
			}
			else
			{
                if (mouse.Genotype == Genotype.AA)
                    homoDMales += 1;
                else if (mouse.Genotype == Genotype.aa)
                    homoRMales += 1;
                else
                    hetMales += 1;
            }
        }

		Data data = GetNode<Data>("VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/Data");
        data.UpdateData(homoDMales, "AA Males");
        data.UpdateData(homoRMales, "aa Males");
        data.UpdateData(hetMales, "Aa Males");
        data.UpdateData(homoDMales, "AA Females");
        data.UpdateData(homoRFemales, "aa Females");
        data.UpdateData(hetFemales, "Aa Females");
    }

	private void OnSetupPressed()
	{
		ClearMice();

		PopulateMice();
	}

	private void OnGoOncePressed()
	{
		AdvanceOneStep();
	}

	private void OnGoPressed()
	{
		_isGoActive = !_isGoActive;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CountMice();

        if (_isGoActive)
		{
			AdvanceOneStep();
		}
	}
}
