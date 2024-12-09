using Godot;
using System;
using System.Collections.Generic;

public class Page8 : VBoxContainer, ILocalizable, ITextResponse, IPage
{
    public new string Name { get {  return nameof(Page8); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");
        RichTextLabel label2 = GetNode<RichTextLabel>("RichTextLabel2");
        RichTextLabel label3 = GetNode<RichTextLabel>("RichTextLabel3");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]Make the simulation look like the figure below:";
            label2.BbcodeText = "[center]\nPredict what will happen after lots of generations if the initial population of mice all has light-colored fur?";
            label3.BbcodeText = "[center]Run an experiment to prove or disprove your answer to the previous question and explain your observations.[/center]";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Haz que la simulación se vea como la figura de abajo:";
            label2.BbcodeText = "[center]\n¿Qué predices que sucederá después de muchas generaciones si la población inicial de ratones tiene todo el pelaje de color claro?";
            label3.BbcodeText = "[center]Realiza un experimento para probar o refutar tu respuesta a la pregunta anterior y explica tus observaciones.";
        }
    }

    public List<string> GetResponses()
    {
        WrapTextEdit text = GetNode<WrapTextEdit>(nameof(WrapTextEdit));
        WrapTextEdit text2 = GetNode<WrapTextEdit>(nameof(WrapTextEdit)+"2");
        return new List<string> { text.Text, text2.Text };
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
