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

        public override void OnEnter()
        {
            GameLoadingView.Show();
        }

        public override IEnumerator OnExecute()
        {
            while (true)
            {
                yield return StartCoroutine(CheckMatchState());

                yield return new WaitForSeconds(CHECK_INTERVAL);
            }
        }

        public override void OnExit()
        {
            GameLoadingView.Hide();
        }

        private IEnumerator CheckMatchState()
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
                case Location.InGameRoom:
                    var roomId = (getUser.response.user.locationDetail as GameRoomLocationDetail).gameRoomId;

                    var getRoom = LOPWebAPI.GetRoom(roomId);
                    yield return getRoom;

                    if (getRoom.isSuccess == false)
                    {
                        Debug.LogError($"Room 정보를 받아오는데 실패하였습니다. 타이틀로 돌아갑니다. error: {getUser.error}");
                        yield break;
                    }

                    if (getRoom.response.room.status == RoomStatus.Ready || getRoom.response.room.status == RoomStatus.Playing)
                    {
                        RoomConnector.Instance.TryToEnterRoomById(roomId);
                    }
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

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }
    }
}
