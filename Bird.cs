using Godot;
using NaturalSelectionCamouflage;
using System;

public class Bird : Area2D
{
    public void Behave()
    {
        Utils.Move(this);
    }

    public void Die()
    {
        this.QueueFree();
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
