using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityDamageHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityDamage entityDamage = gameEvent as EntityDamage;

        var entity = Entities.Get<LOPMonoEntityBase>(entityDamage.damagedID);

        if (entity.EntityType == EntityType.GameItem)
        {
            (entity as GameItem).HP = entityDamage.afterHP;
        }
        else if (entity.EntityType == EntityType.Character)
        {
            (entity as Character).HP = entityDamage.afterHP;
        }

        GameObject goFloatingItem = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.FLOATING_ITEM, LOP.Game.Current.GameUI.GetFloatingItemCanvas().transform);
        FloatingItem floatingItem = goFloatingItem.GetComponent<FloatingItem>();
        floatingItem.SetData(Camera.main.WorldToScreenPoint(entity.Position), string.Format("-{0}", entityDamage.damage), Color.red);

        goFloatingItem.AddComponent<ResourceReturnAgent>().m_fDelayTime = 2f;
    }
}
