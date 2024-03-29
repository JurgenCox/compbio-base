﻿using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Drawing;
using BaseLibS.Graph;
using BaseLibS.Param;
using BaseLibS.Util;
namespace BaseLib.Param{
	public class ParameterGroupPanel : UserControl{
		private readonly ToolTip toolTip1 = new ToolTip();
		public ParameterGroup ParameterGroup{ get; private set; }
		private TableLayoutPanel grid;
		public void Init(ParameterGroup parameters1){
			Init(parameters1, 200F, 1050);
		}
		public void Init(ParameterGroup parameters1, float paramNameWidth, int totalWidth){
			ParameterGroup = parameters1;
			int nrows = ParameterGroup.Count;
			grid = new TableLayoutPanel();
			grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, paramNameWidth));
			grid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, totalWidth - paramNameWidth));
			grid.Margin = new Padding(0);
			for (int i = 0; i < nrows; i++){
				Parameter p = ParameterGroup[i];
				float h = p.Height;
				grid.RowStyles.Add(new RowStyle(SizeType.Absolute, h));
			}
			grid.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			for (int i = 0; i < nrows; i++){
				AddParameter(ParameterGroup[i], i);
			}
			grid.Controls.Add(new Control(), 0, nrows);
			Controls.Add(grid);
			Name = "ParameterPanel";
			Margin = new Padding(0, 3, 0, 3);
			grid.Dock = DockStyle.Fill;
			Dock = DockStyle.Fill;
		}
		public void SetParameters(){
			ParameterGroup p1 = ParameterGroup;
			for (int i = 0; i < p1.Count; i++){
				p1[i].SetValueFromControl();
			}
		}
		private void AddParameter(Parameter p, int i){
			LabelModel txt1 = new LabelModel(){
				Text = p.Name,
				Visible = p.Visible
			};
			BasicControl bc = BasicControl.CreateControl(txt1);
			if (!string.IsNullOrEmpty(p.Help)){
				toolTip1.SetToolTip(bc, StringUtils.ReturnAtWhitespace(p.Help));
			}
			Control c = FormUtil.GetControl(p.CreateControl());
			c.Dock = DockStyle.Fill;
			c.Margin = new Padding(0);
			c.Visible = p.Visible;
			grid.Controls.Add(c, 1, i);
			grid.Controls.Add(bc, 0, i);
		}
		public void Enable(){
			grid.Enabled = true;
		}
		public void Disable(){
			grid.Enabled = false;
		}
	}
}