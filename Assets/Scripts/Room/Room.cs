﻿using System.Collections;
using UnityEngine;
using Photon;
using GameFramework;

namespace LOP
{
    public class Room : MonoSingleton<Room>
    {
        [SerializeField] private Game game = null;

        public float Latency { get; private set; } = 0.04f;     //  sec     실제 ping data 활용해야 함

        private RoomPunBehaviour roomPunBehaviour = null;

        #region MonoBehaviour
        private IEnumerator Start()
        {
            yield return Initialize();

            PhotonNetwork.isMessageQueueRunning = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Clear();
        }
        #endregion

        private IEnumerator Initialize()
        {
            roomPunBehaviour = gameObject.AddComponent<RoomPunBehaviour>();

            yield return game.Initialize();
        }

        private void Clear()
        {
        }
    }
}
