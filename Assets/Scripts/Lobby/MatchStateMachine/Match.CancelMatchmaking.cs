using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class CancelMatchmaking : MonoStateBase
    {
        public override void OnEnter()
        {
            LOPWebAPI.CancelMatchmaking(((WaitingRoomLocationDetail)AppDataContainer.Get<UserData>().user.locationDetail).matchmakingTicketId,
                result =>
                {
                    if (!IsCurrent) return;

                    if (result.code == ResponseCode.ALREADY_IN_GAME)
                    {
                        FSM.MoveNext(MatchStateInput.MatchInGameRoomState);
                        return;
                    }
                    else if (result.code == ResponseCode.MATCH_MAKING_TICKET_NOT_EXIST)
                    {
                        Debug.LogError("매치메이킹 티켓이 존재하지 않습니다.");
                        FSM.MoveNext(MatchStateInput.MatchIdleState);
                        return;
                    }
                    else if (result.code == ResponseCode.NOT_MATCH_MAKING_STATE)
                    {
                        Debug.LogError("매치메이킹 상태가 아니었습니다.");
                        FSM.MoveNext(MatchStateInput.MatchIdleState);
                        return;
                    }
                    else if (result.code != ResponseCode.SUCCESS)
                    {
                        Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
                        return;
                    }

                    FSM.MoveNext(MatchStateInput.MatchIdleState);
                },
                error =>
                {
                    Debug.LogError("Match 상태를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다.");
                }
            );
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
                case MatchStateInput.MatchIdleState:
                    return gameObject.GetOrAddComponent<Idle>();

                case MatchStateInput.MatchInGameRoomState:
                    return gameObject.GetOrAddComponent<InGameRoom>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
