using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class LOPWebAPI
{
    public static void CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbPUT, "/match/matchmakingTicket", request, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static void CancelMatchmakingTicket(string userId, Action<CancelMatchmakingTicketResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbDELETE, $"/match/matchmakingTicket/{userId}", null, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static void GetUserMatchState(string userId, Action<GetUserMatchStateResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbGET, $"/users/matchState/{userId}", null, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
}
