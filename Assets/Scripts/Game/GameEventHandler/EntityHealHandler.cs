﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityHealHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityHeal entityHeal = gameEvent as EntityHeal;

        LOPEntityBase entity = Entities.Get<LOPEntityBase>(entityHeal.healedEntityID);

        if (entity.EntityType == EntityType.Character)
        {
            (entity as Character).HP = entityHeal.afterHP;
        }

        GameObject goFloatingItem = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.FLOATING_ITEM, LOP.Game.Current.GameUI.GetFloatingItemCanvas().transform);
        FloatingItem floatingItem = goFloatingItem.GetComponent<FloatingItem>();
        floatingItem.SetData(Camera.main.WorldToScreenPoint(entity.Position), string.Format("+{0}", entityHeal.heal), Color.green);

        goFloatingItem.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
