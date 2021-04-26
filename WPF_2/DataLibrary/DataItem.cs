using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DataLibrary
{
    [Serializable]
    public struct DataItem: ISerializable
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

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("X", vec.X, typeof(float));
            info.AddValue("Y", vec.Y, typeof(float));
            info.AddValue("Z", vec.Z, typeof(float));
            info.AddValue("t", t, typeof(float));
        }

        public DataItem(SerializationInfo info, StreamingContext context)
        {
            float x = info.GetSingle("X");
            float y = info.GetSingle("Y");
            float z = info.GetSingle("Z");
            vec = new Vector3(x, y, z);
            t = info.GetSingle("t");
        }

    }
}
