using Godot;
using System;

public class Page10 : VBoxContainer, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page10); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]There are two alleles \"A\" (dominant) and \"a\" (recessive). Based on your investigations so far, can you say what would the phenotype (fur color: light or dark) be of the three genotypes \"AA\", \"Aa\", and \"aa\"?\n\nAA: Light or Dark\nAa: Light or Dark\naa: Light or Dark\n\nPlease explain how you figured out the answer to the previous question.";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Hay dos alelos \"A\" (dominante) y \"a\" (recesivo). Basándote en tus investigaciones hasta ahora, ¿puedes decir cuál sería el fenotipo (color del pelaje: claro u oscuro) de los tres genotipos \"AA\", \"Aa\" y \"aa\"?\n\nAA: Claro u Oscuro\nAa: Claro u Oscuro\naa: Claro u Oscuro\n\nPor favor, explica cómo llegaste a la respuesta de la pregunta anterior.";
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
