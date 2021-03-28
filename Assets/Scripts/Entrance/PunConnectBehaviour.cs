using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using System;

public class PunConnectBehaviour : PunBehaviour
{
    public event Action<DisconnectCause> onFailedToConnectToPhoton;
    public event Action<DisconnectCause> onConnectionFail;
    public event Action onConnectedToMaster;
    public event Action onJoinedLobby;

    public override void OnFailedToConnectToPhoton(DisconnectCause cause)
    {
        onFailedToConnectToPhoton?.Invoke(cause);
    }

    public override void OnConnectionFail(DisconnectCause cause)
    {
        onConnectionFail?.Invoke(cause);
    }

    public override void OnConnectedToMaster()
    {
        onConnectedToMaster?.Invoke();
    }

    public override void OnJoinedLobby()
    {
        onJoinedLobby?.Invoke();
    }

    private void OnDestroy()
    {
        onFailedToConnectToPhoton = null;
        onConnectionFail = null;
        onConnectedToMaster = null;
        onJoinedLobby = null;
    }
}
