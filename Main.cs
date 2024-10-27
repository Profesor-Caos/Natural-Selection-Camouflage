using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;

public class Main : Node
{
	private bool _isGoActive = false;

	// Colors
	private static Color DEFAULT_BROWN = Color.Color8(185, 122, 86);
	private static Color DARK_BROWN = Color.Color8(66, 46, 30);
	private static Color LIGHT_BROWN = Color.Color8(216, 197, 182);

    [Export]
	public PackedScene MouseScene;

	private List<int> homozygousDominantCounts = new List<int>();
	private List<int> homozygousRecessiveCounts = new List<int>();
	private List<int> heterozygousCounts = new List<int>();

	private static int DELTA_QUEUE_SIZE = 100;
	private float deltaSinceLastUpdate = 0;
	private float runningDeltaSum = 0;
	private Queue<float> runningDeltas = new Queue<float>(DELTA_QUEUE_SIZE);

	private void ClearMice()
	{
		// Note that for calling Godot-provided methods with strings,
		// we have to use the original Godot snake_case name.

		// Clear any mice that already exist.
		GetTree().CallGroup("mice", "queue_free");
		homozygousDominantCounts.Clear();
		homozygousRecessiveCounts.Clear();
		heterozygousCounts.Clear();
	}

	private void ResetDefaults()
    {
        InitialSettings settings = GetNode("VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/InitialSettings") as InitialSettings;
		settings.ResetDefaults();

        Buttons buttons = GetNode("VBoxContainer/HBoxContainer/Buttons") as Buttons;
		buttons.ResetDefaults();

        SpinBoxSlider speedSlider = GetNode<SpinBoxSlider>("VBoxContainer/SpeedSliderContainer/SpeedSlider/");
		speedSlider.ResetDefault();

        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");
		poly.Color = DEFAULT_BROWN;

		// TODO: Predation checkbox
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

		homozygousDominantCounts.Add(settings.AAMales + settings.AAFemales);
		homozygousRecessiveCounts.Add(settings.aaMales + settings.aaFemales);
		heterozygousCounts.Add(settings.AaMales + settings.AaFemales);
	}

	private void Predation()
	{

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

	private void OnResetDefaultsButtonPressed()
	{
		ResetDefaults();
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

	private void OnSetLightBackgroundPressed()
    {
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");
        poly.Color = LIGHT_BROWN;
    }

	private void OnSetDarkBackgroundPressed()
    {
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");
        poly.Color = DARK_BROWN;
    }

	private void OnSetMixedBackgroundPressed()
	{

	}

	private void OnAddAMutantPressed()
	{
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");

        Sex sex = new Sex[] { Sex.Male, Sex.Female }[new Random().Next(0, 2)];
        Mouse mouse = MouseScene.Instance() as Mouse;
        mouse.Initialize(sex, Genotype.Aa);
        mouse.Position = Utils.GetRandomPointInPolygon(poly);
        poly.AddChild(mouse);
    }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		CountMice();

        if (runningDeltas.Count == DELTA_QUEUE_SIZE)
        {
            runningDeltaSum -= runningDeltas.Dequeue();
        }
        runningDeltaSum += delta;
        runningDeltas.Enqueue(delta);
        deltaSinceLastUpdate += delta;

        if (_isGoActive)
		{
			SpinBoxSlider speedSlider = GetNode<SpinBoxSlider>("VBoxContainer/SpeedSliderContainer/SpeedSlider/");
			int speed = speedSlider.Value;

			// This will result in different behavior for the first DELTA_QUEUE_SIZE number of frames, but ¯\_(ツ)_/¯
			if (deltaSinceLastUpdate >= (runningDeltaSum / DELTA_QUEUE_SIZE) * DELTA_QUEUE_SIZE / speed)
			{
				AdvanceOneStep();
				deltaSinceLastUpdate = 0;
			}
		}
	}
}
