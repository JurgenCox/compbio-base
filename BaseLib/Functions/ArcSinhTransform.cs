﻿using BaseLibS.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace BaseLib.Functions
{
    public partial class ArcSinhTransform : Form
    {
        public double widthbasisvalue = 0;
        public double maxvalueclick = 0;
        public ArcSinhTransform(string title, double minValue, double maxValue)
        {
            InitializeComponent();
            MinValue = minValue;
            MaxValue = maxValue;
            maxvalueclick = maxValue;
            Title = title;
            this.NegValues.Minimum = 0.00;
            this.NegValues.Maximum = 3.00;
            this.NegValues.Value = 0.5;
            this.trackbasis.Minimum = 25;
            this.trackbasis.Maximum = 455;
            maxValueTextBox.Text = maxvalueclick.ToString();
            minusMaxValue.Click += minusMaxValue_OnClick;
            plusMaxValue.Click += plusMaxValue_OnClick;
            // Make button1's dialog result OK.
            okButton.DialogResult = DialogResult.OK;
            // Make button2's dialog result Cancel.
           cancelButton.DialogResult = DialogResult.Cancel;
            widthbasisvalue = this.trackbasis.Minimum;
            this.basisText.Text = trackbasis.Minimum.ToString();
            trackbasis.ValueChanged += new System.EventHandler(trackbasis_ValueChanged);
            NegValues.ValueChanged += new System.EventHandler(NegValues_ValueChanged);
            this.Controls.Add(this.trackbasis);
            this.Controls.Add(this.NegValues);

            this.NegText.Text = (this.NegValues.Value).ToString();
        }


        private void minusMaxValue_OnClick(object sender, EventArgs e)
        {
            maxvalueclick = maxvalueclick + (-(Math.Pow(10, (-0.10)) * 10));
            maxValueTextBox.Text = maxvalueclick.ToString();
        }

        private void plusMaxValue_OnClick(object sender, EventArgs e)
        {
            maxvalueclick = maxvalueclick + ((Math.Pow(10, (-0.10)) * 10));
            maxValueTextBox.Text = maxvalueclick.ToString();
        }

        private void trackbasis_ValueChanged(object sender, System.EventArgs e)
        {
            basisText.Text = trackbasis.Value.ToString();
       //     takebasisvalue(trackbasis.Value);
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


        internal string Title
        {
            get => titleTextBox.Text;
            set => titleTextBox.Text = value;
        }

        public double ExtraNeg
        {
            get => Parser.Double(NegText.Text);
            set => NegText.Text = "" + value;
        }

        public double GetMaximumZoom
        {
            get => Parser.Double(maxValueTextBox.Text);
            set => maxValueTextBox.Text = "" + value;
        }



        public double MinValue
        {
            get
            {
                double x;
                bool s = Parser.TryDouble(minValueTextBox.Text, out x);
                if (!s)
                {
                    return double.NaN;
                }
                return double.IsInfinity(x) ? double.NaN : x;
            }
            set => minValueTextBox.Text = "" + value;
        }
        public double MaxValue
        {
            get
            {
                double x;
                bool s = Parser.TryDouble(maxValueTextBox.Text, out x);
                if (!s)
                {
                    return double.NaN;
                }
                return double.IsInfinity(x) ? double.NaN : x;
            }
            set => maxValueTextBox.Text = "" + value;
        }

    }
}
