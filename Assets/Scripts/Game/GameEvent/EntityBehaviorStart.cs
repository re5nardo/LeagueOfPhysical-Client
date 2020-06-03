using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

namespace GameEvent
{
    [Serializable]
    public class EntityBehaviorStart : IGameEvent
    {
        public int seq { get; }
        public int tick { get; set; }
        public int entityID;
        public int behaviorMasterID;
        public object[] param;
    }
}
