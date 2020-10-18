using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_SynchronizationHandler
{
    public static void Handle(IPhotonEventMessage msg)
    {
        SC_Synchronization synchronization = msg as SC_Synchronization;

        foreach(var snap in synchronization.snaps)
        {
            //Debug.Log(snap.ToString());

            if (snap is MonoEntitySnap)
            {
                Handle(snap as MonoEntitySnap);
            }
            else if (snap is EntityTransformSnap)
            {
                Handle(snap as EntityTransformSnap);
            }
            else if (snap is BehaviorSnap)
            {
                Handle(snap as BehaviorSnap);
            }
        }
    }

    public static void Handle(MonoEntitySnap monoEntitySnap)
    {
        var entity = Entities.Get<Entity.MonoEntityBase>(int.Parse(monoEntitySnap.Id));
        
        entity?.GetComponent<MonoEntitySynchronization>()?.OnReceiveSynchronization(monoEntitySnap);
    }

    public static void Handle(EntityTransformSnap entityTransformSnap)
    {
        var entity = Entities.Get<Entity.MonoEntityBase>(int.Parse(entityTransformSnap.Id));

        entity?.GetComponent<EntityTransformSynchronization>()?.OnReceiveSynchronization(entityTransformSnap);
    }

    public static void Handle(BehaviorSnap behaviorSnap)
    {
    }
}
