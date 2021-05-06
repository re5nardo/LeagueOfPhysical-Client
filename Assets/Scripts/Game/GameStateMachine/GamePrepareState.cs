using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class GamePrepareState : MonoStateBase
{
    public override void Enter()
    {
        StopCoroutine("Procedure");
        StartCoroutine("Procedure");
    }

    public override void Exit()
    {
        StopCoroutine("Procedure");
    }

    //public override void OnGameStateMessage(SC_GameState msg)
    //{
    //    switch (msg.gameState)
    //    {
    //        case "SubGameProgressState":
    //            FSM.MoveNext(GameStateInput.SubGameProgressState);
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
            case GameStateInput.SubGameProgressState:
                return gameObject.GetOrAddComponent<SubGameProgressState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
    }

    private IEnumerator Procedure()
    {
        //  게임에 기본 필요한 준비 (리소스 등등)

        //  Send packet
        //  ...

        FSM.MoveNext(GameStateInput.StateDone);

        yield break;
    }
}
