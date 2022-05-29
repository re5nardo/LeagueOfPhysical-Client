using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckUserComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "유저 상태를 확인중입니다.";
    }

    public override IEnumerator OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);

        yield return getUser;

        if (getUser.isError)
        {
            IsSuccess = false;
            yield break;
        }

        if (getUser.response.code == ResponseCode.SUCCESS)
        {
            IsSuccess = true;
            yield break;
        }
        else if (getUser.response.code == ResponseCode.USER_NOT_EXIST)
        {
            yield return CreateUser();
        }
        else
        {
            IsSuccess = false;
            yield break;
        }

        IsSuccess = true;
    }

    private IEnumerator CreateUser()
    {
        var createUser = LOPWebAPI.CreateUser(new CreateUserRequest
        {
            id = LOP.Application.UserId,
            nickname = $"{LOP.Application.UserId} nickname",
        });

        yield return createUser;
    }
}
