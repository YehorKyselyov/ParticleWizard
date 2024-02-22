using System;

namespace ParticleWizard.Main;

public static class Utils
{
    private static readonly double SecondToRevGeVConverter = 1.51927d * Math.Pow(10, 24);

    public static double ConvertSecondsToReverseGeV(this double seconds)
    {
        return seconds * SecondToRevGeVConverter;
    }
    public static double ConvertGeVToReverseSeconds(this double GeV)
    {
        return GeV * SecondToRevGeVConverter;
    }
}