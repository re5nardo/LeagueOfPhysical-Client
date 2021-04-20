﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class MatchStateCheck : MonoStateBase
    {
        public override void Enter()
        {
            base.Enter();

            CheckMatchState();
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
                        case "inWaitingRoom":
                            FSM.MoveNext(MatchStateInput.MatchInWaitingRoomState);
                            break;

                        case "inGameRoom":
                            FSM.MoveNext(MatchStateInput.MatchInGameRoomState);
                            break;

                        default:
                            FSM.MoveNext(MatchStateInput.MatchIdleState);
                            break;
                    }
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
