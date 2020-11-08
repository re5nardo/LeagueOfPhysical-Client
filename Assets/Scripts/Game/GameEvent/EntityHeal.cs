using System.Collections;
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
        public int healingEntityID;
        public int healedEntityID;
        public int heal;
        public int afterHP;
    }
}
