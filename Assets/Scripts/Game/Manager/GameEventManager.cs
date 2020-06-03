using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GameEventManager : MonoBehaviour
{
    private GameEventDispatcher gameEventDispatcher = null;

    private void Awake()
    {
        gameEventDispatcher = gameObject.AddComponent<GameEventDispatcher>();
    }

    public void Apply(IGameEvent gameEvent)
    {
        gameEventDispatcher.DispatchGameEvent(gameEvent);
    }

    public void Apply(List<IGameEvent> gameEvents)
    {
        gameEvents.ForEach(x => Apply(x));
    }
}
