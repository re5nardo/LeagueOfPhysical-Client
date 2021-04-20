using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class RequestMatchmaking : MonoStateBase
    {
        public override void Enter()
        {
            base.Enter();

            var matchSelectData = SceneDataContainer.Get<MatchSelectData>();

            LOPWebAPI.CreateMatchmakingTicket(new CreateMatchmakingTicketRequest
            {
                userId = PhotonNetwork.AuthValues.UserId,
                matchType = matchSelectData.currentMatchSetting.Value.matchType.ToString(),
                subGameId = matchSelectData.currentMatchSetting.Value.subGameId,
                mapId = matchSelectData.currentMatchSetting.Value.mapId,
            },
            result =>
            {
                if (!IsCurrent) return;

                if (result.code != ResponseCode.SUCCESS)
                {
                    Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
                    return;
                }

                FSM.MoveNext(MatchStateInput.MatchInWaitingRoomState);
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
                case MatchStateInput.MatchInWaitingRoomState:
                    return gameObject.GetOrAddComponent<InWaitingRoom>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
