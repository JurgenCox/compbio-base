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
    public partial class Transform : Form
    {

        public Transform(string title, double minValue, double maxValue)
        {
            InitializeComponent();
            MinValue = minValue;
            MaxValue = maxValue;
            Title = title;
            // Make button1's dialog result OK.
            okButton.DialogResult = DialogResult.OK;
            // Make button2's dialog result Cancel.
           cancelButton.DialogResult = DialogResult.Cancel;
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

        internal string Title
        {
            get => titleTextBox.Text;
            set => titleTextBox.Text = value;
        }


    }
}
