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
        //SetText(currentIterationBox, i.ToString());
        //SetText(currentLearningErrorBox, learningError.ToString("F3"));
        //SetText(currentPredictionErrorBox, predictionError.ToString("F3"));

    }
}
