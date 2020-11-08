using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class MonoEntitySynchronization : MonoComponentBase, ISynchronizableComposite
{
    #region ISynchronizable
    public ISynchronizable Parent { get; set; } = null;
    public List<ISynchronizable> Children { get; } = new List<ISynchronizable>();
    public bool Enable { get; set; } = true;
    public bool EnableInHierarchy => Parent == null ? Enable : Parent.EnableInHierarchy && Enable;
    public bool HasCoreChange => LastSendSnap == null ? true : !LastSendSnap.EqualsCore(CurrentSnap.Set(this));
    public bool IsDirty => isDirty || LastSendSnap == null ? true : !LastSendSnap.EqualsValue(CurrentSnap.Set(this));
    #endregion

    private bool isDirty = false;
    private MonoEntitySnap LastSendSnap { get; set; } = new MonoEntitySnap();
    private MonoEntitySnap CurrentSnap { get; set; } = new MonoEntitySnap();

    #region ISynchronizable
    public void SetDirty()
    {
        isDirty = true;

        Children.ForEach(child => child.SetDirty());
    }

    public ISnap GetSnap()
    {
        throw new NotImplementedException();
    }

    public void UpdateSynchronizable()
    {
        throw new NotImplementedException();
    }

    public void SendSynchronization()
    {
        throw new NotImplementedException();
    }

    public void OnReceiveSynchronization(ISnap snap)
    {
        MonoEntitySnap monoEntitySnap = snap as MonoEntitySnap;

        SC_SynchronizationHandler.Handle(monoEntitySnap.snaps);
    }

    public void Add(ISynchronizable child)
    {
        Children.Add(child);

        child.Parent = this;
    }

    public void Remove(ISynchronizable child)
    {
        Children.Remove(child);

        child.Parent = null;
    }
    #endregion
}
