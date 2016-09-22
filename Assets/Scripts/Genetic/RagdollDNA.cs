using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RagdollDNA
{
    // Muscles : neck, left arm, right arm, back
    // [Mus01X][Mus01Y][Mus01Z][Mus01Power][Mus02X][Mus02Y][Mus02Z][Mus02Power]...

    public List<int> Chromosome { get; private set; }

    private RagdollDNA(List<int> chrom)
    {
        Chromosome = chrom;
    }

    public RagdollDNA CrossOver(RagdollDNA dna)
    {
        List<int> newChrom = new List<int>();
        List<int> indexes = new List<int>();

        int i = 0;
        foreach (int val in Chromosome)
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
        List<int> chrom = new List<int>();
        for (int i = 0; i < GameProperties.MUSCLE_COUNT; ++i)
        {
            var vec = new Vector3(Random.Range(-50, 50), Random.Range(-50, 50), Random.Range(-50, 50));
            chrom.Add((int)vec.x);
            chrom.Add((int)vec.y);
            chrom.Add((int)vec.z);
            chrom.Add(Random.Range(-50, 50));
        }
        return new RagdollDNA(chrom);
    }
}
