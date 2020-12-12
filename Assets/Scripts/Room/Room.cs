using System.Collections;
using UnityEngine;
using Photon;
using GameFramework;

namespace LOP
{
    public class Room : MonoSingleton<Room>
    {
        [SerializeField] private Game game = null;
        [SerializeField] private RoomProtocolDispatcher roomProtocolDispatcher = null;
        [SerializeField] private RoomPunBehaviour roomPunBehaviour = null;

        public float Latency { get; private set; } = 0.03f;     //  sec

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
            yield return game.Initialize();
            
            RoomNetwork.Instance.onMessage += OnNetworkMessage;
        }

        private void Clear()
        {
            if (RoomNetwork.HasInstance())
            {
                RoomNetwork.Instance.onMessage -= OnNetworkMessage;
            }
        }

        private void OnNetworkMessage(IMessage msg, object[] objects)
        {
            roomProtocolDispatcher.DispatchProtocol(msg as IPhotonEventMessage);
        }
    }
}
