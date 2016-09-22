using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class APunchSequence
{
    protected List<Vector2> sequence = new List<Vector2>();
    protected int canonCount;

    public void InitSequence(int count)
    {
        canonCount = count;
        GenerateSequencePart(10);
    }

    public Vector2 Get(int i)
    {
        if (i >= sequence.Count)
        {
            GenerateSequencePart(10);
            return Get(i);
        }
        else
            return sequence[i];
    }

    protected abstract void GenerateSequencePart(int n);
}
