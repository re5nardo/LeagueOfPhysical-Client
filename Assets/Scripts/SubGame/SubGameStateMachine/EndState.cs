using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace SubGameState
{
    public class EndState : MonoStateBase
    {
        public override IState GetNext<I>(I input)
        {
            if (!Enum.TryParse(input.ToString(), out SubGameStateInput subGameStateInput))
            {
                Debug.LogError($"Invalid input! input : {input}");
                return default;
            }

            switch (subGameStateInput)
            {
                case SubGameStateInput.None: return FSM.CurrentState;
                case SubGameStateInput.StateDone: return gameObject.GetOrAddComponent<SubGameState.EndState>();

                case SubGameStateInput.EntryState: return gameObject.GetOrAddComponent<SubGameState.EntryState>();
                case SubGameStateInput.PrepareState: return gameObject.GetOrAddComponent<SubGameState.PrepareState>();
                case SubGameStateInput.ProgressState: return gameObject.GetOrAddComponent<SubGameState.ProgressState>();
                case SubGameStateInput.ClearState: return gameObject.GetOrAddComponent<SubGameState.ClearState>();
                case SubGameStateInput.EndState: return gameObject.GetOrAddComponent<SubGameState.EndState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {subGameStateInput}");
        }
    }
}
