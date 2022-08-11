using System.Collections;
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

        private RoomProtocolDispatcher roomProtocolDispatcher;

        #region MonoBehaviour
        private IEnumerator Start()
        {
            yield return Initialize();
            yield return ConnectRoomServer();

            //  룸 서버 세션 맺고, 세션이 정상적으로 맺어져야 해당 룸 존재 및 정상 입장 한 것으로 간주 JoinRoom(string userId) -> JoinRoomResponse(snapshot)
            //  Request RoomInfo -> Run Game
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

            RoomId = RoomConnector.Instance.RoomId;

            SceneDataContainer.Get<MatchData>().matchId = RoomConnector.Instance.MatchId;
            SceneDataContainer.Get<MatchData>().matchSetting = RoomConnector.Instance.MatchSetting;

            yield return game.Initialize();
        }

        private IEnumerator ConnectRoomServer()
        {
            if (NetworkClient.ready)
            {
                NetworkManager.singleton.StopClient();
            }

            yield return new WaitUntil(() => !NetworkClient.ready);

            NetworkManager.singleton.networkAddress = LOP.Application.IP == RoomConnector.Instance.Room.room.ip ? "localhost" : RoomConnector.Instance.Room.room.ip;
            (Transport.activeTransport as kcp2k.KcpTransport).Port = (ushort)RoomConnector.Instance.Room.room.port;

            NetworkManager.singleton.StartClient();

            yield return new WaitUntil(() => NetworkClient.ready);
        }

        private void Clear()
        {
            if (NetworkClient.ready)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
}
