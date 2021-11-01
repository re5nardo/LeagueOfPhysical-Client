using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;

public class JumpWang : SubGameBase
{
    [SerializeField] private JumpWangUI jumpWangUI = null;

    private void Start()
    {
        SceneMessageBroker.AddSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
    }

    private void OnDestroy()
    {
        SceneMessageBroker.RemoveSubscriber<GameMessage.EntityRegister>(OnEntityRegister);
    }

    private void OnEntityRegister(GameMessage.EntityRegister message)
    {
        var entity = Entities.Get<LOPMonoEntityBase>(message.entityId);

        entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    protected override IEnumerator OnInitialize()
    {
        yield return SceneManager.LoadSceneAsync(LOP.Game.Current.GameManager.MapData.sceneName, LoadSceneMode.Additive);

        foreach (var entity in Entities.GetAll<LOPMonoEntityBase>())
        {
            entity.Rigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }

        Physics.gravity *= LOP.Game.Current.GameManager.MapData.mapEnvironment.GravityFactor;
    }

    protected override IEnumerator OnFinalize()
    {
        yield return SceneManager.UnloadSceneAsync(LOP.Game.Current.GameManager.MapData.sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
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
