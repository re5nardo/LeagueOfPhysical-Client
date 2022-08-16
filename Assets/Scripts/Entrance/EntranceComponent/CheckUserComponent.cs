using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using System;

public class CheckUserComponent : EntranceComponentBase
{
    public override async Task OnBeforeExecute()
    {
        Entrance.Instance.stateText.text = "���� ���¸� Ȯ�����Դϴ�.";
    }

    public override async Task OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);

        await getUser;

        if (getUser.isError)
        {
            throw new Exception($"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}");
        }

        if (getUser.response.code == ResponseCode.SUCCESS)
        {
            AppDataContainer.Get<UserData>().user = getUser.response.user;

            var verifyUserLocation = LOPWebAPI.VerifyUserLocation(getUser.response.user.id);
            await verifyUserLocation;

            if (verifyUserLocation.isError || verifyUserLocation.response.code != ResponseCode.SUCCESS)
            {
                throw new Exception($"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}");
            }

            AppDataContainer.Get<UserData>().user = verifyUserLocation.response.user;
        }
        else if (getUser.response.code == ResponseCode.USER_NOT_EXIST)
        {
            var createUser = LOPWebAPI.CreateUser(new CreateUserRequest
            {
                id = LOP.Application.UserId,
                nickname = $"{LOP.Application.UserId} nickname",
            });

            await createUser;

            if (createUser.isError || createUser.response.code != ResponseCode.SUCCESS)
            {
                throw new Exception($"���� ������ �����Ͽ����ϴ�. error: {getUser.error}");
            }

            AppDataContainer.Get<UserData>().user = createUser.response.user;
        }
        else
        {
            throw new Exception($"���� ���¸� �������µ� �����Ͽ����ϴ�. error: {getUser.error}");
        }
    }
}
