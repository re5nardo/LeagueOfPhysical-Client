using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;

public class LOPWebAPI
{
    /// <summary>
    /// Create a matchmaking ticket as a client.
    /// </summary>
    public static void CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> resultCallback, Action<LOPHttpError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
    {
        LOPHttp.MakeApiCall(UnityWebRequest.kHttpVerbPOST, "/match/createMatchmakingTicket", request, resultCallback, errorCallback, customData, extraHeaders, LOPServerSettings.Get("LOPServerSettings_Matchmaking"));
    }

    /// <summary>
    /// Cancel a matchmaking ticket.
    /// </summary>
    public static void CancelMatchmakingTicket(CancelMatchmakingTicketRequest request, Action<CancelMatchmakingTicketResult> resultCallback, Action<LOPHttpError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
    {
        LOPHttp.MakeApiCall(UnityWebRequest.kHttpVerbPOST, "/match/cancelMatchmakingTicket", request, resultCallback, errorCallback, customData, extraHeaders, LOPServerSettings.Get("LOPServerSettings_Matchmaking"));
    }

    /// <summary>
    /// Get a user's matchState.
    /// </summary>
    public static void GetUserMatchState(Action<GetUserMatchStateResult> resultCallback, Action<LOPHttpError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
    {
        LOPHttp.MakeApiCall(UnityWebRequest.kHttpVerbGET, $"/users/matchState/{PhotonNetwork.player.UserId}", null, resultCallback, errorCallback, customData, extraHeaders, LOPServerSettings.Get("LOPServerSettings_Lobby"));
    }
}
