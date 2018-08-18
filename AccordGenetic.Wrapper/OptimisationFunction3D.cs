using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Accord.Genetic.OptimizationFunction2D;



// AForge Genetic Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2007-2011
// contacts@aforgenet.com
//

namespace Accord.Genetic
{



    public abstract class OptimizationFunction3D : IFitnessFunction
    {


        // optimization ranges
        private Range _rangeX = new Range(0, 1);
        private Range _rangeY = new Range(0, 1);
        private Range _rangeZ = new Range(0, 1);
        // optimization mode
        private Modes mode = Modes.Maximization;

        /// <summary>
        /// X variable's optimization range.
        /// </summary>
        /// 
        /// <remarks>Defines function's X range. The function's extreme will
        /// be searched in this range only.
        /// </remarks>
        /// 
        public Range RangeX
        {
            get { return _rangeX; }
            set { _rangeX = value; }
        }

        /// <summary>
        /// Y variable's optimization range.
        /// </summary>
        /// 
        /// <remarks>Defines function's Y range. The function's extreme will
        /// be searched in this range only.
        /// </remarks>
        /// 
        public Range RangeY
        {
            get { return _rangeY; }
            set { _rangeY = value; }
        }


        public Range RangeZ
        {
            get { return _rangeZ; }
            set { _rangeZ = value; }

        }
        /// <summary>
        /// Optimization mode.
        /// </summary>
        ///
        /// <remarks>Defines optimization mode - what kind of extreme to search.</remarks> 
        ///
        public Modes Mode
        {
            get { return mode; }
            set { mode = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OptimizationFunction2D"/> class.
        /// </summary>
        ///
        /// <param name="rangeX">Specifies X variable's range.</param>
        /// <param name="rangeY">Specifies Y variable's range.</param>
        ///
        public OptimizationFunction3D(Range rangeX, Range rangeY, Range rangeZ)
        {
            this._rangeX = rangeX;
            this._rangeY = rangeY;
            this._rangeZ = rangeZ;
        }


        /// <summary>
        /// Evaluates chromosome.
        /// </summary>
        /// 
        /// <param name="chromosome">Chromosome to evaluate.</param>
        /// 
        /// <returns>Returns chromosome's fitness value.</returns>
        ///
        /// <remarks>The method calculates fitness value of the specified
        /// chromosome.</remarks>
        ///
        public double Evaluate(IChromosome chromosome)
        {

            // do native translation first
            double[] xyz = Translate(chromosome);
            // get function value
            double functionValue = OptimizationFunction(xyz[0], xyz[1],xyz[2]);
            // return fitness value
            return (mode == Modes.Maximization) ? functionValue : 1 / functionValue;
        }

        /// <summary>
        /// Translates genotype to phenotype 
        /// </summary>
        /// 
        /// <param name="chromosome">Chromosome, which genoteype should be
        /// translated to phenotype</param>
        ///
        /// <returns>Returns chromosome's fenotype - the actual solution
        /// encoded by the chromosome</returns> 
        /// 
        /// <remarks>The method returns array of two double values, which
        /// represent function's input point (X and Y) encoded by the specified
        /// chromosome.</remarks>
        ///
        public double[] Translate(IChromosome chromosome)
        {
            // get chromosome's value
            ulong val = ((BinaryChromosome)chromosome).Value;
            // chromosome's length
            int length = ((BinaryChromosome)chromosome).Length;
            // length of X component
            int xLength = length / 3;
            // length of Y component
            int yLength = length /3;

            // length of Z component
            int zLength = length - (xLength+yLength);


            // X maximum value - equal to X mask
            ulong xMax = 0xFFFFFFFFFFFFFFFF >> (64 - xLength);
            // Y maximum value
            ulong yMax = 0xFFFFFFFFFFFFFFFF >> (64 - yLength);
            // Z maximum value
            ulong zMax = 0xFFFFFFFFFFFFFFFF >> (64 - zLength);


            // X component
            double xPart = val & xMax;
            // Y component;
            double yPart = val >> xLength;

            // Z component;
            double zPart = val >>( xLength & yLength);


            // translate to optimization's funtion space
            double[] ret = new double[2];

            ret[0] = xPart * _rangeX.Length / xMax + _rangeX.Min;
            ret[1] = yPart * _rangeY.Length / yMax + _rangeY.Min;
            ret[2] = zPart * _rangeZ.Length / zMax + _rangeZ.Min;

            return ret;
        }

        /// <summary>
        /// Function to optimize.
        /// </summary>
        ///
        /// <param name="x">Function X input value.</param>
        /// <param name="y">Function Y input value.</param>
        /// 
        /// <returns>Returns function output value.</returns>
        /// 
        /// <remarks>The method should be overloaded by inherited class to
        /// specify the optimization function.</remarks>
        ///
        public abstract double OptimizationFunction(double x, double y,double z);
    }
}
