using System;
using System.Collections.Generic;
using System.IO;
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
using DataLibrary;
using Microsoft.Win32;
using System.Numerics;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        V1MainCollection collection = new V1MainCollection();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = collection;
        }

        private void FilterByDatacollection(object sender, FilterEventArgs e)
        {
            
            if (e.Item is V1DataCollection)
            {
                e.Accepted = true;
            } else
            {
                e.Accepted = false;
            }
        }

        private void FilterByDataOnGrid(object sender, FilterEventArgs e)
        {
            if (e.Item is V1DataOnGrid)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = false;
            }
        }
        void check()
        {
            if (collection.Changed_not_save)
            {
                var res = System.Windows.Forms.MessageBox.Show("Data not saved. Save?", "Saving", System.Windows.Forms.MessageBoxButtons.YesNo);
                if(res == System.Windows.Forms.DialogResult.Yes)
                {
                    Save();
                }
            }
        }

        void Save()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All(*.*)|*.*";
            dlg.FilterIndex = 2;
            if (dlg.ShowDialog() == true)
            {
                collection.Save(dlg.FileName);
            }
        }
        private void MenuItem_Click_New(object sender, RoutedEventArgs e)
        {
            check();
            collection = new V1MainCollection();
            DataContext = collection;
        }

        private void MenuItem_Click_Open(object sender, RoutedEventArgs e)
        {
            check(); 
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "All(*.*)|*.*";
            dlg.FilterIndex = 2;
            if (dlg.ShowDialog() == true)
            {
                collection.Load(dlg.FileName);            
            }
        }

        private void MenuItem_Click_Save(object sender, RoutedEventArgs e)
        {
            Save();
        }

        private void MenuItem_Click_Add_Default(object sender, RoutedEventArgs e)
        {
            collection.AddDefaults();
            //MessageBox.Show(collection.MaxLength.ToString());
        }

        private void MenuItem_Click_Add_Default_V1DataCollection(object sender, RoutedEventArgs e)
        {
            string info = "info";
            DateTime date = DateTime.Now;
            V1DataCollection tmp = new V1DataCollection(info, date);
            tmp.InitRandom(1, 0, 10, -10, 10);
            collection.Add(tmp);
        }

        private void MenuItem_Click_Add_Default_V1DataOnGrid(object sender, RoutedEventArgs e)
        {
            string info = "info";
            DateTime date = DateTime.Now;
            DataLibrary.Grid grid = new DataLibrary.Grid { t_begin = 0f, t_step = 5f, count = 2 };
            V1DataOnGrid tmp = new V1DataOnGrid(info, date, grid);
            tmp.InitRandom(-10, 10);
            collection.Add(tmp);
        }

        private void MenuItem_Click_Add_from_file(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Text files (*.txt)|*.txt|All(*.*)|*.*";
            dlg.FilterIndex = 2;
            try
            {
                if (dlg.ShowDialog() == true)
                {
                    V1DataOnGrid tmp = new V1DataOnGrid(dlg.FileName);
                    collection.Add(tmp);
                } else
                {
                    MessageBox.Show("File was not selected!");
                }
            } catch(Exception ex)
            {
                MessageBox.Show("Read Error: " + ex.Message);
            }
        }

        private void MenuItem_Click_Remove(object sender, RoutedEventArgs e)
        {
            V1Data tmp = (V1Data)lisBox_Main.SelectedItem;
            collection.Remove(tmp.info, tmp.date);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            check();
        }
    }

    public partial class Converter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            float v = (float)value;
            return $"{v} ";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }

    public partial class Converter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            Vector3 v = (Vector3)value;
            return $"{v.ToString()} {v.Length()}";
        }

        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
    public partial class Converter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            V1DataOnGrid v = (V1DataOnGrid)value;
            if (v != null && v.grid.count > 0)
            {
                return $"{v.grid.t_begin} {v.values[0]}";
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
    public partial class Converter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            V1DataOnGrid v = (V1DataOnGrid)value;
            if (v != null && v.grid.count > 0)
            {
                return $"{v.grid.t_begin + v.grid.t_step * (v.grid.count - 1)} {v.values[v.grid.count - 1]}";
            }
            else return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
