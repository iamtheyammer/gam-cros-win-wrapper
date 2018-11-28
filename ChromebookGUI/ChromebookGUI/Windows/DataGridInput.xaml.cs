using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChromebookGUI
{
    /// <summary>
    /// Interaction logic for RadioInput.xaml
    /// </summary>
    public partial class DataGridInput : Window
    {
        public DataGridInput()
        {
            InitializeComponent();
        }

        private void radioGrid_Selected(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine(radioGrid.SelectedItems[0]);
        }

        private void dataGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            string result = null;
            foreach (var item in e.AddedCells)
            {
                string textBoxValue = "uh oh";
                var col = item.Column as DataGridColumn;
                var fc = col.GetCellContent(item.Item);
                if(fc is TextBlock && col.DisplayIndex == 0)
                {
                    textBoxValue = (fc as TextBlock).Text;
                }
                result += textBoxValue + "|";
                //// Like this for all available types of cells
            }
            inputTextBox.Text = result;
            this.Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
