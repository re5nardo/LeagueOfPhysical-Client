using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngameDebugConsole;
using GameFramework;

public class DebugCommandRegister : MonoSingleton<DebugCommandRegister>
{
    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        DebugLogConsole.AddCommand<float, float>("SpeedFactor", "Set SpeedFactor  [  example : SpeedFactor 0.2 1.5  ]", DebugCommandHandler.SpeedFactorHandler.Handle);
    }

    private void OnDestroy()
    {
        DebugLogConsole.RemoveCommand("SpeedFactor");
    }
}
