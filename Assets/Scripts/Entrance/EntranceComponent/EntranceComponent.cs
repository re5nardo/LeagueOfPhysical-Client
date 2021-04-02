using System.Collections;
using UnityEngine;

public abstract class EntranceComponent : MonoBehaviour, IEnumerator
{
    public object Current { get; protected set; }
    protected bool IsSuccess { get; set; }

    public bool MoveNext()
    {
        OnUpdate();

        return !IsSuccess;
    }

    public void Reset()
    {
        Current = null;
        IsSuccess = false;
    }

    public EntranceComponent Do()
    {
        OnStart();

        return this;
    }

    public abstract void OnStart();
    protected virtual void OnUpdate() { }

}
