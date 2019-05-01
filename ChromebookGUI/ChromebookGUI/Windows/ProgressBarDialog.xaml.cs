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

namespace ChromebookGUI.Windows
{
    /// <summary>
    /// Interaction logic for ProgressBar.xaml
    /// </summary>
    public partial class ProgressBarDialog : Window
    {
        public ProgressBarDialog()
        {
            InitializeComponent();
        }
         
        public void UpdateBarAndText(int value, string text)
        {
            ProgressBarField.Value = value;
            ProgressText.Text = text;
        }

        public void UpdateBar(int value)
        {
            ProgressBarField.Value = value;
        }

        public void UpdateText(string text)
        {
            ProgressText.Text = text;
        }
    }
}
