using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class RequestMatchmaking : MonoStateBase
    {
        public override IEnumerator OnExecute()
        {
            var matchSelectData = SceneDataContainer.Get<MatchSelectData>();

            if (LOPSettings.Get().connectLocalServer)
            {
                RoomConnector.Instance.TryToEnterRoomById("EditorTestRoom");
                yield break;
            }

            var requestMatchmaking = LOPWebAPI.RequestMatchmaking(new MatchmakingRequest
            {
                userId = LOP.Application.UserId,
                matchType = matchSelectData.currentMatchSetting.Value.matchType,
                subGameId = matchSelectData.currentMatchSetting.Value.subGameId,
                mapId = matchSelectData.currentMatchSetting.Value.mapId,
            });

            yield return requestMatchmaking;

            if (!IsCurrent)
            {
                yield break;
            }

            if (requestMatchmaking.isSuccess == false || requestMatchmaking.response.code != ResponseCode.SUCCESS)
            {
                Debug.LogError($"Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다. error: {requestMatchmaking.error}");
                yield break;
            }

            FSM.MoveNext(MatchStateInput.MatchInWaitingRoomState);
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
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
