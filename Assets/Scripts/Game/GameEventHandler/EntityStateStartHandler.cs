using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityStateStartHandler : IHandler<IGameEvent>
{
    public void Handle(IGameEvent gameEvent)
    {
        EntityStateStart entityStateStart = gameEvent as EntityStateStart;
    }
}
