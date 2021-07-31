using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class InGameRoom : MonoStateBase
    {
        private const int CHECK_INTERVAL = 2;   //  sec
        private DateTime lastCheckTime;

        public override void Enter()
        {
            base.Enter();

            GameLoadingView.Show();
        }

        public override void Execute()
        {
            base.Execute();

            if ((DateTime.UtcNow - lastCheckTime).TotalSeconds > CHECK_INTERVAL)
            {
                CheckMatchState();

                lastCheckTime = DateTime.UtcNow;
            }
        }

        public override void Exit()
        {
            base.Exit();

            GameLoadingView.Hide();
        }

        private void CheckMatchState()
        {
            LOPWebAPI.GetUserMatchState(PhotonNetwork.AuthValues.UserId,
            result =>
            {
                if (!IsCurrent) return;

                if (result.code != ResponseCode.SUCCESS)
                {
                    Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
                    return;
                }

                switch (result.userMatchState.state)
                {
                    case "inGameRoom":
                        RoomConnector.Instance.TryToEnterRoomById(result.userMatchState.stateValue);
                        break;
                }
            },
            error =>
            {
                Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
            });
        }

        public override IState GetNext<I>(I input)
        {
            if (!input.TryParse(out MatchStateInput matchStateInput))
            {
                Debug.LogError($"Invalid input! input : {input}");
                return default;
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
