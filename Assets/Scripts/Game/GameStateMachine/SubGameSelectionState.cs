using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class SubGameSelectionState : MonoStateBase
{
    //public override void OnGameStateMessage(SC_GameState msg)
    //{
    //    switch (msg.gameState)
    //    {
    //        case "SubGamePrepareState":
    //            FSM.MoveNext(GameStateInput.SubGamePrepareState);
    //            break;
    //    }
    //}

    public override IState GetNext<I>(I input)
    {
        if (!Enum.TryParse(input.ToString(), out GameStateInput gameStateInput))
        {
            Debug.LogError($"Invalid input! input : {input}");
            return default;
        }

        switch (gameStateInput)
        {
            case GameStateInput.StateDone:
            case GameStateInput.SubGamePrepareState:
                return gameObject.GetOrAddComponent<SubGamePrepareState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
    }
}
