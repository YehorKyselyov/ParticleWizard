using System;
using System.Globalization;

namespace ParticleWizard.Main;

public class CalculateBranchingTask
{
    public Meson InputParticle;
    public Meson OutParticle;
    public ParticleType NewPhysicsParticle;

    public Func<double, double>? BranchingFunction; //Output

    public CalculateBranchingTask(Meson inputParticle, Meson outParticle, ParticleType newPhysicsParticle)
    {
        InputParticle = inputParticle;
        OutParticle = outParticle;
        NewPhysicsParticle = newPhysicsParticle;
        BranchingFunction = null;
    }
}

public class BranchingCalculator : Service<CalculateBranchingTask>
{
    public override Validator<CalculateBranchingTask> Validator { get; set; } = new BranchingValidator();

    protected override void ProcessImpl(CalculateBranchingTask task)
    {
        task.BranchingFunction = task.NewPhysicsParticle switch
        {
            ParticleType.Scalar => CalculateScalarBranching(task),
            ParticleType.Vector => CalculateVectorBranching(task),
            _ => throw new ArgumentOutOfRangeException(nameof(task.NewPhysicsParticle))
        };
    }

    private Func<double, double>? CalculateVectorBranching(CalculateBranchingTask task)
    {
        Func<double, double>? Mfi2 = CalculateMatrixElementForVector(task);
        var Xi = FigureNeededCoeff(task.InputParticle.Quarks.Quark, task.OutParticle.Quarks.Quark);
        var higgsMean = 246.22; //GeV
        var Br = new Func<double, double>(ms => 1 / task.InputParticle.DecayWidth * Math.Pow(Xi, 2) *
            Mfi2(ms) * Mfi2(ms) *
            Math.Pow(task.InputParticle.Quarks.Quark.Mass, 2) / (32 * Math.PI * Math.Pow(higgsMean, 2)) *
            absPS(task, ms) / Math.Pow(task.InputParticle.Mass, 2));
        return Br;
    }
    
    private Func<double, double>? CalculateMatrixElementForVector(CalculateBranchingTask task)
    {
        var formFactor = task.OutParticle.GetFormFactorFunction();
        Func<double, double>? output = task.OutParticle.Type switch
        {
            ParticleType.Scalar => ms =>
                2 / (task.OutParticle.Quarks.Quark.Mass + task.InputParticle.Quarks.Quark.Mass) * formFactor(ms) *
                absPS(task, ms) * task.InputParticle.Mass / ms,
            ParticleType.PseudoScalar => q =>
                2 / (task.OutParticle.Quarks.Quark.Mass + task.InputParticle.Quarks.Quark.Mass) * formFactor(q) *
                absPS(task, q) * task.InputParticle.Mass / q,
                // formFactor(q) * formFactor(q) /
                // Math.Pow(task.OutParticle.Quarks.Quark.Mass + task.InputParticle.Quarks.Quark.Mass, 2) *
                // (Math.Pow(task.OutParticle.Mass, 2) - Math.Pow(q, 2)) / (3 * Math.Pow(q, 2)) *
                // (Math.Pow(task.OutParticle.Mass, 2) + Math.Pow(task.InputParticle.Mass, 2) + 2 * absPS(task, q) + 2 *
                //     Math.Sqrt(Math.Pow(task.InputParticle.Mass, 2) + Math.Pow(absPS(task, q), 2)) *
                //     Math.Sqrt(Math.Pow(task.InputParticle.Mass, 2) + Math.Pow(absPS(task, q), 2))),
            _ => throw new ArgumentOutOfRangeException()
        };
        return output;
    }

    private Func<double, double>? CalculateScalarBranching(CalculateBranchingTask task)
    {
        Func<double, double>? Mfi = CalculateMatrixElementForScalar(task);
        var Xi = FigureNeededCoeff(task.InputParticle.Quarks.Quark, task.OutParticle.Quarks.Quark);
        var higgsMean = 246.22; //GeV
        var Br = new Func<double, double>(ms => 1 / task.InputParticle.DecayWidth * Math.Pow(Xi, 2) *
            Mfi(ms) * Mfi(ms) *
            Math.Pow(task.InputParticle.Quarks.Quark.Mass, 2) / (32 * Math.PI * Math.Pow(higgsMean, 2)) *
            absPS(task, ms) / Math.Pow(task.InputParticle.Mass, 2));
        return Br;
    }

    private Func<double, double>? CalculateMatrixElementForScalar(CalculateBranchingTask task)
    {
        Func<double, double>? output = null;
        var formFactor = task.OutParticle.GetFormFactorFunction();
        output = task.OutParticle.Type switch
        {
            ParticleType.Scalar => ms =>
                (Math.Pow(task.InputParticle.Mass, 2) - Math.Pow(task.OutParticle.Mass, 2) - Math.Pow(ms, 2)) /
                (task.OutParticle.Quarks.Quark.Mass + task.InputParticle.Quarks.Quark.Mass) * formFactor(ms),
            ParticleType.PseudoScalar => ms =>
                (Math.Pow(task.InputParticle.Mass, 2) - Math.Pow(task.OutParticle.Mass, 2)) /
                (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * formFactor(ms),
            ParticleType.Vector => ms =>
                2 * task.InputParticle.Mass * absPS(task, ms) /
                (task.InputParticle.Quarks.Quark.Mass + task.OutParticle.Quarks.Quark.Mass) * formFactor(ms),
            ParticleType.PseudoVector => ms =>
                2 * task.InputParticle.Mass * absPS(task, ms) /
                (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * formFactor(ms),
            ParticleType.Tensor => ms =>
                -2 / (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * Math.Sqrt(2d / 3d) *
                task.InputParticle.Mass / task.OutParticle.Mass * Math.Pow(absPS(task, ms), 2) * formFactor(ms),
            _ => throw new ArgumentOutOfRangeException()
        };

        return output;
    }

    private double absPS(CalculateBranchingTask task, double ms) // particle's momentum in rest frame of initial meson
    {
        return 0.5 * Math.Sqrt(Math.Pow(task.InputParticle.Mass, 4) -
                               2 * Math.Pow(task.InputParticle.Mass, 2) *
                               (Math.Pow(task.OutParticle.Mass, 2) + Math.Pow(ms, 2)) +
                               Math.Pow(Math.Pow(task.OutParticle.Mass, 2) - Math.Pow(ms, 2), 2)) /
               task.InputParticle.Mass;
    }

    private double FigureNeededCoeff(Quark initialQuark, Quark outQuark)
    {
        if (initialQuark is BQuark && outQuark is SQuark) return 3.6e-4;
        return 0;
    }
}

public class BranchingValidator : Validator<CalculateBranchingTask>
{
    public override bool PreValidate(CalculateBranchingTask task)
    {
        return task.InputParticle.Type == ParticleType.PseudoScalar ||
               WithError("Currently only production from PseudoScalar is supported");
    }

    public override bool PostValidate(CalculateBranchingTask task)
    {
        return task.BranchingFunction != null || WithError("Wasn't able to calculate Branching");
    }
}