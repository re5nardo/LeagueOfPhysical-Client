using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MoveInput
{
    public Vector3 direction;

    public MoveInput(Vector3 direction)
    {
        this.direction = direction;
    }
}

public struct JumpInput
{
    public float normalizedPower;
    public Vector3 direction;
}
