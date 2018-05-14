using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BaseLib.Forms{
	public partial class MultiFileParameterControl : UserControl{
		public MultiFileParameterControl(){
			InitializeComponent();
			addButton.Click += AddButton_OnClick;
			removeButton.Click += RemoveButton_OnClick;
		}

		private void AddButton_OnClick(object sender, EventArgs e){
			OpenFileDialog ofd = new OpenFileDialog{Multiselect = true, Filter = Filter};
			if (ofd.ShowDialog() == DialogResult.OK){
				AddFastaFiles(ofd.FileNames, listBox1);
			}
		}

		private void RemoveButton_OnClick(object sender, EventArgs e){
			if (listBox1.SelectedIndex >= 0){
				listBox1.Items.RemoveAt(listBox1.SelectedIndex);
			}
		}

		public string[] Filenames{
			get => ToStrings(listBox1.Items);
			set{
				listBox1.Items.Clear();
				foreach (string s in value){
					listBox1.Items.Add(s);
				}
			}
		}

		public string Filter { get; set; }

		private static void AddFastaFiles(IEnumerable<string> filenames, ListBox listbox){
			string[] names = ToStrings(listbox.Items);
			foreach (string file in filenames.Where(file => !names.Contains(file))){
				listbox.Items.Add(file);
			}
		}

		private static string[] ToStrings(IList collection){
			string[] result = new string[collection.Count];
			for (int i = 0; i < result.Length; i++){
				result[i] = collection[i].ToString();
			}
			return result;
		}
	}
}