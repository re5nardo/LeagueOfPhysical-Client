using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMatchmakingTicketRequest : LOPHttpRequestBase
{
    public string userId;
    public string gameType;
    public string matchType;
    public int rating;
}
