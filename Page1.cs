using Godot;
using System;

public class Page1 : VBoxContainer, ILocalizable
{
    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]Press the “Setup” button of the simulation. Describe what you see in this simulation.[/center]";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Presiona el botón \"Configurar\" de la simulación. Describe lo que ves en esta simulación.[/center]";
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
