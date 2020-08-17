using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SC_PlayerMoveInputResponseHandler : IHandler<IPhotonEventMessage>
{
    public void Handle(IPhotonEventMessage msg)
    {
        SC_PlayerMoveInputResponse playerMoveInputResponse = msg as SC_PlayerMoveInputResponse;

        FPM_Manager.Instance.OnPlayerMoveInputResponse(playerMoveInputResponse);
    }
}
