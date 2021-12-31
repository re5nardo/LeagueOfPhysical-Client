﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace GameState
{
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
                case GameStateInput.None: return FSM.CurrentState;
                case GameStateInput.StateDone: return gameObject.GetOrAddComponent<GameState.EndState>();

                case GameStateInput.EntryState: return gameObject.GetOrAddComponent<GameState.EntryState>();
                case GameStateInput.PrepareState: return gameObject.GetOrAddComponent<GameState.PrepareState>();
                case GameStateInput.SubGameSelectionState: return gameObject.GetOrAddComponent<GameState.SubGameSelectionState>();
                case GameStateInput.SubGamePrepareState: return gameObject.GetOrAddComponent<GameState.SubGamePrepareState>();
                case GameStateInput.SubGameProgressState: return gameObject.GetOrAddComponent<GameState.SubGameProgressState>();
                case GameStateInput.SubGameClearState: return gameObject.GetOrAddComponent<GameState.SubGameClearState>();
                case GameStateInput.SubGameEndState: return gameObject.GetOrAddComponent<GameState.SubGameEndState>();
                case GameStateInput.EndState: return gameObject.GetOrAddComponent<GameState.EndState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
