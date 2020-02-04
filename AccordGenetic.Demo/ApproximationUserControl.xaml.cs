using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Accord.Math;
using AccordGenetic.Wrap;
using OxyPlot;
using OxyPlot.Series;

namespace AccordGenetic.Demo
{
    /// <summary>
    /// Interaction logic for ApproximationUserControl.xaml
    /// </summary>
    public partial class ApproximationUserControl : UserControl
    {
        PlotModel plotModel = new PlotModel();
        public ApproximationUserControl()
        {
            InitializeComponent();
            PlotView.Model = plotModel;
            LoadData();
        }

        private void LoadData()
        {

            dataToShow = new double[8, 2]{
                {1,1},
            {2,1.5},
            {3,2.5},
            {4,4},
            {5,6},
            {6,8.5},
            {7,11.5},
            {8,15}};
        }

        private void LoadData2()
        {

            dataToShow = new double[9, 2]
            {
                {1, 0},
                {2, 4},
                {3, 7},
                {4, 9},
                {5, 10},
                {6, 9},
                {7, 7},
                {8, 4},
                {9, 0}
            };
        }

    
        protected double[,] dataToShow = null;
        protected int selectionMethod = 0;
        protected int populationSize = 100;
        protected int iterations = 1000;
        protected CancellationTokenSource cts = new CancellationTokenSource();
        protected bool IsClosed;

        // On button "Start"
        // On button "Start"
        protected void startButton_Click(object sender, System.EventArgs e)
        {
            //EnableControls(false);
            //solutionBox.Text = string.Empty;

            AccordGenetic.Wrap.ApproximationWrap wrap = new AccordGenetic.Wrap.ApproximationWrap(
               data: dataToShow,
              functionsSet: FunctionsSetBox.Value.HasValue ? FunctionsSetBox.Value.Value : 0,
               populationSize: 40/*PopulationSize.Value.Value*/,
               geneticMethod: 1,//GeneticMethod.Value.Value,
              selectionMethod: 0 /*SelectionMethod.Value.Value*/,
              minRange: (float)dataToShow[0,0]/* (float)MinRange.Value.Value*/,
              lengthRange: (float)dataToShow[dataToShow.GetLength(0)-1, 0]/*chart.RangeX.Length*/);


            SearchSolution(wrap);


        }


        // Worker thread
        void SearchSolution(AccordGenetic.Wrap.ApproximationWrap wrap)
        {
            iterations = 10000;// Iterations.Value.Value;

            var progressHandler = new Progress<KeyValuePair<int, AccordGenetic.Wrap.Result>>(kvp => ProgressUpdate(kvp, wrap));

            cts = new CancellationTokenSource();

            Task tsk = Task.Run(() => wrap.RunMultipleEpochs(iterations, cts.Token, progressHandler));
            tsk.ContinueWith(
               t =>
               {
                   // faulted with exception
                   if (t.IsFaulted)
                   {

                       Exception ex = t.Exception;
                       while (ex is AggregateException && ex.InnerException != null)
                           ex = ex.InnerException;
                       MessageBox.Show("Error: " + ex.Message);
                   }
                   else if (t.IsCanceled)
                   {
                       //MessageBox.Show("Cancelled");
                   }
                   else
                   {
                       if (!cts.IsCancellationRequested)
                       { /* final update*/}

                   }
                   // completed successfully/ check if closed button has been clicked
                   if (!IsClosed)
                   {
                       //EnableControls(true);
                   }

               });


        }


        private int cnt = 0;
        public void ProgressUpdate(KeyValuePair<int, AccordGenetic.Wrap.Result> kvp, ApproximationWrap wrap)
        {
            // update info

            if (++cnt % 200 == 1)
            {
                var error = wrap.EvaluateError();
                //chart.UpdateDataSeries("solution", kvp.Value.Output);
                var list = new List<DataPoint>();

                for (int i = 0; i < kvp.Value.Output.GetLength()[0] - 1; i++)
                {
                    list.Add(new DataPoint(kvp.Value.Output[i, 0], kvp.Value.Output[i, 1]));
                }

                this.Dispatcher?.InvokeAsync(() =>
                {
                    PlotView.Model ??= new PlotModel { Title = "Genetic" };
                    PlotView.Model.Series.Add(

                        new LineSeries { ItemsSource = list }
                    );
                    PlotView.Model.InvalidatePlot(true);

                    Iterations.Value = kvp.Key;
                    Error.Text = error.Prediction.ToString("F3");
                    Solution.Text = kvp.Value.BestSolution;
                });
            }
        }


        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            LoadData();
            Run();
        }


        private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
        {
            LoadData2();
            Run();
        }

        private void Run()
        {
            cts?.Cancel();
            var list = new List<DataPoint>();
            for (int i = 0; i < dataToShow.GetLength()[0] - 1; i++)
            {
                list.Add(new DataPoint(dataToShow[i, 0], dataToShow[i, 1]));
            }
            PlotView.Model?.Series?.Clear();
            PlotView.Model ??= new PlotModel { Title = "Genetic" };
            PlotView.Model.Series.Add(

                new LineSeries { ItemsSource = list }
            );
          
            PlotView.Model.InvalidatePlot(true);
            Task.Delay(2000).ContinueWith(a =>
            startButton_Click(null, null), TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
