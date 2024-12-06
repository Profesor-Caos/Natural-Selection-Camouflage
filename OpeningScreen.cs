using Godot;
using System;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using Godot.Collections;

public class OpeningScreen : VBoxContainer, ILocalizable
{
    public event EventHandler Finished;
    public Language Language;
    public int StudentID;
    private bool _isSubmitting = false;

    public int TestGroup;
    private string URL = "https://nscbackend.onrender.com";
    private string[] Headers = { "Content-Type: application/json" };

    private void OnEnglishPressed()
    {
        GetNode<CheckBox>("LanguageChoices/Spanish").Pressed = false;

        if (GetNode<CheckBox>("LanguageChoices/English").Pressed)
            Localize(Language.English);
    }

    private void OnSpanishPressed()
    {
        GetNode<CheckBox>("LanguageChoices/English").Pressed = false;

        if (GetNode<CheckBox>("LanguageChoices/Spanish").Pressed)
            Localize(Language.Spanish);
    }

    private void OnStudentIDTextChanged(string newText)
    {
        LineEdit editor = GetNode<LineEdit>("StudentID");
        int caretPosition = editor.CaretPosition;

        string validatedText = Regex.Replace(newText, @"[^0-9]", "");
        if (newText != validatedText)
        {
            editor.Text = validatedText;
            editor.CaretPosition = caretPosition - 1;
        }
    }

    private void OnRequestCompleted(int result, int responseCode, string[] headers, byte[] body, object request)
    {
        (request as HTTPRequest).QueueFree();
        this.RemoveChild(request as HTTPRequest);

        string responseText = System.Text.Encoding.UTF8.GetString(body);

        GD.Print("Response Code: ", responseCode);
        GD.Print("Response: ", responseText);

        // TODO: wrap in try and if errors happen, select a random test group and log it...
        if (responseCode == 200)
        {
            var student = JSON.Parse(responseText);
            if (student.Result is Dictionary dictionary)
            {
                var testGroup = dictionary["TestGroup"];
                this.TestGroup = Convert.ToInt32(testGroup);

                var studentID = dictionary["StudentID"];
                this.StudentID = Convert.ToInt32(studentID);
            }
            this.Language = GetNode<CheckBox>("LanguageChoices/Spanish").Pressed ? Language.Spanish : Language.English;
            this.Finished?.Invoke(this , new EventArgs());
            return;
        }
        else
        {
            AcceptDialog ad = new AcceptDialog();
            ad.WindowTitle = "Response: " + responseCode;
            ad.DialogText = responseText;
            this.AddChild(ad);
            ad.PopupCentered();
            _isSubmitting = false;
            return;
        }
    }

    private HTTPRequest CreateRequest()
    {
        var http = new HTTPRequest();
        AddChild(http);
        http.Connect("request_completed", this, nameof(OnRequestCompleted), new Godot.Collections.Array(http));
        return http;
    }

    private void OnSubmitPressed()
    {
        LineEdit editor = GetNode<LineEdit>("StudentID");
        // Special codes for testing that skip the server check.
        if (editor.Text == "444444" || editor.Text == "555555" || editor.Text == "666666")
        {
            this.TestGroup = Int32.Parse(editor.Text[0].ToString()) - 3;
            this.Language = GetNode<CheckBox>("LanguageChoices/Spanish").Pressed ? Language.Spanish : Language.English;
            this.Finished?.Invoke(this, new EventArgs());
            return;
        }
        string url = URL + $"/students/{editor.Text}";
        var http = CreateRequest();
        Error err = http.Request(url, Headers, false, HTTPClient.Method.Get);

        if (err != Error.Ok)
            GD.PrintErr("Failed to submit request to server: ", err);

        Button submitButton = GetNode<Button>("Submit");
        submitButton.Disabled = true;
        _isSubmitting = true;
    }

    private void SetSubmitStatus()
    {
        if (_isSubmitting)
            return;

        Button submitButton = GetNode<Button>("Submit");
        LineEdit editor = GetNode<LineEdit>("StudentID");
        if (editor.Text.Length != 6)
        {
            submitButton.Disabled = true;
            return;
        }

        if (!GetNode<CheckBox>("LanguageChoices/Spanish").Pressed && !GetNode<CheckBox>("LanguageChoices/English").Pressed)
        {
            submitButton.Disabled = true;
            return;
        }

        submitButton.Disabled = false;
    }

    public void Localize(Language language)
    {
        Label languageSelection = GetNode<Label>("LanguageSelection");
        Label studentID = GetNode<Label>("StudentIDLabel");
        Button submit = GetNode<Button>("Submit");

        if (language == Language.English)
        {
            languageSelection.Text = "Language";
            studentID.Text = "Student ID";
            submit.Text = "Submit";
        }
        else if (language == Language.Spanish)
        {
            languageSelection.Text = "Idioma";
            studentID.Text = "ID de estudiante";
            submit.Text = "Enviar";
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        SetSubmitStatus();
    }
}
