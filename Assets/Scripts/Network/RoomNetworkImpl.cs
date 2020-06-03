using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using GameFramework;

public class RoomNetworkImpl : MonoBehaviour, INetworkImpl
{
	private Action<IMessage, object[]> onMessage__;
	public Action<IMessage, object[]> onMessage
	{
		get { return onMessage__; }
		set { onMessage__ = value; }
	}

	private void Awake()
	{
		PhotonNetwork.OnEventCall += OnEvent;
	}

	private void OnDestroy()
	{
		onMessage = null;

		PhotonNetwork.OnEventCall -= OnEvent;
	}

	public void Send(IMessage msg, int nTargetID, bool bReliable = true, bool bInstant = false)
	{
		IPhotonEventMessage eventMsg = msg as IPhotonEventMessage;
        eventMsg.senderID = PhotonNetwork.player.ID;

		PhotonNetwork.RaiseEvent(eventMsg.GetEventID(), msg, bReliable, new RaiseEventOptions { TargetActors = new int[] { nTargetID } });

		if (bInstant)
		{
			PhotonNetwork.SendOutgoingCommands();
		}
	}

	public void SendToAll(IMessage msg, bool bReliable = true, bool bInstant = false)
	{
	}

	public void SendToNear(IMessage msg, Vector3 vec3Center, float fRadius, bool bReliable = true, bool bInstant = false)
	{
	}

	#region PhotonEvent
	private void OnEvent(byte eventcode, object content, int senderId)
	{
		onMessage?.Invoke(content as IMessage, new object[] { senderId });
	}
	#endregion
}
