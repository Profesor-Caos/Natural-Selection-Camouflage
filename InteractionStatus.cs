using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelectionCamouflage
{
    internal class InteractionStatus
    {
        // Test Groups are 1, 2, and 3.
        // 1 Receives no hints, 2 receives hints, 3 is asked if they'd like hints.
        public static int TestGroup = 0;

        public static Language Language;

        public static Main Main;

        private static bool PredationEnabled
        {
            get { return Main.GetNode<CheckBox>("Simulation/VBoxContainer/HBoxContainer/Buttons/PredationEnabled").Pressed; }
        }

        private static InitialSettings Settings
        {
            get { return Main.GetNode<InitialSettings>("Simulation/VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/InitialSettings"); }
        }

        private static Data Data
        {
            get { return Main.GetNode<Data>("Simulation/VBoxContainer/HBoxContainer2/VBoxContainer/HBoxContainer/Data"); }
        }

        public static int PageNumber = 0;

        public static string Prompt3 = "Change the sliders under initial settings. Press SETUP afterwards to see the effects!";
        public static string EsPrompt3 = "Cambia los deslizadores bajo configuración inicial. ¡Presiona CONFIGURAR después para ver los efectos!";
        public static string Prompt7 = "Make sure you press setup after unchecking the \"Predation?\" box.";
        public static string Prompt8A = "Remember to adjust the initial settings sliders so that all the mice are white.";
        public static string Prompt8B = "Remember that the Homozygous recessive mice were white.";
        public static string Prompt9A = "Remember to adjust the initial settings sliders so that all the mice are black.";
        public static string Prompt9B = "Remember that the Homozygous recessive mice were white.";

        public static bool CheckInteraction(string controlName, out string message)
        {
            message = null;

            if (TestGroup == 1)
                return true;

            if (PageNumber == 3)
            {
                message = Language == Language.English ? Prompt3 : EsPrompt3;
                if (controlName == "Go Once")
                    return false;
                if (controlName == "Go")
                    return false;
                if (controlName == "Set Light Background")
                    return false;
                if (controlName == "Set Dark Background")
                    return false;
                if (controlName == "Set Mixed Background")
                    return false;
                if (controlName == "Add A Mutant")
                    return false;
                if (controlName == "Predation")
                    return false;
                if (controlName == "% Chance of Predation")
                    return false;
            }

            if (PageNumber == 7)
            {
                // If mice are not all white when they press go prompt
                if (controlName == "Go")
                {
                    // In this case we have no white mice.
                    if (Data.aaMales.Value == 0 && Data.aaFemales.Value == 0)
                    {

                    }
                }
            }

            if (PageNumber == 8)
            {

            }

            return true;
        }
    }
}
