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
            // PlotScalarBranchings();
            PlotVectorBranchings();
        }

        private void PlotVectorBranchings()
        {
            var branchingCalculator = new BranchingCalculator();
            var myModel = new PlotModel { Title = "Branching channels", IsLegendVisible = true };
            
            var calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KMeson(), ParticleType.Vector);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var series = new FunctionSeries(calculateScalarBrTask.BranchingFunction, 0, 5, 0.001, "K");
            series.Color = OxyColor.FromRgb(0, 0, 255);
            myModel.Series.Add(series);
            
            var calculateVectorBrTask = new CalculateBranchingTask(new BMeson(), new KStar1430Meson(), ParticleType.Vector);
            branchingCalculator.ProcessAsync(calculateVectorBrTask);
            var K1430Br = calculateVectorBrTask.BranchingFunction;
            calculateVectorBrTask = new CalculateBranchingTask(new BMeson(), new KStar700Meson(), ParticleType.Vector);
            branchingCalculator.ProcessAsync(calculateVectorBrTask);
            var K700Br = calculateVectorBrTask.BranchingFunction;
             series = new FunctionSeries(K1430Br + K700Br , 0, 5, 0.001, "K*0");
            series.Color = OxyColor.FromRgb(255, 165, 0);
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
                Maximum = 100,
                Base = 10 // This is the logarithm base
            });
            myModel.Axes.Add(new LinearAxis()
            {
                Position = AxisPosition.Bottom,
                Minimum = 0.1,
                Maximum = 5,
                Title = "Mass [GeV]"
            });
            
            MyPlot.Model = myModel;
        }

        private void PlotScalarBranchings()
        {
            var branchingCalculator = new BranchingCalculator();
            var myModel = new PlotModel { Title = "Branching channels", IsLegendVisible = true };

            var calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KMeson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var series = new FunctionSeries(calculateScalarBrTask.BranchingFunction, 0, 5, 0.001, "K");
            series.Color = OxyColor.FromRgb(0, 0, 255);
            myModel.Series.Add(series);
            
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar1430Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1430Br = calculateScalarBrTask.BranchingFunction;
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar700Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K700Br = calculateScalarBrTask.BranchingFunction;
            series = new FunctionSeries(K1430Br + K700Br , 0, 5, 0.001, "K*0");
            series.Color = OxyColor.FromRgb(255, 165, 0);
            myModel.Series.Add(series);
            
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar892Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K892Br = calculateScalarBrTask.BranchingFunction;
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar1410Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1410Br = calculateScalarBrTask.BranchingFunction;
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KStar1680Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1680r = calculateScalarBrTask.BranchingFunction;
            series = new FunctionSeries(K892Br + K1410Br + K1680r, 0, 5, 0.001, "K*");
            series.Color = OxyColor.FromRgb(255, 0, 0);
            myModel.Series.Add(series);
            
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KOne1270Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1270Br = calculateScalarBrTask.BranchingFunction;
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KOne1400Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K1400Br = calculateScalarBrTask.BranchingFunction;
            series = new FunctionSeries(K1270Br + K1400Br, 0.01, 5, 0.001, "K1*");
            myModel.Series.Add(series);
            
            calculateScalarBrTask = new CalculateBranchingTask(new BMeson(), new KTwoStar1430Meson(), ParticleType.Scalar);
            branchingCalculator.ProcessAsync(calculateScalarBrTask);
            var K2Star = calculateScalarBrTask.BranchingFunction;
            series = new FunctionSeries(K2Star, 0.01, 5, 0.001, "K2*");
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