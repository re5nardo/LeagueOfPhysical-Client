using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RememberGame : SubGameBase
{
    [SerializeField] private string bgSceneName = "RiftOfSummoner";

    protected override IEnumerator OnInitialize()
    {
        yield return SceneManager.LoadSceneAsync(bgSceneName, LoadSceneMode.Additive);
    }

    protected override IEnumerator OnFinalize()
    {
        yield return SceneManager.UnloadSceneAsync(bgSceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }

    protected override void OnGameStart()
    {
    }

    protected override void OnGameEnd()
    {
    }

    protected override void OnTick(int tick)
    {
    }

    protected override void OnEarlyTickEnd(int tick)
    {
    }
}
