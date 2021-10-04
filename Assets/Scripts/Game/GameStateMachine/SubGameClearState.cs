using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.FSM;
using System;

public class SubGameClearState : MonoStateBase
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
    //        case "GameEndState":
    //            FSM.MoveNext(GameStateInput.MatchEndState);
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
            case GameStateInput.GameEndState:
                return gameObject.GetOrAddComponent<GameEndState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
    }

    private IEnumerator Procedure()
    {
        yield return SceneManager.UnloadSceneAsync(LOP.Game.Current.GameManager.SubGameData.sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        LOP.Game.Current.GameManager.subGameId = null;
        LOP.Game.Current.GameManager.mapId = null;

        FSM.MoveNext(GameStateInput.StateDone);
    }
}
