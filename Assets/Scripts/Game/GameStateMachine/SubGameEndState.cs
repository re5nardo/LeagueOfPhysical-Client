﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

public class SubGameEndState : GameStateBase
{
    public override void OnGameStateMessage(SC_GameState msg)
    {
        switch (msg.gameState)
        {
            case "SubGameSelectionState":
                FSM.MoveNext(GameStateInput.SubGameSelectionState);
                break;

            case "MatchEndState":
                FSM.MoveNext(GameStateInput.MatchEndState);
                break;
        }
    }

    public override IState<GameStateInput> GetNext(GameStateInput input)
    {
        switch (input)
        {
            case GameStateInput.SubGameSelectionState:
                return gameObject.GetOrAddComponent<SubGameSelectionState>();

            case GameStateInput.MatchEndState:
                return gameObject.GetOrAddComponent<MatchEndState>();
        }

        throw new Exception($"Invalid transition: {GetType().Name} with {input}");
    }
}
