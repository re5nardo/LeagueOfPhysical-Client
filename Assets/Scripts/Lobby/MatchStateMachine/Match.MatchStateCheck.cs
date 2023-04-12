using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class MatchStateCheck : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);

            yield return getUser;

            if (!IsCurrent)
            {
                yield break;
            }

            if (getUser.isSuccess == false || getUser.response.code != ResponseCode.SUCCESS)
            {
                Debug.LogError($"Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다. error: {getUser.error}");
                yield break;
            }

            AppDataContainer.Get<UserData>().user = getUser.response.user;

            switch (getUser.response.user.location)
            {
                case Location.InWaitingRoom:
                    FSM.MoveNext(MatchStateInput.MatchInWaitingRoomState);
                    break;

                case Location.InGameRoom:
                    FSM.MoveNext(MatchStateInput.MatchInGameRoomState);
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
                case MatchStateInput.MatchInWaitingRoomState:
                    return gameObject.GetOrAddComponent<InWaitingRoom>();

                case MatchStateInput.MatchInGameRoomState:
                    return gameObject.GetOrAddComponent<InGameRoom>();

                case MatchStateInput.MatchIdleState:
                    return gameObject.GetOrAddComponent<Idle>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
