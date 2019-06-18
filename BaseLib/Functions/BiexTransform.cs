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
        public int extranegvalue = 34;
        public int widthbasisvalue = 0;
        public bool Ok { get; private set; }
        public BiexTransform()
        {
            InitializeComponent();
            this.NegValues.Minimum = 34;
            this.NegValues.Maximum = 133;
            this.trackbasis.Minimum = 25;
            this.trackbasis.Maximum = 455;
            extranegvalue = this.NegValues.Minimum;
            widthbasisvalue = this.trackbasis.Minimum;
            this.basisText.Text = trackbasis.Minimum.ToString();
            this.NegText.Text = NegValues.Minimum.ToString();

            apply_button.Text = BaseLibS.Util.Loc.Ok;
            cancel_button.Text = BaseLibS.Util.Loc.Cancel;
            trackbasis.ValueChanged += new System.EventHandler(trackbasis_ValueChanged);
            NegValues.ValueChanged += new System.EventHandler(NegValues_ValueChanged);
            minvalue.TextChanged += new System.EventHandler(minvalue_TextChanged);
            apply_button.Click += apply_button_OnClick;
            cancel_button.Click += cancel_button_OnClick;
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.NegValues);
        }

        private void apply_button_OnClick(object sender, EventArgs e)
        {
            Ok = true;
            if (extranegvalue != Convert.ToInt32(NegText.Text))
            {
                extranegvalue = Convert.ToInt32(NegText.Text);
                MessageBox.Show(extranegvalue.ToString());
            }
            Close();
        }
        private void cancel_button_OnClick(object sender, EventArgs e)
        {
            Close();
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

        public string DataValueNeg
        {
            get { return NegText.Text; }
            set { NegText.Text = value; }
        }

        private void minvalue_TextChanged(object sender, System.EventArgs e)
        {
            minvalue.Text = MinValue.ToString();
        }

        public string MinValue {  get { return minvalue.Text; } set { minvalue.Text = value; } }
        public string MaxValue {  get { return maxvalue.Text; } set { maxvalue.Text = value; } }


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
