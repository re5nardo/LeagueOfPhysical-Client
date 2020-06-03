using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameEvent;

public class EntityBehaviorEndHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityBehaviorEnd entityBehaviorEnd = gameEvent as EntityBehaviorEnd;

        IEntity entity = EntityManager.Instance.GetEntity(entityBehaviorEnd.entityID);
        if (entity == null)
        {
            return;
        }
    }
}
