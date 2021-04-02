using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLocationComponent : EntranceComponent
{
    public override void OnStart()
    {
        Entrance.Instance.stateText.text = "매치 상태를 확인중입니다.";

        LOPWebAPI.GetUserMatchState(PhotonNetwork.AuthValues.UserId,
            result =>
            {
                if (result.code != 200)
                {
                    Entrance.Instance.stateText.text = "Match 상태를 받아오는데 실패하였습니다.";
                    return;
                }

                switch(result.userMatchState.state)
                {
                    case "inWaitingRoom":
                        SceneManager.LoadScene("Lobby");
                        IsSuccess = true;
                        break;

                    case "inGameRoom":
                        RoomConnector.TryToEnterRoom(result.userMatchState.stateValue);
                        IsSuccess = true;
                        break;

                    default:
                        SceneManager.LoadScene("Lobby");
                        IsSuccess = true;
                        break;
                }
            },
            error =>
            {
                Entrance.Instance.stateText.text = "Match 상태를 받아오는데 실패하였습니다.";
            }
        );
    }
}
