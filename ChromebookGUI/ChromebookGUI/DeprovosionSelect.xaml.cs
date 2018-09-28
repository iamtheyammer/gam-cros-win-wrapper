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
    /// Interaction logic for DeprovosionSelect.xaml
    /// </summary>
    public partial class DeprovosionSelect : Window
    {
        public DeprovosionSelect()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) //cancel button click
        {
            sameModelReplacementRadio.IsChecked = false;
            retiringDeviceRadio.IsChecked = false;
            differentModelReplacementRadio.IsChecked = false;
            return;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void sameModelReplacementRadio_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
