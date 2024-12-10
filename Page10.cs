using Godot;
using System;
using System.Collections.Generic;

public class Page10 : VBoxContainer, ILocalizable, ITextResponse, IPage, ILogger
{
    public event EventHandler<LogEventArgs> LogEvent;

    public new string Name { get { return nameof(Page10); } }

    public void Localize(Language language)
    {
        RichTextLabel label = GetNode<RichTextLabel>("RichTextLabel");
        RichTextLabel label2 = GetNode<RichTextLabel>("RichTextLabel2");
        CheckBox light = GetNode<CheckBox>("AABox/Light");
        CheckBox light2 = GetNode<CheckBox>("AaBox/Light");
        CheckBox light3 = GetNode<CheckBox>("aaBox/Light");
        CheckBox dark = GetNode<CheckBox>("AABox/Dark");
        CheckBox dark2 = GetNode<CheckBox>("AaBox/Dark");
        CheckBox dark3 = GetNode<CheckBox>("aaBox/Dark");

        if (language == Language.English)
        {
            label.BbcodeText = "[center]There are two alleles \"A\" (dominant) and \"a\" (recessive). Based on your investigations so far, can you say what would the phenotype (fur color: light or dark) be of the three genotypes \"AA\", \"Aa\", and \"aa\"?";
            label2.BbcodeText = "\n[center]Please explain how you figured out the answer to the previous question.";
            light.Text = "Light";
            light2.Text = "Light";
            light3.Text = "Light";
            dark.Text = "Dark";
            dark2.Text = "Dark";
            dark3.Text = "Dark";
        }
        else if (language == Language.Spanish)
        {
            label.BbcodeText = "[center]Hay dos alelos \"A\" (dominante) y \"a\" (recesivo). Basándote en tus investigaciones hasta ahora, ¿puedes decir cuál sería el fenotipo (color del pelaje: claro u oscuro) de los tres genotipos \"AA\", \"Aa\" y \"aa\"?";
            label2.BbcodeText = "[center]Por favor, explica cómo llegaste a la respuesta de la pregunta anterior.";
            light.Text = "Claro";
            light2.Text = "Claro";
            light3.Text = "Claro";
            dark.Text = "Oscuro";
            dark2.Text = "Oscuro";
            dark3.Text = "Oscuro";
        }
    }

    public bool CanAdvance()
    {
        WrapTextEdit text = GetNode<WrapTextEdit>(nameof(WrapTextEdit));
        CheckBox light = GetNode<CheckBox>("AABox/Light");
        CheckBox light2 = GetNode<CheckBox>("AaBox/Light");
        CheckBox light3 = GetNode<CheckBox>("aaBox/Light");
        CheckBox dark = GetNode<CheckBox>("AABox/Dark");
        CheckBox dark2 = GetNode<CheckBox>("AaBox/Dark");
        CheckBox dark3 = GetNode<CheckBox>("aaBox/Dark");
        return (text.Text.Length > 0 && !String.IsNullOrWhiteSpace(text.Text)
            && (light.Pressed ^ dark.Pressed)
            && (light2.Pressed ^ dark2.Pressed)
            && (light3.Pressed ^ dark3.Pressed));
    }

    public List<string> GetResponses()
    {
        WrapTextEdit text = GetNode<WrapTextEdit>(nameof(WrapTextEdit));
        return new List<string> { text.Text };
    }

    public void OnLightPressed()
    {
        CheckBox dark = GetNode<CheckBox>("AABox/Dark");
        dark.Pressed = false;

        CheckBox light = GetNode<CheckBox>("AABox/Light");
        if (light.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "AA Light Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "AA Light Deselected"));
    }

    public void OnDarkPressed()
    {
        CheckBox light = GetNode<CheckBox>("AABox/Light");
        light.Pressed = false;

        CheckBox dark = GetNode<CheckBox>("AABox/Dark");
        if (dark.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "AA Dark Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "AA Dark Deselected"));
    }

    public void OnLight2Pressed()
    {
        CheckBox dark = GetNode<CheckBox>("AaBox/Dark");
        dark.Pressed = false;

        CheckBox light = GetNode<CheckBox>("AaBox/Light");
        if (light.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Aa Light Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Aa Light Deselected"));
    }

    public void OnDark2Pressed()
    {
        CheckBox light = GetNode<CheckBox>("AaBox/Light");
        light.Pressed = false;

        CheckBox dark = GetNode<CheckBox>("AaBox/Dark");
        if (dark.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Aa Dark Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "Aa Dark Deselected"));
    }

    public void OnLight3Pressed()
    {
        CheckBox dark = GetNode<CheckBox>("aaBox/Dark");
        dark.Pressed = false;

        CheckBox light = GetNode<CheckBox>("aaBox/Light");
        if (light.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "aa Light Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "aa Light Deselected"));
    }

    public void OnDark3Pressed()
    {
        CheckBox light = GetNode<CheckBox>("aaBox/Light");
        light.Pressed = false;

        CheckBox dark = GetNode<CheckBox>("aaBox/Dark");
        if (dark.Pressed)
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "aa Dark Selected"));
        else
            LogEvent?.Invoke(this, new LogEventArgs(DateTime.Now, "aa Dark Deselected"));
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
