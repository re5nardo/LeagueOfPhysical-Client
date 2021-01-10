using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class WaitForPlayersState : GameStateBase
{
    public override void OnGameStateMessage(SC_GameState msg)
    {
        switch (msg.gameState)
        {
            case "SubGameSelectionState":
                FSM.MoveNext(GameStateInput.SubGameSelectionState);
                break;
        }
    }

    public override IState<GameStateInput> GetNext(GameStateInput input)
    {
        switch (input)
        {
            case GameStateInput.StateDone:
            case GameStateInput.SubGameSelectionState:
                return gameObject.GetOrAddComponent<SubGameSelectionState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }
}
