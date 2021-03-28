using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class InWaitingRoom : MonoStateBase
    {
        private const int CHECK_INTERVAL = 2;   //  sec
        private DateTime lastCheckTime;

        public override void Enter()
        {
            base.Enter();

            Lobby.Default.AddSubscriber("OnCancelMatchmakingButtonClicked", OnCancelMatchmakingButtonClicked);
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

            Lobby.Default.RemoveSubscriber("OnCancelMatchmakingButtonClicked", OnCancelMatchmakingButtonClicked);
        }

        private void CheckMatchState()
        {
            LOPWebAPI.GetUserMatchState(PhotonNetwork.AuthValues.UserId,
            result =>
            {
                if (!IsValid) return;

                if (result.code != 200)
                {
                    Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
                    return;
                }

                switch (result.userMatchState.state)
                {
                    case "inGameRoom":
                        FSM.MoveNext(MatchStateInput.MatchInGameRoomState);
                        break;

                    case "":
                        FSM.MoveNext(MatchStateInput.MatchIdleState);
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

            switch (matchStateInput)
            {
                case MatchStateInput.CancelMatchmaking:
                    return gameObject.GetOrAddComponent<CancelMatchmaking>();

                case MatchStateInput.MatchInGameRoomState:
                    return gameObject.GetOrAddComponent<InGameRoom>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }

        private void OnCancelMatchmakingButtonClicked(object obj)
        {
            GetNext(MatchStateInput.CancelMatchmaking);
        }
    }
}
