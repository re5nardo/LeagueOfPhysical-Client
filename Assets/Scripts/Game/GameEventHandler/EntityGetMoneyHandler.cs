using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityGetMoneyHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityGetMoney entityGetMoney = gameEvent as EntityGetMoney;

        GameObject goFloatingGetMoney = ResourcePool.Instance.GetResource(Define.ResourcePath.UI.FLOATING_GET_MONEY, LOP.Game.Current.GameUI.GetFloatingItemCanvas().transform);

        FloatingItem floatingItem = goFloatingGetMoney.GetComponent<FloatingItem>();

        floatingItem.SetData(Camera.main.WorldToScreenPoint(entityGetMoney.position), string.Format("+{0}", entityGetMoney.money));

        goFloatingGetMoney.AddComponent<ResourceReturnAgent>().m_fDelayTime = 1.5f;
    }
}
