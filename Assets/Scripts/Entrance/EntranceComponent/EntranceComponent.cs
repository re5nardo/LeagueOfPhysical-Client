using System.Collections;
using UnityEngine;
using System;

public abstract class EntranceComponent : MonoBehaviour, IEnumerator
{
    public object Current { get; protected set; }
    protected bool IsSuccess { get; set; }

    protected Action<string> logger;

    public bool MoveNext()
    {
        return !IsSuccess;
    }

    public void Reset()
    {
        Current = null;
        IsSuccess = false;
    }

    public EntranceComponent Do(Action<string> logger = null)
    {
        this.logger = logger;

        OnStart();

        return this;
    }

    public abstract void OnStart();
}
