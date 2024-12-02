using Godot;
using NaturalSelectionCamouflage;
using System;

public partial class SpinBoxSlider : VBoxContainer
{
	public event EventHandler<LogEventArgs> LogEvent;

	private bool _dragged = false;

	private bool _sliderChanged = false;

	private bool _updating = false;

	private bool _frameDelay = false;

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
        if (_updating)
            return;

		string message;
		if (!InteractionStatus.CheckInteraction(Label, out message))
        {
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{Label} value was attempted to be changed to {SpinBox.Value}."));
            _updating = true;
			SpinBox.Value = _value;
			_updating = false;
			AcceptDialog ad = new AcceptDialog();
			ad.DialogText = message;
			this.AddChild(ad);
			ad.PopupCentered();
			return;
		}

		_updating = true;
		_value = (int)value;
		Slider.Value = _value;
		_updating = false;

		LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{Label} value changed to {value}."));
	}

	private void OnHSliderDragEnded(bool value_changed)
	{
		_dragged = false;
    }

    private void OnHSliderDragStarted()
    {
		_dragged = true;
    }

    private void OnHSliderValueChanged(float value)
    {
		if (_updating)
			return;

        _sliderChanged = true;

        _updating = true;
		SpinBox.Value = value;
		_updating = false;
	}

    public void ResetDefault()
	{
		//_updating = true;
		this.Value = this.Default;
		//_updating = false;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SpinBox spinBox = GetNode<SpinBox>($"{nameof(HBoxContainer)}/{nameof(SpinBox)}");
		spinBox.SetAnchorsPreset(LayoutPreset.TopRight);
	}

	//	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (_sliderChanged)
		{
			if (_dragged)
				return;

			if (!_frameDelay)
			{
				_frameDelay = true;
				// Delay processing this so it's not processed as being dragged when it was only clicked.
				return;
			}

			_frameDelay = false;

            string message;
			if (!InteractionStatus.CheckInteraction(Label, out message))
			{
                LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{Label} value was attempted to be changed to {Slider.Value}."));
                _updating = true;
				Slider.Value = _value;
				SpinBox.Value = _value;
				_updating = false;
				AcceptDialog ad = new AcceptDialog();
				ad.DialogText = message;
				this.AddChild(ad);
				ad.PopupCentered();
				_sliderChanged = false;
				return;
			}

			_value = (int)Slider.Value;
			_sliderChanged = false;

            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, $"{Label} value changed to {_value}."));

            return;
		}
	}
}
