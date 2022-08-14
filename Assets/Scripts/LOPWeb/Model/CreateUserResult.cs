using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Newtonsoft.Json.Linq;

public class CreateUserResult : HttpResultBase, IPostDeserialize
{
    public User user;

    public void OnPostDeserialize(string value)
    {
        var jObject = JObject.Parse(value);
        var locationDetail = jObject["user"]["locationDetail"];

        switch (user.location)
        {
            case Location.InWaitingRoom:
                user.locationDetail = locationDetail.ToObject<WaitingRoomLocationDetail>();
                break;

            case Location.InGameRoom:
                user.locationDetail = locationDetail.ToObject<GameRoomLocationDetail>();
                break;
        }

    }
}
