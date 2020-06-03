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

        GameObject goMoneyGetUI = ResourcePool.Instance.GetResource("UI/MoneyGetUI", LOP.Game.Current.GameUI.GetTopMostGameRoomPanel().transform);

        MoneyGetUI moneyGetUI = goMoneyGetUI.GetComponent<MoneyGetUI>();
        moneyGetUI.SetData(entityGetMoney.position, string.Format("+{0}", entityGetMoney.money));

        goMoneyGetUI.AddComponent<ResourceReturnAgent>().m_fDelayTime = 1.5f;
    }
}
