using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;

//  Fast-Paced Multiplayer (FPM)
//  https://www.gabrielgambetta.com/client-server-game-architecture.html

public class FPM_Manager : MonoSingleton<FPM_Manager>
{
    private FPM_Move fpm_Move = null;
    private FPM_Jump fpm_Jump = null;

    protected override void Awake()
    {
        base.Awake();

        fpm_Move = gameObject.AddComponent<FPM_Move>();
        fpm_Jump = gameObject.AddComponent<FPM_Jump>();

        SceneMessageBroker.AddSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        SceneMessageBroker.RemoveSubscriber<TickMessage.EarlyTick>(OnEarlyTick);
    }

    private void OnEarlyTick(TickMessage.EarlyTick message)
    {
        fpm_Jump.ProcessJumpInputData();
        fpm_Move.ProcessPlayerMoveInput();
    }
}
