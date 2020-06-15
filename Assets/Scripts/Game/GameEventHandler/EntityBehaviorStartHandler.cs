using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityBehaviorStartHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityBehaviorStart entityBehaviorStart = gameEvent as EntityBehaviorStart;

        IEntity entity = EntityManager.Instance.GetEntity(entityBehaviorStart.entityID);
        if (entity == null)
        {
            return;
        }

        if (entityBehaviorStart.behaviorMasterID == Define.MasterData.BehaviorID.MOVE
        || entityBehaviorStart.behaviorMasterID == Define.MasterData.BehaviorID.ROTATION)
        {
            //	Do nothing
            //	이 녀석들도 마찬가지로 클라이언트에서 플레이 해야 할까...?
            return;
        }

        entity.GetComponent<BehaviorController>()?.StartBehavior(entityBehaviorStart.behaviorMasterID);
    }
}
