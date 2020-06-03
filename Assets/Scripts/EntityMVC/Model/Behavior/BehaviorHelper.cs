using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Behavior;

public class BehaviorHelper
{
    public static void BehaviorDestroyer(BehaviorBase behavior)
    {
		behavior.Entity.DetachComponent(behavior);

		Object.Destroy(behavior);
    }
}
