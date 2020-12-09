using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.IO.Compression;

public partial class Util
{
    public class Math
    {
        //	https://answers.unity.com/questions/1032673/how-to-get-0-360-degree-from-two-points.html
        public static float FindDegree(Vector2 vector2)
        {
            float degree = Mathf.Atan2(vector2.x, vector2.y) * Mathf.Rad2Deg;
            if (degree < 0)
            {
                degree += 360;
            }

            return degree;
        }

        public static Vector3 RotateClamp(Vector3 start, Vector3 angularVelocity, float elapsed, Vector3 maximum)
        {
            var rotated = start + angularVelocity * elapsed;

            var currentDegreesDelta = Vector3.Angle(Quaternion.Euler(start) * Vector3.forward, Quaternion.Euler(rotated) * Vector3.forward);
            var maxDegreesDelta = Vector3.Angle(Quaternion.Euler(start) * Vector3.forward, Quaternion.Euler(maximum) * Vector3.forward);

            if (currentDegreesDelta > maxDegreesDelta)
            {
                return maximum;
            }

            return rotated;
        }
    }
}
