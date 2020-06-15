using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityGetExpHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityGetExp entityGetExp = gameEvent as EntityGetExp;

        Character userCharacter = EntityManager.Instance.GetMyCharacter();

        GameObject goFloatingItem = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.FLOATING_ITEM, LOP.Game.Current.GameUI.GetTopMostCanvas().transform);

        FloatingItem floatingItem = goFloatingItem.GetComponent<FloatingItem>();
        floatingItem.SetData(Camera.main.WorldToScreenPoint(userCharacter.Position), string.Format("+Exp {0}", entityGetExp.exp));

        goFloatingItem.AddComponent<ResourceReturnAgent>().m_fDelayTime = 1.5f;
    }
}
