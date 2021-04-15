using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;

public class LOPWebAPI
{
    public static HttpRequestContainer<JoinLobbyResult> JoinLobby(JoinLobbyRequest request, Action<JoinLobbyResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put("/joinLobby", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<LeaveLobbyResult> LeaveLobby(LeaveLobbyRequest request, Action<LeaveLobbyResult> onResult, Action<string> onError)
    {
        return Http.Put("/leaveLobby", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<CreateMatchmakingTicketResult> CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> onResult, Action<string> onError)
    {
        return Http.Put("/match/matchmakingTicket", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }
}
