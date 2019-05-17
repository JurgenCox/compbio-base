using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLib.Functions
{
    public class BiexFunction
    {
        public double TopScale(IList<double> values)
        {
            double[] result = new double[values.Count];
            double largest = result[0];
            for (int i = 1; i < result.Length; i++)
            {
                if (result[i] > largest)
                    largest = result[i];
            }
            return largest;
        }


        public double MinScale(IList<double> values)
        {
            double[] result = new double[values.Count];
            double min = result[0];
            for (int i = 1; i < result.Length; i++)
            {
                if (result[i] < min)
                    min = result[i];
            }
            return min;
        }

    }
}
