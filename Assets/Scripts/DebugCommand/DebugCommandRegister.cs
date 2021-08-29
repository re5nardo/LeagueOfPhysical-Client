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
        DebugLogConsole.AddCommand<float>("TransformDelayFactor", "Set TransformDelayFactor  [  example : TransformDelayFactor 0.2  ]", DebugCommandHandler.TransformDelayFactorHandler.Handle);
        DebugLogConsole.AddCommand<float, float, float>("MyEntityPosition", "Set My Entity Position  [  example : MyEntityPosition 0 0 0  ]", DebugCommandHandler.MyCharacterPositionHandler.Handle);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        DebugLogConsole.RemoveCommand("SpeedFactor");
        DebugLogConsole.RemoveCommand("TransformDelayFactor");
        DebugLogConsole.RemoveCommand("MyEntityPosition");
    }
}
