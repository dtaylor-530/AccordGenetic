using Accord;
using Accord.Genetic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccordGenetic.Wrapper
{

    public class UserFunction : OptimizationFunction1D
    {
        public UserFunction() : base(new Range(0, 255)) { }

        public override double OptimizationFunction(double x)
        {
            return Math.Cos(x / 23) * Math.Sin(x / 50) + 2;
        }
    }



    public class DefaultFunction2D : OptimizationFunction2D
    {
        public DefaultFunction2D() : base(new Range(0, 255), new Range(0, 255)) { }

        public override double OptimizationFunction(double x, double y)
        {
            return Math.Cos(x / 23) * Math.Sin(y / 50) + 2;
        }
    }

    public class UserFunction2D : OptimizationFunction2D
    {

        Func<double, double, double> _func;
        public UserFunction2D(Func<double, double, double> func) : base(new Range(0, 50), new Range(0, 50)) { _func = func; }


        public override double OptimizationFunction(double x, double y)
        {
            return _func(x, y);
        }
    }
}
