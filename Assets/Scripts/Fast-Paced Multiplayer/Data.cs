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
