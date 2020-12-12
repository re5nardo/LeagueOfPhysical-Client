using System.Collections;
using UnityEngine;
using Photon;
using GameFramework;

namespace LOP
{
    public class Room : PunBehaviour
    {
        public static Room Instance { get; private set; }

        public float Latency { get; private set; } = 0.03f;     //  sec

        private RoomProtocolDispatcher protocolDispatcher = null;
        private Game game = null;

        public static bool IsInstantiated()
        {
            return Instance != null;
        }

        #region MonoBehaviour
        private void Awake()
        {
            Instance = this;
        }

        private IEnumerator Start()
        {
            yield return StartCoroutine(Initialize());

            PhotonNetwork.isMessageQueueRunning = true;
        }

        private void OnDestroy()
        {
            Clear();

            Instance = null;
        }
        #endregion

        #region PunBehaviour
        public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
        {
            Debug.LogError("[OnMasterClientSwitched] Error!");
        }
        #endregion

        private IEnumerator Initialize()
        {
            protocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
            game = GetGame();

            yield return StartCoroutine(game.Initialize());
            
            RoomNetwork.Instance.onMessage += OnNetworkMessage;
        }

        private Game GetGame()
        {
            return FindObjectOfType<Game>();
        }

        private void Clear()
        {
            if (protocolDispatcher != null)
            {
                Destroy(protocolDispatcher);
                protocolDispatcher = null;
            }

            if (RoomNetwork.HasInstance())
            {
                RoomNetwork.Instance.onMessage -= OnNetworkMessage;
            }
        }

        private void OnNetworkMessage(IMessage msg, object[] objects)
        {
            protocolDispatcher.DispatchProtocol(msg as IPhotonEventMessage);
        }
    }
}
