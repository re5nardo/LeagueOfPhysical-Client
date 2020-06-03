using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityDieHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityDie entityDie = gameEvent as EntityDie;

        Character dead = EntityManager.Instance.GetEntity(entityDie.deadID) as Character;

        BasicController controller = dead.GetComponent<BasicController>();
        //controller.Die();
    }
}
