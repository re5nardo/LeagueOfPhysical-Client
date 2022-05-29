using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;

public class LOPWebAPI
{
    #region Lobby
    public static HttpRequestContainer<JoinLobbyResult> JoinLobby(string userId, Action<JoinLobbyResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put($"lobby/join/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<LeaveLobbyResult> LeaveLobby(string userId, Action<LeaveLobbyResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put($"lobby/leave/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
    #endregion

    #region MatchmakingTicket
    public static HttpRequestContainer<CreateMatchmakingTicketResult> CreateMatchmakingTicket(CreateMatchmakingTicketRequest request, Action<CreateMatchmakingTicketResult> onResult = null, Action<string> onError = null)
    {
        return Http.Post("matchmakingTicket", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static HttpRequestContainer<CancelMatchmakingTicketResult> CancelMatchmakingTicket(string userId, Action<CancelMatchmakingTicketResult> onResult = null, Action<string> onError = null)
    {
        return Http.Delete($"matchmakingTicket/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }
    #endregion

    #region User
    public static HttpRequestContainer<GetUserResult> GetUser(string userId, Action<GetUserResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"user/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }

    public static HttpRequestContainer<CreateUserResult> CreateUser(CreateUserRequest request, Action<CreateUserResult> onResult = null, Action<string> onError = null)
    {
        return Http.Post("user", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
    #endregion

    #region Room
    public static HttpRequestContainer<GetRoomResult> GetRoom(string roomId, Action<GetRoomResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"room/{roomId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
    #endregion
}
