namespace ParticleWizard.Main;

public class Quark
{
    public double Mass { get; } //GeV
    public static double Spin => 0.5;

    protected Quark(double mass)
    {
        Mass = mass;
    }
}

public class UQuark : Quark
{
    public UQuark() : base(2.3e-3)
    {
    }
}

public class SQuark : Quark
{
    public SQuark() : base(0.096) 
    {
    }
}

public class BQuark : Quark
{
    public BQuark() : base(4.18)
    {
    }
}
