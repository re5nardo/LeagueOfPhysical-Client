using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckLocationComponent : EntranceComponent
{
    public override void OnStart()
    {
        //StartCoroutine(Body());

        SceneManager.LoadScene("Lobby");
        IsSuccess = true;
    }

    //private IEnumerator Body()
    //{
    //    CheckMatchState();

    //    IsSuccess = true;
    //}

    private void CheckMatchState()
    {
        if (IsInvoking("CheckMatchState"))
        {
            CancelInvoke("CheckMatchState");
        }

        MatchStateHelper.GetMatchState(
            result =>
            {
                switch ((string)result["state"])
                {
                    //case "Matching":
                    //    MatchStateManager.CancelMatchRequest();
                    //    Invoke("CheckMatchState", 1f);
                    //    break;
                    case "Matched":
                        Debug.Log((string)result["gameId"]);
                        RoomConnector.TryToEnterRoom((string)result["gameId"]);
                        break;
                    default:
                        logger?.Invoke("로비에 접속중입니다.");
                        SceneManager.LoadScene("Lobby");
                        break;
                }
            },
            error =>
            {
                Debug.LogError(error);
            }
        );
    }
}
