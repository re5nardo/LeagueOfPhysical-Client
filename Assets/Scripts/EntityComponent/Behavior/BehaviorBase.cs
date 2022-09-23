﻿using System.Collections;
using UnityEngine;
using System;
using System.Linq;
using GameFramework;

namespace Behavior
{
    public abstract class BehaviorBase : LOPMonoEntityComponentBase
    {
        public event Action<BehaviorBase> onBehaviorEnd = null;
        public event Action<BehaviorBase> onLateBehaviorEnd = null;

        public int MasterDataId { get; protected set; } = -1;
        public bool IsPlaying { get; private set; }

        protected int startTick = -1;
        protected int lastTick = -1;

        protected double DeltaTime => Game.Current.CurrentTick == 0 ? 0 : Game.Current.TickInterval;
        protected double CurrentUpdateTime => Game.Current.CurrentTick == 0 ? 0 : (Game.Current.CurrentTick - startTick + 1) * Game.Current.TickInterval;
        protected double LastUpdateTime => lastTick == -1 ? -1 : (lastTick - startTick + 1) * Game.Current.TickInterval;

        private BehaviorMasterData masterData = null;
        public BehaviorMasterData MasterData => masterData ?? (masterData = MasterDataUtil.Get<BehaviorMasterData>(MasterDataId));

        protected virtual void OnInitialize(BehaviorParam behaviorParam) { }
        protected virtual void OnBehaviorStart() { }
        protected abstract bool OnBehaviorUpdate();
        protected virtual void OnBehaviorEnd() { }

        public void Initialize(BehaviorParam behaviorParam)
        {
            this.MasterDataId = behaviorParam.masterDataId;

            MasterData.behaviorAttributes?.ForEach(behaviorAttribute =>
            {
                BehaviorAttributeDispatcher.Dispatch(this, behaviorAttribute);
            });

            OnInitialize(behaviorParam);
        }

        public void StartBehavior()
        {
            if (IsPlaying == true)
            {
                Debug.LogWarning("Behavior is playing, StartBehavior() is ignored!");
                return;
            }

            IsPlaying = true;

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
            behaviors.RemoveAll(x => !x.IsPlaying);

            foreach (var behavior in behaviors)
            {
                if (!MasterData.compatibleBehaviors.Any(x => x.id == behavior.MasterDataId))
                {
                    behavior.StopBehavior();
                }
            }
        }

        public void OnTick(int tick)
        {
            if (lastTick == tick)
            {
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
            IsPlaying = false;

            OnBehaviorEnd();

            onBehaviorEnd?.Invoke(this);
            onBehaviorEnd = null;

            onLateBehaviorEnd?.Invoke(this);
            onLateBehaviorEnd = null;
        }

        public void StopBehavior()
        {
            if (IsPlaying == false)
            {
                Debug.LogWarning("Behavior is not playing, StopBehavior() is ignored!");
                return;
            }

            EndBehavior();
        }
    }
}
