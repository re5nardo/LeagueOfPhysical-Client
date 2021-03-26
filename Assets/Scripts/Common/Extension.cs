using UnityEngine;
using System;
using System.Collections.Generic;

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
}
