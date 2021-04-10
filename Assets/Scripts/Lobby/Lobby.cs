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
    public void OnRequestMatchBtnClicked()
    {
        MessageBroker.Publish("OnRequestMatchingButtonClicked");
    }
    #endregion
}
