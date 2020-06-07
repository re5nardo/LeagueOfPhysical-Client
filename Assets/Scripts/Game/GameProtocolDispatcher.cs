using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class GameProtocolDispatcher : MonoBehaviour
{
    private Dictionary<byte, Action<IPhotonEventMessage>> protocolHandlers = new Dictionary<byte, Action<IPhotonEventMessage>>();

    private void Awake()
    {
        protocolHandlers.Add(PhotonEvent.SC_EnterRoom, LOP.Game.Current.OnEnterRoom);
    }

    private void OnDestroy()
    {
        protocolHandlers.Clear();
    }

    public void DispatchProtocol(IPhotonEventMessage msg)
    {
        Action<IPhotonEventMessage> handler = null;

        if (protocolHandlers.TryGetValue(msg.GetEventID(), out handler))
        {
            handler?.Invoke(msg);
        }
    }
}
