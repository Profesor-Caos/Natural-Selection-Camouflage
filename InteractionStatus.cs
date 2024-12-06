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

        public static Dictionary<int, int> PageMistakeCount = new Dictionary<int, int>();
        public static Dictionary<int, bool> TestGroup3Overrides = new Dictionary<int, bool>();

        public static int PageNumber = 0;

        public static string Prompt3 = "Change the sliders under initial settings. Press SETUP afterwards to see the effects!";
        public static string EsPrompt3 = "Cambia los deslizadores bajo configuración inicial. ¡Presiona CONFIGURAR después para ver los efectos!";
        public static string Prompt6 = "Make sure you press setup after unchecking the \"Predation?\" box.";
        public static string EsPrompt6 = "Asegúrate de desmarcar la depredación y presionar configurar.";
        public static string Prompt7 = "Remember to adjust the initial settings sliders so that all the mice are white then press setup.";
        public static string EsPrompt7 = "Recuerda ajustar los deslizadores de configuración inicial para que todos los ratones sean blancos y luego presiona configurar.";
        public static string Prompt8 = "Remember to adjust the initial settings sliders so that all the mice are black then press setup.";
        public static string EsPrompt8 = "Recuerda ajustar los deslizadores de configuración inicial para que todos los ratones sean negros y luego presiona configurar.";

        public static void Initialize()
        {
            Main.PopupClosed += Main_PopupClosed;

            int[] pages = new int[]{ 3, 6, 7, 8 };
            foreach (int i in pages)
            {
                PageMistakeCount[i] = 0;
                TestGroup3Overrides[i] = false;
            }
        }

        public static bool CheckInteractionHelper(string controlName, out string message)
        {
            message = null;
            if (PageNumber == 3)
            {
                message = Language == Language.English ? Prompt3 : EsPrompt3;
                // TODO: Maybe a better message when Go/Go Once is pressed?
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

            if (PageNumber == 6 || PageNumber == 7 || PageNumber == 8)
            {
                if (PredationEnabled)
                {
                    if (controlName == "Go" || controlName == "Go Once" || controlName == "Setup")
                    {
                        message = Language == Language.English ? Prompt6 : EsPrompt6;
                        return false;
                    }
                }
            }

            if (PageNumber == 7)
            {
                // If mice are not all white when they press go prompt
                if (controlName == "Go" || controlName == "Go Once")
                {
                    // In this case we have no white mice.
                    if (Data.aaMales.Value == 0 && Data.aaFemales.Value == 0)
                    {
                        message = Language == Language.English ? Prompt7 : EsPrompt7;
                        return false;
                    }
                    // In this case, we have any number of black mice.
                    if (Data.AAMales.Value > 0 || Data.AaMales.Value > 0 || Data.AAFemales.Value > 0 || Data.AaFemales.Value > 0)
                    {
                        message = Language == Language.English ? Prompt7 : EsPrompt7;
                        return false;
                    }
                }
            }

            if (PageNumber == 8)
            {
                // TODO: If we started with 0 white and pressed Go Once, there might now be white
                // Need to address this and really probably just check the sliders...
                // If mice are not all black when they press go prompt
                if (controlName == "Go" || controlName == "Go Once")
                {
                    // In this case we have any number of white mice.
                    if (Data.aaMales.Value > 0 || Data.aaFemales.Value > 0)
                    {
                        message = Language == Language.English ? Prompt8 : EsPrompt8;
                        return false;
                    }
                    // In this case, we have any number of black mice.
                    if (Data.AAMales.Value == 0 && Data.AaMales.Value == 0 && Data.AAFemales.Value == 0 && Data.AaFemales.Value == 0)
                    {
                        message = Language == Language.English ? Prompt8 : EsPrompt8;
                        return false;
                    }
                }
            }

            return true;
        }

        public static bool CheckInteraction(string controlName, out string message)
        {
            message = null;

            if (TestGroup == 1)
                return true;


            bool returnValue = CheckInteractionHelper(controlName, out message);

            if (returnValue)
                return true;

            if (TestGroup == 2 || TestGroup3Overrides[PageNumber])
            {
                AcceptDialog ad = new AcceptDialog();
                ad.DialogText = message;
                ad.WindowTitle = Language == Language.English ? "Reminder" : "Recordatorio";
                Main.AddChild(ad);
                ad.PopupCentered();
                return false;
            }

            if (TestGroup == 3)
            {
                if (!returnValue)
                {
                    // First mistake
                    if (PageMistakeCount[PageNumber] == 0)
                    {
                        AssistanceRequest ar = ((PackedScene)ResourceLoader.Load("res://AssistanceRequest.tscn")).Instance() as AssistanceRequest;
                        Main.AddChild(ar);
                        ar.Assistance = message;
                        Main.ShowPopup(ar);
                        return false;
                    }
                    else
                    {
                        PageMistakeCount[PageNumber]++;
                        // Reset counter after 3 mistakes so prompts keep happening
                        if (PageMistakeCount[PageNumber] == 3)
                            PageMistakeCount[PageNumber] = 0;
                    }
                }
            }
            return true;
        }

        private static void Main_PopupClosed(object sender, AssistanceRequestEventArgs e)
        {
            if (e.Result == Result.Yes)
            {
                TestGroup3Overrides[PageNumber] = true;
                AcceptDialog ad = new AcceptDialog();
                ad.DialogText = e.AssistanceMessage;
                ad.WindowTitle = Language == Language.English ? "Reminder" : "Recordatorio";
                Main.AddChild(ad);
                ad.PopupCentered();
            }
            else
            {
                PageMistakeCount[PageNumber]++;
            }
        }
    }
}
