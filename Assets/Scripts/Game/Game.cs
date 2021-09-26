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

        private GameEventManager gameEventManager = null;
        private GameManager gameManager = null;
        private MyInfo myInfo = null;

        public override IEnumerator Initialize()
        {
            Physics.autoSimulation = false;

            tickUpdater = gameObject.AddComponent<LOPTickUpdater>();
            gameEventManager = gameObject.AddComponent<GameEventManager>();
            gameManager = gameObject.AddComponent<GameManager>();
            myInfo = gameObject.AddComponent<MyInfo>();

            SceneMessageBroker.AddSubscriber<SC_EnterRoom>(OnEnterRoom);
            SceneMessageBroker.AddSubscriber<SC_SyncTick>(SC_SyncTickHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_EmotionExpression>(SC_EmotionExpressionHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_Synchronization>(SC_SynchronizationHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_GameEnd>(SC_GameEndHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_OwnerChanged>(SC_OwnerChangedHandler.Handle);
            SceneMessageBroker.AddSubscriber<GameMessage.EntityRegister>(OnEntityRegister);

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

            SceneMessageBroker.RemoveSubscriber<SC_EnterRoom>(OnEnterRoom);
            SceneMessageBroker.RemoveSubscriber<SC_SyncTick>(SC_SyncTickHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_EmotionExpression>(SC_EmotionExpressionHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_Synchronization>(SC_SynchronizationHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_GameEnd>(SC_GameEndHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_OwnerChanged>(SC_OwnerChangedHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<GameMessage.EntityRegister>(OnEntityRegister);

            GameUI.Clear();
        }

        protected override void OnBeforeRun()
        {
            //InvokeRepeating("NotifyPlayerLookAtPosition", 0f, 0.1f);

            GameManager.StartGame();
        }

        private void OnTick(int tick)
        {
            SceneMessageBroker.Publish(new TickMessage.EarlyTick(tick));
            SceneMessageBroker.Publish(new TickMessage.Tick(tick));
            SceneMessageBroker.Publish(new TickMessage.LateTick(tick));
        }

        private void OnTickEnd(int tick)
        {
            SceneMessageBroker.Publish(new TickMessage.EarlyTickEnd(tick));
            SceneMessageBroker.Publish(new TickMessage.TickEnd(tick));
            SceneMessageBroker.Publish(new TickMessage.LateTickEnd(tick));
        }

        private void OnUpdateElapsedTime(float time)
        {
            SceneMessageBroker.Publish(new TickMessage.BeforePhysicsSimulation(CurrentTick));

            Physics.Simulate(time);

            SceneMessageBroker.Publish(new TickMessage.AfterPhysicsSimulation(CurrentTick));
        }

        private void NotifyPlayerLookAtPosition()
        {
            //CS_NotifyPlayerLookAtPosition notifyPlayerLookAtPosition = new CS_NotifyPlayerLookAtPosition();
            //notifyPlayerLookAtPosition.m_vec3Position = GameUI.CameraController.GetLookAtPosition();

            //RoomNetwork.Instance.Send(notifyPlayerLookAtPosition, PhotonNetwork.masterClient.ID);
        }

        private void OnEntityRegister(GameMessage.EntityRegister entityRegister)
        {
            if (entityRegister.entityId == Entities.MyEntityID)
            {
                var character = Entities.Get<Character>(entityRegister.entityId);

                GameUI.CameraController.SetTarget(character.Transform);
                GameUI.CameraController.StartFollowTarget();

                GameUI.PlayerInputController.SetCharacterID(character.EntityID);
            }
        }

        private void OnEnterRoom(SC_EnterRoom enterRoom)
        {
            MyInfo.EntityID = enterRoom.entityId;
            GameUI.EmotionExpressionSelector.SetData(0, 1, 2, 3);   //  Dummy

            Run(enterRoom.tick);
        }
    }
}
