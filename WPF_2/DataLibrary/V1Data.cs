using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataLibrary
{
    [Serializable]
    public abstract class V1Data : INotifyPropertyChanged
    {
        string Info;
        DateTime Date;
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string property_name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property_name));
        }
        public string info
        {
            get { return Info; }
            set { Info = value;
                OnPropertyChanged(Info);
            }
        }
        public DateTime date
        {
            get { return Date; }
            set { Date = value;
                OnPropertyChanged(Date.ToString());
            }
        }
        public V1Data(string i, DateTime d)
        {
            info = i;
            date = d;
        }
        
        public abstract float[] NearZero(float eps);
        public abstract string ToLongString();
        public abstract string ToLongString(string format);
        public override string ToString() => $"{info} {date.ToShortDateString()}";
    }
}
