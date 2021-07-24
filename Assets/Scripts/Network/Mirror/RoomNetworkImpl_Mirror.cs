using UnityEngine;
using System;
using GameFramework;
using Mirror;

public class RoomNetworkImpl_Mirror : MonoBehaviour, INetworkImpl
{
    public Action<IMessage> OnMessage { get; set; }

    private void Awake()
    {
        RegisterMessage();
    }

    private void OnDestroy()
    {
        OnMessage = null;

        UnregisterMessage();
    }

    public void Send(IMessage msg, int targetId, bool reliable = true, bool instant = false)
    {
        IMirrorMessage mirrorMessage = msg as IMirrorMessage;

        CustomMirrorMessage customMirrorMessage = new CustomMirrorMessage();
        customMirrorMessage.id = mirrorMessage.GetMessageId();
        customMirrorMessage.payload = mirrorMessage;

        NetworkClient.Send(customMirrorMessage);
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

    private void InternalOnMessage(IMessage iMessage)
    {
        OnMessage?.Invoke(iMessage);
    }

    private void RegisterMessage()
    {
        NetworkClient.RegisterHandler<CustomMirrorMessage>(message =>
        {
            InternalOnMessage(message.payload);
        });
    }

    private void UnregisterMessage()
    {
        NetworkClient.UnregisterHandler<CustomMirrorMessage>();
    }
}
