using UnityEngine;
using System.Collections.Generic;

public class GeneticAI : MonoBehaviour
{
    private RagdollDNA DNA;
    //private RagdollCharacter character;
    private Marionette character;

    public void InitWithDNA(RagdollDNA dna)
    {
        DNA = dna;
        Vector3 main = new Vector3(DNA.Chromosome[0], DNA.Chromosome[1], DNA.Chromosome[2]);
        Vector4 param;
        List<Vector4> paramList = new List<Vector4>();
        for (int i = 3; i < DNA.Chromosome.Count;)
        {
            param.x = DNA.Chromosome[i++];
            param.y = DNA.Chromosome[i++];
            param.z = DNA.Chromosome[i++];
            param.w = DNA.Chromosome[i++];
            paramList.Add(param);
        }

        character.Initialize(main, paramList.ToArray());
    }

    void Awake()
    {
        character = GetComponent<Marionette>();
        //character = GetComponent<RagdollCharacter>();
    }

    //private IEnumerator impulse()
    //{
    //    for (int i = 0; i < GameProperties.MUSCLE_COUNT; ++i)
    //    {
    //        var vec = new Vector3(DNA.Chromosome[i], DNA.Chromosome[i + 1], DNA.Chromosome[i + 2]).normalized;
    //        var force = DNA.Chromosome[i + 3];
    //        character.moveMuscle(i, vec, force);
    //    }
    //    yield return new WaitForSeconds(GameProperties.IMPULSION_FREQUENCY);
    //    StartCoroutine(impulse());
    //}
}
