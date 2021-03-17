using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class MatchEndState : MonoStateBase
{
    public override IState GetNext<I>(I input)
    {
        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }
}
