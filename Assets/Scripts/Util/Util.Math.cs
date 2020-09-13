using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Compression;

public partial class Util
{
    public class Math
    {
        public static float Constant_E = 2.718282f;

        //	https://answers.unity.com/questions/1032673/how-to-get-0-360-degree-from-two-points.html
        public static float FindDegree(Vector2 vec2Position)
        {
            float value = (float)((Mathf.Atan2(vec2Position.x, vec2Position.y) / Mathf.PI) * 180f);
            if (value < 0)
                value += 360f;

            return value;
        }
    }
}
