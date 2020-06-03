using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameEvent;
using System;

public class GameEventDispatcher : MonoBehaviour
{
    private Dictionary<Type, IHandler<IGameEvent>> gameEventHandler = new Dictionary<Type, IHandler<IGameEvent>>();

    private void Awake()
    {
        gameEventHandler.Add(typeof(EntityBehaviorStart),    new EntityBehaviorStartHandler());
        gameEventHandler.Add(typeof(EntityBehaviorEnd),      new EntityBehaviorEndHandler());
        gameEventHandler.Add(typeof(EntityStateStart),       new EntityStateStartHandler());
        gameEventHandler.Add(typeof(EntityStateEnd),         new EntityStateEndHandler());
        gameEventHandler.Add(typeof(EntityDamage),           new EntityDamageHandler());
        gameEventHandler.Add(typeof(EntityHeal),             new EntityHealHandler());
        gameEventHandler.Add(typeof(EntityDie),              new EntityDieHandler());
        gameEventHandler.Add(typeof(EntityGetExp),           new EntityGetExpHandler());
        gameEventHandler.Add(typeof(EntityGetMoney),         new EntityGetMoneyHandler());
        gameEventHandler.Add(typeof(EntityLevelUp),          new EntityLevelUpHandler());
    }

    private void OnDestroy()
    {
        gameEventHandler.Clear();
    }

    public void DispatchGameEvent(IGameEvent gameEvent)
    {
        IHandler<IGameEvent> handler = null;

        if (gameEventHandler.TryGetValue(gameEvent.GetType(), out handler))
        {
            handler?.Handle(gameEvent);
        }
    }
}
