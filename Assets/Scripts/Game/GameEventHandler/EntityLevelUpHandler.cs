using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvent;
using GameFramework;
using Entity;

public class EntityLevelUpHandler
{
    public static void Handle(IGameEvent gameEvent)
    {
        EntityLevelUp entityLevelUp = gameEvent as EntityLevelUp;

        Entities.MyCharacter.Level = entityLevelUp.level;
    }
}
