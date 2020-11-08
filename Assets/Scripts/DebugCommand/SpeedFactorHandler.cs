using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace DebugCommandHandler
{
    public class SpeedFactorHandler
    {
        public static void Handle(float value1, float value2)
        {
            DebugCommandPubSubService.Publish("SpeedFactor", new object[] { value1, value2 });
        }
    }
}
