using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;
using GameFramework;
using System.Threading.Tasks;

namespace LOP
{
    public partial class Game : GameFramework.Game
    {
        [SerializeField] private GameUI gameUI = null;

        public const int GRID_SIZE = 100;

        public new static Game Current => GameFramework.Game.Current as Game;

        public GameUI GameUI => gameUI;
        public GameEventManager GameEventManager { get; private set; }
        public GameStateMachine GameStateMachine { get; private set; }

        public SubGameData SubGameData => SubGameData.Get(SceneDataContainer.Get<MatchData>().matchSetting.subGameId);
        public MapData MapData => MapData.Get(SceneDataContainer.Get<MatchData>().matchSetting.mapId);

        public override async Task Initialize()
        {
            Physics.autoSimulation = false;

            tickUpdater = gameObject.AddComponent<LOPTickUpdater>();
            GameEventManager = gameObject.AddComponent<GameEventManager>();
            GameStateMachine = new GameObject("GameStateMachine").AddComponent<GameStateMachine>();
            GameStateMachine.StartStateMachine();

            gameObject.AddComponent<TickSyncController>();

            SceneMessageBroker.AddSubscriber<SC_EmotionExpression>(SC_EmotionExpressionHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_Synchronization>(SC_SynchronizationHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_GameEnd>(SC_GameEndHandler.Handle);
            SceneMessageBroker.AddSubscriber<SC_OwnerChanged>(SC_OwnerChangedHandler.Handle);

            tickUpdater.Initialize(1 / 30f, true, Room.Instance.Latency, OnTick, OnTickEnd, OnUpdateElapsedTime);
            GameUI.Initialize();

            EntityManager.Instantiate();
            ResourcePool.Instantiate();
            FPM_Manager.Instantiate();

            Initialized = true;
        }

        protected override void Clear()
        {
            base.Clear();

            Physics.autoSimulation = true;

            if (GameStateMachine != null)
            {
                Destroy(GameStateMachine.gameObject);
            }

            SceneMessageBroker.RemoveSubscriber<SC_EmotionExpression>(SC_EmotionExpressionHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_Synchronization>(SC_SynchronizationHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_GameEnd>(SC_GameEndHandler.Handle);
            SceneMessageBroker.RemoveSubscriber<SC_OwnerChanged>(SC_OwnerChangedHandler.Handle);

            GameUI.Clear();
        }

        protected override void OnBeforeRun()
        {
            //InvokeRepeating("NotifyPlayerLookAtPosition", 0f, 0.1f);

            GameStateMachine.MoveNext(GameStateInput.PrepareState);
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

        private void OnUpdateElapsedTime(double time)
        {
            SceneMessageBroker.Publish(new TickMessage.BeforePhysicsSimulation(CurrentTick));

            Physics.Simulate((float)time);

            SceneMessageBroker.Publish(new TickMessage.AfterPhysicsSimulation(CurrentTick));
        }

        private void NotifyPlayerLookAtPosition()
        {
            //CS_NotifyPlayerLookAtPosition notifyPlayerLookAtPosition = new CS_NotifyPlayerLookAtPosition();
            //notifyPlayerLookAtPosition.m_vec3Position = GameUI.CameraController.GetLookAtPosition();

            //RoomNetwork.Instance.Send(notifyPlayerLookAtPosition, PhotonNetwork.masterClient.ID);
        }

        public void StartGame(int tick)
        {
            GameUI.EmotionExpressionSelector.SetData(0, 1, 2, 3);   //  Dummy

            GameUI.CameraController.Target = Entities.MyCharacter.Transform;
            GameUI.CameraController.FollowTarget = true;

            GameUI.PlayerInputController.SetCharacterID(Entities.MyEntityID);

            Run(tick);
        }
    }
}
