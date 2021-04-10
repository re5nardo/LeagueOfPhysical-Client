using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class CreateMatchmakingTicketRequest : HttpRequestBase
{
    public string userId;
    public string matchType;
    public string subGameId;
    public string mapId;
}
