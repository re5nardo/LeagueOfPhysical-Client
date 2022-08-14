using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework;

public class CheckLocationComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "매치 상태를 확인중입니다.";
    }

    public override IEnumerator OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);
        yield return getUser;

        if (getUser.isError || getUser.response.code != ResponseCode.SUCCESS)
        {
            Entrance.Instance.stateText.text = "유저 정보를 받아오는데 실패하였습니다.";
            IsSuccess = false;
            yield break;
        }

        AppDataContainer.Get<UserData>().user = getUser.response.user;

        switch (getUser.response.user.location)
        {
            case Location.InGameRoom:
                var roomId = (getUser.response.user.locationDetail as GameRoomLocationDetail).gameRoomId;
                var getRoom = LOPWebAPI.GetRoom(roomId);
                yield return getRoom;

                if (getRoom.isError || getRoom.response.code != ResponseCode.SUCCESS)
                {
                    Entrance.Instance.stateText.text = "룸 정보를 받아오는데 실패하였습니다.";
                    IsSuccess = false;
                    yield break;
                }

                if (getRoom.response.room.status == RoomStatus.Ready || getRoom.response.room.status == RoomStatus.Playing)
                {
                    RoomConnector.Instance.TryToEnterRoomById(roomId);
                    IsSuccess = true;
                }
                break;

            case Location.InWaitingRoom:
            default:
                SceneManager.LoadScene("Lobby");
                IsSuccess = true;
                break;
        }
    }
}
