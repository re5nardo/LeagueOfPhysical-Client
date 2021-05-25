using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct TransformHistory
{
    public int tick;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 positionChange;
    public Vector3 rotationChange;

    public TransformHistory(int tick, Vector3 position, Vector3 rotation, Vector3 positionChange, Vector3 rotationChange)
    {
        this.tick = tick;
        this.position = position;
        this.rotation = rotation;
        this.positionChange = positionChange;
        this.rotationChange = rotationChange;
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
