using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;

public class BehaviorHelper
{
    public static void BehaviorDestroyer(BehaviorBase behavior)
    {
		behavior.Entity.DetachEntityComponent(behavior);

		Object.Destroy(behavior);
    }
}
