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
            inputWindow.inputBox.SelectionStart = 0;
            inputWindow.inputBox.SelectionLength = inputBoxPrefill.Length;
            inputWindow.ShowDialog();
            inputWindow.Activate();
            if (inputWindow.inputBox.Text == inputBoxPrefill || inputWindow.inputBox.Text.Length < 1)
            {
                return null;
            } else
            {
                return inputWindow.inputBox.Text;
            }
        }
        
        public static string getInput(String instructionText)
        {
            return getInput(instructionText, "Enter value here...");
        }

        public static int GetDeprovisionReason()
        {
            DeprovosionSelect window = new DeprovosionSelect();
            window.ShowDialog();
            window.Activate();

            // window has closed
            if (window.sameModelReplacementRadio.IsChecked == true)
            {
                return 1;
            }
            else if (window.differentModelReplacementRadio.IsChecked == true)
            {
                return 2;
            } else if (window.retiringDeviceRadio.IsChecked == true)
            {
                return 3;
            } else
            {
                return 0;
            }
        }
        public static List<string> GetDataGridSelection(String instructionText, String instructionTextBoxText, List<OrgUnit> inputData)
        {
            DataGridInput inputWindow = new DataGridInput();
            inputWindow.instructionTextBlock.Text = instructionText;
            inputWindow.radioGrid.ItemsSource = inputData;
            inputWindow.inputTextBox.Text = instructionTextBoxText;
            inputWindow.ShowDialog();
            string inputTextBoxData = inputWindow.inputTextBox.Text;
            return inputWindow.inputTextBox.Text.Split('|').ToList();
        }

        public static List<string> GetDataGridSelection(String instructionText, String instructionTextBoxText, List<BasicDeviceInfo> inputData)
        {
            DataGridInput inputWindow = new DataGridInput();
            inputWindow.instructionTextBlock.Text = instructionText;
            inputWindow.radioGrid.ItemsSource = inputData;
            inputWindow.inputTextBox.Text = instructionTextBoxText;
            inputWindow.ShowDialog();
            string inputTextBoxData = inputWindow.inputTextBox.Text;
            return inputWindow.inputTextBox.Text.Split('|').ToList();
        }
    }
}
