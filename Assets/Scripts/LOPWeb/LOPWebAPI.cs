using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class LOPWebAPI
{
    /// <summary>
    /// Create a matchmaking ticket as a client.
    /// </summary>
    public static void CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbPOST, "/match/createMatchmakingTicket", request, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    /// <summary>
    /// Cancel a matchmaking ticket.
    /// </summary>
    public static void CancelMatchmakingTicket(CancelMatchmakingTicketRequest request, Action<CancelMatchmakingTicketResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbPOST, "/match/cancelMatchmakingTicket", request, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    /// <summary>
    /// Get a user's matchState.
    /// </summary>
    public static void GetUserMatchState(Action<GetUserMatchStateResult> resultCallback, Action<string> errorCallback, Dictionary<string, string> extraHeaders = null)
    {
        GameFramework.Http.MakeApiCall(UnityWebRequest.kHttpVerbGET, $"/users/matchState/{PhotonNetwork.player.UserId}", null, resultCallback, errorCallback, extraHeaders, GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
}
