using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityStateStartHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityStateStart entityStateStart = gameEvent as EntityStateStart;
    }
}
