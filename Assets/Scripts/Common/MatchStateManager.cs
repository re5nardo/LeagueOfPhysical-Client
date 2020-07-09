using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class MatchStateManager : MonoSingleton<MatchStateManager>
{
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void GetMatchState(Action<Dictionary<string, object>> resultCallback = null, Action<string> errorCallback = null)
    {
        PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest()
        {
            PlayFabId = PhotonNetwork.AuthValues.UserId,
            Keys = new List<string> { "MatchState" }
        },
        result =>
        {
            try
            {
                var data = JsonFx.Json.JsonReader.Deserialize<Dictionary<string, object>>(result.Data["MatchState"].Value);

                resultCallback?.Invoke(data);
            }
            catch (Exception e)
            {
                errorCallback?.Invoke(e.Message);
            }
        },
        error =>
        {
            Debug.Log("Got error retrieving user read only data");
            Debug.Log(error.GenerateErrorReport());

            errorCallback?.Invoke(error.ErrorMessage);
        });
    }

    public void RequestMatch()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "RequestMatch",
        },
        result =>
        {
        },
        error =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }

    public void CancelMatchRequest()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "CancelMatchRequest",
        },
        result =>
        {
        },
        error =>
        {
            Debug.LogError(error.ErrorMessage);
        });
    }
}
