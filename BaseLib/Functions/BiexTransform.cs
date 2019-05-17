using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Graph;
using BaseLibS.Graph.Axis;
using BaseLib.Forms;

namespace BaseLib.Functions
{
    public partial class BiexTransform : Form
    {

        public BiexTransform()
        {
            InitializeComponent();
        }

    

        public string TextBoxMinValue { get { return minvalue.Text; } set { minvalue.Text = value; } }
        public string TextBoxMaxValue { get { return maxvalue.Text; } set { maxvalue.Text = value; } }

        public string MinValue {  get { return minvalue.Text; } set { minvalue.Text = value; } }
        public string MaxValue {  get { return maxvalue.Text; } set { maxvalue.Text = value; } }

        public int NegTrackMinValue {  get { return NegValues.Minimum; } set { NegValues.Minimum = value; } }
        public int BasisTrackMinValue { get { return trackbasis.Minimum; } set { trackbasis.Minimum = value; } }
        public int NegTrackMaxValue { get { return NegValues.Maximum; } set { NegValues.Maximum = value; } }
        public int BasisTrackMaxValue { get { return trackbasis.Maximum; } set { trackbasis.Maximum = value; } }


        private void BiexTransform_Load(object sender, EventArgs e)
        {
        }

        private void basisvalue_Click(object sender, EventArgs e)
        {
            basisvalue.Text = trackbasis.Value.ToString();
        }

        private void trackbasis_Scroll(object sender, EventArgs e)
        {
            basisvalue.Text = trackbasis.Value.ToString();
        }

        private void NegValues_Scroll(object sender, EventArgs e)
        {
            n_value.Text = NegValues.Value.ToString();
        }

        private void n_value_Click(object sender, EventArgs e)
        {
            n_value.Text = NegValues.Value.ToString();
        }
    }
}
