using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class GameEventManager : MonoBehaviour
{
    private GameEventDispatcher gameEventDispatcher = null;

    private void Awake()
    {
        SceneMessageBroker.AddSubscriber<SC_GameEvents>(OnSC_GameEvents);

        gameEventDispatcher = gameObject.AddComponent<GameEventDispatcher>();
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<SC_GameEvents>(OnSC_GameEvents);
    }

    private void OnSC_GameEvents(SC_GameEvents gameEvents)
    {
        gameEvents.listGameEvent.ForEach(gameEvent =>
        {
            gameEventDispatcher.DispatchGameEvent(gameEvent);
        });
    }
}
