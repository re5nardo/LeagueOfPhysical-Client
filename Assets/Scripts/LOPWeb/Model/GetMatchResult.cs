using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using System;

public class GetMatchResult : HttpResultBase
{
    public MatchResponse match;
}

[Serializable]
public class MatchResponse
{
    public string id;
    public MatchType matchType;
    public string subGameId;
    public string mapId;
    public MatchStatus status;
    public string[] playerList;
}

public enum MatchStatus
{
    None = 0,
    MatchStart = 1,
    MatchEnd = 2,
}
