using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BaseLib.Forms
{
    public partial class RegexReplaceParamControl : UserControl
    {
        private Regex _pattern;
        private string _replacement;
        private List<string> _previews;

        public RegexReplaceParamControl(Regex pattern, string replacement, List<string> previews)
        {
            InitializeComponent();
            _pattern = pattern;
            _replacement = replacement;
            _previews = previews;

            PatternTextBox.Text = Pattern;
            PatternTextBox.TextChanged += (sender, args) =>
            {
                Pattern = PatternTextBox.Text;
            };
            ReplaceTextBox.Text = Replacement;
            ReplaceTextBox.TextChanged += (sender, args) =>
            {
                Replacement = ReplaceTextBox.Text;
            };

            CurrentListBox.Items.Clear();
            CurrentListBox.Items.AddRange(Current);

            UpdatePreview();
        }

        private string[] Current => _previews.ToArray();
        private string[] Preview => _previews.Select(str => _pattern.Replace(str, _replacement)).ToArray();

        public string Replacement
        {
            get => _replacement;
	        set
            {
                if (_replacement != value)
                {
                    _replacement = value;
                    UpdatePreview();
                }
            }
        }

        public string Pattern
        {
            get => _pattern.ToString();
	        set
            {
                try
                {
                    Regex pattern = new Regex(value);
                    if (_pattern != pattern)
                    {
                        _pattern = pattern;
                        UpdatePreview();
                    }
                }
                catch (ArgumentException)
                {
                    Debug.WriteLine("Illegal regex");
                }
            }
        }

        private void UpdatePreview()
        {
            PreviewListBox.Items.Clear();
            PreviewListBox.Items.AddRange(Preview);
        }

        public Tuple<Regex, string> GetValue() => Tuple.Create(_pattern, _replacement);
    }
}
