using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

namespace GameEvent
{
    [Serializable]
    public class EntityDamage : IGameEvent
    {
        public int Seq { get; }
        public int Tick { get; set; }
        public int attackerID;
        public int damagedID;
        public int damage;
        public int afterHP;
    }
}
