

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
    // private const double f0 = 0.33;
    // private const double mx = 6.16; 
    //
    // public override Func<double, double> GetFormFactorFunction()
    // {
    //     return ms => f0 / (1 - Math.Pow(ms, 2) / Math.Pow(mx, 2));
    // }
    
    private const double r1 = 0.162;
    private const double r2 = 0.173;
    private const double mr = 5.41;
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms =>
            r1 / (1 - Math.Pow(ms, 2) / Math.Pow(mr, 2)) +
                  r2 / Math.Pow(1 - Math.Pow(ms, 2) / Math.Pow(mr, 2), 2);
    }
    
    public KMeson() : base(0.497, 1.238 * 1e-8, (new SQuark(), new UQuark()), ParticleType.PseudoScalar)
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

public class KStar892Meson : Meson, IManyFormFactors
{
    private const double r1 = 1.364;
    private const double r2 = -0.99;
    private const double mr = 5.28;
    private const double mFit = 6.06465;
    private const double Aa0 = 0.374;
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => ms == 0 ? Aa0 : r1 / (1 - Math.Pow(ms,2)/ Math.Pow(mr,2)) + r2 / (1 - Math.Pow(ms,2)/ Math.Pow(mFit,2)) ;
    }
    public KStar892Meson() : base(0.895, (new SQuark(), new UQuark()), ParticleType.Vector, 0.046)
    {
    }

    public double F0(double m)
    {
        throw new NotImplementedException();
    }

    public double A0(double m)
    {
        throw new NotImplementedException();
    }

    public double A1(double q)
    {
        return 0.29 / (1 - q * q / 40.38);
    }

    public double A2(double q)
    {
        return -0.084 / (1 - q * q / 52) + 0.342 / Math.Pow(1 - q * q / 52, 2);
    }

    public double V0(double q)
    {
        return 0.923 / (1 - q * q / Math.Pow(5.32, 2)) - 0.511 / (1 - q * q / 49.40);
    }
}

public class KStar1410Meson : Meson, IManyFormFactors
{
    private const double perpen = 0.28;
    private const double parallel = 0.22;
    private const double Aa0 = 0.3;
    private const double mb = 5.28;

    public override Func<double, double> GetFormFactorFunction()
    {
        return ms =>
            ms == 0
                ? Aa0
                : ((1 - 2 * Math.Pow(Mass, 2) / (Math.Pow(mb, 2) + Math.Pow(Mass, 2) - Math.Pow(ms, 2))) * parallel +
                   Mass / mb * perpen) / (1 - Math.Pow(ms, 2) / Math.Pow(mb, 2));
    }
    public KStar1410Meson() : base(1.414, (new SQuark(), new UQuark()), ParticleType.Vector, 0.232)
    {
    }
    public double F0(double m)
    {
        throw new NotImplementedException();
    }

    public double A0(double m)
    {
        throw new NotImplementedException();
    }

    public double A1(double q)
    {
        return 0.29 / (1 - q * q / 40.38);
    }

    public double A2(double q)
    {
        return -0.084 / (1 - q * q / 52) + 0.342 / Math.Pow(1 - q * q / 52, 2);
    }

    public double V0(double q)
    {
        return 0.923 / (1 - q * q / Math.Pow(5.32, 2)) - 0.511 / (1 - q * q / 49.40);
    }
}
public class KStar1680Meson : Meson, IManyFormFactors
{
    private const double perpen = 0.24;
    private const double parallel = 0.18;
    private const double Aa0 = 0.22;
    private const double mb = 5.28;
    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms =>
            ms == 0
                ? Aa0
                : ((1 - 2 * Math.Pow(Mass, 2) / (Math.Pow(mb, 2) + Math.Pow(Mass, 2) - Math.Pow(ms, 2))) * parallel +
                   Mass / mb * perpen) / (1 - Math.Pow(ms, 2) / Math.Pow(mb, 2));
    }
    public KStar1680Meson() : base(1.68, (new SQuark(), new UQuark()), ParticleType.Vector, 0.322)
    {
    }
    public double F0(double m)
    {
        throw new NotImplementedException();
    }

    public double A0(double m)
    {
        throw new NotImplementedException();
    }

    public double A1(double q)
    {
        return 0.29 / (1 - q * q / 40.38);
    }

    public double A2(double q)
    {
        return -0.084 / (1 - q * q / 52) + 0.342 / Math.Pow(1 - q * q / 52, 2);
    }

    public double V0(double q)
    {
        return 0.923 / (1 - q * q / Math.Pow(5.32, 2)) - 0.511 / (1 - q * q / 49.40);
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

public class KTwoStar1430Meson : Meson, IManyFormFactors
{
    private const double Ff0 = 0.23;
    private const double A = 1.23;
    private const double B = 0.76;
    private const double Mb = 5.28;

    
    public override Func<double, double> GetFormFactorFunction()
    {
        return ms => Ff0 / (1 - A * Math.Pow(ms, 2) / Math.Pow(Mb, 2) + B * Math.Pow(ms, 4) / Math.Pow(Mb, 4)) /
                     (1 - Math.Pow(ms, 2) / Math.Pow(Mb, 2));
    }
    
    public KTwoStar1430Meson() : base(1.430, (new SQuark(), new UQuark()), ParticleType.Tensor, 0.232)
    { 
    }

    public double F0(double m)
    {
        throw new NotImplementedException();
    }

    public double A0(double m)
    {
        throw new NotImplementedException();
    }

    public double A1(double m)
    {
        var f0 = 0.22d;
        var a = 1.42d;
        var b = 0.5d;
        var mb = Mb;
        return f0 / (1 - a * Math.Pow(m, 2) / Math.Pow(mb, 2) + b * Math.Pow(m, 4) / Math.Pow(mb, 4)) /
               (1 - Math.Pow(m, 2) / Math.Pow(mb, 2));
    }

    public double A2(double m)
    {
        var f0 = 0.21d;
        var a = 1.96d;
        var b = 1.79d;
        var mb = Mb;
        return f0 / (1 - a * Math.Pow(m, 2) / Math.Pow(mb, 2) + b * Math.Pow(m, 4) / Math.Pow(mb, 4)) /
               (1 - Math.Pow(m, 2) / Math.Pow(mb, 2));
    }

    public double V0(double m)
    {
        throw new NotImplementedException();
    }
}

interface IManyFormFactors
{
    double F0(double m);
    double A0(double m);
    double A1(double m);
    double A2(double m);
    double V0(double m);
}