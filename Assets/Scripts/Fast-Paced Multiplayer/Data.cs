using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TransformHistory
{
    public int tick;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 velocity;
    public Vector3 positionChange;
    public Vector3 rotationChange;
    public Vector3 velocityChange;

    public TransformHistory(int tick, Vector3 position, Vector3 rotation, Vector3 velocity, Vector3 positionChange, Vector3 rotationChange, Vector3 velocityChange)
    {
        this.tick = tick;
        this.position = position;
        this.rotation = rotation;
        this.velocity = velocity;
        this.positionChange = positionChange;
        this.rotationChange = rotationChange;
        this.velocityChange = velocityChange;
    }
}

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
