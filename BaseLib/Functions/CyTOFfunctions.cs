using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLib.Functions
{
    public class CyTOFfunctions
    {
        public double TopBiex(IList<double> values, double topscale, double a, double w)
        {
            MessageBox.Show("topscale:" + topscale + "a:" + a + "w:" + w);
            double[] result = new double[values.Count];
            double largest = result[0];
            for (int i = 1; i < result.Length; i++)
            {
                if (result[i] > largest)
                    largest = result[i];
            }
            return largest;
        }

    }
}
