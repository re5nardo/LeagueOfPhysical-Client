using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public abstract class LOPMonoEntitySyncControllerBase<T> : LOPMonoEntityComponentBase, ISyncController<T> where T : ISyncData
{
    public string ControllerId { get; private set; }
    public string OwnerId { get; private set; }
    public bool HasAuthority => OwnerId == LOP.Application.UserId || OwnerId == "local";
    public bool IsDirty { get; private set; }
    public virtual SyncScope SyncScope { get; protected set; } = SyncScope.Local;

    protected override void OnAttached(IEntity entity)
    {
        base.OnAttached(entity);

        OnInitialize();
    }

    protected override void OnDetached()
    {
        base.OnDetached();

        OnFinalize();
    }

    public virtual void OnInitialize()
    {
        ControllerId = $"{Entity.EntityID}_{GetType().Name}";
        OwnerId = Entity.OwnerId;

        SceneMessageBroker.AddSubscriber<SC_SyncController>(OnSyncController).Where(syncController => syncController.syncControllerData.controllerId == ControllerId);
        SceneMessageBroker.AddSubscriber<SC_Synchronization>(OnSynchronization).Where(synchronization => synchronization.syncDataEntry.meta.controllerId == ControllerId);
    }

    public virtual void OnFinalize()
    {
        if (SceneMessageBroker.HasInstance())
        {
            SceneMessageBroker.RemoveSubscriber<SC_SyncController>(OnSyncController);
            SceneMessageBroker.RemoveSubscriber<SC_Synchronization>(OnSynchronization);
        }
    }

    private void OnSyncController(SC_SyncController syncController)
    {
        OwnerId = syncController.syncControllerData.ownerId;
    }

    private void OnSynchronization(SC_Synchronization synchronization)
    {
        if (OwnerId != synchronization.syncDataEntry.meta.senderId)
        {
            Debug.LogWarning($"User (not owner) request synchronization. It is ignored. senderId: {synchronization.syncDataEntry.meta.senderId}, ownerId: {OwnerId}");
            return;
        }

        if (synchronization.syncDataEntry.meta.senderId == LOP.Application.UserId)
        {
            return;
        }

        OnSync(synchronization.syncDataEntry);
        OnSync((T)synchronization.syncDataEntry.data);
    }

    public abstract T GetSyncData();

    public SyncDataEntry GetSyncDataEntry()
    {
        var syncData = GetSyncData();

        return new SyncDataEntry
        {
            meta = new SyncDataMeta(Game.Current.CurrentTick, LOP.Application.UserId, ControllerId, syncData.ObjectToHash()),
            data = syncData,
        };
    }

    public SyncControllerData GetSyncControllerData()
    {
        return new SyncControllerData
        {
            controllerId = ControllerId,
            ownerId = OwnerId,
        };
    }

    public void SetDirty()
    {
        IsDirty = true;
    }

    public void Sync(T value)
    {
        if (!HasAuthority)
        {
            Debug.LogWarning("You must have authority to sync.");
            return;
        }

        //  request syncData to server
        var disposer = PoolObjectDisposer<CS_Synchronization>.Get();
        var synchronization = disposer.PoolObject;
        synchronization.syncDataEntry = new SyncDataEntry
        {
            meta = new SyncDataMeta(Game.Current.CurrentTick, LOP.Application.UserId, ControllerId, value.ObjectToHash()),
            data = value,
        };

        RoomNetwork.Instance.Send(synchronization, 0, instant: true);
    }
    
    public virtual void OnSync(T value) { }
    public virtual void OnSync(SyncDataEntry value) { }
}
