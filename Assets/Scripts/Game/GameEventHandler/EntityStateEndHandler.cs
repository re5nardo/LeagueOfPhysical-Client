using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;

public class EntityStateEndHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityStateEnd entityStateEnd = gameEvent as EntityStateEnd;
    }
}
