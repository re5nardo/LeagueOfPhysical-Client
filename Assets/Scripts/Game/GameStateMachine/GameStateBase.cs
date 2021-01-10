using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;

public abstract class GameStateBase : MonoBehaviour, IState<GameStateInput>
{
    public IFiniteStateMachine<IState<GameStateInput>, GameStateInput> FSM
    {
        get => gameObject.GetOrAddComponent<GameStateMachine>() as IFiniteStateMachine<IState<GameStateInput>, GameStateInput>;
    }
  
    public virtual void Enter()
    {
    }

    public virtual void Execute()
    {
    }

    public virtual void Exit()
    {
    }

    public virtual void OnGameStateMessage(SC_GameState msg)
    {
    }

    public abstract IState<GameStateInput> GetNext(GameStateInput input);
}
