using Godot;
using System;

public partial class SpinBoxSlider : GridContainer
{
	private int _value;
	[Export]
	public int Value 
	{ 
		get { return _value; } 
		set
		{
			_value = value;
			HSlider slider = GetNode<HSlider>(nameof(HSlider));
			slider.Value = value;
			SpinBox spinBox = GetNode<SpinBox>($"{nameof(GridContainer)}/{nameof(SpinBox)}");
			spinBox.Value = value;
		}
	}

	private string _label;
	[Export]
	public string Label
	{
		get { return _label; }
		set
		{
			_label = value;
			Label label = GetNode<Label>($"{nameof(GridContainer)}/{nameof(Label)}");
			label.Text = value;
		}
	}

	private void OnSpinBoxValueChanged(float value)
	{
		_value = (int)value;
		HSlider slider = GetNode<HSlider>(nameof(HSlider));
		slider.Value = value;
	}
	
	private void OnHSliderValueChanged(float value)
	{
		_value = (int)value;
		SpinBox spinBox = GetNode<SpinBox>($"{nameof(GridContainer)}/{nameof(SpinBox)}");
		spinBox.Value = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
//	public override void _Process(double delta)
//	{
//	}
}
