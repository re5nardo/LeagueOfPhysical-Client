using UnityEngine;
using UniRx;

public class Lobby : MonoBehaviour
{
    public static readonly MessageBroker MessageBroker = new MessageBroker();

    private void OnDestroy()
    {
        MessageBroker.Dispose();
    }
}
