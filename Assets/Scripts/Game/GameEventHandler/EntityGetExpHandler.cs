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

        GameObject goExpGetUI = ResourcePool.Instance.GetResource("UI/ExpGetUI", LOP.Game.Current.GameUI.GetTopMostGameRoomPanel().transform);

        ExpGetUI expGetUI = goExpGetUI.GetComponent<ExpGetUI>();
        expGetUI.SetData(userCharacter.Position, string.Format("+Exp {0}", entityGetExp.exp));

        goExpGetUI.AddComponent<ResourceReturnAgent>().m_fDelayTime = 1.5f;
    }
}
