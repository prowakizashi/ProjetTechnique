using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class RandomPunchSequence : APunchSequence
{
    protected override void GenerateSequencePart(int n)
    {
        for (int i = 0; i < n; ++i)
            sequence.Add(new Vector2(UnityEngine.Random.Range(0, canonCount - 1), UnityEngine.Random.Range(0.5f, 3.0f)));
    }
}
