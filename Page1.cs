using Godot;
using System;
using System.Collections.Generic;

public class Page1 : VBoxContainer, ILocalizable, ITextResponse, IPage
{
    public new string Name { get { return nameof(Page1); } }

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

    public bool CanAdvance()
    {
        WrapTextEdit text = GetNode<WrapTextEdit>(nameof(WrapTextEdit));
        return (text.Text.Length > 0 && !String.IsNullOrWhiteSpace(text.Text));
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
