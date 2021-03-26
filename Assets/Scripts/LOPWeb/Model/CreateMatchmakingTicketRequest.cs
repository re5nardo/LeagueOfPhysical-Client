using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class CreateMatchmakingTicketRequest : HttpRequestBase
{
    public string userId;
    public string gameType;
    public string matchType;
    public int rating;
}
