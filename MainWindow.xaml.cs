using System;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using ParticleWizard.Main;

namespace ParticleWizard
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PlotScalarBranchings();
        }

        private void PlotScalarBranchings()
        {
            var branchingCalculator = new BranchingCalculator();
            var calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KMeson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);

            var myModel = new PlotModel { Title = "Branching channels", IsLegendVisible = true };
            
            var series = new FunctionSeries(calculateScalarBrTask.BranchingFunction, 0, 5, 0.001, "K");
            myModel.Series.Add(series);
            
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar1430Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1430Br = calculateScalarBrTask.BranchingFunction;
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar700Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K700Br = calculateScalarBrTask.BranchingFunction;

            series = new FunctionSeries(K1430Br + K700Br , 0, 5, 0.001, "K*0(1430)+K*0(700)");
            myModel.Series.Add(series);
            
            var legend = new Legend
            {
                IsLegendVisible = true,
                LegendTitle = "Series Legend",
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.BottomLeft,
                LegendBackground = OxyColor.FromAColor(200, OxyColors.White),
                LegendBorder = OxyColors.Black
            };
            myModel.Legends.Add(legend);
            
            myModel.Axes.Add(new LogarithmicAxis
            {
                Position = AxisPosition.Left,
                Title = "Br",
                Minimum = 0.01,
                Maximum = 1,
                Base = 10 // This is the logarithm base
            });
            myModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Maximum = 5,
                Title = "Mass [GeV]"
            });
            
            MyPlot.Model = myModel;
        }
    }
}