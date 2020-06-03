using UnityEngine;

public class Lobby : MonoBehaviour
{
    #region Event Handler
    public void OnRoomBtnClicked()
    {
        RoomConnector.TryToEnterRoom("TestField_1");
    }
    #endregion
}
