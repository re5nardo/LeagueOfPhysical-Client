using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobbyComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "로비에 입장중입니다.";
    }

    public override IEnumerator OnExecute()
    {
        var joinLobby = LOPWebAPI.JoinLobby(LOP.Application.UserId);

        yield return joinLobby;

        if (joinLobby.isError || joinLobby.response.code != ResponseCode.SUCCESS)
        {
            Entrance.Instance.stateText.text = "로비 접속에 실패하였습니다.";
            IsSuccess = false;
        }
        else
        {
            IsSuccess = true;
        }
    }
}
