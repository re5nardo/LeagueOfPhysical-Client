using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GameFramework;

public class LOPWebAPI
{
    #region Lobby
    public static WebRequest<JoinLobbyResult> JoinLobby(string userId)
    {
        return new WebRequestBuilder<JoinLobbyResult>()
            .SetUri($"{LOP.Application.Env.lobbyBaseURL}/lobby/join/{userId}")
            .SetMethod(HttpMethod.PUT)
            .Build();
    }

    public static WebRequest<LeaveLobbyResult> LeaveLobby(string userId)
    {
        return new WebRequestBuilder<LeaveLobbyResult>()
            .SetUri($"{LOP.Application.Env.lobbyBaseURL}/lobby/leave/{userId}")
            .SetMethod(HttpMethod.PUT)
            .Build();
    }
    #endregion

    #region MatchmakingTicket
    public static WebRequest<MatchmakingResult> RequestMatchmaking(MatchmakingRequest request)
    {
        return new WebRequestBuilder<MatchmakingResult>()
            .SetUri($"{LOP.Application.Env.matchmakingBaseURL}/matchmaking")
            .SetMethod(HttpMethod.POST)
            .SetRequestBody(request)
            .Build();
    }

    public static WebRequest<CancelMatchmakingResult> CancelMatchmaking(string ticketId)
    {
        return new WebRequestBuilder<CancelMatchmakingResult>()
            .SetUri($"{LOP.Application.Env.matchmakingBaseURL}/matchmaking/{ticketId}")
            .SetMethod(HttpMethod.DELETE)
            .Build();
    }
    #endregion

    #region User
    public static WebRequest<GetUserResult> GetUser(string userId)
    {
        return new WebRequestBuilder<GetUserResult>()
            .SetUri($"{LOP.Application.Env.lobbyBaseURL}/user/{userId}")
            .SetMethod(HttpMethod.GET)
            .SetDeserialize(GetUserResult.Deserialize)
            .Build();
    }

    public static WebRequest<CreateUserResult> CreateUser(CreateUserRequest request)
    {
        return new WebRequestBuilder<CreateUserResult>()
            .SetUri($"{LOP.Application.Env.lobbyBaseURL}/user")
            .SetMethod(HttpMethod.POST)
            .SetRequestBody(request)
            .SetDeserialize(CreateUserResult.Deserialize)
            .Build();
    }

    public static WebRequest<VerifyUserLocationResult> VerifyUserLocation(string userId)
    {
        return new WebRequestBuilder<VerifyUserLocationResult>()
            .SetUri($"{LOP.Application.Env.lobbyBaseURL}/user/verify-location/{userId}")
            .SetMethod(HttpMethod.PUT)
            .Build();
    }
    #endregion

    #region Room
    public static WebRequest<GetRoomResult> GetRoom(string roomId)
    {
        return new WebRequestBuilder<GetRoomResult>()
            .SetUri($"{LOP.Application.Env.roomBaseURL}/room/{roomId}")
            .SetMethod(HttpMethod.GET)
            .Build();
    }
    #endregion

    #region Match
    public static WebRequest<GetMatchResult> GetMatch(string matchId)
    {
        return new WebRequestBuilder<GetMatchResult>()
            .SetUri($"{LOP.Application.Env.roomBaseURL}/match/{matchId}")
            .SetMethod(HttpMethod.GET)
            .Build();
    }
    #endregion
}
