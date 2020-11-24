﻿using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lab1
{
    class V1DataOnGrid : V1Data, IEnumerable<DataItem>
    {
        public Grid grid { get; set; }
        public Vector3[] values { get; set; }
        public V1DataOnGrid(string info, DateTime date, Grid grid) : base(info, date)
        {
            this.grid = grid;
        }
        public V1DataOnGrid(string filename) : base("", new DateTime())
        {
            string str;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    str = sr.ReadLine();
                }
                string[] values = str.Split(' ');
                info = values[0];
                string dateString = values[1] + " " + values[2] + " " + values[3]; // "5/1/2008 8:30:52 AM";
                date = DateTime.Parse(dateString,
                                          System.Globalization.CultureInfo.InvariantCulture);
                grid = new Grid(float.Parse(values[4]), float.Parse(values[5]), Int32.Parse(values[6]));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            return (IEnumerator<DataItem>)GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < values.Length; i++)
            {
                DataItem tmp = new DataItem(grid.t_begin + i * grid.t_step, values[i]);
                yield return tmp;
            }
        }

        public void InitRandom(float minValue, float maxValue)
        {
            Random rand = new Random();
            values = new Vector3[grid.count];
            for (int i = 0; i < values.Length; i++)
            {
                float rand_x = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                float rand_y = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                float rand_z = (float)rand.NextDouble() * (maxValue - minValue) + minValue;
                values[i] = new Vector3(rand_x, rand_y, rand_z);
            }
        }
        public static explicit operator V1DataCollection(V1DataOnGrid data)
        {
            V1DataCollection collection = new V1DataCollection(data.info, data.date);
            collection.DataItemlist = new List<DataItem>();
            for (int i = 0; i < data.values.Length; i++)
            {
                DataItem tmp = new DataItem(data.grid.t_begin + i * data.grid.t_step, data.values[i]);
                collection.DataItemlist.Add(tmp);
            }
            return collection;
        }
        public override float[] NearZero(float eps)
        {
            int k = 0;
            int i;
            for (i = 0; i < values.Length; i++)
            {
                if (values[i].Length() < eps)
                {
                    k++;
                }
            }
            float[] NearZero_array = new float[k];
            i = 0;
            int j = 0;
            while (k > 0)
            {
                if (values[i].Length() < eps)
                {
                    NearZero_array[j] = grid.t_begin + i * grid.t_step;
                    j++;
                    k--;
                }
                i++;
            }
            return NearZero_array;
        }
        public override string ToString() => $"V1DataOnGrid {base.ToString()} {grid.ToString()}";
        public override string ToLongString()
        {
            string ans = this.ToString() + "\n";
            for (int i = 0; i < values.Length; i++)
            {
                ans += $"{grid.t_begin + i * grid.t_step} {values[i]}\n";
            }
            return ans;
        }
        public override string ToLongString(string format)
        {
            string ans = $"V1DataOnGrid {base.ToString()} {grid.ToString(format)}" + "\n";
            for (int i = 0; i < values.Length; i++)
            {
                ans += $"{(grid.t_begin + i * grid.t_step).ToString(format)} {values[i].ToString(format)} {values[i].Length().ToString(format)}\n";
            }
            return ans;
        }

    }
}
