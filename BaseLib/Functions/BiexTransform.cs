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
            this.basisText.Text = "0";
            this.NegText.Text = "0";
            trackbasis.ValueChanged += new System.EventHandler(trackbasis_ValueChanged);
            NegValues.ValueChanged += new System.EventHandler(NegValues_ValueChanged);
            minvalue.TextChanged += new System.EventHandler(minvalue_TextChanged);
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.NegValues);
        }

        private void trackbasis_ValueChanged(object sender, System.EventArgs e)
        {
            basisText.Text = trackbasis.Value.ToString();
            takebasisvalue(trackbasis.Value);
        }
         public int takebasisvalue(int ciao)
        {
            int boh = ciao;
            return boh;
        }

        private void NegValues_ValueChanged(object sender, System.EventArgs e)
        {
            NegText.Text = NegValues.Value.ToString();
        }

        private void minvalue_TextChanged(object sender, System.EventArgs e)
        {
            minvalue.Text = MinValue.ToString();
        }

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
     
        }

        private void trackbasis_Scroll(object sender, EventArgs e)
        {
          
        }

        private void NegValues_Scroll(object sender, EventArgs e)
        {
        
        }

        private void n_value_Click(object sender, EventArgs e)
        {
          
        }
    }
}
