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
            RoomId = RoomConnector.Instance.Room.roomId;
            MatchSetting = RoomConnector.Instance.Room.matchSetting;

            yield return game.Initialize();
        }

        private void Clear()
        {
        }
    }
}
