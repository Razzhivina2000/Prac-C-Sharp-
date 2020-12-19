using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Start");
            V1DataOnGrid Obj1 = new V1DataOnGrid("input.txt");

            V1MainCollection Obj2 = new V1MainCollection();
            Obj2.DataChanged += DataChangedInList;
            Obj2.AddDefaults();
            Obj2.Add(Obj1);
            Obj2[2] = Obj1;
            Obj1.info = "new_information";
            Obj2.Remove(Obj1.info, Obj1.date);
            
        }
        static void DataChangedInList(object source, DataChangedEventArgs args)
        {
            Console.WriteLine(args.ToString());
        }

    }

    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);
}
