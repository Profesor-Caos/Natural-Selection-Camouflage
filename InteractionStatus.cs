using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaturalSelectionCamouflage
{
    internal class InteractionStatus
    {
        public static bool IsControlGroup = false;

        public static int PageNumber = 0;

        public static string Prompt3 = "Change the sliders under initial settings. Press SETUP afterwards to see the effects!";
        public static string Prompt7 = "Make sure you press setup after unchecking the \"Predation?\" box.";
        public static string Prompt8A = "Remember to adjust the initial settings sliders so that all the mice are white.";
        public static string Prompt8B = "Remember that the Homozygous recessive mice were white.";
        public static string Prompt9A = "Remember to adjust the initial settings sliders so that all the mice are black.";
        public static string Prompt9B = "Remember that the Homozygous recessive mice were white.";

        public static bool CheckInteraction(string controlName, out string message)
        {
            message = null;

            if (IsControlGroup)
                return true;

            if (PageNumber != 3)
                return true;

            if (PageNumber == 3)
            {
                message = Prompt3;
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

            if (PageNumber == 8 || PageNumber == 9)
            {
                if (controlName == "Go")
                {

                }
            }

            if (PageNumber == 8)
            {

            }

            return true;
        }
    }
}
