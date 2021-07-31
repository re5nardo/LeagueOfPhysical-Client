using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameFramework;

public class RoomConnector : MonoSingleton<RoomConnector>
{
    private bool tryingToEnterRoom = false;

    public GetRoomResult Room { get; private set; }

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

        var request = LOPWebAPI.GetRoom(roomId);

        yield return request;

        if (request.isError)
        {
            Debug.LogError(request.error);
            yield break;
        }

        tryingToEnterRoom = false;

        Room = request.response;

        SceneManager.LoadScene("Room");
    }
}
