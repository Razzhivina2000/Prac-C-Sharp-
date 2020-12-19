using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lab3
{
    class V1DataOnGrid : V1Data, IEnumerable<DataItem>
    {
        public Grid grid { get; set; }
        public Vector3[] values { get; set; }
        public V1DataOnGrid(string info, DateTime date, Grid grid) : base(info, date)
        {
            this.grid = grid;
            values = new Vector3[grid.count];
        }
        public V1DataOnGrid(string filename) : base("", new DateTime())
        { 
            /*
             Первая строка
             (Информация) (дата) (параметры для сетки)
             Компоненты вектора для каждого узла сетки через пробел на отдельной строке

             information 5/1/2020 8:30:40 AM 0 5 3
             0 0 0
             14 15 6
             1 2 3
             */

            string str;
            try
            {
                using (var sr = new StreamReader(filename))
                {
                    str = sr.ReadLine();
                    string[] args = str.Split(' ');
                    info = args[0];
                    string dateString = args[1] + " " + args[2] + " " + args[3]; // "5/1/2008 8:30:52 AM";
                    date = DateTime.Parse(dateString,
                                              System.Globalization.CultureInfo.InvariantCulture);
                    grid = new Grid(float.Parse(args[4]), float.Parse(args[5]), Int32.Parse(args[6]));
                    values = new Vector3[grid.count];
                    for (int i = 0; i < grid.count; ++i)
                    {
                        str = sr.ReadLine();
                        string[] val = str.Split(' ');
                        values[i] = new Vector3(float.Parse(val[0]), float.Parse(val[1]), float.Parse(val[2]));
                    }
                }
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
            string ans = ToString() + "\n";
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
