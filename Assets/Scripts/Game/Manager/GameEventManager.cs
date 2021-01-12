using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    private RoomProtocolHandler roomProtocolHandler = null;
    private GameEventDispatcher gameEventDispatcher = null;

    private void Awake()
    {
        roomProtocolHandler = gameObject.AddComponent<RoomProtocolHandler>();
        roomProtocolHandler[typeof(SC_GameEvents)] = (msg) =>
        {
            SC_GameEvents gameEvents = msg as SC_GameEvents;

            gameEvents.gameEvents.ForEach(gameEvent =>
            {
                gameEventDispatcher.DispatchGameEvent(gameEvent);
            });
        };

        gameEventDispatcher = gameObject.AddComponent<GameEventDispatcher>();
    }
}
