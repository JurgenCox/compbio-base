using BaseLibS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseLib.Functions
{
    public partial class LogarithmicTransform : Form
    {
        public double minvalueclick = 0;
        public double maxvalueclick = 0;
        public LogarithmicTransform(string title, double minValue, double maxValue)
        {
            InitializeComponent();
            minvalueclick = minValue;
            maxvalueclick = maxValue;
            minValueTextBox.Text = minvalueclick.ToString();
            maxValueTextBox.Text = maxvalueclick.ToString();
            Title = title;
            minusMinValue.Click += minusMinValue_OnClick;
            minusMaxValue.Click += minusMaxValue_OnClick;
            plusMinValue.Click += plusMinValue_OnClick;
            plusMaxValue.Click += plusMaxValue_OnClick;
            // Make button1's dialog result OK.
            okButton.DialogResult = DialogResult.OK;
            // Make button2's dialog result Cancel.
           cancelButton.DialogResult = DialogResult.Cancel;

        }


        private void minusMinValue_OnClick(object sender, EventArgs e)
        {
            minvalueclick = minvalueclick + (-(Math.Pow(10, (-0.10)) * 10));
            minValueTextBox.Text = minvalueclick.ToString();
        }

        private void minusMaxValue_OnClick(object sender, EventArgs e)
        {
            maxvalueclick = maxvalueclick + (-(Math.Pow(10, (-0.10)) * 10));
            maxValueTextBox.Text = maxvalueclick.ToString();
        }

        private void plusMinValue_OnClick(object sender, EventArgs e)
        {
            minvalueclick = minvalueclick + ((Math.Pow(10, (-0.10)) * 10));
            minValueTextBox.Text = minvalueclick.ToString();
        }

        private void plusMaxValue_OnClick(object sender, EventArgs e)
        {
            maxvalueclick = maxvalueclick + ((Math.Pow(10, (-0.10)) * 10));
            maxValueTextBox.Text = maxvalueclick.ToString();
        }

        internal string Title
        {
            get => titleTextBox.Text;
            set => titleTextBox.Text = value;
        }




        private void minValueTextBox_TextChanged(object sender, EventArgs e)
        {

        }


    }
}
