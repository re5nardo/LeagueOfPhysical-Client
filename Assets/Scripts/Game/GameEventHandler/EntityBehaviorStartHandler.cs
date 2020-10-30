using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityBehaviorStartHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityBehaviorStart entityBehaviorStart = gameEvent as EntityBehaviorStart;

        IEntity entity = Entities.Get(entityBehaviorStart.entityID);
        if (entity == null)
        {
            return;
        }

        entity.GetComponent<BehaviorController>()?.StartBehavior(entityBehaviorStart.behaviorMasterID, entityBehaviorStart.param);
    }
}
