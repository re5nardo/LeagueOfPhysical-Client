using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using Newtonsoft.Json.Linq;

public class GetUserResult : HttpResultBase, IPostDeserialize
{
    public User user;

    public void OnPostDeserialize(string value)
    {
        try
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
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
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
