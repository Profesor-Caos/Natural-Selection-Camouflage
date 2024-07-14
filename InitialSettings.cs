using Godot;
using System;

public class InitialSettings : GridContainer
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private int maxWidth = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		SpinBoxSlider slider = GetNode<SpinBoxSlider>("AA Females");
		maxWidth = (int)slider.RectSize.x;

		var AAMSlider = GetNode<SpinBoxSlider>("AA Males").GetNode<Slider>("HSlider");
		AAMSlider.MarginRight = maxWidth;
		var AAMSpinBox = GetNode<SpinBoxSlider>("AA Males").GetNode<SpinBox>("GridContainer/SpinBox");
		AAMSpinBox.MarginLeft = maxWidth - (int)AAMSpinBox.RectSize.x;
		AAMSpinBox.MarginRight = maxWidth;
		
		var AaMSlider = GetNode<SpinBoxSlider>("Aa Males").GetNode<Slider>("HSlider");
		AaMSlider.MarginRight = maxWidth;
		var AaMSpinBox = GetNode<SpinBoxSlider>("Aa Males").GetNode<SpinBox>("GridContainer/SpinBox");
		AaMSpinBox.MarginLeft = maxWidth - (int)AaMSpinBox.RectSize.x;
		AaMSpinBox.MarginRight = maxWidth;

		var aaMSlider = GetNode<SpinBoxSlider>("aa Males").GetNode<Slider>("HSlider");
		aaMSlider.MarginRight = maxWidth;
		var aaMSpinBox = GetNode<SpinBoxSlider>("aa Males").GetNode<SpinBox>("GridContainer/SpinBox");
		aaMSpinBox.MarginLeft = maxWidth - (int)aaMSpinBox.RectSize.x;
		aaMSpinBox.MarginRight = maxWidth;


		var AaFSlider = GetNode<SpinBoxSlider>("Aa Females").GetNode<Slider>("HSlider");
		AaFSlider.MarginRight = maxWidth;
		var AaFSpinBox = GetNode<SpinBoxSlider>("Aa Females").GetNode<SpinBox>("GridContainer/SpinBox");
		AaFSpinBox.MarginLeft = maxWidth - (int)AaFSpinBox.RectSize.x;
		AaFSpinBox.MarginRight = maxWidth;

		var aaFSlider = GetNode<SpinBoxSlider>("aa Females").GetNode<Slider>("HSlider");
		aaFSlider.MarginRight = maxWidth;
		var aaFSpinBox = GetNode<SpinBoxSlider>("aa Females").GetNode<SpinBox>("GridContainer/SpinBox");
		aaFSpinBox.MarginLeft = maxWidth - (int)aaFSpinBox.RectSize.x;
		aaFSpinBox.MarginRight = maxWidth;
	}
}
