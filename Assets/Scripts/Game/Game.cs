﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using NetworkModel.Mirror;
using GameFramework;

namespace LOP
{
    public partial class Game : GameFramework.Game
    {
        [SerializeField] private GameUI gameUI = null;

        public new static Game Current => GameFramework.Game.Current as Game;

        public GameUI GameUI => gameUI;
        public GameEventManager GameEventManager { get; private set; }
        public GameStateMachine GameStateMachine { get; private set; }

        public SubGameData SubGameData => SubGameData.Get(AppDataContainer.Get<MatchSettingData>().matchSetting.subGameId);
        public MapData MapData => MapData.Get(AppDataContainer.Get<MatchSettingData>().matchSetting.mapId);

        public override IEnumerator Initialize()
        {
            Physics.autoSimulation = false;

            tickUpdater = gameObject.AddComponent<LOPTickUpdater>();
            GameEventManager = gameObject.AddComponent<GameEventManager>();
            GameStateMachine = new GameObject("GameStateMachine").AddComponent<GameStateMachine>();
            GameStateMachine.StartStateMachine();

            gameObject.AddComponent<TickSyncController>();

            SceneMessageBroker.AddSubscriber<SC_EnterRoom>(OnEnterRoom);
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

            if (GameStateMachine != null)
            {
                Destroy(GameStateMachine.gameObject);
            }

            SceneMessageBroker.RemoveSubscriber<SC_EnterRoom>(OnEnterRoom);
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

                GameUI.CameraController.Target = character.Transform;
                GameUI.CameraController.FollowTarget = true;

                GameUI.PlayerInputController.SetCharacterID(character.EntityID);
            }
        }

        private void OnEnterRoom(SC_EnterRoom enterRoom)
        {
            SceneDataContainer.Get<MyInfo>().EntityId = enterRoom.entityId;

            GameUI.EmotionExpressionSelector.SetData(0, 1, 2, 3);   //  Dummy

            Run(enterRoom.tick);

            enterRoom.syncControllerDataList.ForEach(item =>
            {
                SceneMessageBroker.Publish(item);
            });

            enterRoom.syncDataEntries.ForEach(item =>
            {
                SceneMessageBroker.Publish(item);
            });

            //  최초에 동기화 어떻게 맞추지?? controllerId도 그렇고, 최초에 author한테 정보 받아야 할듯한데
        }
    }
}
