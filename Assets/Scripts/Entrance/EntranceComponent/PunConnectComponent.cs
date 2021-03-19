using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunConnectComponent : EntranceComponent
{
    [SerializeField] private PunConnectBehaviour punConnectBehaviour;

    private void Awake()
    {
        punConnectBehaviour.onConnectedToMaster += () => ConnectToLobby();
        punConnectBehaviour.onFailedToConnectToPhoton += cause => logger(cause.ToString());
        punConnectBehaviour.onConnectionFail += cause => logger(cause.ToString());
        punConnectBehaviour.onJoinedLobby += () => IsSuccess = true;
    }

    public override void OnStart()
    {
        ConnectToMasterServer();
    }

    private void ConnectToMasterServer()
    {
        logger?.Invoke("PUN 마스터 서버에 접속중입니다.");

        PhotonNetwork.ConnectUsingSettings("v1.0");
    }

    private void ConnectToLobby()
    {
        logger?.Invoke("PUN 로비에 접속중입니다.");

        PhotonNetwork.JoinLobby();
    }
}
