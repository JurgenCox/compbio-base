using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibS.Num
{
    public class kineticsmeasures
    {
        public double calculatemedian(double[] range)
        {
            double median = 0;
            int counts = range.Length;
            for (int i = 0; i < range.Length; i++)
            {
                median = (range.Sum() / counts);
            }
            return median;
        }
    }
}
