using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework;

public class RoomConnector : MonoSingleton<RoomConnector>
{
    private bool tryingToEnterRoom = false;

    public RoomResponse Room { get; private set; }
    public MatchResponse Match { get; private set; }

    public string RoomId => Room.id;
    public string MatchId => Room.matchId;
    public MatchSetting MatchSetting => new MatchSetting(Match.matchType, Match.subGameId, Match.mapId);

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void TryToEnterRoomById(string roomId)
    {
        Instance?.InternalTryToEnterRoomById(roomId);
    }

    private void InternalTryToEnterRoomById(string roomId)
    {
        StartCoroutine(_InternalTryToEnterRoomById(roomId));
    }

    private IEnumerator _InternalTryToEnterRoomById(string roomId)
    {
        if (tryingToEnterRoom)
            yield break;

        tryingToEnterRoom = true;

        //  Get Room
        var request = LOPWebAPI.GetRoom(roomId);

        yield return request;

        if (request.isError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        Room = request.response.room;

        //  Get Match
        var getMatch = LOPWebAPI.GetMatch(Room.matchId);
        yield return getMatch;

        if (getMatch.isError)
        {
            Debug.LogError(getMatch.error);
            yield break;
        }

        tryingToEnterRoom = false;

        Match = getMatch.response.match;

        SceneManager.LoadScene("Room");
    }
}
