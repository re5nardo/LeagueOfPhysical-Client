using UnityEngine;
using GameFramework;

public class Lobby : MonoBehaviour
{
    public static readonly SimplePubSubService<string, object> Default = new SimplePubSubService<string, object>();

    private void OnDestroy()
    {
        Default.Clear();
    }

    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }

    public void OnRequestMatchBtnClicked()
    {
        Default.Publish("OnRequestMatchmakingButtonClicked", null);
    }

    public void OnCancelMatchBtnClicked()
    {
        Default.Publish("OnCancelMatchmakingButtonClicked", null);
    }
    #endregion
}
