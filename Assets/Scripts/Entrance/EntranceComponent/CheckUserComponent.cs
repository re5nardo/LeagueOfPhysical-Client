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
        Entrance.Instance.stateText.text = "유저 상태를 확인중입니다.";
    }

    public override async Task OnExecute()
    {
        var getUser = LOPWebAPI.GetUser(LOP.Application.UserId);

        await getUser;

        if (getUser.isError)
        {
            throw new Exception($"유저 상태를 가져오는데 실패하였습니다. error: {getUser.error}");
        }

        if (getUser.response.code == ResponseCode.SUCCESS)
        {
            AppDataContainer.Get<UserData>().user = getUser.response.user;

            var verifyUserLocation = LOPWebAPI.VerifyUserLocation(getUser.response.user.id);
            await verifyUserLocation;

            if (verifyUserLocation.isError || verifyUserLocation.response.code != ResponseCode.SUCCESS)
            {
                throw new Exception($"유저 상태를 가져오는데 실패하였습니다. error: {getUser.error}");
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
                throw new Exception($"유저 생성에 실패하였습니다. error: {getUser.error}");
            }

            AppDataContainer.Get<UserData>().user = createUser.response.user;
        }
        else
        {
            throw new Exception($"유저 상태를 가져오는데 실패하였습니다. error: {getUser.error}");
        }
    }
}
