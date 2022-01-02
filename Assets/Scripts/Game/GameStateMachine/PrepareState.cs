using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using NetworkModel.Mirror;
using GameFramework;

namespace GameState
{
    public class PrepareState : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            //  Load game resource
            //  ...

            //  Send GamePreparation
            var gamePreparation = new CS_GamePreparation();
            gamePreparation.entityId = Entities.MyEntityID;
            gamePreparation.preparation = 1;

            RoomNetwork.Instance.Send(gamePreparation, 0);

            yield return new WaitWhile(() => nameof(PrepareState) == SceneDataContainer.Get<GameData>().gameState);

            FSM.MoveNext(SceneDataContainer.Get<GameData>().gameState.TryEnumParse(GameStateInput.None));
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
                case GameStateInput.SubGameSelectionState:
                case GameStateInput.SubGamePrepareState:
                case GameStateInput.SubGameProgressState:
                    return gameObject.GetOrAddComponent<GameState.SubGameSelectionState>();

                case GameStateInput.SubGameClearState: 
                case GameStateInput.SubGameEndState:
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.SubGameClearState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
