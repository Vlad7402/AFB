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
using AFB.logic;

namespace AFB.Desktop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BTCount.Click += Count;
        }

        private void Count(object sender, RoutedEventArgs e)
        {
            var xStart = TBXStart.Text;
            var xEnd = TBXEnd.Text;
            var xStep = TBXStep.Text;
            var function = TBFunction.Text;
            
        }
    }
}
