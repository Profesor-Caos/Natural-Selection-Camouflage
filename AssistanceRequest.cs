using Godot;
using NaturalSelectionCamouflage;
using System;

public enum Result
{
    Yes,
    No
}

public class AssistanceRequest : WindowDialog, ILocalizable
{
    public Result Result = Result.No;

    private void OnYesPressed()
    {
        this.Result = Result.Yes;
        this.Hide();
        this.QueueFree();
    }

    private void OnNoPressed()
    {
        this.Result = Result.No;
        this.Hide();
        this.QueueFree();
    }

    public void Localize(Language language)
    {
        RichTextLabel text = GetNode<RichTextLabel>("VBoxContainer/RichTextLabel");
        Button yes = GetNode<Button>("VBoxContainer/HBoxContainer/Yes");
        Button no = GetNode<Button>("VBoxContainer/HBoxContainer/No");

        if (language == Language.English)
        {
            this.WindowTitle = "Assistance Prompt";
            text.BbcodeText = "[center]You seem to be having a bit of trouble with this question. Would you like a hint?";
            yes.Text = "Yes";
            no.Text = "No";
        }
        else if (language == Language.Spanish)
        {
            this.WindowTitle = "Assistance Prompt";
            text.BbcodeText = "[center]You seem to be having a bit of trouble with this question. Would you like a hint?";
            yes.Text = "Si";
            no.Text = "No";
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Localize(InteractionStatus.Language);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
