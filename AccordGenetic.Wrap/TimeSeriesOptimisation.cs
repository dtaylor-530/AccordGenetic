
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Threading;

namespace AccordGenetic.Wrap
{

    public class TimeSeriesOptimisation2DWrap<ROutput> : Optimisation2DWrap<IEnumerable<KeyValuePair<DateTime, double>>, ROutput>
    {

        public TimeSeriesOptimisation2DWrap(Func<IEnumerable<KeyValuePair<DateTime, double>>, Func<double, double, ROutput>> func,
            IEnumerable<KeyValuePair<DateTime, double>> input,
            Func<IEnumerable<KeyValuePair<DateTime, double>>, ROutput, double> error, IProgress<KeyValuePair<int, Result>> progress, CancellationToken token, int count = 1) : base(func, input, error, progress, token, count)
        {

        }

        public TimeSeriesOptimisation2DWrap(Func<IEnumerable<KeyValuePair<DateTime, double>>, Func<double, double, ROutput>> func,
    IEnumerable<KeyValuePair<DateTime, double>> input,
    Func<IEnumerable<KeyValuePair<DateTime, double>>, ROutput, double> error, IProgress<KeyValuePair<int, Result>> progress, int count = 1) : base(func, input, error, progress, count)
        {

        }

        public TimeSeriesOptimisation2DWrap(Func<IEnumerable<KeyValuePair<DateTime, double>>, Func<double, double, ROutput>> func,
    IEnumerable<KeyValuePair<DateTime, double>> input,
    Func<IEnumerable<KeyValuePair<DateTime, double>>, ROutput, double> error,    int count = 1) : base(func, input, error, count)
        {

        }

    }


    // minimises error in two variable function, func, according to error function, error,
    public class Optimisation2DWrap<TInput, ROutput>
    {

        //public ISubject<IO<ROutput>> Subject { get; set; } = new Subject<IO<ROutput>>();

        public Optimisation2DWrap(Func<TInput, Func<double, double, ROutput>> func, TInput input, Func<TInput, ROutput, double> error, int count = 10)
        {

            Initialise(func, input, error, null, default(CancellationToken), count);
        }
        public Optimisation2DWrap(Func<TInput, Func<double, double, ROutput>> func, TInput input, Func<TInput, ROutput, double> error, IProgress<KeyValuePair<int, Result>> progress, CancellationToken token, int count = 10)
        {
            Initialise(func, input, error, progress, token, count);

        }

        public Optimisation2DWrap(Func<TInput, Func<double, double, ROutput>> func, TInput input, Func<TInput, ROutput, double> error, IProgress<KeyValuePair<int, Result>> progress, int count = 10)
        {

            Initialise(func, input, error, progress, default(CancellationToken), count);
        }


        private void Initialise(Func<TInput, Func<double, double, ROutput>> func, TInput input, Func<TInput, ROutput, double> error, IProgress<KeyValuePair<int, Result>> progress, CancellationToken token, int count)
        {

            //function to optimise
            Func<double, double, double> fc = (a, b) => error(input, func(input)(a, b));
            // wrap-up in class
            var of = new UserFunction2D(fc);
            // build default genetic algorithm class
            var w = AccordWrapFactory.BuildDefaultMinimisation(of);

            if (progress == null & token == default(CancellationToken))
                w.RunMultipleEpochs(count);
            else 
                w.RunMultipleEpochs(count, progress);
        
        }



        public static Progress<KeyValuePair<int, Result>> BuildProgress(Func<double, double, ROutput> func, TInput input, Action<IO<ROutput>> a)
        {
            double score = 10000000000;
            return new Progress<KeyValuePair<int, Result>>(
               _ =>
               {
                   if (score >= _.Value.Output[0, 1])
                   {
                       score = _.Value.Output[0, 1];

                       double[] xy = _.Value.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray();
                       var io = new IO<ROutput> { Parameters = xy, Score = _.Value.Output[0, 1], Iteration = _.Key, Output = func(xy[0], xy[1]) };

                       a(io);

                   }
               });


        }



    //    public static IObservable<IO<ROutput>> Optimise2D(Func<TInput, Func<double, double, ROutput>> func, TInput input, Func<TInput, ROutput, double> error, int count = 10)
    //    {
    //        //function to optimise
    //        Func<double, double, double> fc = (a, b) => error(input, func(input)(a, b));
    //        // wrap-up in class
    //        var of = new UserFunction2D(fc);
    //        // build default genetic algorithm class
    //        var w = AccordWrapFactory.BuildDefaultMinimisation(of);

    //        double score = 10000000000;

    //        return w.RunMultipleEpochs(count).Select(_ =>
    //        {
    //            if (score > _.Output[0, 1])
    //            {
    //                score = _.Output[0, 1];
    //                double[] xy = _.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray();
    //                return new IO<ROutput> { Parameters = xy, Score = _.Output[0, 1], Output = func(input)(xy[0], xy[1]) };
    //            }
    //            return null;
    //        }).Where(_ => _ != null);

    //    }

    //}


    //public static class Helper
    //{

    //    public static IObservable<IO<R>> GetSubjectAsObservable<T, R>(this Optimisation2DWrap<T, R> tso)
    //    {
    //        return ((IObservable<IO<R>>)tso.Subject);

    //    }




    }

    public class IO<T>
    {
        public double[] Parameters { get; internal set; }
        public double Score { get; internal set; }
        public int Iteration { get; internal set; }
        public object Output { get; internal set; }
    }
}
