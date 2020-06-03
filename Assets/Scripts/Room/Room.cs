using System.Collections;
using UnityEngine;
using Photon;
using GameFramework;

namespace LOP
{
    public class Room : PunBehaviour
    {
        private Game game = null;
        private RoomProtocolDispatcher protocolDispatcher = null;

        public static Room Instance { get; private set; }

        public static bool IsInstantiated()
        {
            return Instance != null;
        }

        #region MonoBehaviour
        private IEnumerator Start()
        {
            Instance = this;

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
            game = gameObject.AddComponent<Game>();
            yield return StartCoroutine(game.Initialize());

            protocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
            
            RoomNetwork.Instance.onMessage += OnNetworkMessage;
        }

        private void Clear()
        {
            if (game != null)
            {
                Destroy(game);
                game = null;
            }

            if (protocolDispatcher != null)
            {
                Destroy(protocolDispatcher);
                protocolDispatcher = null;
            }

            if (RoomNetwork.IsInstantiated())
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
