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

        ////  로컬 유저의 move, rotation, jump은 로컬에서 (선처리) 플레이가 되기 때문에 서버의 내용은 무시한다.
        //if (entity.EntityID == Entities.MyEntityID)
        //{
        //    if (entityBehaviorStart.behaviorMasterID == Define.MasterData.BehaviorID.MOVE
        //        || entityBehaviorStart.behaviorMasterID == Define.MasterData.BehaviorID.ROTATION
        //        || entityBehaviorStart.behaviorMasterID == Define.MasterData.BehaviorID.JUMP)
        //    {
        //        return;
        //    }
        //}

        //var behaviorController = entity.GetEntityComponent<BehaviorController>();
        //if (behaviorController.IsBehaviorRunning(entityBehaviorStart.behaviorMasterID, out var behavior))
        //{
        //    //behavior.SetData()
        //}
        //else
        //{
        //    entity.GetEntityComponent<BehaviorController>()?.StartBehavior(entityBehaviorStart.behaviorMasterID, entityBehaviorStart.param);
        //}
    }
}
