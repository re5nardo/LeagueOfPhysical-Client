using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_GameEventsHandler : IHandler<IPhotonEventMessage>
{
    public void Handle(IPhotonEventMessage msg)
    {
        SC_GameEvents gameEvents = msg as SC_GameEvents;

        LOP.Game.Current.GameEventManager.Apply(gameEvents.gameEvents);
    }
}
