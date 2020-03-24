using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AccordGenetic.Wrap
{
    public interface IGeneticWrap
    {
        Population Population { get; set; }
        Result Evaluate();
    }




    public static class IGeneticWrapEx
    {





        public static IObservable<Result> RunMultipleEpochs(this IGeneticWrap wrap, int iterations)
        {
           return Enumerable.Range(1, iterations).ToObservable()
    .SelectMany(x => Observable.Start(() =>
    {
        wrap.Population.RunEpoch();
        return wrap.Evaluate();
    }, TaskPoolScheduler.Default));


        }

        public static void RunMultipleEpochs(this IGeneticWrap wrap, int iterations, IProgress<KeyValuePair<int, Result>> progress)
        {
            for (int i = 1; i < iterations; i++)
            {
                // run one epoch of genetic algorithm
                wrap.Population.RunEpoch();
                var t = wrap.Evaluate();
                progress?.Report(new KeyValuePair<int, Result>(i, t));

            }

        }

        public static void RunMultipleEpochs(this IGeneticWrap wrap, int iterations, CancellationToken token, IProgress<KeyValuePair<int, Result>> progress = null)
        {

            for (int i = 1; i < iterations; i++)
            {

                if (token.IsCancellationRequested == true) return;

                wrap.Population.RunEpoch();
                var t = wrap.Evaluate();

                progress?.Report(new KeyValuePair<int, Result>(i, t));


            }

        }


    }
}
