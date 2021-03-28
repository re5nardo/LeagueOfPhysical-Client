using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLocationComponent : EntranceComponent
{
    public override void OnStart()
    {
        LOPWebAPI.GetUserMatchState(PhotonNetwork.AuthValues.UserId,
            result =>
            {
                if (result.code != 200)
                {
                    Debug.LogError("Match 상태를 받아오는데 실패하였습니다.");
                    return;
                }

                switch(result.userMatchState.state)
                {
                    case "inWaitingRoom":
                        logger?.Invoke("로비에 접속중입니다.");
                        SceneManager.LoadScene("Lobby");
                        IsSuccess = true;
                        break;

                    case "inGameRoom":
                        RoomConnector.TryToEnterRoom(result.userMatchState.stateValue);
                        IsSuccess = true;
                        break;

                    default:
                        logger?.Invoke("로비에 접속중입니다.");
                        SceneManager.LoadScene("Lobby");
                        IsSuccess = true;
                        break;
                }
            },
            error =>
            {
                Debug.LogError("Match 상태를 받아오는데 실패하였습니다.");
            }
        );
    }
}
