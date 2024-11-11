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

        public static bool CheckInteraction(string controlName, out string message)
        {
            message = null;

            if (IsControlGroup)
                return true;

            //if (PageNumber == 0)
            //    return false;

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
            }

            return true;
        }
    }
}
