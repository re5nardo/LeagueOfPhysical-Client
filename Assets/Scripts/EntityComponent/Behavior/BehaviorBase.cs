﻿using System.Collections;
using UnityEngine;
using System;
using System.Linq;
using GameFramework;
using Entity;

namespace Behavior
{
    public abstract class BehaviorBase : MonoComponentBase, ISynchronizable
    {
        public event Action<BehaviorBase> onBehaviorEnd = null;

        protected virtual void OnBehaviorStart() { }
        protected abstract bool OnBehaviorUpdate();			//  False : Finish
        protected virtual void OnBehaviorEnd() { }			//  중간에 Stop 된 경우에도 호출됨

        protected int m_nBehaviorMasterID = -1;
        protected int startTick = -1;
        protected int lastTick = -1;

        private bool isPlaying = false;

        #region ISynchronizable
        public virtual bool Enable { get; set; } = true;
        public bool HasCoreChange => LastSendSnap == null ? true : !LastSendSnap.EqualsCore(CurrentSnap.Set(this));
        public bool IsDirty => isDirty || LastSendSnap == null ? true : !LastSendSnap.EqualsValue(CurrentSnap.Set(this));
        #endregion

        private bool isDirty = false;
        protected virtual ISnap LastSendSnap { get; set; } = new BehaviorSnap();
        protected virtual ISnap CurrentSnap { get; set; } = new BehaviorSnap();

        protected float DeltaTime
        {
            get
            {
                return lastTick == -1 ? CurrentUpdateTime : CurrentUpdateTime - LastUpdateTime;
            }
        }

        protected float CurrentUpdateTime
        {
            get
            {
                return Game.Current.CurrentTick == 0 ? 0 : (Game.Current.CurrentTick - startTick + 1) * Game.Current.TickInterval;
            }
        }

        protected float LastUpdateTime
        {
            get
            {
                return lastTick == -1 ? -1 : (lastTick - startTick + 1) * Game.Current.TickInterval;
            }
        }

        new protected MonoEntityBase Entity = null;

        private MasterData.Behavior masterData = null;
        public MasterData.Behavior MasterData
        {
            get
            {
                if (masterData == null)
                {
                    masterData = MasterDataManager.Instance.GetMasterData<MasterData.Behavior>(m_nBehaviorMasterID);
                }

                return masterData;
            }
        }

		#region IComponent
		public override void OnAttached(IEntity entity)
		{
			base.OnAttached(entity);

			Entity = entity as MonoEntityBase;

            TickPubSubService.AddSubscriber("TickEnd", OnTickEnd);
        }

		public override void OnDetached()
		{
			base.OnDetached();

            TickPubSubService.RemoveSubscriber("TickEnd", OnTickEnd);

            Entity = null;
		}
        #endregion

        private void OnTickEnd(int tick)
        {
            UpdateSynchronizable();
        }

        public virtual void SetData(int nBehaviorMasterID, params object[] param)
        {
            m_nBehaviorMasterID = nBehaviorMasterID;
        }

        public void StartBehavior()
        {
            if (isPlaying == true)
            {
                Debug.LogWarning("Behavior is playing, StartBehavior() is ignored!");
                return;
            }

            isPlaying = true;

            StopPreviousBehaviors();

            startTick = Game.Current.CurrentTick;
            lastTick = -1;

            OnBehaviorStart();

            OnTick(Game.Current.CurrentTick);
        }

        private void StopPreviousBehaviors()
        {
            var behaviors = GetComponents<BehaviorBase>().ToList();
            behaviors.Remove(this);
            behaviors.RemoveAll(x => !x.IsPlaying());

            foreach (var behavior in behaviors)
            {
                if (!MasterData.CompatibleBehaviorIDs.Contains(behavior.GetBehaviorMasterID()))
                {
                    behavior.StopBehavior();
                }
            }
        }

        public void OnTick(int tick)
        {
            if (lastTick == tick)
            {
                //Debug.LogWarning("Tick() is ignored! lastTick == tick");
                return;
            }

            if (!OnBehaviorUpdate())
            {
                EndBehavior();
            }

            lastTick = tick;
        }

        private void EndBehavior()
        {
            isPlaying = false;

            OnBehaviorEnd();

            onBehaviorEnd?.Invoke(this);

            onBehaviorEnd = null;
        }

        public void StopBehavior()
        {
            if (isPlaying == false)
            {
                Debug.LogWarning("Behavior is not playing, StopBehavior() is ignored!");
                return;
            }

            EndBehavior();
        }

        public bool IsPlaying()
        {
            return isPlaying;
        }

        public int GetBehaviorMasterID()
        {
            return m_nBehaviorMasterID;
        }

        private void OnDisable()
        {
            if(IsPlaying())
            {
                StopBehavior();
            }
        }

        #region ISynchronizable
        public void SetDirty()
        {
            isDirty = true;
        }

        public virtual ISnap GetSnap()
        {
            throw new NotImplementedException();
        }

        public void UpdateSynchronizable()
        {
        }

        public virtual void SendSynchronization()
        {
            throw new NotImplementedException();
        }

        public virtual void OnReceiveSynchronization(ISnap snap)
        {
        }
        #endregion
    }
}
