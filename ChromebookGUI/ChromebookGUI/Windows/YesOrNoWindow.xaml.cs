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
    /// Interaction logic for YesOrNoWindow.xaml
    /// </summary>
    public partial class YesOrNoWindow : Window
    {
        public YesOrNoWindow()
        {
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            instructionTextBlock.Text = "yes";
            Close();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            instructionTextBlock.Text = "no";
            Close();
        }

        private void ExtraButton_Click(object sender, RoutedEventArgs e)
        {
            instructionTextBlock.Text = "extraButtonClicked";
            Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
