using Godot;
using System;

public class Page7 : VBoxContainer, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page7); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]Next, you will make some predictions and test your predictions by exploring the simulation. \n\nWhile answering the following questions, make sure that the \"Predation?\" box is unchecked.\n\nMake sure you press SETUP after unchecking the \"Predation?\" box.";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]A continuación, harás algunas predicciones y probarás tus predicciones explorando la simulación. \n\nAl responder las siguientes preguntas, asegúrate de que la casilla \"Depredación?\" esté desmarcada.\n\nAsegúrate de presionar CONFIGURAR después de desmarcar la casilla \"Depredación?\".";
        }
    }

    public bool CanAdvance()
    {
        return true;
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
