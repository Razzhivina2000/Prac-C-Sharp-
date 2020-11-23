using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    public struct DataItem
    {
        public float t { get; set; }
        public Vector3 vec { get; set; }
        public DataItem(float t, Vector3 vec)
        {
            this.t = t;
            this.vec = vec;
        }
        public override string ToString() => $"{t} {vec.ToString()}";
        public string ToString(string format) => $"{t.ToString(format)} {vec.ToString(format)} {vec.Length().ToString(format)}";
    }
}
