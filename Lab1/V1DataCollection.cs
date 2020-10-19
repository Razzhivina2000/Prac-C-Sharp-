using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    class V1DataCollection : V1Data
    {
        public List<DataItem> DataItemlist { get; set; }
        public V1DataCollection(string info, DateTime date) : base(info, date) { }
        public void InitRandom(int nItems, float tmin, float tmax, float minValue, float maxValue)
        {
            Random rand = new Random();
            DataItemlist = new List<DataItem>();
            for (int i = 0; i < nItems; i++)
            {
                float rand_t = (float)rand.NextDouble() * (tmax - tmin) + tmin;
                float rand_x = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                float rand_y = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                float rand_z = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                DataItem tmp = new DataItem(rand_t, new Vector3(rand_x, rand_y, rand_z));
                DataItemlist.Add(tmp);
            }
        }
        public override float[] NearZero(float eps)
        {
            int k = 0;
            foreach (DataItem value in DataItemlist)
            {
                if (value.vec.Length() < eps)
                {
                    k++;
                }
            }
            float[] NearZero_array = new float[k];
            int i = 0;
            foreach (DataItem value in DataItemlist)
            {
                if (value.vec.Length() < eps)
                {
                    NearZero_array[i] = value.t;
                    k--;
                    i++;
                    if (k == 0)
                    {
                        break;
                    }
                }
            }
            return NearZero_array;
        }
        public override string ToString() => $"V1DataCollection {base.ToString()} {DataItemlist.Count}";
        public override string ToLongString()
        {
            string ans = this.ToString() + "\n";
            foreach (DataItem value in DataItemlist)
            {
                ans += $"{value.ToString()}\n";
            }
            return ans;
        }
    }
}