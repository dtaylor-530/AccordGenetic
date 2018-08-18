using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccordGenetic.Wrapper
{
    public struct Result
    {

        public string BestSolution { get; }

        public double[,]Output { get; }

        public Result(double[,] output,string solution)
        {
             BestSolution = solution;
            Output = output;
        }


    }

    public struct Result2<ROutput>
    {

        public double Score { get; }

        public ROutput Output { get; }

        public double[] Parameters { get; }

        public Result2(ROutput output, double score, double[] parameters)
        {
            Score = score;
            Output = output;
            Parameters = parameters;
        }


    }

}
