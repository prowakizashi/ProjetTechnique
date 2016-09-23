using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollDNA
{
    //// Muscles : neck, left arm, right arm, back
    //// [Mus01X][Mus01Y][Mus01Z][Mus01Power][Mus02X][Mus02Y][Mus02Z][Mus02Power]...

    //public List<int> Chromosome { get; private set; }

    //private RagdollDNA(List<int> chrom)
    //{
    //    Chromosome = chrom;
    //}

    //public RagdollDNA CrossOver(RagdollDNA dna)
    //{
    //    List<int> newChrom = new List<int>();
    //    List<int> indexes = new List<int>();

    //    int i = 0;
    //    foreach (int val in Chromosome)
    //    {
    //        newChrom.Add(val);
    //        indexes.Add(i++);
    //    }

    //    for (int n = 0; n < GameProperties.CROSS_COUNT; ++n)
    //    {
    //        int rand = Random.Range(0, indexes.Count - 1);
    //        newChrom[indexes[rand]] = dna.Chromosome[indexes[rand]];
    //        indexes.RemoveAt(rand);
    //    }

    //    if (Random.Range(1, 100) <= GameProperties.MUTATION_RATE)
    //    {
    //        int rand = Random.Range(0, indexes.Count - 1);
    //        newChrom[indexes[rand]] = Random.Range(-50, 50);
    //        indexes.RemoveAt(rand);
    //    }

    //    return new RagdollDNA(newChrom);
    //}

    //public static RagdollDNA Generate()
    //{
    //    List<int> chrom = new List<int>();
    //    for (int i = 0; i < GameProperties.MUSCLE_COUNT; ++i)
    //    {
    //        var vec = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
    //        chrom.Add((int)vec.x);
    //        chrom.Add((int)vec.y);
    //        chrom.Add((int)vec.z);
    //        chrom.Add(Random.Range(-50, 50));
    //    }
    //    return new RagdollDNA(chrom);
    //}

    // Main cable: torso
    // Limbs: left elbow, right elbow, Left hand, right hand
    // [MainCablePow][MainCableX][MainCableZ][Limb01Power][Limb01X][Limb01Y][Limb01Z][Limb02Power]...

    public List<float> Chromosome { get; private set; }

    private RagdollDNA(List<float> chrom)
    {
        Chromosome = chrom;
    }

    public RagdollDNA CrossOver(RagdollDNA dna)
    {
        List<float> newChrom = new List<float>();
        List<int> indexes = new List<int>();

        int i = 0;
        foreach (float val in Chromosome)
        {
            newChrom.Add(val);
            indexes.Add(i++);
        }

        for (int n = 0; n < GameProperties.CROSS_COUNT; ++n)
        {
            int rand = Random.Range(0, indexes.Count - 1);
            newChrom[indexes[rand]] = dna.Chromosome[indexes[rand]];
            indexes.RemoveAt(rand);
        }

        if (Random.Range(1, 100) <= GameProperties.MUTATION_RATE)
        {
            int rand = Random.Range(0, indexes.Count - 1);
            newChrom[indexes[rand]] = Random.Range(-50, 50);
            indexes.RemoveAt(rand);
        }

        return new RagdollDNA(newChrom);
    }

    public static RagdollDNA Generate()
    {
        List<float> chrom = new List<float>();

        var mainVariation = Random.insideUnitCircle * GameProperties.MAIN_VARIATION_CIRCLE_RADIUS;
        chrom.Add(Random.Range(GameProperties.MAIN_POWER_MIN, GameProperties.MAIN_POWER_MAX));
        chrom.Add(mainVariation.x);
        chrom.Add(mainVariation.y);

        for (int i = 0; i < GameProperties.LIMB_CABLE_COUNT; ++i)
        {
            var variation = Random.insideUnitSphere * GameProperties.VARIATION_SPHERE_RADIUS;
            chrom.Add(Random.Range(GameProperties.LIMB_POWER_MIN, GameProperties.MAIN_POWER_MAX));
            chrom.Add(variation.x);
            chrom.Add(variation.y);
            chrom.Add(variation.z);
        }
        return new RagdollDNA(chrom);
    }
}
