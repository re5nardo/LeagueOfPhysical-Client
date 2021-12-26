using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using GameFramework;

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

    public static byte[] ObjectToHash(this object obj)
    {
        if (obj == null) throw new Exception("obj is null!");

        using (var memoryStream = new MemoryStream())
        {
            Util.Formatter.Serialize(memoryStream, obj);

            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(memoryStream.ToArray());
            }
        }
    }

    public static float GameTime(this SyncDataEntry value)
    {
        return value.meta.tick * Game.Current.TickInterval;
    }
}
