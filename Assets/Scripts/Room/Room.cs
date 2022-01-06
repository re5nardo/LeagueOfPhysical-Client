﻿using System.Collections;
using UnityEngine;
using GameFramework;
using Mirror;

namespace LOP
{
    public class Room : MonoSingleton<Room>
    {
        public string RoomId { get; private set; }

        [SerializeField] private Game game = null;

        public float Latency => (float)Mirror.NetworkTime.rtt * 0.5f;

        private RoomProtocolDispatcher roomProtocolDispatcher = null;

        #region MonoBehaviour
        private IEnumerator Start()
        {
            yield return Initialize();

            NetworkManager.singleton.networkAddress = LOP.Application.IP == RoomConnector.Instance.Room.ip ? "localhost" : RoomConnector.Instance.Room.ip;
            (Transport.activeTransport as kcp2k.KcpTransport).Port = (ushort)RoomConnector.Instance.Room.port;
            NetworkManager.singleton.StartClient();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Clear();
        }
        #endregion

        private IEnumerator Initialize()
        {
            roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();

            RoomId = RoomConnector.Instance.Room.roomId;

            SceneDataContainer.Get<MatchData>().matchId = RoomConnector.Instance.Room.matchId;
            SceneDataContainer.Get<MatchData>().matchSetting = RoomConnector.Instance.Room.matchSetting;

            yield return game.Initialize();
        }

        private void Clear()
        {
        }
    }
}
