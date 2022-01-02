using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using GameFramework;

namespace GameState
{
    public class SubGameProgressState : MonoStateBase
    {
        public override void OnEnter()
        {
            SubGameBase.Current.StartGame();    //  서버와 tick 동기화?
        }

        public override IEnumerator OnExecute()
        {
            yield return new WaitWhile(() => nameof(SubGameProgressState) == SceneDataContainer.Get<GameData>().gameState);

            FSM.MoveNext(SceneDataContainer.Get<GameData>().gameState.TryEnumParse(GameStateInput.None));
        }

        public override void OnExit()
        {
            SubGameBase.Current.EndGame();
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
                case GameStateInput.SubGameClearState:
                case GameStateInput.SubGameEndState:
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.SubGameClearState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
