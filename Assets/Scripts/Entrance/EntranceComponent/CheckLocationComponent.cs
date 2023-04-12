using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class CheckLocationComponent : EntranceComponentBase
{
    public override async Task OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "매치 상태를 확인중입니다.";
    }

    public override async Task OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);

        await getUser;

        if (getUser.isSuccess == false || getUser.response.code != ResponseCode.SUCCESS)
        {
            throw new Exception($"유저 정보를 받아오는데 실패하였습니다. error: {getUser.error}");
        }

        AppDataContainer.Get<UserData>().user = getUser.response.user;

        switch (getUser.response.user.location)
        {
            case Location.InGameRoom:
                var roomId = (getUser.response.user.locationDetail as GameRoomLocationDetail).gameRoomId;
                var getRoom = LOPWebAPI.GetRoom(roomId);

                await getRoom;

                if (getRoom.isSuccess == false || getRoom.response.code != ResponseCode.SUCCESS)
                {
                    throw new Exception($"룸 정보를 받아오는데 실패하였습니다. error: {getRoom.error}");
                }

                if (getRoom.response.room.status == RoomStatus.Ready || getRoom.response.room.status == RoomStatus.Playing)
                {
                    RoomConnector.Instance.TryToEnterRoomById(roomId);
                }
                break;

            case Location.InWaitingRoom:
            default:
                SceneManager.LoadScene("Lobby");
                break;
        }
    }
}
