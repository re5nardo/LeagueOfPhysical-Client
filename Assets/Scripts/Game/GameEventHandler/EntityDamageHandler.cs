using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityDamageHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityDamage entityDamage = gameEvent as EntityDamage;

        MonoEntityBase entity = EntityManager.Instance.GetEntity(entityDamage.damagedID) as MonoEntityBase;

        if (entity.EntityType == EntityType.GameItem)
        {
            (entity as GameItem).CurrentHP = entityDamage.afterHP;
        }
        else if (entity.EntityType == EntityType.Character)
        {
            (entity as Character).CurrentHP = entityDamage.afterHP;
        }

        GameObject goCommonTextUI = ResourcePool.Instance.GetResource("UI/CommonTextUI", LOP.Game.Current.GameUI.GetTopMostGameRoomPanel().transform);
        CommonTextUI commonTextUI = goCommonTextUI.GetComponent<CommonTextUI>();
        commonTextUI.SetData(entity.Position, string.Format("-{0}", entityDamage.damage), Color.red);

        goCommonTextUI.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
