using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TickNSequence
{
    public int tick;
    public long sequence;

    public TickNSequence(int tick, long sequence)
    {
        this.tick = tick;
        this.sequence = sequence;
    }
}

public class TransformHistory
{
    public int tick;
    public Vector3 positionChange;
    public Vector3 rotationChange;
    public Vector3 velocityChange;
    public Vector3 angularVelocityChange;

    public TransformHistory(int tick, Vector3 positionChange, Vector3 rotationChange, Vector3 velocityChange, Vector3 angularVelocityChange)
    {
        this.tick = tick;
        this.positionChange = positionChange;
        this.rotationChange = rotationChange;
        this.velocityChange = velocityChange;
        this.angularVelocityChange = angularVelocityChange;
    }
}
