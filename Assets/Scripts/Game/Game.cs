using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework;
using Entity;
using System.Linq;

namespace LOP
{
    public partial class Game : GameFramework.Game
    {
        public new static Game Current { get { return GameFramework.Game.Current as Game; } }

        public GameUI GameUI { get { return gameUI; } }
        public MyInfo MyInfo { get { return myInfo; } }
        public GameEventManager GameEventManager { get { return gameEventManager; } }

        private GameUI gameUI = null;
        private MyInfo myInfo = null;
        private GameEventManager gameEventManager = null;
        private GameProtocolDispatcher protocolDispatcher = null;

        public override IEnumerator Initialize()
        {
            GameFramework.Game.Current = this;

            yield return SceneManager.LoadSceneAsync("RiftOfSummoner", LoadSceneMode.Additive);

            Physics.autoSimulation = false;

            EntityManager.Instantiate();
            ResourcePool.Instantiate();
            FPM_Manager.Instantiate();

            gameUI = GetGameUI();
            myInfo = gameObject.AddComponent<MyInfo>();
            gameEventManager = gameObject.AddComponent<GameEventManager>();
            protocolDispatcher = gameObject.AddComponent<GameProtocolDispatcher>();
            tickUpdater = gameObject.AddComponent<TickUpdater>();

            GameUI.Initialize();
            tickUpdater.Initialize(1 / 30f, true, Room.Instance.Latency, OnTick, OnTickEnd);

            RoomNetwork.Instance.onMessage += OnNetworkMessage;

            initialized = true;
        }

        protected override void Clear()
        {
            base.Clear();

            GameUI.Clear();

            if (myInfo != null)
            {
                Destroy(myInfo);
                myInfo = null;
            }

            if (gameEventManager != null)
            {
                Destroy(gameEventManager);
                gameEventManager = null;
            }

            if (protocolDispatcher != null)
            {
                Destroy(protocolDispatcher);
                protocolDispatcher = null;
            }

            if (tickUpdater != null)
            {
                Destroy(tickUpdater);
                tickUpdater = null;
            }

            if (RoomNetwork.IsInstantiated())
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
        }

        private GameUI GetGameUI()
        {
            return FindObjectOfType<GameUI>();
        }

        private void OnNetworkMessage(IMessage msg, object[] objects)
        {
            protocolDispatcher.DispatchProtocol(msg as IPhotonEventMessage);
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
