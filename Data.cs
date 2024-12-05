using Godot;
using System;

public class Data : VBoxContainer, ILocalizable
{
    public void UpdateData(int value, string dataLabel)
    {
        string labelPath = $"HBoxContainer/{(dataLabel.Contains("Females") ? "FemaleDataContainer" : "MaleDataContainer")}/{dataLabel}";
        DataBox dataBox = GetNode<DataBox>(labelPath);
        dataBox.Value = value;
    }

    public DataBox AAMales
    {
        get
        {
            DataBox AAMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/AA Males");
            return AAMales;
        }
    }

    public DataBox AaMales
    {
        get
        {
            DataBox AaMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/Aa Males");
            return AaMales;
        }
    }

    public DataBox aaMales
    {
        get
        {
            DataBox aaMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/aa Males");
            return aaMales;
        }
    }

    public DataBox AAFemales
    {
        get
        {
            DataBox AAFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/AA Females");
            return AAFemales;
        }
    }

    public DataBox AaFemales
    {
        get
        {
            DataBox AaFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/Aa Females");
            return AaFemales;
        }
    }

    public DataBox aaFemales
    {
        get
        {
            DataBox aaFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/aa Females");
            return aaFemales;
        }
    }

    public void SetTotalMice(int value)
    {
        DataBox totalMice = GetNode<DataBox>("TotalMice");
        totalMice.Value = value;
    }


    public void IncrementGeneration()
    {
        DataBox generation = GetNode<DataBox>("Generations");
        generation.Value++;
    }

    public void ResetGeneration()
    {
        DataBox generation = GetNode<DataBox>("Generations");
        generation.Value = 0;
    }

    public void Localize(Language language)
    {
        DataBox AAMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/AA Males");
        DataBox AaMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/Aa Males");
        DataBox aaMales = GetNode<DataBox>("HBoxContainer/MaleDataContainer/aa Males");
        DataBox AAFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/AA Females");
        DataBox AaFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/Aa Females");
        DataBox aaFemales = GetNode<DataBox>("HBoxContainer/FemaleDataContainer/aa Females");
        DataBox generation = GetNode<DataBox>("Generations");
        DataBox totalMice = GetNode<DataBox>("TotalMice");

        if (language == Language.English)
        {
            AAMales.Label = "AA Males";
            AaMales.Label = "Aa Males";
            aaMales.Label = "aa Males";
            AAFemales.Label = "AA Females";
            AaFemales.Label = "Aa Females";
            aaFemales.Label = "aa Females";
            generation.Label = "Generations";
            totalMice.Label = "Total Mice";
        }
        else if (language == Language.Spanish)
        {
            AAMales.Label = "Machos AA";
            AaMales.Label = "Machos Aa";
            aaMales.Label = "Machos aa";
            AAFemales.Label = "Hembras AA";
            AaFemales.Label = "Hembras Aa";
            aaFemales.Label = "Hembras aa";
            generation.Label = "Generaciones";
            totalMice.Label = "Total de Ratones";
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
