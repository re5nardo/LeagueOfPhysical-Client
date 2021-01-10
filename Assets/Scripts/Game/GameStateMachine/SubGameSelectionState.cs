using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class SubGameSelectionState : GameStateBase
{
    public override void OnGameStateMessage(SC_GameState msg)
    {
        switch (msg.gameState)
        {
            case "SubGamePrepareState":
                FSM.MoveNext(GameStateInput.SubGamePrepareState);
                break;
        }
    }

    public override IState<GameStateInput> GetNext(GameStateInput input)
    {
        switch (input)
        {
            case GameStateInput.StateDone:
            case GameStateInput.SubGamePrepareState:
                return gameObject.GetOrAddComponent<SubGamePrepareState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }
}
