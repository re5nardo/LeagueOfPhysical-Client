using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityHealHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityHeal entityHeal = gameEvent as EntityHeal;

        MonoEntityBase entity = EntityManager.Instance.GetEntity(entityHeal.healedEntityID) as MonoEntityBase;

        if (entity.EntityType == EntityType.Character)
        {
            (entity as Character).CurrentHP = entityHeal.afterHP;
        }

        GameObject goCommonTextUI = ResourcePool.Instance.GetResource("UI/CommonTextUI", LOP.Game.Current.GameUI.GetTopMostGameRoomPanel().transform);
        CommonTextUI commonTextUI = goCommonTextUI.GetComponent<CommonTextUI>();
        commonTextUI.SetData(entity.Position, string.Format("+{0}", entityHeal.heal), Color.green);

        goCommonTextUI.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
