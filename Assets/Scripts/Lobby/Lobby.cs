using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class Lobby : MonoBehaviour
{
    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }

    public void OnRequestMatchBtnClicked()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "RequestMatch",
            GeneratePlayStreamEvent = true, // Optional - Shows this event in PlayStream
        },
        result =>
        {
        },
        error =>
        {
        });
    }
    #endregion
}
