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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SpeedReading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SpeedReaderControl reader;
        public MainWindow()
        {
            InitializeComponent();

            //Testing test = new Testing();

            //theGrid.Children.Add(test);

            reader = new SpeedReaderControl();

            theGrid.Children.Add(reader);
        }

        private void cmdStart_Click(object sender, RoutedEventArgs e)
        {
            reader.Start();
        }

        
    }

    
}
