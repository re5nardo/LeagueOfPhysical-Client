using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class CheckUserComponent : MonoEnumerator
{
    public override void OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "���� ���¸� Ȯ�����Դϴ�.";
    }

    public override IEnumerator OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);
        yield return getUser;

        if (getUser.isError)
        {
            IsSuccess = false;
            Entrance.Instance.stateText.text = $"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}";
            yield break;
        }

        if (getUser.response.code == ResponseCode.SUCCESS)
        {
            AppDataContainer.Get<UserData>().user = getUser.response.user;

            var verifyUserLocation = LOPWebAPI.VerifyUserLocation(getUser.response.user.id);
            yield return verifyUserLocation;

            if (verifyUserLocation.isError || verifyUserLocation.response.code != ResponseCode.SUCCESS)
            {
                IsSuccess = false;
                Entrance.Instance.stateText.text = $"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}";
                yield break;
            }

            AppDataContainer.Get<UserData>().user = verifyUserLocation.response.user;

            IsSuccess = true;
            yield break;
        }
        else if (getUser.response.code == ResponseCode.USER_NOT_EXIST)
        {
            var createUser = LOPWebAPI.CreateUser(new CreateUserRequest
            {
                id = LOP.Application.UserId,
                nickname = $"{LOP.Application.UserId} nickname",
            });
            yield return createUser;

            if (createUser.isError || createUser.response.code != ResponseCode.SUCCESS)
            {
                IsSuccess = false;
                Entrance.Instance.stateText.text = $"���� ������ �����Ͽ����ϴ�. error: {getUser.error}";
                yield break;
            }

            AppDataContainer.Get<UserData>().user = createUser.response.user;

            IsSuccess = true;
            yield break;
        }
        else
        {
            Entrance.Instance.stateText.text = $"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}";
            IsSuccess = false;
            yield break;
        }
    }
}
