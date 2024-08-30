using Godot;
using System;

public class DataBox : PanelContainer
{
    private int _value;
    [Export]
    public int Value
    {
        get { return _value; }
        set
        {
            _value = value;
            LineEdit lineEdit = GetNode<LineEdit>($"{nameof(VBoxContainer)}/{nameof(LineEdit)}");
            lineEdit.Text = value.ToString();
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
            Label label = GetNode<Label>($"{nameof(VBoxContainer)}/{nameof(Label)}");
            label.Text = value;
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
