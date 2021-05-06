using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SynchronizationManager : MonoSingleton<SynchronizationManager>
{
    public static void Handle(List<ISnap> snaps)
    {
        Instance.HandleInternal(snaps);
    }

    private void HandleInternal(List<ISnap> snaps)
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

    private void Handle(MonoEntitySnap monoEntitySnap)
    {
        var entity = Entities.Get<Entity.MonoEntityBase>(monoEntitySnap.entityId);

        entity?.GetComponent<MonoEntitySynchronization>().OnReceiveSynchronization(monoEntitySnap);
    }

    private void Handle(EntityTransformSnap entityTransformSnap)
    {
        var entity = Entities.Get<Entity.MonoEntityBase>(entityTransformSnap.entityId);

        entity?.GetComponent<EntityTransformSynchronization>().OnReceiveSynchronization(entityTransformSnap);
    }

    private void Handle(MoveSnap moveSnap)
    {
        var entity = Entities.Get(moveSnap.entityId);

        entity?.GetEntityComponent<Behavior.Move>()?.OnReceiveSynchronization(moveSnap);
    }

    private void Handle(BehaviorSnap behaviorSnap)
    {
        var entity = Entities.Get(behaviorSnap.entityId);
        if (entity != null && entity.GetEntityComponent<BehaviorController>().IsBehaviorRunning(behaviorSnap.behaviorMasterId, out var behavior))
        {
            behavior?.OnReceiveSynchronization(behaviorSnap);
        }
    }
}
