using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.FSM;
using System;

public class SubGamePrepareState : MonoStateBase
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
        GameBlackboard.keyValues["sceneName"] = "RememberGame";

        yield return SceneManager.LoadSceneAsync(GameBlackboard.keyValues["sceneName"], LoadSceneMode.Additive);

        yield return SubGameBase.Current.Initialize();

        //  Send packet
        //  ...

        FSM.MoveNext(GameStateInput.StateDone);
    }
}
