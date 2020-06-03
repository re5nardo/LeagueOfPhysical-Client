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
        [SerializeField] private GameUI gameUI = null;

        public new static Game Current { get { return GameFramework.Game.Current as Game; } }

        public GameUI GameUI { get { return gameUI; } }
        public MyInfo MyInfo { get { return myInfo; } }
        public GameEventManager GameEventManager { get { return gameEventManager; } }

        private MyInfo myInfo = null;
        private GameEventManager gameEventManager = null;
        private GameProtocolDispatcher protocolDispatcher = null;

        public override IEnumerator Initialize()
        {
            yield return SceneManager.LoadSceneAsync("RiftOfSummoner", LoadSceneMode.Additive);

            Physics.autoSimulation = false;

            GameUI.Initialize();
            EntityManager.Instantiate();
            ResourcePool.Instantiate();

            myInfo = gameObject.AddComponent<MyInfo>();
            tickUpdater = gameObject.AddComponent<TickUpdater>();
            gameEventManager = gameObject.AddComponent<GameEventManager>();

            RoomNetwork.Instance.onMessage += OnNetworkMessage;

            tickUpdater.Initialize(1 / 30f, false, OnTick, OnTickEnd);
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

            if (tickUpdater != null)
            {
                Destroy(tickUpdater);
                tickUpdater = null;
            }

            if (gameEventManager != null)
            {
                Destroy(gameEventManager);
                gameEventManager = null;
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
            var entities = EntityManager.Instance.GetAllEntities().Cast<MonoEntityBase>().ToList();

            //  sort
            //  ...

            entities.ForEach(entity =>
            {
                //  Iterating중에 Entity가 Destroy 안되었는지 체크
                if (entity.IsValid)
                {
                    entity.Tick(tick);
                }
            });

            entities.ForEach(entity =>
            {
                if (entity.IsValid)
                {
                    entity.OnBeforePhysicsSimulation(tick);
                }
            });

            Physics.Simulate(TickInterval);

            entities.ForEach(entity =>
            {
                if (entity.IsValid)
                {
                    entity.OnAfterPhysicsSimulation(tick);
                }
            });
        }

        private void OnTickEnd(int tick)
        {
            //BroadCastGameEvent();

            //gameEvents.Clear();
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

            GameUI.PlayerInputUI.SetCharacterID(character.EntityID);
        }
    }
}
