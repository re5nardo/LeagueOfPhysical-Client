using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using UnityEngine.SceneManagement;
using NetworkModel.Mirror;
using Mirror;

public class SC_GameEndHandler
{
    public static void Handle(IMessage msg)
    {
        SC_GameEnd gameEnd = msg as SC_GameEnd;

        NetworkManager.singleton.StopClient();

        SceneManager.LoadScene("Lobby");
    }
}
