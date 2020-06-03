using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityStateEndHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityStateEnd entityStateEnd = gameEvent as EntityStateEnd;
    }
}
