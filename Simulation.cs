using Godot;
using NaturalSelectionCamouflage;
using System;
using System.Collections.Generic;
using System.Runtime;

public class Simulation : Node
{
	private bool _isGoActive = false;

	// Colors
	private static Color DEFAULT_BROWN = Color.Color8(185, 122, 86);
	private static Color DARK_BROWN = Color.Color8(66, 46, 30);
	private static Color LIGHT_BROWN = Color.Color8(216, 197, 182);

	// Camouflage constants
    private static int WHITE_CAMO = 10;
    private static int LIGHT_BROWN_CAMO = 8;
    private static int DEFAULT_BROWN_CAMO = 5;
    private static int DARK_BROWN_CAMO = 2;
	private static int BLACK_CAMO = 0;

	private Color leftColor = DEFAULT_BROWN;
	private Color rightColor = DEFAULT_BROWN;

    [Export]
	public PackedScene MouseScene;

	[Export]
	public PackedScene BirdScene;

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

	private void ClearBirds()
	{
		GetTree().CallGroup("birds", "queue_free");
	}

	private void ResetDefaults()
    {
        InitialSettings settings = GetNode("VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/InitialSettings") as InitialSettings;
		settings.ResetDefaults();

        Buttons buttons = GetNode("VBoxContainer/HBoxContainer/Buttons") as Buttons;
		buttons.ResetDefaults();

        SpinBoxSlider speedSlider = GetNode<SpinBoxSlider>("VBoxContainer/SpeedSliderContainer/SpeedSlider/");
		speedSlider.ResetDefault();

        SetDefaultBackground();

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

	private void SpawnBird()
    {
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");

        Bird bird = BirdScene.Instance() as Bird;
        bird.Position = Utils.GetRandomPointInPolygon(poly);
        poly.AddChild(bird);
    }

	private void PopulatePredators()
    {
        CheckBox predation = GetNode("VBoxContainer/HBoxContainer/Buttons/PredationEnabled") as CheckBox;
		bool predationEnabled = predation.Pressed;
		if (!predationEnabled)
			return;

        int predationValue = (GetNode("VBoxContainer/HBoxContainer/Buttons/PredationSlider") as SpinBoxSlider).Value;

        for (int i = 0; i < predationValue; i++)
		{
			SpawnBird();
        }
    }

	private void MaintainPredatorPopulation(bool predationEnabled)
    {
        var birds = GetTree().GetNodesInGroup("birds");


        if (!predationEnabled)
        {
			if (birds.Count > 0)
				ClearBirds();
			return;
        }

        int predationValue = (GetNode("VBoxContainer/HBoxContainer/Buttons/PredationSlider") as SpinBoxSlider).Value;

        if (birds.Count > predationValue)
		{
			// kill off a bird if there are too many.
			(birds[birds.Count - 1] as Bird).Die();
		}
		else if (birds.Count < predationValue)
        {
			SpawnBird();
        }
    }

	private int GetBackgroundCamo(Color color)
	{
		if (color == DARK_BROWN)
			return DARK_BROWN_CAMO;
		else if (color == LIGHT_BROWN)
			return LIGHT_BROWN_CAMO;
		else
			return DEFAULT_BROWN_CAMO;
	}

    private void AdvanceOneStep()
    {
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");

        CheckBox predation = GetNode("VBoxContainer/HBoxContainer/Buttons/PredationEnabled") as CheckBox;
        bool predationEnabled = predation.Pressed;

        int predationValue = (GetNode("VBoxContainer/HBoxContainer/Buttons/PredationSlider") as SpinBoxSlider).Value;

        var mice = GetTree().GetNodesInGroup("mice");
		foreach (Mouse mouse in mice)
		{
			var coords = mouse.Position;

			float camouflage;
			int mouseColor = mouse.Genotype == Genotype.aa ? WHITE_CAMO : BLACK_CAMO;
			int backColor;
			if (mouse.Position.x < poly.Polygon[1].x / 2)
				backColor = GetBackgroundCamo(leftColor);
			else
				backColor = GetBackgroundCamo(rightColor);
			camouflage = 1 / Math.Abs(mouseColor - backColor);
			mouse.Behave(predationEnabled, predationValue, camouflage);
		}

		foreach (Mouse mouse in mice)
		{
			mouse.SeekAPartner();
		}

		MaintainPredatorPopulation(predationEnabled);

        var birds = GetTree().GetNodesInGroup("birds");
        foreach (Bird bird in birds)
        {
            bird.Behave();
        }

        List<Mouse> newGeneration = new List<Mouse>();
		foreach (Mouse mouse in mice)
		{
			Mouse[] newMice = mouse.Reproduce(MouseScene);
			if (newMice != null)
				newGeneration.AddRange(newMice);
		}

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
		PopulatePredators();
	}

	private void OnGoOncePressed()
	{
		AdvanceOneStep();
	}

	private void OnGoPressed()
	{
		_isGoActive = !_isGoActive;
	}

	private void SetBackground()
    {
        Node canvas = GetNode($"{nameof(VBoxContainer)}/HBoxContainer2/Canvas");
        Polygon2D poly = canvas.GetNode<Polygon2D>($"Canvas");

        var uv = poly.Uv;

        Shader shader = new Shader();
        shader.Code = @"
			shader_type canvas_item;
			uniform vec4 left_color;
			uniform vec4 right_color;

			void fragment() {
				if (UV.x < 0.5) {
					COLOR = left_color;
				} else {
					COLOR = right_color;
				}
			}
		";

        ShaderMaterial shaderMaterial = new ShaderMaterial();
        shaderMaterial.Shader = shader;

        shaderMaterial.SetShaderParam("left_color", leftColor);
        shaderMaterial.SetShaderParam("right_color", rightColor);

        poly.Material = shaderMaterial;

    }

	private void OnSetLightBackgroundPressed()
    {
        this.leftColor = LIGHT_BROWN;
		this.rightColor = LIGHT_BROWN;
        SetBackground();
    }

	private void OnSetDarkBackgroundPressed()
    {
        this.leftColor = DARK_BROWN;
        this.rightColor = DARK_BROWN;
        SetBackground();
    }

	private void OnSetMixedBackgroundPressed()
    {
        this.leftColor = LIGHT_BROWN;
        this.rightColor = DARK_BROWN;
        SetBackground();
    }

	private void SetDefaultBackground()
    {
        this.leftColor = DEFAULT_BROWN;
        this.rightColor = DEFAULT_BROWN;
        SetBackground();
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
		SetDefaultBackground();
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
