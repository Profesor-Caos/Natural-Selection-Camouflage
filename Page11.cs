using Godot;
using System;

public class Page11 : RichTextLabel, ILocalizable, IPage
{
    public new string Name { get { return nameof(Page11); } }

    public void Localize(Language language)
    {
        if (language == Language.English)
        {
            this.BbcodeText = "\n\n[center]Finished:\n\nRaise your hand!";
        }
        else if (language == Language.Spanish)
        {
            this.BbcodeText = "\n\n[center]Terminados:\n\n¡Levanten la mano!";
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
