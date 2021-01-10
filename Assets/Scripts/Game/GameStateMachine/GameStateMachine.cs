using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using GameFramework;

public class GameStateMachine : MonoBehaviour, IFiniteStateMachine<GameStateBase, GameStateInput>
{
    public GameStateBase InitState => gameObject.GetOrAddComponent<WaitForPlayersState>();
    public GameStateBase CurrentState { get; private set; }

    private void Awake()
    {
        RoomNetwork.Instance.onMessage += OnNetworkMessage;
    }

    private void OnDestroy()
    {
        if (RoomNetwork.HasInstance())
        {
            RoomNetwork.Instance.onMessage -= OnNetworkMessage;
        }
    }

    private void OnNetworkMessage(IMessage msg, object[] objects)
    {
        if (msg is SC_GameState gameState)
        {
            CurrentState.OnGameStateMessage(gameState);
        }
    }

    public void StartStateMachine()
    {
        CurrentState = InitState;
        CurrentState.Enter();
    }
 
    private void Update()
    {
        CurrentState?.Execute();
    }

    public GameStateBase MoveNext(GameStateInput input)
    {
        var next = CurrentState.GetNext(input) as GameStateBase;

        if (CurrentState == next)
        {
            return CurrentState;
        }

        CurrentState.Exit();

        CurrentState = next;

        CurrentState.Enter();

        return CurrentState;
    }
}

public enum GameStateInput
{
    StateDone = 0,

    //  States
    WaitForPlayersState = 100,
    SubGameSelectionState = 101,
    SubGamePrepareState = 102,
    SubGameProgressState = 103,
    SubGameEndState = 104,
    MatchEndState = 105,
}
