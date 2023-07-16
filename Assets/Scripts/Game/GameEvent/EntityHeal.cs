﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

namespace GameEvent
{
    [Serializable]
    public class EntityHeal : IGameEvent
    {
        public int Seq { get; }
        public int Tick { get; set; }
        public int healingEntityId;
        public int healedEntityId;
        public int heal;
        public int afterHP;
    }
}
