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
            V1DataOnGrid Obj1 = new V1DataOnGrid("input.txt");
            Console.WriteLine(Obj1.ToLongString());
            
        }
    }
}
