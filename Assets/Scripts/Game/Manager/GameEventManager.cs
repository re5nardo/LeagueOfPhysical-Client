using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkModel.Mirror;

public class GameEventManager : MonoBehaviour
{
    private RoomProtocolDispatcher roomProtocolDispatcher = null;
    private GameEventDispatcher gameEventDispatcher = null;

    private void Awake()
    {
        roomProtocolDispatcher = gameObject.AddComponent<RoomProtocolDispatcher>();
        roomProtocolDispatcher[typeof(SC_GameEvents)] = (msg) =>
        {
            SC_GameEvents gameEvents = msg as SC_GameEvents;

            gameEvents.listGameEvent.ForEach(gameEvent =>
            {
                gameEventDispatcher.DispatchGameEvent(gameEvent);
            });
        };

        gameEventDispatcher = gameObject.AddComponent<GameEventDispatcher>();
    }
}
