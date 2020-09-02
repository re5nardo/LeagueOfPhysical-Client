using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityDieHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityDie entityDie = gameEvent as EntityDie;

        Character dead = Entities.Get<Character>(entityDie.deadID);

        BehaviorController behaviorController = dead.GetComponent<BehaviorController>();
        //behaviorController.Die();
    }
}
