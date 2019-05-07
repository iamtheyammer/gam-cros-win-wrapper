using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChromebookGUI.Classes
{
    /// <summary>
    /// To be used very carefully. Basically helps remove extraneous code from the views.
    /// </summary>
    class AutoComplete
    {
        private static readonly string basePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\iamtheyammer\ChromebookGUI\autocomplete.txt";
        private static List<string> List { get; set; }

        public static void Init()
        {
            

            try
            {
                List = File.ReadAllLines(basePath).ToList();
            }
            catch
            {
                return;
            }
        }

        public static void Save()
        {
            
            File.WriteAllLines(basePath, List);
        }

        public static List<string> GetList()
        {
            return List;
        }

        public static void AddItemToList(string item)
        {
            if (!List.Contains(item)) List.Add(item);
            return;
        }

        /// <summary>
        /// Closes the AutoComplete panel.
        /// </summary>
        /// <param name="stackPanel"></param>
        public static void Close(StackPanel stackPanel)
        {
            Border border = (stackPanel.Parent as ScrollViewer).Parent as Border;
            stackPanel.Children.Clear();
            border.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Should be sent from your OnKeyUp event.
        /// </summary>
        /// <param name="sender">Sender from the KeyUp event</param>
        /// <param name="e">From the KeyUp event</param>
        /// <param name="stackPanel">The stack panel of your AutoComplete</param>
        /// <param name="textBox">The text box that will be AutoCompleted</param>
        /// <param name="data">The data you want the AutoComplete to use.</param>
        public static void OnKeyUp(object sender, KeyEventArgs e, StackPanel stackPanel, TextBox textBox)
        {
            bool found = false;
            Border border = (stackPanel.Parent as ScrollViewer).Parent as Border;

            string query = (sender as TextBox).Text;

            if (query.Length == 0 || query == "Enter a Device ID, Asset ID, Query String, Serial Number or Email...")
            {
                // Clear   
                stackPanel.Children.Clear();
                border.Visibility = Visibility.Collapsed;
            }
            else
            {
                border.Visibility = Visibility.Visible;
            }

            // Clear the list
            stackPanel.Children.Clear();

            // Add the result   
            foreach (string obj in List)
            {
                if (obj.ToLower().StartsWith(query.ToLower()))
                {
                    // The word starts with this... Autocomplete must work   
                    AddItemToStack(obj, stackPanel, textBox);
                    found = true;
                }
            }

            if (!found)
            {
                stackPanel.Children.Add(new TextBlock() { Text = "No results found." });
            }
        }

        /// <summary>
        /// Focuses the completion below the current completion, or the first completion if none is selected.
        /// </summary>
        /// <param name="stackPanel">The StackPanel holding the autocompletions</param>
        /// <param name="textBox">The destination text box that the autocomplete will complete.</param>
        public static void FocusNextCompletion(StackPanel stackPanel, TextBox textBox)
        {
            for(int i = 0; i < stackPanel.Children.Count; i++)
            {
                AutoCompleteResult child = (stackPanel.Children[i] as AutoCompleteResult);
                if(child.IsFocused && stackPanel.Children.Count > 1 && i + 1 < stackPanel.Children.Count)
                {
                    textBox.Text = (stackPanel.Children[i + 1] as AutoCompleteResult).Text;
                    return;
                }
            }
            if(stackPanel.Children.Count > 0)
            {
                AutoCompleteResult destination = stackPanel.Children[0] as AutoCompleteResult;
                destination.Focus();
                textBox.Text = destination.Text;
            }
        }

        /// <summary>
        /// Like FocusNextCompletion but focuses the previous completion.
        /// </summary>
        /// <param name="stackPanel"></param>
        public static void FocusPreviousCompletion(StackPanel stackPanel, TextBox textBox)
        {
            for (int i = 0; i < stackPanel.Children.Count; i++)
            {
                TextBlock child = (stackPanel.Children[i] as TextBlock);
                if (child.IsFocused && stackPanel.Children.Count > 1 && i - 1 > -1)
                {
                    AutoCompleteResult toFocus = (stackPanel.Children[i] as AutoCompleteResult);
                    AutoCompleteResult takeTextFrom = (stackPanel.Children[i - 1] as AutoCompleteResult);
                    toFocus.Focus();
                    textBox.Text = (takeTextFrom.Text);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds item to the stack panel.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="stackPanel"></param>
        /// <param name="textBox"></param>
        public static void AddItemToStack(string text, StackPanel stackPanel, TextBox textBox)
        {
            stackPanel.Children.Add(AutoCompleteResult.Generate(text, stackPanel, textBox));
        }
    }
}
