using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using NetworkModel.Mirror;

public abstract class LOPMonoSyncControllerBase<T> : LOPMonoEntityComponentBase, ISyncController<T> where T : ISyncData
{
    public int EntityId => Entity.EntityID;
    public string OwnerId { get; private set; }
    public bool HasAuthority => OwnerId == LOP.Application.UserId || OwnerId == "local";
    public bool IsDirty { get; private set; }

    protected override void OnAttached(IEntity entity)
    {
        OwnerId = Entity.OwnerId;

        SceneMessageBroker.AddSubscriber<SC_SyncController>(OnSyncController).Where(syncController => syncController.syncControllerData.type == GetType().Name && syncController.syncControllerData.entityId == Entity.EntityID);
        SceneMessageBroker.AddSubscriber<SC_Synchronization>(OnSynchronization).Where(synchronization => synchronization.syncDataEntry.meta.type == GetType().Name && synchronization.syncDataEntry.meta.entityId == Entity.EntityID);
    }

    protected override void OnDetached()
    {
        SceneMessageBroker.RemoveSubscriber<SC_SyncController>(OnSyncController);
        SceneMessageBroker.RemoveSubscriber<SC_Synchronization>(OnSynchronization);
    }

    private void OnSyncController(SC_SyncController syncController)
    {
        OwnerId = syncController.syncControllerData.ownerId;
    }

    private void OnSynchronization(SC_Synchronization synchronization)
    {
        if (OwnerId != synchronization.syncDataEntry.meta.userId)
        {
            Debug.LogWarning($"User (not owner) request synchronization. It is ignored. userId: {synchronization.syncDataEntry.meta.userId}, ownerId: {OwnerId}");
            return;
        }

        if (synchronization.syncDataEntry.meta.userId == LOP.Application.UserId)
        {
            return;
        }

        OnSync(synchronization.syncDataEntry);
        OnSync((T)synchronization.syncDataEntry.data);
    }

    public abstract T GetSyncData();

    public void SetDirty()
    {
        IsDirty = true;
    }

    public void Sync(T value)
    {
        if (!HasAuthority)
        {
            Debug.LogWarning("You must have authority to write.");
            return;
        }

        //  request syncData to server
        var synchronization = ObjectPool.Instance.GetObject<CS_Synchronization>();
        synchronization.syncDataEntry = new SyncDataEntry();
        synchronization.syncDataEntry.meta = new SyncDataMeta(Game.Current.CurrentTick, GetType().Name, LOP.Application.UserId, Entity.EntityID, value.ObjectToHash());
        synchronization.syncDataEntry.data = value;
       
        RoomNetwork.Instance.Send(synchronization, 0, instant: true);
    }

    public virtual void OnSync(T value) { }
    public virtual void OnSync(SyncDataEntry value) { }
}
