using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GeneticEvolver
{
    private Dictionary<RagdollDNA, float> DNAs;
    private RagdollDNA[] bestDNAs = new RagdollDNA[2];
    public List<RagdollDNA> newDNAs { get; private set; }

    public GeneticEvolver(Dictionary<RagdollDNA, float> dnas)
    {
        newDNAs = new List<RagdollDNA>();
        if (dnas == null || dnas.Count == 0)
        {
            initNewGame();
            return;
        }

        dnas.OrderByDescending(e => { return e.Value; });
        DNAs = dnas;
        pickBestDNAs();
        crossChromosomes();
        generateNewDNAs();
    }

    private void pickBestDNAs()
    {
        bestDNAs[0] = DNAs.ElementAt(0).Key;
        bestDNAs[1] = DNAs.ElementAt(1).Key;
        newDNAs.Add(bestDNAs[0]);
        newDNAs.Add(bestDNAs[1]);
    }

    private void crossChromosomes()
    {
        int numToCreate = (GameProperties.POPULATION_SIZE - 2) / 2;
        List<RagdollDNA> eligibleChroms = new List<RagdollDNA>();
        eligibleChroms.Add(bestDNAs[0]);
        eligibleChroms.Add(bestDNAs[1]);

        for (int i = 0; i < numToCreate; ++i)
            eligibleChroms.Add(DNAs.ElementAt(2 + i).Key);

        for (int i = 0; i < numToCreate; ++i)
        {
            int rand1 = Random.Range(0, eligibleChroms.Count);
            int rand2 = 0;

            do
            {
                rand2 = Random.Range(0, eligibleChroms.Count);
            }
            while (rand1 == rand2);

            newDNAs.Add(eligibleChroms[rand1].CrossOver(eligibleChroms[rand2]));
        }
    }

    private void generateNewDNAs()
    {
        int numToCreate = GameProperties.POPULATION_SIZE - 2 - newDNAs.Count;
        for (int i = 0; i < numToCreate; ++i)
            newDNAs.Add(RagdollDNA.Generate());
    }

    private void initNewGame()
    {
        for (int i = 0; i < GameProperties.POPULATION_SIZE; ++i)
            newDNAs.Add(RagdollDNA.Generate());
    }
}
