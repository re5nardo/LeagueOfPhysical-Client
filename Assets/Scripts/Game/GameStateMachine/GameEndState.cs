using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace GameState
{
    public class GameEndState : MonoStateBase
    {
        public override IState GetNext<I>(I input)
        {
            throw new Exception($"Invalid transition: {GetType().Name} with {input}");
        }
    }
}
