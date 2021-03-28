using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;

public class MatchStateMachine : MonoStateMachineBase
{
    public override IState InitState => gameObject.GetOrAddComponent<Match.MatchStateCheck>();

    private void Awake()
    {
        StartStateMachine();
    }
}
