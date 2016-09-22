using UnityEngine;
using System.Collections.Generic;

public struct Intervalle
{
    private float max;
    public float Max
    {
        get
        {
            return max;
        }
        set
        {
            if (value < min)
                throw new System.Exception("Max < Min !!");
            max = value;
        }
    }

    private float min;
    public float Min
    {
        get
        {
            return min;
        }
        set
        {
            if (value > max)
                throw new System.Exception("Max < Min !!");
            min = value;
        }
    }
}

public class RandomFloatPairSequence : AFloatPairSequence
{
    public Intervalle FirstValueIntervalle { get; set; }
    public Intervalle SecondValueIntervalle { get; set; }

    public RandomFloatPairSequence(Intervalle firstValueIntervalle, Intervalle secondValueIntervalle) : base()
    {
        FirstValueIntervalle = firstValueIntervalle;
        SecondValueIntervalle = secondValueIntervalle;
    }

    public void AddElements(uint nb)
    {
        for (; nb > 0; --nb)
            add(new Vector2(Random.Range(FirstValueIntervalle.Min, FirstValueIntervalle.Max), Random.Range(SecondValueIntervalle.Min, SecondValueIntervalle.Max)));
        ResetIterator();
    }
}
