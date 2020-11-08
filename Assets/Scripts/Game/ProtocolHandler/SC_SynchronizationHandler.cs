using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_SynchronizationHandler
{
    public static void Handle(IPhotonEventMessage msg)
    {
        SC_Synchronization synchronization = msg as SC_Synchronization;

        Handle(synchronization.snaps);
    }

    public static void Handle(List<ISnap> snaps)
    {
        if (snaps == null)
            return;

        foreach (var snap in snaps)
        {
            if (snap == null)
                continue;

            if (snap is MonoEntitySnap)
            {
                Handle(snap as MonoEntitySnap);
            }
            else if (snap is EntityTransformSnap)
            {
                Handle(snap as EntityTransformSnap);
            }
            else if (snap is MoveSnap)
            {
                Handle(snap as MoveSnap);
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
        
        entity?.GetComponent<MonoEntitySynchronization>().OnReceiveSynchronization(monoEntitySnap);
    }

    public static void Handle(EntityTransformSnap entityTransformSnap)
    {
        var entity = Entities.Get<Entity.MonoEntityBase>(int.Parse(entityTransformSnap.Id));

        entity?.GetComponent<EntityTransformSynchronization>().OnReceiveSynchronization(entityTransformSnap);
    }

    public static void Handle(MoveSnap moveSnap)
    {
        var entity = Entities.Get(int.Parse(moveSnap.Id));

        entity?.GetComponent<Behavior.Move>().OnReceiveSynchronization(moveSnap);
    }

    public static void Handle(BehaviorSnap behaviorSnap)
    {
        var entity = Entities.Get(int.Parse(behaviorSnap.Id));

        if (behaviorSnap.behaviorName == typeof(Behavior.Move).Name)
        {
            entity?.GetComponent<Behavior.Move>().OnReceiveSynchronization(behaviorSnap);
        }
        else if (behaviorSnap.behaviorName == typeof(Behavior.Rotation).Name)
        {
            entity?.GetComponent<Behavior.Rotation>().OnReceiveSynchronization(behaviorSnap);
        }
        else if (behaviorSnap.behaviorName == typeof(Behavior.MeleeAttack).Name)
        {
            entity?.GetComponent<Behavior.MeleeAttack>().OnReceiveSynchronization(behaviorSnap);
        }
        else if (behaviorSnap.behaviorName == typeof(Behavior.RangeAttack).Name)
        {
            entity?.GetComponent<Behavior.RangeAttack>().OnReceiveSynchronization(behaviorSnap);
        }
    }
}
