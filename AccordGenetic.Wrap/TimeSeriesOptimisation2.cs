using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using Accord.Genetic;

namespace AccordGenetic.Wrap
{


    public class ChromosomeWrapper
    {

        public IChromosome Chromosome { get; set; }
        public double[] Input { get; set; }
        public double Score { get; set; }
        public int Iteration { get; set; }

    }


    // minimises error in two variable function, func, according to error function, error,
    public class Optimisation2DWrap2
    {

        //    public static BackgroundWorkerObservable<ChromosomeWrapper> Create<TInput, ROutput>(Func<double, double, ROutput> func, TInput input, Func<TInput, ROutput, double> error, int count = 10)
        //    {      //function to optimise
        //        Func<double, double, double> fc = (a, b) => error(input,func(a, b));
        //        // wrap-up in class
        //        var of = new UserFunction2D(fc);

        //        var main = BuildGeneticProgram(of);
        //        return new BackgroundWorkerObservable<ChromosomeWrapper>(main, Observable.Repeat((bool?)true, 1), Observable.Repeat(count, 1), Observable.Repeat(0, 1));

        //    }

        //    //public static BackgroundWorkerObservable<Accord.Genetic.IChromosome> Create2<TInput, ROutput>(Func<double, double, ROutput> func, TInput input, Func<TInput, ROutput, double> error, int count = 10)
        //    //{

        //    //    var main = BuildFunction2(func, input, error);
        //    //    return new BackgroundWorkerObservable<Accord.Genetic.IChromosome>(main, null, Observable.Repeat((bool?)true, 1), Observable.Repeat(count, 1), Observable.Repeat(0, 1));

        //    //}


        //    public static BackgroundWorkerObservableQueue<ChromosomeWrapper> Create<TInput, ROutput>(IObservable<Tuple<TInput, Func<double, double, ROutput>>> funcinput, Func<TInput, ROutput, double> error, int count = 10)
        //    {



        //        var fc2 = funcinput.Select(_ =>
        //        {
        //            //function to optimise
        //            Func<double, double, double> fc = (a, b) => error(_.Item1,_.Item2(a, b));
        //            // wrap-up in class
        //            var of = new UserFunction2D(fc);

        //            var main = BuildGeneticProgram(of);
        //            return main;
        //        });


        //        return new BackgroundWorkerObservableQueue<ChromosomeWrapper>(fc2, Observable.Repeat((bool?)true, 1), Observable.Repeat(count, 1), Observable.Repeat(0, 1), Observable.Repeat(1, 1));

        //    }







        //public static BackgroundWorkerObservableQueue<ChromosomeWrapper> Create2<TInput, ROutput>(IObservable<Tuple<TInput, Func<double, double, ROutput>>> funcinput, Func<TInput, ROutput, double> error, int count = 10)
        //{
        //    var fc2 = funcinput.Select(_ =>
        //    {
        //        //function to optimise
        //        //function to optimise
        //        Func<double, double, double> fc = (a, b) => error(_.Item1, _.Item2(a, b));
        //        // wrap-up in class
        //        var of = new UserFunction2D(fc);

        //        return BuildFunction2<TInput, ROutput>(of);
        //    });

        //    var c =new ChromosomeWrapper { Chromosome= new BinaryChromosome(64) };

        //    return new BackgroundWorkerObservableQueue<ChromosomeWrapper>(fc2, c,Observable.Repeat((bool?)true, 1), Observable.Repeat(count, 1), Observable.Repeat(0, 1), Observable.Repeat(1, 1));



        //}





        private static Func<int, ChromosomeWrapper> BuildGeneticProgram(OptimizationFunction2D of)
        {


            // build default genetic algorithm class to minimise output
            var w = AccordWrapFactory.BuildDefaultMinimisation(of);
            //cyclical method to run
            Func<int, ChromosomeWrapper> main = (i) =>
            {
                w.Population.RunEpoch();
                // var _ = w.Evaluate();
                //var score = _.Output[0, 1];
                //double[] xy = _.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray();
                w.Population.RunEpoch();
                var x = w.Evaluate();
                return new ChromosomeWrapper { Chromosome = w.Population.BestChromosome, Iteration = i, Score = x.Output[0, 1], Input = x.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray() };

            };

            return main;
        }





        //private static Func<ChromosomeWrapper, Func<int, ChromosomeWrapper>> BuildFunction2<TInput, ROutput>(OptimizationFunction2D of)
        //{


        //    // build default genetic algorithm class to minimise output
        //    Func<ChromosomeWrapper, Func<int, ChromosomeWrapper>> f = (c) =>
        //       {
        //           var w = AccordWrapFactory.BuildDefaultMinimisation(of, c.Chromosome);
        //           Func<int, ChromosomeWrapper> main = (i) =>
        //        {
        //        w.Population.RunEpoch();

        //            w.Population.RunEpoch();
        //            var x = w.Evaluate();
        //            return new ChromosomeWrapper { Chromosome = w.Population.BestChromosome, Iteration = i, Score = x.Output[0, 1], Input = x.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray() };

        //        };
        //           return main;
        //       };
        //    //cyclical method to run


        //    return f;
        //}


        //private static Func<int, ChromosomeWrapper> BuildFunction3<TInput, ROutput>(OptimizationFunction2D of)
        //{
        //    // build default genetic algorithm class to minimise output
        //    var w = AccordWrapFactory.BuildDefaultMinimisation(of);
        //    //cyclical method to run
        //    return (i) =>
        //    {
        //        w.Population.RunEpoch();
        //        var x =w.Evaluate();
        //        return new ChromosomeWrapper {
        //            Chromosome = w.Population.BestChromosome,
        //            Iteration =i,
        //            Score =x.Output[0, 1],
        //            Input = x.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray()
                    
        //        };
        //    };

        //}


        //private static Func<int, Result2<ROutput>> BuildFunction<TInput, ROutput>(Func<double, double, ROutput> func, TInput input, Func<TInput, ROutput, double> error )
        //{
        //    //function to optimise
        //    Func<double, double, double> fc = (a, b) => error(input, func(a, b));
        //    // wrap-up in class
        //    var of = new UserFunction2D(fc);
        //    // build default genetic algorithm class to minimise output
        //    var w = AccordWrapFactory.BuildDefaultMinimisation(of);
        //    //cyclical method to run
        //    Func<int, Result2<ROutput>> main = (i) =>
        //    {
        //        w.Population.RunEpoch();
        //        var _ = w.Evaluate();
        //        var score = _.Output[0, 1];
        //        double[] xy = _.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray();
        //        return new Result2<ROutput>(func(xy[0], xy[1]), score,xy);

        //    };

        //    return main;
        //}


        //private static Func<int, Accord.Genetic.IChromosome> BuildFunction<TInput, ROutput>(Func<double, double, ROutput> func, TInput input, Func<TInput, ROutput, double> error)
        //{
        //    //function to optimise
        //    Func<double, double, double> fc = (a, b) => error(input, func(a, b));
        //    // wrap-up in class
        //    var of = new UserFunction2D(fc);
        //    // build default genetic algorithm class to minimise output
        //    var w = AccordWrapFactory.BuildDefaultMinimisation(of);
        //    //cyclical method to run
        //    Func<int, Accord.Genetic.IChromosome> main = (i) =>
        //    {
        //        w.Population.RunEpoch();
        //        //var _ = w.Evaluate();
        //        //var score = _.Output[0, 1];
        //        //double[] xy = _.BestSolution.Split(' ').Select(d => double.Parse(d)).ToArray();
        //        return w.Population.BestChromosome;

        //    };

        //    return main;
        //}


    }
}