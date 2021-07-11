using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;

public class RoomNetworkImpl_PUN : MonoBehaviour, INetworkImpl
{
    public Action<IMessage> OnMessage { get; set; }

	private void Awake()
	{
		PhotonNetwork.OnEventCall += OnEvent;
	}

	private void OnDestroy()
	{
        OnMessage = null;

		PhotonNetwork.OnEventCall -= OnEvent;
	}

	public void Send(IMessage msg, int targetId, bool reliable = true, bool instant = false)
	{
		IPhotonEventMessage eventMsg = msg as IPhotonEventMessage;
        eventMsg.senderID = PhotonNetwork.player.ID;

		PhotonNetwork.RaiseEvent(eventMsg.GetEventID(), msg, reliable, new RaiseEventOptions { TargetActors = new int[] { targetId } });

		if (instant)
		{
			PhotonNetwork.SendOutgoingCommands();
		}
	}

    public void Send(IMessage msg, ulong targetId, bool reliable = true, bool instant = false)
    {
        throw new NotImplementedException();
    }

    public void SendToAll(IMessage msg, bool reliable = true, bool instant = false)
	{
        throw new NotImplementedException();
    }

	public void SendToNear(IMessage msg, Vector3 center, float radius, bool reliable = true, bool instant = false)
	{
        throw new NotImplementedException();
    }
    
    #region PhotonEvent
    private void OnEvent(byte eventcode, object content, int senderId)
    {
        IPhotonEventMessage photonEventMessage = content as IPhotonEventMessage;
        photonEventMessage.senderID = senderId;

        OnMessage?.Invoke(photonEventMessage);
    }
    #endregion
}
