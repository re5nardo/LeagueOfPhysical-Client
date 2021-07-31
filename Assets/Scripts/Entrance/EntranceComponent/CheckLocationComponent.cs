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
                if (result.code != ResponseCode.SUCCESS)
                {
                    Entrance.Instance.stateText.text = "Match 상태를 받아오는데 실패하였습니다.";
                    return;
                }

                switch(result.userMatchState.state)
                {
                    case "inGameRoom":
                        RoomConnector.Instance.TryToEnterRoomById(result.userMatchState.stateValue);
                        IsSuccess = true;
                        break;

                    case "inWaitingRoom":
                    default:
                        LOPWebAPI.JoinLobby(new JoinLobbyRequest
                        {
                            userId = PhotonNetwork.AuthValues.UserId
                        }, joinLobbyResult =>
                        {
                            SceneManager.LoadScene("Lobby");
                            IsSuccess = true;
                        }, error =>
                        {
                            Entrance.Instance.stateText.text = "Lobby 접속에 실패하였습니다.";
                        });
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
