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
                yield return CheckMatchState();

                yield return new WaitForSeconds(CHECK_INTERVAL);
            }
        }

        public override void OnExit()
        {
            MatchingWaitingView.Hide();
        }

        private IEnumerator CheckMatchState()
        {
            var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);
            yield return getUser;

            if (!IsCurrent)
            {
                yield break;
            }

            if (getUser.isError || getUser.response.code != ResponseCode.SUCCESS)
            {
                Debug.LogError("User 상태를 받아오는데 실패하였습니다.");
                yield break;
            }

            AppDataContainer.Get<UserData>().user = getUser.response.user;

            switch (getUser.response.user.location)
            {
                case Location.InGameRoom:
                    FSM.MoveNext(MatchStateInput.MatchInGameRoomState);
                    break;

                case Location.InWaitingRoom:
                    var verifyUserLocation = LOPWebAPI.VerifyUserLocation(getUser.response.user.id);
                    yield return verifyUserLocation;
                    break;

                default:
                    FSM.MoveNext(MatchStateInput.MatchIdleState);
                    break;
            }
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

                case MatchStateInput.MatchIdleState:
                    return gameObject.GetOrAddComponent<Idle>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }

        private void OnCancelMatchmakingButtonClicked(string message)
        {
            FSM.MoveNext(MatchStateInput.CancelMatchmaking);
        }
    }
}
