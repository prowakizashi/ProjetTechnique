using UnityEngine;
using System.Collections.Generic;

public static class Sequence
{
    public enum Type
    {
        None,
        Random
    };
}

public class AFloatPairSequence
{
    private List<Vector2> sequence;

    public Vector2 CurrentValue
    {
        get
        {
            return iterator.Current;
        }
    }
    private List<Vector2>.Enumerator iterator;

    public AFloatPairSequence()
    {
        sequence = new List<Vector2>();
        ResetIterator();
    }

    public void Clear()
    {
        sequence.Clear();
        ResetIterator();
    }

    public void ResetIterator()
    {
        iterator = sequence.GetEnumerator();
    }

    public bool MoveNext()
    {
        return iterator.MoveNext();
    }

    protected void add(Vector2 newValue)
    {
        sequence.Add(newValue);
    }
}
