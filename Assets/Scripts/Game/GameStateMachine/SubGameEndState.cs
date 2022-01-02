using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using GameFramework;

namespace GameState
{
    public class SubGameEndState : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            yield return new WaitWhile(() => nameof(SubGameEndState) == SceneDataContainer.Get<GameData>().GameState);

            FSM.MoveNext(SceneDataContainer.Get<GameData>().GameState.TryEnumParse(GameStateInput.None));
        }

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
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.EndState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
