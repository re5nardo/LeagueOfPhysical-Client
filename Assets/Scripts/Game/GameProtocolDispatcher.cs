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
        protocolHandlers.Add(PhotonEvent.SC_EnterRoom,                  LOP.Game.Current.OnEnterRoom);
        protocolHandlers.Add(PhotonEvent.SC_GameEvents,                 SC_GameEventsHandler.Handle);
        protocolHandlers.Add(PhotonEvent.SC_SyncTick,                   SC_SyncTickHandler.Handle);
        protocolHandlers.Add(PhotonEvent.SC_EmotionExpression,          SC_EmotionExpressionHandler.Handle);
        protocolHandlers.Add(PhotonEvent.SC_PlayerMoveInputResponse,    SC_PlayerMoveInputResponseHandler.Handle);
        protocolHandlers.Add(PhotonEvent.SC_Synchronization,            SC_SynchronizationHandler.Handle);
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
