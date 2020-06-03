﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.SceneManagement;

public class RoomConnector : PunBehaviour
{
    private bool tryingToEnterRoom = false;

    private static RoomConnector instance = null;
    private static RoomConnector Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("RoomConnector").AddComponent<RoomConnector>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public static void TryToEnterRoom(string roomName)
    {
        Instance?.InternalTryToEnterRoom(roomName);
    }

    private void InternalTryToEnterRoom(string roomName)
    {
        if (tryingToEnterRoom)
            return;

        tryingToEnterRoom = true;

        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CharacterID", Random.Range(MasterDataDefine.CharacterID.EVELYNN, MasterDataDefine.CharacterID.MALPHITE + 1) } });

        PhotonNetwork.JoinRoom(roomName);
    }

    #region MonoBehaviourPunCallbacks
    public override void OnJoinedRoom()
    {
        tryingToEnterRoom = false;

        Debug.Log("[Photon OnJoinedRoom]");

        PhotonNetwork.isMessageQueueRunning = false;

        SceneManager.LoadScene("Room");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        tryingToEnterRoom = false;

        Debug.Log(string.Format("[Photon OnPhotonJoinRoomFailed] returnCode : {0}, message : {1}", codeAndMsg[0], codeAndMsg[1]));
    }
    #endregion

}
