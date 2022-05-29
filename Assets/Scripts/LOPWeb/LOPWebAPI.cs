using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;

public class LOPWebAPI
{
    public static HttpRequestContainer<JoinLobbyResult> JoinLobby(JoinLobbyRequest request, Action<JoinLobbyResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put("joinLobby", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<LeaveLobbyResult> LeaveLobby(LeaveLobbyRequest request, Action<LeaveLobbyResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put("leaveLobby", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<CreateMatchmakingTicketResult> CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put("match/matchmakingTicket", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static HttpRequestContainer<CancelMatchmakingTicketResult> CancelMatchmakingTicket(string userId, Action<CancelMatchmakingTicketResult> onResult = null, Action<string> onError = null)
    {
        return Http.Delete($"match/matchmakingTicket/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static HttpRequestContainer<GetUserMatchStateResult> GetUserMatchState(string userId, Action<GetUserMatchStateResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"users/matchState/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<GetRoomResult> GetRoom(string roomId, Action<GetRoomResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"room/{roomId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
}
