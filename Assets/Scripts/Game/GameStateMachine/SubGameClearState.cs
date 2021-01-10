﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.FSM;
using System;

public class SubGameClearState : GameStateBase
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

    public override void OnGameStateMessage(SC_GameState msg)
    {
        switch (msg.gameState)
        {
            case "MatchEndState":
                FSM.MoveNext(GameStateInput.MatchEndState);
                break;
        }
    }

    public override IState<GameStateInput> GetNext(GameStateInput input)
    {
        switch (input)
        {
            case GameStateInput.StateDone:
            case GameStateInput.MatchEndState:
                return gameObject.GetOrAddComponent<MatchEndState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }

    private IEnumerator Procedure()
    {
        yield return SceneManager.UnloadSceneAsync(GameBlackboard.keyValues["sceneName"], UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        GameBlackboard.keyValues.Remove("sceneName");

        FSM.MoveNext(GameStateInput.StateDone);
    }
}
