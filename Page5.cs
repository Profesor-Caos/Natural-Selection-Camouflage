using Godot;
using System;

public class Page5 : VBoxContainer, ILocalizable
{
    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]To get all mice with light fur, you may change the initial settings as the following:[/center]";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Para obtener todos los ratones con pelaje claro, puedes cambiar la configuración inicial de la siguiente manera:[/center]";
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
