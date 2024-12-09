using Godot;
using System;

public class Page6 : VBoxContainer, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page6); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");

        if (language == Language.English)
        {
            label.BbcodeText = "[center][b]Terminology:[/b]\n\nHomozygous: having two identical alleles of a particular gene or genes.\n\nHeterozygous: having two different alleles of a particular gene or genes.\n\nAlleles: one of two or more alternative forms of a gene that arise by mutation and are found at the same place on a chromosome.\n\n";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center][b]Terminología:[/b]\n\nHomocigoto: tener dos alelos idénticos de un gen o genes particulares.\n\nHeterocigoto: tener dos alelos diferentes de un gen o genes particulares.\n\nAlelos: una de dos o más formas alternativas de un gen que surgen por mutación y se encuentran en el mismo lugar en un cromosoma.\n\n";
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
