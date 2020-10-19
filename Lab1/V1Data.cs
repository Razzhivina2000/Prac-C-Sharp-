using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    public abstract class V1Data
    {
        public string info { get; set; }
        public DateTime date { get; set; }
        public V1Data(string info, DateTime date)
        {
            this.info = info;
            this.date = date;
        }
        public abstract float[] NearZero(float eps);
        public abstract string ToLongString();
        public override string ToString() => $"{info} {date.ToShortDateString()}";
    }
}
