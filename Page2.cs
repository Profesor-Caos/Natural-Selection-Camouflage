using Godot;
using System;

public class Page2 : RichTextLabel, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page2); } }

    public void Localize(Language language)
    {
        if (language == Language.English)
        {
            this.BbcodeText = "[center][b]The Rock Pocket Mice Simulation Environment:[/b]\n\nIn this simulation, there are mice with two types of fur coat colors: light and dark.\n\n[b]●[/b] What you can see like fur color are referred to as [i]phenotypes[/i], which are determined by the genes that a mouse has. There are two kinds of genes in this simulation that change the fur coat color of mice.\n\n[b]●[/b]  In this simulation, \"AA\", \"Aa\", and \"aa\" are genotypes (genetic makeup) of mice. The fur coat color of a mouse is dependent on its genotype. \n\nEach clock tick in the simulation is a mouse generation. In each generation, male and female mice move around randomly, search for a partner, and reproduce if they find a partner. [/center]";
        }
        else if (language == Language.Spanish)
        {
            this.BbcodeText = "[center][b]El Entorno de Simulación de Ratones de Bolsillo Rocosos:[/b]\n\nEn esta simulación, hay ratones con dos tipos de colores de pelaje: claro y oscuro.\n\n[b]●[/b] Lo que puedes ver como el color del pelaje se conoce como [i]fenotipos[/i], que están determinados por los genes que tiene un ratón. Hay dos tipos de genes en esta simulación que cambian el color del pelaje de los ratones.\n\n[b]●[/b] En esta simulación, \"AA\", \"Aa\" y \"aa\" son genotipos (composición genética) de los ratones. El color del pelaje de un ratón depende de su genotipo.\n\nCada tic del reloj en la simulación es una generación de ratones. En cada generación, los ratones macho y hembra se mueven al azar, buscan una pareja y se reproducen si encuentran una pareja.\n[/center]";
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
