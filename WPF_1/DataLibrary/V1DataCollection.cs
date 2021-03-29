using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DataLibrary
{
    [Serializable]
    public class V1DataCollection : V1Data, IEnumerable<DataItem>
    {
        public List<DataItem> DataItemlist { get; set; }
        public V1DataCollection(string info, DateTime date) : base(info, date)
        {
            DataItemlist = new List<DataItem>();
        }

        public void Save(string filename)
        {
            FileStream fileStream = null;
            try
            {
                fileStream = File.Create(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(fileStream, this);
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
            V1DataCollection res = null;
            try
            {
                fileStream = File.OpenRead(filename);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                res = binaryFormatter.Deserialize(fileStream) as V1DataCollection;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Load\n " + ex.Message);
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }
            DataItemlist = res.DataItemlist;
            info = res.info;
            date = res.date;
        }
        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            return DataItemlist.GetEnumerator();
        }
        public IEnumerator GetEnumerator()
        {
            return DataItemlist.GetEnumerator();
        }

        public void InitRandom(int nItems, float tmin, float tmax, float minValue, float maxValue)
        {
            Random rand = new Random();

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
        public override string ToLongString(string format)
        {
            string ans = this.ToString() + "\n";
            foreach (DataItem value in DataItemlist)
            {
                ans += $"{value.ToString(format)}\n";
            }
            return ans;
        }
    }
}