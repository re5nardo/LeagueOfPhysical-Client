using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinLobbyComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "�κ� �������Դϴ�.";
    }

    public override IEnumerator OnExecute()
    {
        var joinLobby = LOPWebAPI.JoinLobby(LOP.Application.UserId);

        yield return joinLobby;

        if (joinLobby.isError || joinLobby.response.code != ResponseCode.SUCCESS)
        {
            Entrance.Instance.stateText.text = "�κ� ���ӿ� �����Ͽ����ϴ�.";
            IsSuccess = false;
        }
        else
        {
            IsSuccess = true;
        }
    }
}
