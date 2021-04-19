using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class SubGameEndState : MonoStateBase
{
    //public override void OnGameStateMessage(SC_GameState msg)
    //{
    //    switch (msg.gameState)
    //    {
    //        case "SubGameSelectionState":
    //            FSM.MoveNext(GameStateInput.SubGameSelectionState);
    //            break;

    //        case "MatchEndState":
    //            FSM.MoveNext(GameStateInput.GameEndState);
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
            case GameStateInput.SubGameSelectionState:
                return gameObject.GetOrAddComponent<SubGameSelectionState>();

            case GameStateInput.GameEndState:
                return gameObject.GetOrAddComponent<GameEndState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
    }
}
