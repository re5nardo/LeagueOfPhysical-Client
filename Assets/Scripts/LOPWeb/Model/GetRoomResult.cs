using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class GetRoomResult : HttpResultBase
{
    public RoomResponse room;
}

[Serializable]
public class RoomResponse
{
    public string id;
    public string matchId;
    public MatchType matchType;
    public string subGameId;
    public string mapId;
    public RoomStatus status;
    public string ip;
    public int port;
}

public enum RoomStatus
{
    None = 0,
    Spawning = 1,
    Spawned = 2,
    Ready = 3,
    Playing = 4,
    Finished = 5,
}

