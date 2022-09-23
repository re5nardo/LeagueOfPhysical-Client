﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;

namespace State
{
	public abstract class StateBase : LOPMonoEntityComponentBase
    {
		public event Action<StateBase> onStateEnd = null;
        public event Action<StateBase> onLateStateEnd = null;

        public int MasterDataId { get; protected set; } = -1;
        public bool IsPlaying { get; private set; }
        public float Lifespan { get; protected set; }

        protected int startTick = -1;
        protected int lastTick = -1;

        protected double DeltaTime => lastTick == -1 ? CurrentUpdateTime : CurrentUpdateTime - LastUpdateTime;
        protected double CurrentUpdateTime => Game.Current.CurrentTick == 0 ? 0 : (Game.Current.CurrentTick - startTick + 1) * Game.Current.TickInterval;
        protected double LastUpdateTime => lastTick == -1 ? -1 : (lastTick - startTick + 1) * Game.Current.TickInterval;

        private StateMasterData masterData = null;
        public StateMasterData MasterData => masterData ?? (masterData = MasterDataUtil.Get<StateMasterData>(MasterDataId));

        protected virtual void OnInitialize(StateParam stateParam) { }
        protected virtual void OnAccumulate(StateParam stateParam) { }
        protected virtual void OnStateStart() { }
		protected abstract bool OnStateUpdate();
        protected virtual void OnStateEnd() { }

        public void Initialize(StateParam stateParam)
		{
            this.MasterDataId = stateParam.masterDataId;

            MasterData.stateAttributes?.ForEach(stateAttribute =>
            {
                StateAttributeDispatcher.Dispatch(this, stateAttribute);
            });

            OnInitialize(stateParam);
        }

        public void Accumulate(StateParam stateParam)
        {
            OnAccumulate(stateParam);
        }

        public void StartState()
        {
            if (IsPlaying == true)
            {
                Debug.LogWarning("State is playing, StartState() is ignored!");
                return;
            }

            IsPlaying = true;

            startTick = Game.Current.CurrentTick;
            lastTick = -1;

            OnStateStart();

            OnTick(Game.Current.CurrentTick);
        }

        public void OnTick(int tick)
        {
            if (lastTick == tick)
            {
                return;
            }

            if (!OnStateUpdate())
            {
                EndState();
            }

            lastTick = tick;
        }

        private void EndState()
        {
            IsPlaying = false;

            OnStateEnd();

            onStateEnd?.Invoke(this);
            onStateEnd = null;

            onLateStateEnd?.Invoke(this);
            onLateStateEnd = null;
        }

        public void StopState()
        {
            if (IsPlaying == false)
            {
                Debug.LogWarning("State is not playing, StopState() is ignored!");
                return;
            }

            EndState();
        }
	}
}
