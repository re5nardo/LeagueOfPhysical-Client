using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        var component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }

        return component;
    }

    public static bool TryParse<T>(this Enum source, out T result) where T : struct
    {
        return Enum.TryParse(source.ToString(), out result);
    }

    public static Vector3 Forward(this Vector3 source)
    {
        float x = Mathf.Sin(Mathf.Deg2Rad * source.y);
        float z = Mathf.Cos(Mathf.Deg2Rad * source.y);

        return new Vector3(x, 0, z);
    }

    public static StringBuilder AppendTab(this StringBuilder stringBuilder)
    {
        return stringBuilder.Append("\t");
    }
}
