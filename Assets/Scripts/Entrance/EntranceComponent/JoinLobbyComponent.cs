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
        Entrance.Instance.stateText.text = "�κ� �������Դϴ�.";
    }

    public override async Task OnExecute()
    {
        var joinLobby = LOPWebAPI.JoinLobby(LOP.Application.UserId);

        await joinLobby;

        if (joinLobby.isSuccess == false || joinLobby.response.code != ResponseCode.SUCCESS)
        {
            throw new Exception($"�κ� ���ӿ� �����Ͽ����ϴ�. error: {joinLobby.error}");
        }
    }
}
