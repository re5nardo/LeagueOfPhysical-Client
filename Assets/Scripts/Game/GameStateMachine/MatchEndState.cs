using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class MatchEndState : GameStateBase
{
    public override IState<GameStateInput> GetNext(GameStateInput input)
    {
        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }
}
