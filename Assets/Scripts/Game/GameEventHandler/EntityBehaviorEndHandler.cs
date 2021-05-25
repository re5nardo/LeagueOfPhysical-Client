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

        //  로컬 유저의 move, rotation은 로컬에서 (선처리) 플레이가 되기 때문에 서버의 내용은 무시한다.
        if (entity.EntityID == Entities.MyEntityID)
        {
            if (entityBehaviorEnd.behaviorMasterID == Define.MasterData.BehaviorID.MOVE || entityBehaviorEnd.behaviorMasterID == Define.MasterData.BehaviorID.ROTATION)
            {
                return;
            }
        }

        entity.GetEntityComponent<BehaviorController>()?.StopBehavior(entityBehaviorEnd.behaviorMasterID);
    }
}
