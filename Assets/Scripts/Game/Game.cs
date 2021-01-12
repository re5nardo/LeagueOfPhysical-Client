using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework;
using Entity;

namespace LOP
{
    public partial class Game : GameFramework.Game
    {
        [SerializeField] private GameUI gameUI = null;

        public new static Game Current => GameFramework.Game.Current as Game;

        public GameUI GameUI => gameUI;
        public MyInfo MyInfo => myInfo;
        public GameEventManager GameEventManager => gameEventManager;

        private GameProtocolDispatcher gameProtocolDispatcher = null;
        private GameEventManager gameEventManager = null;
        private GameManager gameManager = null;
        private MyInfo myInfo = null;

        public override IEnumerator Initialize()
        {
            Physics.autoSimulation = false;

            tickUpdater = gameObject.AddComponent<TickUpdater>();
            gameProtocolDispatcher = gameObject.AddComponent<GameProtocolDispatcher>();
            gameEventManager = gameObject.AddComponent<GameEventManager>();
            gameManager = gameObject.AddComponent<GameManager>();
            myInfo = gameObject.AddComponent<MyInfo>();

            RoomNetwork.Instance.onMessage += OnNetworkMessage;

            tickUpdater.Initialize(1 / 30f, true, Room.Instance.Latency, OnTick, OnTickEnd);
            GameUI.Initialize();

            EntityManager.Instantiate();
            ResourcePool.Instantiate();
            FPM_Manager.Instantiate();

            Initialized = true;

            yield break;
        }

        protected override void Clear()
        {
            base.Clear();

            Physics.autoSimulation = true;

            GameUI.Clear();

            if (RoomNetwork.HasInstance())
            {
                RoomNetwork.Instance.onMessage -= OnNetworkMessage;
            }
        }

        protected override void OnBeforeRun()
        {
            InvokeRepeating("NotifyPlayerLookAtPosition", 0f, 0.1f);
        }

        private void OnTick(int tick)
        {
            TickPubSubService.Publish("EarlyTick", tick);
            TickPubSubService.Publish("Tick", tick);
            TickPubSubService.Publish("LateTick", tick);
        }

        private void OnTickEnd(int tick)
        {
            TickPubSubService.Publish("TickEnd", tick);
            TickPubSubService.Publish("LateTickEnd", tick);
        }
       
        private void OnNetworkMessage(IMessage msg, object[] objects)
        {
            gameProtocolDispatcher.DispatchProtocol(msg as IPhotonEventMessage);
        }

        private void NotifyPlayerLookAtPosition()
        {
            CS_NotifyPlayerLookAtPosition notifyPlayerLookAtPosition = new CS_NotifyPlayerLookAtPosition();
            notifyPlayerLookAtPosition.m_vec3Position = GameUI.CameraController.GetLookAtPosition();

            RoomNetwork.Instance.Send(notifyPlayerLookAtPosition, PhotonNetwork.masterClient.ID);
        }

        public void OnMyCharacterCreated(Character character)
        {
            GameUI.CameraController.SetTarget(character.ModelTransform);
            GameUI.CameraController.StartFollowTarget();

            GameUI.PlayerInputController.SetCharacterID(character.EntityID);
        }
    }
}
