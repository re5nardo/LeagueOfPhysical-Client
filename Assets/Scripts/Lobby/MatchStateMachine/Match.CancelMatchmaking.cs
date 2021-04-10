using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class CancelMatchmaking : MonoStateBase
    {
        public override void Enter()
        {
            base.Enter();

            LOPWebAPI.CancelMatchmakingTicket(PhotonNetwork.AuthValues.UserId,
                result =>
                {
                    if (!IsCurrent) return;

                    if (result.code != 200)
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
