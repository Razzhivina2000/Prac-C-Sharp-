using System;
using System.Windows;
using System.Windows.Data;
using DataLibrary;
using Microsoft.Win32;
using System.Numerics;
using System.ComponentModel;
using Grid = DataLibrary.Grid;
using System.Windows.Input;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        V1MainCollection collection = new V1MainCollection();
        Custom_Collection custom_collection;

        public static RoutedCommand AddCommand = new RoutedCommand("Add", typeof(WpfApp2.MainWindow));

        public MainWindow()
        {
            InitializeComponent();
            DataContext = collection;
            custom_collection = new Custom_Collection(collection);
            text_Box.DataContext = custom_collection;
        }

        private void FilterByDatacollection(object sender, FilterEventArgs e)
        {

            if (e.Item is V1DataCollection)
            {
                e.Accepted = true;
            }
            else
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
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    Save();
                }
            }
        }

        void Save()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "All(*.*)|*.*";
            dlg.FilterIndex = 2;
            dlg.OverwritePrompt = false;
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

        private void MenuItem_Click_Add_Default(object sender, RoutedEventArgs e)
        {
            collection.AddDefaults();
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
                }
                else
                {
                    MessageBox.Show("File was not selected!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Read Error: " + ex.Message);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            check();
        }

        private void MenuItem_Click_Add_Custom(object sender, RoutedEventArgs e)
        {
            custom_collection.Add();
        }

        private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e)
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

        private void CanSaveCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (collection.Changed_not_save == true)
            {
                e.CanExecute = true;
            } else
            {
                e.CanExecute = false;
            }
        }

        private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            Save();
        }

        private void CanDeleteCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            V1Data tmp = (V1Data)lisBox_Main.SelectedItem;
            if (tmp != null)
            {
                e.CanExecute = true;
            }
            else
            {
                e.CanExecute = false;
            }
        }

        private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            V1Data tmp = (V1Data)lisBox_Main.SelectedItem;
            collection.Remove(tmp.info, tmp.date);
        }
        
        private void CanAddCommandHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (custom_collection != null)
            {
                if (Validation.GetHasError(text_Box_Info) == true
                    || Validation.GetHasError(text_Box_Count) == true
                    || Validation.GetHasError(text_Box_MinValue) == true
                    || Validation.GetHasError(text_Box_MaxValue) == true)
                {
                    e.CanExecute = false;
                }
                else
                {
                    e.CanExecute = true;
                }
            } else
            {
                e.CanExecute = false;
            }

        }

        private void AddCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            custom_collection.Add();
        }

    }

    public class Custom_Collection : INotifyPropertyChanged, IDataErrorInfo
    {
        public event PropertyChangedEventHandler PropertyChanged;
        string info = "";
        int count;
        float minValue;
        float maxValue;
        public string Info
        {
            get
            {
                return info;
            }
            set
            {
                info = value;
                OnPropertyChanged("Info");
            }
        }
        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }

        public float MinValue
        {
            get
            {
                return minValue;
            }
            set
            {
                minValue = value;
                OnPropertyChanged("MinValue");
                OnPropertyChanged("MaxValue");
            }
        }

        public float MaxValue
        {
            get
            {
                return maxValue;
            }
            set
            {
                maxValue = value;
                OnPropertyChanged("MaxValue");
                OnPropertyChanged("MinValue");
            }
        }

        V1MainCollection collection = new V1MainCollection();
        public Custom_Collection(V1MainCollection new_collection)
        {
            collection = new_collection;
        }

        public void Add()
        {
            V1DataOnGrid tmp = new V1DataOnGrid(Info, DateTime.Now, new Grid(0, 5, Count));
            tmp.InitRandom(MinValue, MaxValue);
            collection.Add(tmp);
            OnPropertyChanged("Info");
        }

        public string Error { get { return "Error!"; } }

        public string this[string property]
        {
            get
            {
                string msg = null;
                switch (property)
                {
                    case "Info":
                        bool p = false;
                        foreach(V1Data elem in collection)
                        {
                            if(elem.info == Info)
                            {
                                p = true;
                                break;
                            }
                        }
                        if (p) msg = "Info already in collection";
                        if (Info.Length <= 0) msg = "Info is empty";
                        break;
                    case "Count":
                        if (Count > 2) msg = "Count must be less or equal 2";
                        break;
                    case "MinValue":
                        if (minValue >= maxValue) msg = "MinValue must be less MaxValue";
                        break;
                    case "MaxValue":
                        if (minValue >= maxValue) msg = "MinValue must be less MaxValue";
                        break;
                    default:
                        break;
                }
                return msg;
            }
        }

        void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
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
