using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using Mirror;
using UnityEngine.SceneManagement;

namespace GameState
{
    public class EndState : MonoStateBase
    {
        public override void OnEnter()
        {
            if (NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }

            SceneManager.LoadScene("Lobby");
        }

        public override IState GetNext<I>(I input)
        {
            if (!Enum.TryParse(input.ToString(), out GameStateInput gameStateInput))
            {
                Debug.LogError($"Invalid input! input : {input}");
                return default;
            }

            switch (gameStateInput)
            {
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {gameStateInput}");
        }
    }
}
