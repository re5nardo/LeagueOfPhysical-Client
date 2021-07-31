using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class GetRoomResult : HttpResultBase
{
    public string roomId;
    public string matchId;
    public MatchSetting matchSetting;
    public string ip;
    public int port;
}
