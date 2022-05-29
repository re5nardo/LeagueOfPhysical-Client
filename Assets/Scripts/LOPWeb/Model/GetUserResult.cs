using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;

public class GetUserResult : HttpResultBase
{
    public User user;
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
    InGameRoom = 1,
    InWaitingRoom = 2,
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
}
