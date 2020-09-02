using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameEvent;

public class EntityBehaviorEndHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityBehaviorEnd entityBehaviorEnd = gameEvent as EntityBehaviorEnd;

        IEntity entity = Entities.Get(entityBehaviorEnd.entityID);
        if (entity == null)
        {
            return;
        }
    }
}
