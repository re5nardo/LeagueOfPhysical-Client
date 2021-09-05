using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using NetworkModel.Mirror;

namespace LOP
{
    public partial class Game : GameFramework.Game
    {
        [SerializeField] private GameUI gameUI = null;

        public new static Game Current => GameFramework.Game.Current as Game;

        public GameUI GameUI => gameUI;
        public MyInfo MyInfo => myInfo;
        public GameEventManager GameEventManager => gameEventManager;
        public GameManager GameManager => gameManager;

        private RoomProtocolDispatcher roomProtocolDispatcher = null;
        private GameEventManager gameEventManager = null;
        private GameManager gameManager = null;
        private MyInfo myInfo = null;

        public override IEnumerator Initialize()
        {
            Physics.autoSimulation = false;

            tickUpdater = gameObject.AddComponent<LOPTickUpdater>();
            roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
            gameEventManager = gameObject.AddComponent<GameEventManager>();
            gameManager = gameObject.AddComponent<GameManager>();
            myInfo = gameObject.AddComponent<MyInfo>();

            roomProtocolDispatcher[typeof(SC_EnterRoom)] = LOP.Game.Current.OnEnterRoom;
            roomProtocolDispatcher[typeof(SC_SyncTick)] = SC_SyncTickHandler.Handle;
            roomProtocolDispatcher[typeof(SC_EmotionExpression)] = SC_EmotionExpressionHandler.Handle;
            roomProtocolDispatcher[typeof(SC_Synchronization)] = SC_SynchronizationHandler.Handle;
            roomProtocolDispatcher[typeof(SC_GameEnd)] = SC_GameEndHandler.Handle;

            tickUpdater.Initialize(1 / 30f, true, Room.Instance.Latency, OnTick, OnTickEnd, OnUpdateElapsedTime);
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
        }

        protected override void OnBeforeRun()
        {
            //InvokeRepeating("NotifyPlayerLookAtPosition", 0f, 0.1f);

            GameManager.StartGame();
        }

        private void OnTick(int tick)
        {
            TickPubSubService.Publish("EarlyTick", tick);
            TickPubSubService.Publish("Tick", tick);
            TickPubSubService.Publish("LateTick", tick);
        }

        private void OnTickEnd(int tick)
        {
            TickPubSubService.Publish("EarlyTickEnd", tick);
            TickPubSubService.Publish("TickEnd", tick);
            TickPubSubService.Publish("LateTickEnd", tick);
        }

        private void OnUpdateElapsedTime(float time)
        {
            TickPubSubService.Publish("BeforePhysicsSimulation", CurrentTick);

            Physics.Simulate(time);

            TickPubSubService.Publish("AfterPhysicsSimulation", CurrentTick);
        }

        private void NotifyPlayerLookAtPosition()
        {
            //CS_NotifyPlayerLookAtPosition notifyPlayerLookAtPosition = new CS_NotifyPlayerLookAtPosition();
            //notifyPlayerLookAtPosition.m_vec3Position = GameUI.CameraController.GetLookAtPosition();

            //RoomNetwork.Instance.Send(notifyPlayerLookAtPosition, PhotonNetwork.masterClient.ID);
        }

        public void OnMyCharacterCreated(Character character)
        {
            GameUI.CameraController.SetTarget(character.Transform);
            GameUI.CameraController.StartFollowTarget();

            GameUI.PlayerInputController.SetCharacterID(character.EntityID);
        }
    }
}
