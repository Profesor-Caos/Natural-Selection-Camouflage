using Godot;
using System;
using System.Collections.Generic;

public class Page4 : VBoxContainer, ILocalizable, ITextResponse, IPage
{
    public new string Name { get { return nameof(Page4); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]Change the sliders under the \"Initial Settings\" in the simulation. Try to change the settings such that all the mice have light-colored fur. Once you get all mice with light fur, describe the initial settings you used.\n\nMake sure every time you change the sliders that you press SETUP afterward so that you can actually see the effects of your new settings.[/center]\n";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Cambia los deslizadores bajo \"Configuración Inicial\" en la simulación. Intenta cambiar la configuración de manera que todos los ratones tengan pelaje de color claro. Una vez que obtengas todos los ratones con pelaje claro, describe la configuración inicial que usaste.\n\nAsegúrate de que cada vez que cambies los deslizadores presiones CONFIGURAR después para que puedas ver realmente los efectos de tu nueva configuración.\n[/center]";
        }
    }

    public List<string> GetResponses()
    {
        WrapTextEdit text = GetNode<WrapTextEdit>(nameof(WrapTextEdit));
        return new List<string> { text.Text };
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
