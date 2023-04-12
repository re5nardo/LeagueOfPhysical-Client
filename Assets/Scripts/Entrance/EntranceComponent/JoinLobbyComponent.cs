using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class JoinLobbyComponent : EntranceComponentBase
{
    public override async Task OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "로비에 입장중입니다.";
    }

    public override async Task OnExecute()
    {
        var joinLobby = LOPWebAPI.JoinLobby(LOP.Application.UserId);

        await joinLobby;

        if (joinLobby.isSuccess == false || joinLobby.response.code != ResponseCode.SUCCESS)
        {
            throw new Exception($"로비 접속에 실패하였습니다. error: {joinLobby.error}");
        }
    }
}
