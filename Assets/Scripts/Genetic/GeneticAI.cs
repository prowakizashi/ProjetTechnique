using UnityEngine;
using System.Collections;

public class GeneticAI : MonoBehaviour
{
    private RagdollDNA DNA;
    private RagdollCharacter character;

    public void InitWithDNA(RagdollDNA dna)
    {
        DNA = dna;
        StartCoroutine(impulse());
    }

    void Awake()
    {
        character = GetComponent<RagdollCharacter>();
    }

    private IEnumerator impulse()
    {
        for (int i = 0; i < GameProperties.MUSCLE_COUNT; ++i)
        {
            var vec = new Vector3(DNA.Chromosome[i], DNA.Chromosome[i + 1], DNA.Chromosome[i + 2]).normalized;
            var force = DNA.Chromosome[i + 3];
            character.moveMuscle(i, vec, force);
        }
        yield return new WaitForSeconds(GameProperties.IMPULSION_FREQUENCY);
        StartCoroutine(impulse());
    }
}
