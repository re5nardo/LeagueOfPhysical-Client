using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;
using UniRx;

namespace Match
{
    public class Idle : MonoStateBase
    {
        private void Awake()
        {
            Lobby.MessageBroker.Receive<string>().Where(msg => msg == "OnRequestMatchingButtonClicked" && IsValid).Subscribe(OnRequestMatchmakingButtonClicked).AddTo(this);
        }

        public override void Enter()
        {
            base.Enter();

            MatchingWaitingView.Hide();
        }

        public override IState GetNext<I>(I input)
        {
            if (!input.TryParse(out MatchStateInput matchStateInput))
            {
                Debug.LogError($"Invalid input! input : {input}");
                return default;
            }

            switch (matchStateInput)
            {
                case MatchStateInput.RequestMatchmaking:
                    return gameObject.GetOrAddComponent<RequestMatchmaking>();
            }

            throw new Exception($"Invalid transition: {GetType().Name} with {matchStateInput}");
        }

        private void OnRequestMatchmakingButtonClicked(string message)
        {
            FSM.MoveNext(MatchStateInput.RequestMatchmaking);
        }
    }
}
