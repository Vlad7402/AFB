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
    public partial class MainWindow : Window, IWriter
    {
        public MainWindow()
        {
            InitializeComponent();
            BTCount.Click += GetExpression;
            BTClearError.Click += DeleteError;
        }

        private void GetExpression(object sender, RoutedEventArgs e)
        {
            var strXStart = TBXStart.Text;
            var strXEnd = TBXEnd.Text;
            var strXStep = TBXStep.Text;
            var function = TBFunction.Text;
            var files = new Files(this);
            var xStart = files.DoubleParser(strXStart);
            var xEnd = files.DoubleParser(strXEnd);
            var xStep = files.DoubleParser(strXStep);
            files.SaveExpression(xStart, xEnd, xStep, function);
            ShowFunctionInformation(new logic.Expression(this));

        }

        public void PrintError(string errorText)
        {
            TBError.Visibility = Visibility.Visible;
            BTClearError.Visibility = Visibility.Visible;
            TBError.Text = errorText;
        }
        private void ShowFunctionInformation(logic.Expression expression)
        {
            var yValuems = expression.GetValuemsOfY();
            TBYStart.Text = yValuems[0].ToString();
            TBYEnd.Text = yValuems[yValuems.Length - 1].ToString();
            TBRPNFunction.Text = expression.RPNExpression;
        }

        public void PrintTable(string[] table)
        {
            throw new NotImplementedException();          
        }
        private void DeleteError(object sender, RoutedEventArgs e)
        {
            TBError.Visibility = Visibility.Hidden;
            BTClearError.Visibility = Visibility.Hidden;
        }
    }
}
