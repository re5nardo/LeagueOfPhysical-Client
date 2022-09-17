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
    public static HttpRequestContainer<MatchmakingResult> RequestMatchmaking(MatchmakingRequest request, Action<MatchmakingResult> onResult = null, Action<string> onError = null)
    {
        return Http.Post("matchmaking", JsonUtility.ToJson(request), onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
    }

    public static HttpRequestContainer<CancelMatchmakingResult> CancelMatchmaking(string ticketId, Action<CancelMatchmakingResult> onResult = null, Action<string> onError = null)
    {
        return Http.Delete($"matchmaking/{ticketId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Matchmaking"));
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

    public static HttpRequestContainer<VerifyUserLocationResult> VerifyUserLocation(string userId, Action<VerifyUserLocationResult> onResult = null, Action<string> onError = null)
    {
        return Http.Put($"user/verify-location/{userId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Lobby"));
    }
    #endregion

    #region Room
    public static HttpRequestContainer<GetRoomResult> GetRoom(string roomId, Action<GetRoomResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"room/{roomId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Room"));
    }
    #endregion

    #region Match
    public static HttpRequestContainer<GetMatchResult> GetMatch(string matchId, Action<GetMatchResult> onResult = null, Action<string> onError = null)
    {
        return Http.Get($"match/{matchId}", onResult, onError, apiSettings: GameFramework.ServerSettings.Get("ServerSettings_Room"));
    }
    #endregion
}
