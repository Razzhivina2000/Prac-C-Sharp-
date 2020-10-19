using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("dgs\n\n\n\n\n");
            string info1 = "information1";
            DateTime date = DateTime.Now;
            Grid grid = new Grid { t_begin = 0f, t_step = 5f, count = 10 };
            float minValue1 = -10f;
            float maxValue1 = 10f;
            V1DataOnGrid Obj1 = new V1DataOnGrid(info1, date, grid);
            Obj1.InitRandom(minValue1, maxValue1);
            Console.WriteLine(Obj1.ToLongString());
            V1DataCollection Obj2 = (V1DataCollection)Obj1;
            Console.WriteLine(Obj2.ToLongString());
            V1MainCollection Obj3 = new V1MainCollection();
            Obj3.AddDefaults();
            Console.WriteLine(Obj3.ToString());
            foreach (V1Data value in Obj3)
            {
                Console.WriteLine(value.ToLongString());
                float[] array = value.NearZero(10f);
                if (array.Length == 0)
                {
                    Console.WriteLine("empty");
                }
                else
                {
                    foreach (float x in array)
                    {
                        Console.WriteLine(x);
                    }

                }
            }
        }
    }
}
