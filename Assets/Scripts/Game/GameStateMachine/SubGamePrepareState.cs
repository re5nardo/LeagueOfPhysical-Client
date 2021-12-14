using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework.FSM;
using System;

namespace GameState
{
    public class SubGamePrepareState : MonoStateBase
    {
        public override void Enter()
        {
            base.Enter();

            StopCoroutine("Procedure");
            StartCoroutine("Procedure");
        }

        public override void Exit()
        {
            base.Exit();

            StopCoroutine("Procedure");
        }

        //public override void OnGameStateMessage(SC_GameState msg)
        //{
        //    switch (msg.gameState)
        //    {
        //        case "SubGameProgressState":
        //            FSM.MoveNext(GameStateInput.SubGameProgressState);
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
                case GameStateInput.StateDone: return gameObject.GetOrAddComponent<GameState.SubGameProgressState>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }

        private IEnumerator Procedure()
        {
            //  서버로 부터 받아야 됨..
            AppDataContainer.Get<MatchSettingData>().matchSetting.subGameId = "FlapWang";
            AppDataContainer.Get<MatchSettingData>().matchSetting.mapId = "FlapWangMap";

            //LOP.Game.Current.GameManager.subGameId = "FallingGame";
            //LOP.Game.Current.GameManager.mapName = "Falling";

            yield return SceneManager.LoadSceneAsync(LOP.Game.Current.SubGameData.sceneName, LoadSceneMode.Additive);

            //  Send packet
            //  ...

            FSM.MoveNext(GameStateInput.StateDone);
        }
    }
}
