using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;

public class GetUserMatchStateResult : HttpResultBase
{
    public UserMatchState userMatchState;
}

[Serializable]
public class UserMatchState
{
    public string state;
    public string stateValue;
    public string matchmakingTicketId;
}
