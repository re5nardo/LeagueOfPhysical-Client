using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using System;

namespace Match
{
    public class Idle : MonoStateBase
    {
        public override void Enter()
        {
            base.Enter();

            Lobby.Default.AddSubscriber("OnRequestMatchmakingButtonClicked", OnRequestMatchmakingButtonClicked);
        }

        public override void Exit()
        {
            base.Exit();

            Lobby.Default.RemoveSubscriber("OnRequestMatchmakingButtonClicked", OnRequestMatchmakingButtonClicked);
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

        private void OnRequestMatchmakingButtonClicked(object obj)
        {
            FSM.MoveNext(MatchStateInput.RequestMatchmaking);
        }
    }
}
