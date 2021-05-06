using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class EntityTransformSynchronization : MonoComponentBase, ISynchronizable
{
    private const int MAX_BUFFER_COUNT = 5;

    #region ISynchronizable
    public ISynchronizable Parent { get; set; } = null;
    public bool Enable { get; set; } = true;
    public bool EnableInHierarchy => Parent == null ? Enable : Parent.EnableInHierarchy && Enable;
    public bool HasCoreChange => LastSendSnap == null ? true : !LastSendSnap.EqualsCore(CurrentSnap.Set(this));
    public bool IsDirty => isDirty || LastSendSnap == null ? true : !LastSendSnap.EqualsValue(CurrentSnap.Set(this));
    #endregion

    private bool isDirty = false;
    private EntityTransformSnap LastSendSnap { get; set; } = new EntityTransformSnap();
    private EntityTransformSnap CurrentSnap { get; set; } = new EntityTransformSnap();

    public List<EntityTransformSnap> entityTransformSnaps = new List<EntityTransformSnap>();

    public override void OnAttached(IEntity entity)
    {
        base.OnAttached(entity);

        MonoEntitySynchronization monoEntitySynchronization = Entity.GetEntityComponent<MonoEntitySynchronization>();
        monoEntitySynchronization?.Add(this);
    }

    public override void OnDetached()
    {
        base.OnDetached();

        MonoEntitySynchronization monoEntitySynchronization = Entity.GetEntityComponent<MonoEntitySynchronization>();
        monoEntitySynchronization?.Remove(this);
    }

    private void Reconcile(ISnap snap)
    {
    }

    #region ISynchronizable
    public void SetDirty()
    {
        isDirty = true;
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
        entityTransformSnaps.Add(snap as EntityTransformSnap);

        if (entityTransformSnaps.Count > MAX_BUFFER_COUNT)
        {
            entityTransformSnaps.RemoveRange(0, entityTransformSnaps.Count - MAX_BUFFER_COUNT);
        }

        Reconcile(snap);
    }
    #endregion
}
