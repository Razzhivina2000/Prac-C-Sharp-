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
        private V1DataCollection V1DataToV1DataCollection(V1Data val)
        {
            if(val is V1DataOnGrid)
            {
                return (V1DataCollection)(val as V1DataOnGrid);
            }
            return val as V1DataCollection;
        }
        public float MaxLength
        {
            get
            {
                var tmp1 = V1Datalist.Select(V1DataToV1DataCollection).Select(v => v.DataItemlist);
                var tmp2 = from a in tmp1 from tmp in a select tmp;
                return tmp2.Max(tmp => tmp.vec.Length());
            }
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
                var tmp1 = V1Datalist.Select(V1DataToV1DataCollection).Select(v => v.DataItemlist);
                var tmp2 = from a in tmp1 from tmp in a select tmp.t;
                
                return tmp2.Where(x => tmp2.Count(y => x == y) > 1).Distinct();
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

        public void Add(V1Data item)
        {
            V1Datalist.Add(item);
        }
        public bool Remove(string id, DateTime dateTime)
        {
            int ans = V1Datalist.RemoveAll(value => value.info == id && value.date == dateTime);
            return ans != 0;
        }
        public void AddDefaults()
        {
            Grid grid_0 = new Grid { t_begin = 0f, t_step = 0f, count = 0 };
            V1DataOnGrid tmp_0 = new V1DataOnGrid("information", DateTime.Now, grid_0);
            V1Datalist.Add(tmp_0);
            V1DataCollection tmp_1 = new V1DataCollection("information", DateTime.Now);
            V1Datalist.Add(tmp_1);
            Random rand = new Random();
            int k = 2;
            for (int i = 0; i < k; i++)
            {
                string info = "information";
                DateTime date = DateTime.Now;
                Grid grid = new Grid { t_begin = 0f, t_step = 5f, count = 2 };
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
                tmp2.InitRandom(2, rand_tmin, rand_tmax, rand_minValue2, rand_maxValue2);
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
                ans += $"{value.ToLongString(format)}\n";
            }
            return ans;
        }
    }
}
