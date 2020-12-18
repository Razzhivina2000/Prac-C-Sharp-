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
            //Console.WriteLine("Start");
            V1DataOnGrid Obj1 = new V1DataOnGrid("input.txt");
            Console.WriteLine(Obj1.ToLongString("G"));

            V1MainCollection Obj2 = new V1MainCollection();
            Obj2.AddDefaults();
            Console.WriteLine(Obj2.ToLongString("G"));

            Console.WriteLine($"MaxLength - {Obj2.MaxLength}");
            Console.WriteLine($"MaxLengthDataItem - {Obj2.MaxLengthDataItem.ToString("G")}");
            string ans = "MoreOftenT:";

            foreach (float t in Obj2.MoreOftenT)
            {
                ans += t + " ";
            }
            Console.WriteLine(ans);
        }
    }
}
