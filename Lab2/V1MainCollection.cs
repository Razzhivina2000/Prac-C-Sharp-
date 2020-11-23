using System;
using System.Numerics;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    class V1MainCollection : IEnumerable<V1Data>
    {
        List<V1Data> V1Datalist = new List<V1Data>();
        public int Count
        {
            get { return V1Datalist.Count; }
        }
        float MaxLength
        {
            get
            { 
                return V1Datalist.Max(v => v.Max(a => a.vec.Length())).vec.Length();
            }
        }
        float MaxLengthDataItem
        {
            get
            {
                return V1Datalist.Max(v => v.Max(a => a.vec.Length()));
            }
        }
        /*IEnumerable<float> MoreOftenT
        {
            get
            {
                return V1Datalist.Max(v => v.Max(a => a.vec.Length()));
            }
        }*/

        IEnumerator<V1Data> IEnumerable<V1Data>.GetEnumerator()
        {
            return V1Datalist.GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            return V1Datalist.GetEnumerator();
        }

        public void Add(V1Data item)
        {
            V1Datalist.Add(item);
        }
        public bool Remove(string id, DateTime dateTime)
        {
            bool ans = false;
            foreach (V1Data value in V1Datalist)
            {
                if (value.info == id && value.date == dateTime)
                {
                    V1Datalist.Remove(value);
                }
            }
            return ans;
        }
        public void AddDefaults()
        {
            Random rand = new Random();
            int k = 3;
            for (int i = 0; i < k; i++)
            {
                string info = "information";
                DateTime date = DateTime.Now;
                Grid grid = new Grid { t_begin = 0f, t_step = 5f, count = 10 };
                float rand_minValue = (float)(-rand.NextDouble() * 20f);
                float rand_maxValue = (float)(rand.NextDouble() * 30f);
                V1DataOnGrid tmp = new V1DataOnGrid(info, date, grid);
                tmp.InitRandom(rand_minValue, rand_maxValue);
                V1Datalist.Add(tmp);
                V1DataCollection tmp2 = new V1DataCollection(info, date);
                float rand_minValue2 = (float)(-rand.NextDouble() * 20f);
                float rand_maxValue2 = (float)(rand.NextDouble() * 30f);
                float rand_tmin = (float)(rand.NextDouble() * 10f);
                float rand_tmax = rand_tmin + (float)(rand.NextDouble() * 30f);
                tmp2.InitRandom(10, rand_tmin, rand_tmax, rand_minValue2, rand_maxValue2);
                V1Datalist.Add(tmp2);
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
                ans += $"{value.ToLongString()}\n";
            }
            return ans;
        }
    }
}
