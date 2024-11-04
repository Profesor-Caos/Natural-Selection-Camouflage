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

    public void SetTotalMice(int value)
    {
        DataBox totalMice = GetNode<DataBox>("TotalMice");
        totalMice.Value = value;
    }


    public void IncrementGeneration()
    {
        DataBox generation = GetNode<DataBox>("Generations");
        generation.Value++;
    }

    public void ResetGeneration()
    {
        DataBox generation = GetNode<DataBox>("Generations");
        generation.Value = 0;
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
