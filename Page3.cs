using Godot;
using System;

public class Page3 : RichTextLabel, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page3); } }

    public void Localize(Language language)
    {
        if (language == Language.English)
        {
            this.BbcodeText = "[center]\n[b]Exploration[/b]\n\nNow, play around with the simulation. It is totally okay if you don't understand everything mentioned in the simulation. Just explore it for a few minutes, then go to the next page!";
        }
        else if (language == Language.Spanish)
        {
            this.BbcodeText = "[center][b]Exploración[/b]\n\nAhora, juega con la simulación. Está totalmente bien si no entiendes todo lo mencionado en la simulación. ¡Simplemente explórala durante unos minutos, luego pasa a la siguiente página!";
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
