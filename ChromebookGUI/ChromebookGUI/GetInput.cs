using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChromebookGUI
{
    class GetInput
    {
        public static string getInput(String instructionText, String inputBoxPrefill)
        {
            InputWindow inputWindow = new InputWindow();
            inputWindow.instructionTextBlock.Text = instructionText;
            inputWindow.inputBox.Text = inputBoxPrefill;
            inputWindow.Show();
            inputWindow.Activate();
            return inputWindow.inputBox.Text;
        }

    }
}
