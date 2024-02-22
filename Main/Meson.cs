

using System;

namespace ParticleWizard.Main;

public enum ParticleType
{
    Scalar,
    PseudoScalar,
    Vector,
    PseudoVector,
    Tensor
}
public class Meson
{
    public double Mass { get; }
    public double MeanLifetime { get; }
    public double DecayWidth => 1 / MeanLifetime.ConvertSecondsToReverseGeV();
    public (Quark Quark, Quark AntiQuark) Quarks  { get; }
    public ParticleType Type { get; }

    // Constructor for the base Meson class
    protected Meson(double mass, double meanLifetime, (Quark, Quark) quarks, ParticleType type)
    {
        Mass = mass;
        MeanLifetime = meanLifetime;
        Quarks = quarks;
        Type = type;
    }
    protected Meson(double mass, (Quark, Quark) quarks, ParticleType type, double decayWidth)
    {
        Mass = mass;
        MeanLifetime = decayWidth.ConvertGeVToReverseSeconds();
        Quarks = quarks;
        Type = type;
    }
    
    public virtual Func<double, double>? GetFormFactorFunction()
    {
        return null;
    }
}

public class BMeson : Meson
{
    public BMeson() : base(5.3, 1.638 * 1e-12, (new BQuark(), new UQuark()), ParticleType.PseudoScalar)
    {
    }
}

public class KMeson : Meson
{
    private const double f0 = 0.33;
    private const double mx = 6.16; 
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => f0 / (1 - Math.Pow(ms, 2) / Math.Pow(mx, 2));
    }
    public KMeson() : base(0.5, 1.238 * 1e-8, (new SQuark(), new UQuark()), ParticleType.PseudoScalar)
    {
    }
}

public class KStar700Meson : Meson
{
    private const double f0 = 0.46;
    private const double a = 1.6;
    private const double b = 1.35;
    private const double mb = 5.28;

    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => f0 / (1 - a * Math.Pow(ms, 2) / Math.Pow(mb, 2) + b * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }
    public KStar700Meson() : base(0.845, (new SQuark(), new UQuark()), ParticleType.Scalar, 0.468)
    {
    }
}

public class KStar1430Meson : Meson
{
    private const double f0 = 0.17;
    private const double a = 4.4;
    private const double b = 6.4;
    private const double mb = 5.28;

    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => f0 / (1 - a * Math.Pow(ms, 2) / Math.Pow(mb, 2) + b * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }
    public KStar1430Meson() : base(1.425, (new SQuark(), new UQuark()), ParticleType.Scalar, 0.270)
    {
    }
}
