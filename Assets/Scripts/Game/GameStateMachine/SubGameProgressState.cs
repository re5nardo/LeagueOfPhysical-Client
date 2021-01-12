using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class SubGameProgressState : GameStateBase
{
    public override void Enter()
    {
        SubGameBase.Current.StartGame();
    }

    //public override void OnGameStateMessage(SC_GameState msg)
    //{
    //    switch (msg.gameState)
    //    {
    //        case "SubGameEndState":
    //            FSM.MoveNext(GameStateInput.SubGameEndState);
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
            case GameStateInput.SubGameEndState:
                return gameObject.GetOrAddComponent<SubGameEndState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
    }
}
