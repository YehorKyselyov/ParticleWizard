

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

public class KStar892Meson : Meson
{
    private const double r1 = 1.364;
    private const double r2 = -0.99;
    private const double mr = 5.28;
    private const double mFit = 6.06465;
    private const double A0 = 0.374;
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => ms == 0 ? A0 : r1 / (1 - Math.Pow(ms,2)/ Math.Pow(mr,2)) + r2 / (1 - Math.Pow(ms,2)/ Math.Pow(mFit,2)) ;
    }
    public KStar892Meson() : base(0.895, (new SQuark(), new UQuark()), ParticleType.Vector, 0.046)
    {
    }
}

public class KStar1410Meson : Meson
{
    private const double perpen = 0.28;
    private const double parallel = 0.22;
    private const double A0 = 0.3;
    private const double mb = 5.28;

    public override Func<double, double> GetFormFactorFunction()
    {
        return ms =>
            ms == 0
                ? A0
                : ((1 - 2 * Math.Pow(Mass, 2) / (Math.Pow(mb, 2) + Math.Pow(Mass, 2) - Math.Pow(ms, 2))) * parallel +
                   Mass / mb * perpen) / (1 - Math.Pow(ms, 2) / Math.Pow(mb, 2));
    }
    public KStar1410Meson() : base(1.414, (new SQuark(), new UQuark()), ParticleType.Vector, 0.232)
    {
    }
}
public class KStar1680Meson : Meson
{
    private const double perpen = 0.24;
    private const double parallel = 0.18;
    private const double A0 = 0.22;
    private const double mb = 5.28;
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms =>
            ms == 0
                ? A0
                : ((1 - 2 * Math.Pow(Mass, 2) / (Math.Pow(mb, 2) + Math.Pow(Mass, 2) - Math.Pow(ms, 2))) * parallel +
                   Mass / mb * perpen) / (1 - Math.Pow(ms, 2) / Math.Pow(mb, 2));
    }
    public KStar1680Meson() : base(1.718, (new SQuark(), new UQuark()), ParticleType.Vector, 0.322)
    {
    }
}

public class KOne1270Meson : Meson
{
    private const double f0A = 0.22;
    private const double aA = 2.4;
    private const double bA = 1.78;
    private const double f0B = -0.45;
    private const double aB = 1.34;
    private const double bB = 0.69;
    private const double mb = 5.28;
    private const double mKA = 1.31;
    private const double mKB = 1.34;
    private const double theta = -0.593412;

    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => ms == 0 ? -0.52 :(Math.Sin(theta) * mKA * V0A(ms) + Math.Cos(theta) * mKB * V0B(ms)) / Mass;
    }

    private double V0A(double ms)
    {
        return  f0A / (1 - aA * Math.Pow(ms, 2) / Math.Pow(mb, 2) + bA * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }

    private double V0B(double ms)
    {
        return  f0B / (1 - aB * Math.Pow(ms, 2) / Math.Pow(mb, 2) + bB * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }
    
    public KOne1270Meson() : base(1.270, (new SQuark(), new UQuark()), ParticleType.PseudoVector, 0.322)
    {
    }
}
public class KOne1400Meson : Meson
{
    private const double f0A = 0.22;
    private const double aA = 2.4;
    private const double bA = 1.78;
    private const double f0B = -0.45;
    private const double aB = 1.34;
    private const double bB = 0.69;
    private const double mb = 5.28;
    private const double mKA = 1.31;
    private const double mKB = 1.34;
    private const double theta = -0.593412;

    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => ms == 0 ? -0.07 :(Math.Sin(theta) * mKA * V0A(ms) + Math.Cos(theta) * mKB * V0B(ms)) / Mass;
    }

    private double V0A(double ms)
    {
        return  f0A / (1 - aA * Math.Pow(ms, 2) / Math.Pow(mb, 2) + bA * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }

    private double V0B(double ms)
    {
        return  f0B / (1 - aB * Math.Pow(ms, 2) / Math.Pow(mb, 2) + bB * Math.Pow(ms, 4) / Math.Pow(mb, 4));
    }
    public KOne1400Meson() : base(1.400, (new SQuark(), new UQuark()), ParticleType.PseudoVector, 0.322)
    { 
    }
}

public class KTwoStar1430Meson : Meson
{
    private const double f0 = 0.23;
    private const double a = 1.23;
    private const double b = 0.76;
    private const double mb = 5.28;

    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => f0 / (1 - a * Math.Pow(ms, 2) / Math.Pow(mb, 2) + b * Math.Pow(ms, 4) / Math.Pow(mb, 4)) /
                     (1 - Math.Pow(ms, 2) / Math.Pow(mb, 2));
    }
    
    public KTwoStar1430Meson() : base(1.430, (new SQuark(), new UQuark()), ParticleType.Tensor, 0.232)
    { 
    }
}