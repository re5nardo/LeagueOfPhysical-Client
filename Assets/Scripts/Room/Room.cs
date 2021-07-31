using System.Collections;
using UnityEngine;
using GameFramework;
using Mirror;

namespace LOP
{
    public class Room : MonoSingleton<Room>
    {
        public string RoomId { get; private set; }
        public MatchSetting MatchSetting { get; private set; } = new MatchSetting();

        [SerializeField] private Game game = null;

        public float Latency { get; private set; } = 0.04f;     //  sec     실제 ping data 활용해야 함

        #region MonoBehaviour
        private IEnumerator Start()
        {
            yield return Initialize();

            NetworkManager.singleton.networkAddress = LOP.Application.IP == RoomConnector.room.ip ? "localhost" : RoomConnector.room.ip;
            (Transport.activeTransport as kcp2k.KcpTransport).Port = (ushort)RoomConnector.room.port;
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
            RoomId = RoomConnector.room.roomId;
            MatchSetting = RoomConnector.room.matchSetting;

            yield return game.Initialize();
        }

        private void Clear()
        {
        }
    }
}
