using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Entity;

public class JumpWang : SubGameBase
{
    private void Start()
    {
        GamePubSubService.AddSubscriber(GameMessageKey.EntityRegister, OnEntityRegister);
    }

    private void OnDestroy()
    {
        GamePubSubService.RemoveSubscriber(GameMessageKey.EntityRegister, OnEntityRegister);
    }

    private void OnEntityRegister(object[] param)
    {
        int entityId = (int)param[0];

        var entity = Entities.Get<MonoEntityBase>(entityId);

        entity.ModelRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }

    protected override IEnumerator OnInitialize()
    {
        yield return SceneManager.LoadSceneAsync(LOP.Game.Current.GameManager.mapName, LoadSceneMode.Additive);

        foreach (var entity in Entities.GetAll<MonoEntityBase>())
        {
            if (entity is MapObjectBase)
            {
                continue;
            }

            entity.ModelRigidbody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
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
