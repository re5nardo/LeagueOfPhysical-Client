using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class GetUserResult : HttpResultBase
{
    public User user;

    public static GetUserResult Deserialize(string json)
    {
        var getUserResult = JsonConvert.DeserializeObject<GetUserResult>(json);

        try
        {
            var jObject = JObject.Parse(json);
            var locationDetail = jObject["user"]["locationDetail"];

            switch (getUserResult.user.location)
            {
                case Location.InWaitingRoom:
                    getUserResult.user.locationDetail = locationDetail.ToObject<WaitingRoomLocationDetail>();
                    break;

                case Location.InGameRoom:
                    getUserResult.user.locationDetail = locationDetail.ToObject<GameRoomLocationDetail>();
                    break;
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }

        return getUserResult;
    }
}

[Serializable]
public class User
{
    public string id;
    public string nickname;
    public int masterExp;
    public int friendlyRating;
    public int rankRating;
    public int goldCoin;
    public int gem;
    public Location location;
    public LocationDetail locationDetail;
}



[Serializable]
public enum Location
{
    Unknown = 0,
    InWaitingRoom = 1,
    InGameRoom = 2,
}

[Serializable]
public class LocationDetail
{
    public Location location;
}

[Serializable]
public class GameRoomLocationDetail : LocationDetail
{
    public string gameRoomId;
}

[Serializable]
public class WaitingRoomLocationDetail : LocationDetail
{
    public string waitingRoomId;
    public string matchmakingTicketId;
}
