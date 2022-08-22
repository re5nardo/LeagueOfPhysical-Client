using System.Collections;
using UnityEngine;
using GameFramework;
using Mirror;
using NetworkModel.Mirror;
using System;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace LOP
{
    public class Room : MonoSingleton<Room>
    {
        public string RoomId { get; private set; }

        [SerializeField] private Game game = null;

        public float Latency => (float)Mirror.NetworkTime.rtt * 0.5f;

        private RoomProtocolDispatcher roomProtocolDispatcher;

        private SC_EnterRoom enterRoom;

        #region MonoBehaviour
        private async void Start()
        {
            try
            {
                await Initialize();
                await ConnectRoomServer();
                await JoinRoomServer();
                await StartGame();
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                SceneManager.LoadScene("Lobby");
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            Clear();
        }
        #endregion

        private async Task Initialize()
        {
            roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();

            RoomId = RoomConnector.Instance.RoomId;

            SceneDataContainer.Get<MatchData>().matchId = RoomConnector.Instance.MatchId;
            SceneDataContainer.Get<MatchData>().matchSetting = RoomConnector.Instance.MatchSetting;

            SceneMessageBroker.AddSubscriber<SC_EnterRoom>(OnSC_EnterRoom);

            await game.Initialize();
        }

        private async Task ConnectRoomServer()
        {
            if (NetworkClient.ready)
            {
                NetworkManager.singleton.StopClient();

                await UniTask.WaitUntil(() => !NetworkClient.ready);
            }

            NetworkManager.singleton.networkAddress = LOP.Application.IP == RoomConnector.Instance.Room.room.ip ? "localhost" : RoomConnector.Instance.Room.room.ip;
            (Transport.activeTransport as kcp2k.KcpTransport).Port = (ushort)RoomConnector.Instance.Room.room.port;

            NetworkManager.singleton.StartClient();

            await UniTask.WaitUntil(() => NetworkClient.ready);
        }

        private async Task JoinRoomServer()
        {
            await UniTask.WaitUntil(() => enterRoom != null);
        }

        private async Task StartGame()
        {
            SceneDataContainer.Get<MyInfo>().EntityId = enterRoom.entityId;

            //  SyncScope.Global
            enterRoom.syncControllerDataList.ForEach(item =>
            {
                using var disposer = PoolObjectDisposer<SC_SyncController>.Get();
                var message = disposer.PoolObject;
                message.syncControllerData = item;

                SceneMessageBroker.Publish(message);
            });

            enterRoom.syncDataEntries.ForEach(item =>
            {
                using var disposer = PoolObjectDisposer<SC_Synchronization>.Get();
                var message = disposer.PoolObject;
                message.syncDataEntry = item;

                SceneMessageBroker.Publish(message);
            });

            game.StartGame(enterRoom.tick);
        }

        private void Clear()
        {
            if (NetworkClient.ready)
            {
                NetworkManager.singleton.StopClient();
            }

            SceneMessageBroker.RemoveSubscriber<SC_EnterRoom>(OnSC_EnterRoom);

            enterRoom = null;
        }

        private void OnSC_EnterRoom(SC_EnterRoom enterRoom)
        {
            this.enterRoom = enterRoom;
        }
    }
}
