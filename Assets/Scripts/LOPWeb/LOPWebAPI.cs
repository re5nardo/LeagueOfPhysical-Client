using System.Collections;
using System.Collections.Generic;
using System;

public class LOPWebAPI
{
    /// <summary>
    /// Create a matchmaking ticket as a client.
    /// </summary>
    public static void CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> resultCallback, Action<LOPHttpError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
    {
        LOPHttp.MakeApiCall("/match/createMatchmakingTicket", request, resultCallback, errorCallback, customData, extraHeaders, LOPServerSettings.Get());
    }

    /// <summary>
    /// Cancel a matchmaking ticket.
    /// </summary>
    public static void CancelMatchmakingTicket(CancelMatchmakingTicketRequest request, Action<CancelMatchmakingTicketResult> resultCallback, Action<LOPHttpError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null)
    {
        LOPHttp.MakeApiCall("/match/cancelMatchmakingTicket", request, resultCallback, errorCallback, customData, extraHeaders, LOPServerSettings.Get());
    }
}
