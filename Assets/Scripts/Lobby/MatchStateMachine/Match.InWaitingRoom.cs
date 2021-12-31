using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using UniRx;

namespace Match
{
    public class InWaitingRoom : MonoStateBase
    {
        private const int CHECK_INTERVAL = 2;   //  sec

        private void Awake()
        {
            Lobby.MessageBroker.Receive<string>().Where(msg => msg == "OnCancelMatchingButtonClicked" && IsCurrent).Subscribe(OnCancelMatchmakingButtonClicked).AddTo(this);
        }

        public override void OnEnter()
        {
            MatchingWaitingView.Show();
        }

        public override IEnumerator OnExecute()
        {
            while (true)
            {
                CheckMatchState();

                yield return new WaitForSeconds(CHECK_INTERVAL);
            }
        }

        public override void OnExit()
        {
            MatchingWaitingView.Hide();
        }

        private void CheckMatchState()
        {
            LOPWebAPI.GetUserMatchState(LOP.Application.UserId,
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

        private void OnCancelMatchmakingButtonClicked(string message)
        {
            FSM.MoveNext(MatchStateInput.CancelMatchmaking);
        }
    }
}
