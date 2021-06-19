using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AFB.Desktop
{
    /// <summary>
    /// Логика взаимодействия для TableView.xaml
    /// </summary>
    public partial class TableView : Window
    {
        public TableView(logic.Table table)
        {
            //не получилось сделать скрол
            InitializeComponent();
            var sourse = new List<TableString>();
            for (int i = 0; i < table.Lenght; i++)         
                sourse.Add(new TableString(table.Arguments[i], table.Values[i]));

            LVTable.ItemsSource = sourse.ToArray();
        }
        private class TableString
        {
            public string Argument { get; private set; }
            public string Value { get; private set; }
            public TableString(double argument, double value)
            {
                Argument = argument.ToString();
                Value = value.ToString();
            }
        }
    }

}
