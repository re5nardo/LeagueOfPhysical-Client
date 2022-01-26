using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.FSM;
using System;
using NetworkModel.Mirror;
using GameFramework;

namespace GameState
{
    public class SubGamePrepareState : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            var subGameLoader = SceneManager.LoadSceneAsync(LOP.Game.Current.SubGameData.sceneName, LoadSceneMode.Additive);
            var mapLoader = SceneManager.LoadSceneAsync(LOP.Game.Current.MapData.sceneName, LoadSceneMode.Additive);

            yield return new WaitUntil(() => subGameLoader.isDone && mapLoader.isDone);

            yield return SubGameBase.Current.Initialize();

            //  Send GamePreparation
            var disposer = PoolObjectDisposer<CS_SubGamePreparation>.Get();
            var subGamePreparation = disposer.PoolObject;
            subGamePreparation.entityId = Entities.MyEntityID;
            subGamePreparation.preparation = 1;

            RoomNetwork.Instance.Send(subGamePreparation, 0);

            yield return new WaitWhile(() => nameof(SubGamePrepareState) == SceneDataContainer.Get<GameData>().gameState);

            FSM.MoveNext(SceneDataContainer.Get<GameData>().gameState.TryEnumParse(GameStateInput.None));
        }

        public override void OnExit()
        {
            //  hide global loading display?

            //  Clear
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
                case GameStateInput.SubGameProgressState:
                    return gameObject.GetOrAddComponent<GameState.SubGameProgressState>();

                case GameStateInput.SubGameClearState:
                case GameStateInput.SubGameEndState:
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.SubGameClearState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
