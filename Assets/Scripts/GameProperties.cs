using UnityEngine;
using System.Collections;

public static class GameProperties
{
    // EDITABLE
    public static int CROSS_COUNT;
    public static int MUTATION_RATE;
    public static int POPULATION_SIZE = 6;

    public static float MAIN_VARIATION_CIRCLE_RADIUS = 2f;
    public static float MAIN_POWER_MIN = 10f;
    public static float MAIN_POWER_MAX = 20f;
    public static float VARIATION_SPHERE_RADIUS = 2f;
    public static float LIMB_POWER_MIN = 10f;
    public static float LIMB_POWER_MAX = 20f;

    // FIXED
    //public static int MUSCLE_COUNT = 4;
    //public static float IMPULSION_FREQUENCY = 1.5f;
    public static int LIMB_CABLE_COUNT = 4;

    // GUN PARAMS
    public static Sequence.Type SHOTS_SEQUENCE_TYPE = Sequence.Type.Random;
    public static float MIN_TIME_BETWEEN_SHOTS = .2f;
    public static float MAX_TIME_BETWEEN_SHOTS = 2f;
    public static uint SHOT_SEQUENCE_GROWTH_RATE = 5u;

    public static System.Func<AFloatPairSequence> GetSequenceCtor(uint nbCannons)
    {
        switch (SHOTS_SEQUENCE_TYPE)
        {
            case Sequence.Type.Random:
                return () =>
                {
                    Intervalle cannonLimits = new Intervalle();
                    cannonLimits.Max = (float)nbCannons;
                    cannonLimits.Min = 0f;
                    Intervalle timerLimits = new Intervalle();
                    timerLimits.Max = MAX_TIME_BETWEEN_SHOTS;
                    timerLimits.Min = MIN_TIME_BETWEEN_SHOTS;
                    return new RandomFloatPairSequence(cannonLimits, timerLimits);
                };
            default:
                return null;
        }
    }

    public static System.Action<AFloatPairSequence> GetSequenceGrower()
    {
        switch (SHOTS_SEQUENCE_TYPE)
        {
            case Sequence.Type.Random:
                return (AFloatPairSequence seq) =>
                {
                    (seq as RandomFloatPairSequence).AddElements(SHOT_SEQUENCE_GROWTH_RATE);
                };
            default:
                return null;
        }
    }
}
