using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GetUserMatchStateResult : LOPHttpResultBase
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
