using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChromebookGUI.Classes
{
    class AutoCompleteResult : TextBlock {
        private bool _isSelected = false;

        public bool IsSelected {
            get
            {
                return _isSelected;
            }
            set
            {
                Background = value ? Brushes.LightBlue : Brushes.Transparent;
                _isSelected = value;
            }
        }

        public static AutoCompleteResult Generate(string text, StackPanel stackPanel, TextBox textBox)
        {
            AutoCompleteResult final = new AutoCompleteResult
            {

                // Add the text   
                Text = text,
                // A little style...   
                Margin = new Thickness(2, 3, 2, 3),
                Cursor = Cursors.Hand,
                Focusable = true
            };

            // Mouse events   
            final.MouseLeftButtonUp += (sender, e) =>
            {
                textBox.Text = (sender as TextBlock).Text;
                AutoComplete.Close(stackPanel);
            };

            final.MouseEnter += (sender, e) =>
            {
                //TextBlock b = sender as TextBlock;
                //b.Background = System.Windows.Media.Brushes.LightSkyBlue;
                final.IsSelected = true;
            };

            final.MouseLeave += (sender, e) =>
            {
                final.IsSelected = false;
            };

            final.LostFocus += (sender, e) =>
            {
                final.IsSelected = false;
            };

            final.GotFocus += (sender, e) =>
            {
                final.IsSelected = true;
            };

            final.PreviewKeyDown += (sender, e) =>
            {
                switch (e.Key)
                {
                    case Key.Down:
                        AutoComplete.FocusNextCompletion(stackPanel, textBox);
                        return;
                    case Key.Up:
                        AutoComplete.FocusPreviousCompletion(stackPanel, textBox);
                        return;
                    case Key.Escape:
                        AutoComplete.Close(stackPanel);
                        return;
                    default:
                        return;
                }
            };

            //final.LostFocus += (sender, e) =>
            //{
            //    final.IsSelected = false;
            //};
            return final;
        }
    }
}
