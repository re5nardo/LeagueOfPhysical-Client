using UnityEngine;
using UniRx;

public class Lobby : MonoBehaviour
{
    public static readonly MessageBroker Default = new MessageBroker();

    private void OnDestroy()
    {
        Default.Dispose();
    }

    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }

    public void OnRequestMatchBtnClicked()
    {
        Default.Publish("OnRequestMatchmakingButtonClicked");
    }

    public void OnCancelMatchBtnClicked()
    {
        Default.Publish("OnCancelMatchmakingButtonClicked");
    }
    #endregion
}
