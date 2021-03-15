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
        private void GraphicFildSizeChanged(object sender, SizeChangedEventArgs e)
        {
            RedrawGraphic();
        }

        private void RedrawGraphic()
        {
            CanGraphicFild.Children.Clear();

            var xAsic = new Line();
            xAsic.X1 = 0;
            xAsic.X2 = CanGraphicFild.ActualWidth;
            xAsic.Y1 = CanGraphicFild.ActualHeight / 2;
            xAsic.Y2 = xAsic.Y1;
            xAsic.Stroke = Brushes.Black;
            xAsic.StrokeThickness = 1;

            var yAsic = new Line();
            yAsic.X1 = CanGraphicFild.ActualWidth/2;
            yAsic.X2 = CanGraphicFild.ActualWidth /2;
            yAsic.Y1 = CanGraphicFild.ActualHeight;
            yAsic.Y2 = 0;
            yAsic.Stroke = Brushes.Black;
            yAsic.StrokeThickness = 1;

            var xTriangle = new Polygon();
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth, CanGraphicFild.ActualHeight / 2));
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth -10, CanGraphicFild.ActualHeight / 2 + 5));
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth - 10, CanGraphicFild.ActualHeight / 2 - 5));
            xTriangle.Fill = Brushes.Black;

            var yTriangle = new Polygon();
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2, 0));
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2 - 5,  10));
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2 + 5,  10));
            yTriangle.Fill = Brushes.Black;

            var TBXLable = new TextBlock();
            TBXLable.Margin = new Thickness(CanGraphicFild.ActualWidth - 12, CanGraphicFild.ActualHeight / 2 + 6, 0, CanGraphicFild.ActualHeight / 2 + 18);
            TBXLable.Text = "X";

            var TBYLable = new TextBlock();
            TBYLable.Margin = new Thickness(CanGraphicFild.ActualWidth/2 - 12, 0, CanGraphicFild.ActualWidth / 2, CanGraphicFild.ActualHeight - 6);
            TBYLable.Text = "Y";

            var TBZeroLable = new TextBlock();
            TBZeroLable.Margin = new Thickness(CanGraphicFild.ActualWidth / 2 - 10 , CanGraphicFild.ActualHeight / 2 + 4, CanGraphicFild.ActualWidth / 2 + 4, CanGraphicFild.ActualHeight / 2 - 10);
            TBZeroLable.Text = "0";

            CanGraphicFild.Children.Add(xAsic);
            CanGraphicFild.Children.Add(yAsic);
            CanGraphicFild.Children.Add(xTriangle);
            CanGraphicFild.Children.Add(yTriangle);
            CanGraphicFild.Children.Add(TBXLable);
            CanGraphicFild.Children.Add(TBYLable);
            CanGraphicFild.Children.Add(TBZeroLable);
        }
    }
}
