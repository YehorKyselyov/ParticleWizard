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
        task.BranchingFunction = CalculateBranching(task);
    }

    private Func<double, double>? CalculateBranching(CalculateBranchingTask task)
    {
        var Mfi = CalculateMatrixElement(task);
        var Xi = FigureNeededCoeff(task.InputParticle.Quarks.Quark, task.OutParticle.Quarks.Quark);
        var higgsMean = 246.22; //GeV
        var Br = new Func<double, double>(ms => 1 / task.InputParticle.DecayWidth * Math.Pow(Xi, 2) *
            Mfi(ms) * Mfi(ms) *
            Math.Pow(task.InputParticle.Quarks.Quark.Mass, 2) / (32 * Math.PI * Math.Pow(higgsMean, 2)) *
            absPS(task, ms) / Math.Pow(task.InputParticle.Mass, 2));
        return Br;
    }

    private Func<double, double>? CalculateMatrixElement(CalculateBranchingTask task)
    {
        Func<double, double>? output = null;
        var formFactor = task.OutParticle.GetFormFactorFunction();
        switch (task.OutParticle.Type)
        {
            case ParticleType.Scalar:
                output = ms =>
                    (Math.Pow(task.InputParticle.Mass, 2) - Math.Pow(task.OutParticle.Mass, 2) - Math.Pow(ms, 2)) /
                    (task.OutParticle.Quarks.Quark.Mass + task.InputParticle.Quarks.Quark.Mass) * formFactor(ms);
                break;
            case ParticleType.PseudoScalar:
                output = ms =>
                    (Math.Pow(task.InputParticle.Mass, 2) - Math.Pow(task.OutParticle.Mass, 2)) /
                    (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * formFactor(ms);
                break;
            case ParticleType.Vector:
                output = ms =>
                    2 * task.InputParticle.Mass * absPS(task, ms)  /
                    (task.InputParticle.Quarks.Quark.Mass + task.OutParticle.Quarks.Quark.Mass) * formFactor(ms);
                break;
            case ParticleType.PseudoVector:
                output = ms =>
                    2 * task.InputParticle.Mass * absPS(task, ms)  /
                    (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * formFactor(ms);
                break;
            case ParticleType.Tensor:
                output = ms =>
                    -2 /
                    (task.OutParticle.Quarks.Quark.Mass - task.InputParticle.Quarks.Quark.Mass) * Math.Sqrt(2d / 3d) *
                    task.InputParticle.Mass / task.OutParticle.Mass * Math.Pow(absPS(task, ms), 2) * formFactor(ms);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

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