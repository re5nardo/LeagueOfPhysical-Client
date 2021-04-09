using UnityEngine;
using UniRx;

public class Lobby : MonoBehaviour
{
    public static readonly MessageBroker MessageBroker = new MessageBroker();

    private void OnDestroy()
    {
        MessageBroker.Dispose();
    }

    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }

    public void OnRequestMatchBtnClicked()
    {
        MessageBroker.Publish("OnRequestMatchmakingButtonClicked");
    }

    public void OnCancelMatchBtnClicked()
    {
        MessageBroker.Publish("OnCancelMatchmakingButtonClicked");
    }
    #endregion
}
