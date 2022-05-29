using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            Entrance.Instance.stateText.text = "Match 상태를 받아오는데 실패하였습니다.";
            IsSuccess = false;
            yield break;
        }

        switch (getUser.response.user.location)
        {
            case Location.InGameRoom:
                var roomId = (getUser.response.user.locationDetail as GameRoomLocationDetail).gameRoomId;
                RoomConnector.Instance.TryToEnterRoomById(roomId);
                break;

            case Location.InWaitingRoom:
            default:
                SceneManager.LoadScene("Lobby");
                break;
        }

        IsSuccess = true;
    }
}
