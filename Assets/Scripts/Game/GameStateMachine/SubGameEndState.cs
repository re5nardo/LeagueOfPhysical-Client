using System.Collections;
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
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.EndState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
