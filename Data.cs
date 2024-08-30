using Godot;
using System;

public class Data : VBoxContainer
{
    public void UpdateData(int value, string dataLabel)
    {
        string labelPath = $"HBoxContainer/{(dataLabel.Contains("Females") ? "FemaleDataContainer" : "MaleDataContainer")}/{dataLabel}";
        DataBox dataBox = GetNode<DataBox>(labelPath);
        dataBox.Value = value;
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
