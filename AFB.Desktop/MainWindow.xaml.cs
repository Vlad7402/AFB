﻿using System;
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
        private bool isGrapticAvailable;
        private double zoomX = 1;
        private double zoomY = 1;
        private double zoomMain = 1;
        private double maxX = 0;
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
            try
            {
                var files = new Files(this);
                var xStart = files.DoubleParser(strXStart);
                var xEnd = files.DoubleParser(strXEnd);
                var xStep = files.DoubleParser(strXStep);
                files.SaveExpression(xStart, xEnd, xStep, function);
                var expression = new logic.Expression(this);
                ShowFunctionInformation(expression);
                var valuemsOfY = expression.ValuemsOfY;
                if (valuemsOfY.Length > 1)
                {
                    isGrapticAvailable = true;
                    RedrawGraphic();
                }
            }              
            catch (Exception exeption)
            {
                PrintError(exeption.Message);
            }
        }

        public void PrintError(string errorText)
        {
            TBError.Visibility = Visibility.Visible;
            BTClearError.Visibility = Visibility.Visible;
            TBError.Text = errorText;
        }
        private void ShowFunctionInformation(logic.Expression expression)
        {
            var yValuems = expression.ValuemsOfY;
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
            GetExpression(sender, e);
        }

        private void RedrawGraphic()
        {
            CanGraphicFild.Children.Clear();
            Polygon whiteBackGround = new Polygon();
            whiteBackGround.Points.Add(new Point(CanGraphicFild.ActualWidth - 2, CanGraphicFild.ActualHeight - 2));
            whiteBackGround.Points.Add(new Point(1, CanGraphicFild.ActualHeight - 2));
            whiteBackGround.Points.Add(new Point(1, 1));
            whiteBackGround.Points.Add(new Point(CanGraphicFild.ActualWidth - 2, 1));
            whiteBackGround.Fill = Brushes.White;
            CanGraphicFild.Children.Add(whiteBackGround);
            var xAsic = new Line
            {
                X1 = 0,
                X2 = CanGraphicFild.ActualWidth,
                Y1 = CanGraphicFild.ActualHeight / 2
            };
            xAsic.Y2 = xAsic.Y1;
            xAsic.Stroke = Brushes.Black;
            xAsic.StrokeThickness = 1;

            var yAsic = new Line
            {
                X1 = CanGraphicFild.ActualWidth / 2,
                X2 = CanGraphicFild.ActualWidth / 2,
                Y1 = CanGraphicFild.ActualHeight,
                Y2 = 0,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            var xTriangle = new Polygon();
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth, CanGraphicFild.ActualHeight / 2));
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth - 10, CanGraphicFild.ActualHeight / 2 + 5));
            xTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth - 10, CanGraphicFild.ActualHeight / 2 - 5));
            xTriangle.Fill = Brushes.Black;

            var yTriangle = new Polygon();
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2, 0));
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2 - 5, 10));
            yTriangle.Points.Add(new Point(CanGraphicFild.ActualWidth / 2 + 5, 10));
            yTriangle.Fill = Brushes.Black;

            var TBXLable = new TextBlock
            {
                Margin = new Thickness(CanGraphicFild.ActualWidth - 12, CanGraphicFild.ActualHeight / 2 + 6, 0, CanGraphicFild.ActualHeight / 2 + 18),
                Text = "X"
            };

            var TBYLable = new TextBlock
            {
                Margin = new Thickness(CanGraphicFild.ActualWidth / 2 - 12, 0, CanGraphicFild.ActualWidth / 2, CanGraphicFild.ActualHeight - 6),
                Text = "Y"
            };

            var TBZeroLable = new TextBlock
            {
                Margin = new Thickness(CanGraphicFild.ActualWidth / 2 - 10, CanGraphicFild.ActualHeight / 2 + 4, CanGraphicFild.ActualWidth / 2 + 4, CanGraphicFild.ActualHeight / 2 - 10),
                Text = "0"
            };

            var notches = GetNotches(CanGraphicFild);

            CanGraphicFild.Children.Add(xAsic);
            CanGraphicFild.Children.Add(yAsic);
            CanGraphicFild.Children.Add(xTriangle);
            CanGraphicFild.Children.Add(yTriangle);
            CanGraphicFild.Children.Add(TBXLable);
            CanGraphicFild.Children.Add(TBYLable);
            CanGraphicFild.Children.Add(TBZeroLable);
            foreach (var notch in notches)
                CanGraphicFild.Children.Add(notch);

            if (isGrapticAvailable)
            {
                var expression = new logic.Expression(this);

                var lables = GetLablesForNotches(CanGraphicFild, expression);

                foreach (var lable in lables)
                    CanGraphicFild.Children.Add(lable);

                var lines = GetGraphic(expression);
                foreach (var line in lines)
                {
                    line.Stroke = Brushes.Black;
                    line.StrokeThickness = 1;
                    CanGraphicFild.Children.Add(line);
                }
            }

        }
        private List<Line> GetNotches(Canvas grathicFild)
        {
            var notches = new List<Line>();
            for (int i = 1; i < 20; i++)
            {
                if (i == 10) continue;

                var xNotch = new Line
                {
                    Y1 = grathicFild.ActualHeight / 2 + 3,
                    Y2 = grathicFild.ActualHeight / 2 - 3,
                    X1 = grathicFild.ActualWidth / 20 * i
                };
                xNotch.X2 = xNotch.X1;
                xNotch.Stroke = Brushes.Black;
                xNotch.StrokeThickness = 1;
                notches.Add(xNotch);
            }
            for (int i = 1; i < 20; i++)
            {
                if (i == 10) continue;

                var yNotch = new Line
                {
                    X1 = grathicFild.ActualWidth / 2 + 3,
                    X2 = grathicFild.ActualWidth / 2 - 3,
                    Y1 = grathicFild.ActualHeight / 20 * i
                };
                yNotch.Y2 = yNotch.Y1;
                yNotch.Stroke = Brushes.Black;
                yNotch.StrokeThickness = 1;
                notches.Add(yNotch);
            }

            return notches;
        }
        private List<TextBlock> GetLablesForNotches(Canvas grathicFild, logic.Expression expression)
        {
            var lables = new List<TextBlock>();
            var maxX = GetAbsolutMax(expression.ValuemsOfX) / (zoomMain * zoomX);
            for (int i = 1; i < 20; i++)
            {
                if (i == 10) continue;

                var xValue = Math.Round(-(maxX-(maxX / 10 *i)), 3);

                var xLable = new TextBlock
                {
                    Margin = new Thickness(grathicFild.ActualWidth / 20 * i - 7, grathicFild.ActualHeight / 2 + 6, grathicFild.ActualWidth / 20 * (20 - i) - 7, grathicFild.ActualHeight / 2 - 12),
                    Text = xValue.ToString()
                };
                lables.Add(xLable);
            }

            var maxY = GetAbsolutMax(expression.ValuemsOfY) / (zoomMain * zoomY);

            for (int i = 1; i < 20; i++)
            {
                if (i == 10) continue;

                var yValue = Math.Round(((-maxY +(maxY / 10 * i))*-1), 3);

                var yLable = new TextBlock
                {
                    Margin = new Thickness(grathicFild.ActualWidth /2 + 6, grathicFild.ActualHeight / 20 * i - 6, grathicFild.ActualWidth / 2 - 15, grathicFild.ActualHeight / 20 * (20 - i) - 6),
                    Text = yValue.ToString()
                };
                lables.Add(yLable);
            }
            return lables;
        }

        private List<Polyline> GetGraphic(logic.Expression expression)
        {
            var pointsCollections = new List<PointCollection>();
            var valuemsOfX = expression.ValuemsOfX;
            var valuemsOfY = expression.ValuemsOfY;
            var maxX = GetAbsolutMax(valuemsOfX) / (zoomMain * zoomX);
            var maxY = GetAbsolutMax(valuemsOfY) / (zoomMain * zoomY);
            this.maxX = maxX;
            for (int i = 0; i < valuemsOfX.Length; i++)
            {
                var point = GetPointForGraphic(maxX, maxY, valuemsOfX[i], valuemsOfY[i]);
                if (point.X <= CanGraphicFild.ActualWidth && point.Y <= CanGraphicFild.ActualHeight &&
                    (point.X >= 0 && point.Y >=0))
                {
                    PointCollection points = new PointCollection();
                    while (point.X <= CanGraphicFild.ActualWidth && point.Y <= CanGraphicFild.ActualHeight &&
                    (point.X >= 0 && point.Y >= 0))
                    {
                        points.Add(point);
                        i++;
                        if (i == valuemsOfX.Length)
                            break;

                        point = GetPointForGraphic(maxX, maxY, valuemsOfX[i], valuemsOfY[i]);
                    }
                    pointsCollections.Add(points);
                }
                else
                {
                    while (point.X > CanGraphicFild.ActualWidth || point.Y > CanGraphicFild.ActualHeight ||
                        point.X < 0 || point.Y < 0)
                    {
                        i++;
                        if (i == valuemsOfX.Length)
                            break;

                        point = GetPointForGraphic(maxX, maxY, valuemsOfX[i], valuemsOfY[i]);
                    }
                }
            }
            var lines = new List<Polyline>();
            foreach (var points in pointsCollections)
                lines.Add(new Polyline { Points = points });

            return lines;
        }
        private Point GetPointForGraphic(double maxX, double maxY, double xValue, double yValue)
        {
            return new Point(GetCoordinateForPoint(CanGraphicFild.ActualWidth, maxX, xValue, zoomX), 
                             GetCoordinateForPoint(CanGraphicFild.ActualHeight, maxY, -yValue, zoomY));
        }
        private double GetCoordinateForPoint(double widthOrHeight, double maxValue, double currentValue, double zoom)
        {
            return ((currentValue / maxValue * widthOrHeight / 2) + widthOrHeight / 2);
        }
        private void SBZoomChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // В разработке
            zoomMain = Math.Round(SBZoomMain.Value, 1);
            TBzoomMain.Text = Math.Round(SBZoomMain.Value, 1).ToString();
            zoomX =  Math.Round(SBZoomX.Value, 1);
            TBzoomX.Text = Math.Round(SBZoomX.Value, 1).ToString();
            zoomY = Math.Round(SBZoomY.Value, 1);
            TBzoomY.Text = Math.Round(SBZoomY.Value, 1).ToString();
            GetExpression(null, null);
        }
        private double GetAbsolutMax(double[] input)
        {
            var max = input.Max();
            var min = Math.Abs(input.Min());
            if (min > max) return min;
            return max;
        }

        private void ShowCurrentCoordinte(object sender, MouseEventArgs e)
        {
            if (isGrapticAvailable)
            {
                var files = new Files(this);
                var coordinateX = ((e.GetPosition((IInputElement)sender).X - CanGraphicFild.ActualWidth / 2) 
                                    / CanGraphicFild.ActualWidth * maxX) * 2;
                files.SaveExpression(coordinateX, coordinateX + 0.5, 1, TBFunction.Text);
                try
                {
                    var expression = new logic.Expression(this);
                    TBCurrentX.Text = (expression.ValuemsOfX[0]).ToString();
                    TBCurrentY.Text = (expression.ValuemsOfY[0]).ToString();
                }
                catch (Exception)
                {
                    TBCurrentX.Text = "Err";
                    TBCurrentY.Text = "Err";
                }
            }          
        }

        private void ClearCurrentCoordinates(object sender, MouseEventArgs e)
        {
            TBCurrentX.Text = string.Empty;
            TBCurrentY.Text = string.Empty;
        }

        private void ShowTable(object sender, RoutedEventArgs e)
        {
            if (isGrapticAvailable)
            {
                GetExpression(null, null);
                var expression = new logic.Expression(this);
                var table = new TableView(new logic.Table(expression.ValuemsOfX, expression.ValuemsOfY));
                table.Show();
            }
        }
    }
}