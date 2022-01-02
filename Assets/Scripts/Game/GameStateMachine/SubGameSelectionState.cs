using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using GameFramework;

namespace GameState
{
    public class SubGameSelectionState : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            //  서버로 부터 받아야 됨..
            AppDataContainer.Get<MatchSettingData>().matchSetting.subGameId = "FlapWang";
            AppDataContainer.Get<MatchSettingData>().matchSetting.mapId = "FlapWangMap";

            //LOP.Game.Current.GameManager.subGameId = "FallingGame";
            //LOP.Game.Current.GameManager.mapName = "Falling";

            yield return new WaitWhile(() => nameof(SubGameSelectionState) == SceneDataContainer.Get<GameData>().GameState);

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
                case GameStateInput.SubGamePrepareState:
                case GameStateInput.SubGameProgressState:
                    return gameObject.GetOrAddComponent<GameState.SubGamePrepareState>();

                case GameStateInput.SubGameClearState:
                case GameStateInput.SubGameEndState:
                case GameStateInput.EndState:
                    return gameObject.GetOrAddComponent<GameState.SubGameClearState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
