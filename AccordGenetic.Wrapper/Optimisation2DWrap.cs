using Accord;
using Accord.Genetic;
using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace AccordGenetic.Wrapper
{

    //public enum Optimisation
    //{
    //    Maximum,
    //    Minimum 


    //}


    public enum Selection
    {
        Elite,
        Rank,
        Roulette

    }


    public class Optimisation2DWrap : IGeneticWrap
    {
        public Population Population { get; set; }
        bool _showOnlyBest;
        double[,] _solution;
        OptimizationFunction2D _userFunction;
        private int _populationSize;

        public Optimisation2DWrap(int populationSize, int chromosomeLength, OptimizationFunction2D userFunction, Selection selection, bool showOnlyBest)
        {
            _showOnlyBest = showOnlyBest;
            _userFunction = userFunction;
            _populationSize = populationSize;

            // create population
            Population = new Population(
               size: populationSize,
               ancestor: new BinaryChromosome(chromosomeLength),
               fitnessFunction: userFunction,
               selectionMethod: (selection == Selection.Elite) ? (ISelectionMethod)new EliteSelection() : (selection == Selection.Rank) ? (ISelectionMethod)new RankSelection() : (ISelectionMethod)new RouletteWheelSelection());
            

            _solution = new double[(showOnlyBest) ? 1 : populationSize, 2];


        }

        public Optimisation2DWrap(int populationSize, IChromosome chromosome, OptimizationFunction2D userFunction, Selection selection, bool showOnlyBest)
        {
            _showOnlyBest = showOnlyBest;
            _userFunction = userFunction;
            _populationSize = populationSize;

            // create population
            Population = new Population(
               size: populationSize,
               ancestor:chromosome,
               fitnessFunction: userFunction,
               selectionMethod: (selection == Selection.Elite) ? (ISelectionMethod)new EliteSelection() : (selection == Selection.Rank) ? (ISelectionMethod)new RankSelection() : (ISelectionMethod)new RouletteWheelSelection());


            _solution = new double[(showOnlyBest) ? 1 : populationSize, 2];

        }



        public Result Evaluate()
        {

            //// show current solution
            //if (_showOnlyBest)
            //{
            var d = _userFunction.Translate(Population.BestChromosome);
            _solution[0, 1] = _userFunction.OptimizationFunction(d[0], d[1]);
            //}
            //else
            //{
            //    for (int j = 0; j < _populationSize; j++)
            //    {
            //        _solution[j, 0] = _userFunction.Translate(Population[j]);
            //        _solution[j, 1] = _userFunction.OptimizationFunction(_solution[j, 0]);
            //    }
            //}

            return new Result(_solution, d.ToString("F3"));
        }



    }






    public static class AccordWrapFactory
    {


        public static Optimisation2DWrap BuildDefaultMaximisation(OptimizationFunction2D of)
        {
            of.Mode = OptimizationFunction2D.Modes.Maximization;
            return new Optimisation2DWrap(20, 64, of, Selection.Elite, false);
        }

        public static Optimisation2DWrap BuildDefaultMinimisation(OptimizationFunction2D of)
        {
            of.Mode = OptimizationFunction2D.Modes.Minimization;
            return new Optimisation2DWrap(100, 64, of, Selection.Elite, false);
        }


        public static Optimisation2DWrap BuildDefaultMinimisation(OptimizationFunction2D of,IChromosome bc)
        {
            of.Mode = OptimizationFunction2D.Modes.Minimization;
            return new Optimisation2DWrap(100, bc, of, Selection.Elite, false);
        }
    }
}
