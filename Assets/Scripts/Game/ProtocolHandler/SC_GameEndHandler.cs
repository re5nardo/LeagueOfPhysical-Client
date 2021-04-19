using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.SceneManagement;

public class SC_GameEndHandler
{
    public static void Handle(IMessage msg)
    {
        SC_GameEnd gameEnd = msg as SC_GameEnd;

        PhotonNetwork.LeaveRoom();

        SceneManager.LoadScene("Lobby");
    }
}
