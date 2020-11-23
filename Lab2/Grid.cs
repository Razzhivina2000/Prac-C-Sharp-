using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;

namespace Lab1
{
    public struct Grid
    {
        public float t_begin { get; set; }
        public float t_step { get; set; }
        public int count { get; set; }
        public Grid(float t_begin, float t_step, int count)
        {
            this.t_begin = t_begin;
            this.t_step = t_step;
            this.count = count;
        }
        public override string ToString() => $"{t_begin} {t_step} {count}";
        public string ToString(string format) => $"{t_begin.ToString(format)} {t_step.ToString(format)} {count}";
    }
}
