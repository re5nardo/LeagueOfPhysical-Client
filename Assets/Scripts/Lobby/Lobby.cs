using UnityEngine;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    [SerializeField] private Text text = null;

    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }

    public void OnRequestMatchBtnClicked()
    {
        MatchStateManager.Instance.RequestMatch();

        Invoke("CheckMatchState", 0.5f);
    }

    public void OnCancelMatchRequestBtnClicked()
    {
        MatchStateManager.Instance.CancelMatchRequest();

        CheckMatchState();
    }
    #endregion

    private void CheckMatchState()
    {
        if (IsInvoking("CheckMatchState"))
        {
            CancelInvoke("CheckMatchState");
        }

        MatchStateManager.Instance.GetMatchState(
            result =>
            {
                switch ((string)result["state"])
                {
                    case "Matching":
                        text.text = "Matching중입니다.";
                        Invoke("CheckMatchState", 1f);
                        break;
                    case "Matched":
                        text.text = "Room에 입장합니다.";
                        RoomConnector.TryToEnterRoom((string)result["gameId"]);
                        break;
                    default:
                        text.text = "";
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
