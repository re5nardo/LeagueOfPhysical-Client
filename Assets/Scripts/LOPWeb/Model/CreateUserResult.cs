using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class CreateUserResult : HttpResponseBase
{
    public User user;

    public static CreateUserResult Deserialize(string json)
    {
        var createUserResult = JsonConvert.DeserializeObject<CreateUserResult>(json);

        try
        {
            var jObject = JObject.Parse(json);
            var locationDetail = jObject["user"]["locationDetail"];

            switch (createUserResult.user.location)
            {
                case Location.InWaitingRoom:
                    createUserResult.user.locationDetail = locationDetail.ToObject<WaitingRoomLocationDetail>();
                    break;

                case Location.InGameRoom:
                    createUserResult.user.locationDetail = locationDetail.ToObject<GameRoomLocationDetail>();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        return createUserResult;
    }
}
