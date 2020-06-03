using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GlobalMonoBehavior : MonoSingleton<GlobalMonoBehavior>
{
    public new static Coroutine StartCoroutine(IEnumerator routine)
    {
        return  (Instance as MonoBehaviour).StartCoroutine(routine);
    }

    public new static void StopCoroutine(IEnumerator routine)
    {
        (Instance as MonoBehaviour).StopCoroutine(routine);
    }
}
