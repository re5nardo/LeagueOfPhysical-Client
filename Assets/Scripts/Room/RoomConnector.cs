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

        if (LOPSettings.Get().connectLocalServer)
        {
            Room = new RoomResponse
            {
                id = "EditorTestRoom",
                matchId = "EditorTestMatch",
                status = RoomStatus.Ready,
                ip = "localhost",
                port = 7777,
            };

            Match = new MatchResponse
            {
                id = "EditorTestMatch",
                matchType = MatchType.Friendly,
                subGameId = "FlapWang",
                mapId = "FlapWangMap",
                status = MatchStatus.MatchStart,
                playerList = null,
            };
        }
        else
        {
            //  Get Room
            var getRoom = LOPWebAPI.GetRoom(roomId);
            yield return getRoom;

            if (getRoom.isSuccess == false)
            {
                Debug.LogError(getRoom.error);
                yield break;
            }

            if (getRoom.response.code != ResponseCode.SUCCESS)
            {
                Debug.LogError($"getRoom.response.code: {getRoom.response.code}");
                yield break;
            }

            Room = getRoom.response.room;

            //  Get Match
            var getMatch = LOPWebAPI.GetMatch(Room.matchId);
            yield return getMatch;

            if (getMatch.isSuccess == false)
            {
                Debug.LogError(getMatch.error);
                yield break;
            }

            if (getMatch.response.code != ResponseCode.SUCCESS)
            {
                Debug.LogError($"getMatch.response.code: {getMatch.response.code}");
                yield break;
            }

            Match = getMatch.response.match;
        }

        tryingToEnterRoom = false;

        SceneManager.LoadScene("Room");
    }
}
