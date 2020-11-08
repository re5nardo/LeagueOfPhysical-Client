using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using GameEvent;
using System;

public class GameEventDispatcher : MonoBehaviour
{
    private Dictionary<Type, Action<IGameEvent>> gameEventHandler = new Dictionary<Type, Action<IGameEvent>>();

    private void Awake()
    {
        gameEventHandler.Add(typeof(EntityBehaviorStart),    EntityBehaviorStartHandler.Handle);
        gameEventHandler.Add(typeof(EntityBehaviorEnd),      EntityBehaviorEndHandler.Handle);
        gameEventHandler.Add(typeof(EntityStateStart),       EntityStateStartHandler.Handle);
        gameEventHandler.Add(typeof(EntityStateEnd),         EntityStateEndHandler.Handle);
        gameEventHandler.Add(typeof(EntityDamage),           EntityDamageHandler.Handle);
        gameEventHandler.Add(typeof(EntityHeal),             EntityHealHandler.Handle);
        gameEventHandler.Add(typeof(EntityDie),              EntityDieHandler.Handle);
        gameEventHandler.Add(typeof(EntityGetExp),           EntityGetExpHandler.Handle);
        gameEventHandler.Add(typeof(EntityGetMoney),         EntityGetMoneyHandler.Handle);
        gameEventHandler.Add(typeof(EntityLevelUp),          EntityLevelUpHandler.Handle);
    }

    private void OnDestroy()
    {
        gameEventHandler.Clear();
    }

    public void DispatchGameEvent(IGameEvent gameEvent)
    {
        if (gameEventHandler.TryGetValue(gameEvent.GetType(), out Action<IGameEvent> handler))
        {
            handler?.Invoke(gameEvent);
        }
    }
}
