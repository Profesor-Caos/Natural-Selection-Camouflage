using Godot;
using System;

public partial class SpinBoxSlider : VBoxContainer
{
	private SpinBox _spinBox;
	public SpinBox SpinBox
	{
		get { if (_spinBox != null)
				return _spinBox;
			_spinBox = GetNode<SpinBox>($"{nameof(HBoxContainer)}/{nameof(SpinBox)}");
			return _spinBox;
		}
	}

    private HSlider _slider;
    public HSlider Slider
    {
        get
        {
            if (_slider != null)
                return _slider;
            _slider = GetNode<HSlider>($"{nameof(HSlider)}");
            return _slider;
        }
    }

    private int _minValue;
	[Export]
	public int MinValue
	{ 
		get
		{
			return _minValue;
		}
		set
		{
			_minValue = value;
			Slider.MinValue = value;
			SpinBox.MinValue = value;
		} 
	}

    private int _maxValue;
    [Export] 
	public int MaxValue
	{
		get
		{
			return _maxValue;
		}
		set
		{
			_maxValue = value; 
			Slider.MaxValue = value;
            SpinBox.MaxValue = value;
		}
	}

	private int _default;
	[Export]
	public int Default
	{
		get
		{
			return _default; 
		}
		set
		{
			_default = value;
			Value = value;
		}
	}

	private int _value;
	[Export]
	public int Value 
	{ 
		get { return _value; } 
		set
		{
			_value = value;
			HSlider slider = GetNode<HSlider>($"{nameof(HSlider)}");
			slider.Value = value;
			SpinBox spinBox = GetNode<SpinBox>($"{nameof(HBoxContainer)}/{nameof(SpinBox)}");
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
			Label label = GetNode<Label>($"{nameof(HBoxContainer)}/{nameof(Label)}");
			label.Text = value;
		}
	}

	private void OnSpinBoxValueChanged(float value)
	{
		_value = (int)value;
		HSlider slider = GetNode<HSlider>($"{nameof(HSlider)}");
		slider.Value = value;
	}
	
	private void OnHSliderValueChanged(float value)
	{
		_value = (int)value;
		SpinBox spinBox = GetNode<SpinBox>($"{nameof(HBoxContainer)}/{nameof(SpinBox)}");
		spinBox.Value = value;
	}

	public void ResetDefault()
	{
		this.Value = this.Default;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpinBox spinBox = GetNode<SpinBox>($"{nameof(HBoxContainer)}/{nameof(SpinBox)}");
		spinBox.SetAnchorsPreset(LayoutPreset.TopRight);
	}

//	// Called every frame. 'delta' is the elapsed time since the previous frame.
//	public override void _Process(double delta)
//	{
//
//	}
}
