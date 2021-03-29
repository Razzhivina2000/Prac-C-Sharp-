using System;
using System.Numerics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataLibrary
{
    public delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

    [Serializable]
    public class V1MainCollection : IEnumerable<V1Data>, INotifyCollectionChanged, INotifyPropertyChanged
    {
        List<V1Data> V1Datalist = new List<V1Data>();
        public bool Changed_not_save { get; set; } = false;
        public int Count
        {
            get { return V1Datalist.Count; }
        }

        [field: NonSerialized]
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        [field: NonSerialized]
        public event DataChangedEventHandler DataChanged;

        public V1MainCollection()
        {
            CollectionChanged += CollectionChangedInList;
        }
        void CollectionChangedInList(object source, NotifyCollectionChangedEventArgs args)
        {
            if(!Changed_not_save)
            {
                Changed_not_save = true;
                OnPropertyChanged("Changed_not_save");
            }
            MaxLength = max_lenght();
            OnPropertyChanged("MaxLength");
        }
        void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
        void OnCollectionChanged(NotifyCollectionChangedAction act)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(act));
        }
        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, this.V1Datalist);
                if (Changed_not_save)
                {
                    Changed_not_save = false;
                    OnPropertyChanged("Changed_not_save");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Save\n " + ex.Message);
            }
            finally
            {
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
        }

        public void Load(string filename)
        {
            FileStream fileStream = null;
            List<V1Data> res = null;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                res = binaryFormatter.Deserialize(fileStream) as List<V1Data>;
                V1Datalist = res;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load\n " + ex.Message);
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        void OnDataChanged(ChangeInfo changeInfo, string message)
        {
            if (DataChanged != null)
            {
                DataChanged(this, new DataChangedEventArgs(changeInfo, message));
            }
        }

        public V1Data this[int index]
        {
            get { return V1Datalist[index]; }
            set
            {
                V1Datalist[index] = value;
                OnDataChanged(ChangeInfo.Replace, V1Datalist[index].info);
                OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            }
        }

        void ItemChangedInList(object source, PropertyChangedEventArgs args)
        {
            OnDataChanged(ChangeInfo.ItemChanged, args.PropertyName);
        }

        public void Add(V1Data item)
        {
            V1Datalist.Add(item);
            item.PropertyChanged += ItemChangedInList;
            OnDataChanged(ChangeInfo.Add, item.info);
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
        }

        public bool Remove(string id, DateTime dateTime)
        {
            foreach(V1Data v in V1Datalist)
            {
                if(v.info == id && v.date == dateTime)
                {
                    v.PropertyChanged -= ItemChangedInList;
                }
            }
            int ans = V1Datalist.RemoveAll(value => value.info == id && value.date == dateTime);
            OnDataChanged(ChangeInfo.Remove, id);
            OnCollectionChanged(NotifyCollectionChangedAction.Reset);
            return ans != 0;
        }

        private V1DataCollection V1DataToV1DataCollection(V1Data val)
        {
            if (val is V1DataOnGrid)
            {
                return (V1DataCollection)(val as V1DataOnGrid);
            }
            return val as V1DataCollection;
        }

        public float MaxLength 
        { 
            get 
            {
                return max_lenght();
            } 
            set {}
        }
        float max_lenght()
        {
            var tmp1 = V1Datalist.Select(V1DataToV1DataCollection).Select(v => v.DataItemlist);
            var tmp2 = from a in tmp1 from tmp in a select tmp;
            if (tmp2.Count() > 0)
                return tmp2.Max(tmp => tmp.vec.Length());
            return 0;
        }

        public DataItem MaxLengthDataItem
        {
            get
            {
                var tmp1 = V1Datalist.Select(V1DataToV1DataCollection).Select(v => v.DataItemlist);
                var tmp2 = from a in tmp1 from tmp in a select tmp;
                return tmp2.OrderBy(tmp => tmp.vec.Length()).Last();
            }
        }
        public IEnumerable<float> MoreOftenT
        {
            get
            {
                var tmp1 = from i in V1Datalist
                           select V1DataToV1DataCollection(i).DataItemlist;
                var tmp2 = from a in tmp1 from tmp in a select tmp.t;
                var tmp3 = from x in tmp2
                           where tmp2.Count(y => x == y) > 1
                           select x;
                return tmp3.Distinct();
            }
        }

        IEnumerator<V1Data> IEnumerable<V1Data>.GetEnumerator()
        {
            return V1Datalist.GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            return V1Datalist.GetEnumerator();
        }

        public void AddDefaults()
        {
            Grid grid_0 = new Grid { t_begin = 0f, t_step = 0f, count = 0 };
            V1DataOnGrid tmp_0 = new V1DataOnGrid("info", DateTime.Now, grid_0);
            Add(tmp_0);
            V1DataCollection tmp_1 = new V1DataCollection("info", DateTime.Now);
            Add(tmp_1);
            Random rand = new Random();
            int k = 2;
            for (int i = 0; i < k; i++)
            {
                string info = "info";
                DateTime date = DateTime.Now;
                Grid grid = new Grid { t_begin = 0f, t_step = 5f, count = 2 };
                float rand_minValue = (float)(-rand.NextDouble() * 20f);
                float rand_maxValue = (float)(rand.NextDouble() * 30f);
                V1DataOnGrid tmp = new V1DataOnGrid(info, date, grid);
                tmp.InitRandom(rand_minValue, rand_maxValue);
                Add(tmp);
                V1DataCollection tmp2 = new V1DataCollection(info, date);
                float rand_minValue2 = (float)(-rand.NextDouble() * 20f);
                float rand_maxValue2 = (float)(rand.NextDouble() * 30f);
                float rand_tmin = (float)(rand.NextDouble() * 10f);
                float rand_tmax = rand_tmin + (float)(rand.NextDouble() * 30f);
                tmp2.InitRandom(2, rand_tmin, rand_tmax, rand_minValue2, rand_maxValue2);
                Add(tmp2);
            }
        }
        public override string ToString()
        {
            string ans = "";
            foreach (V1Data value in V1Datalist)
            {
                ans += $"{value.ToString()}\n";
            }
            return ans;
        }
        public string ToLongString(string format)
        {
            string ans = "";
            foreach (V1Data value in V1Datalist)
            {
                ans += $"{value.ToLongString(format)}\n";
            }
            return ans;
        }
    }
}
