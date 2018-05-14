using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using BaseLib.Forms.Base;
using BaseLibS.Graph;

namespace BaseLib.Graphic{
	public static class ExportGraphics{
		public static void ExportGraphic(Control control, string filename){
			ExportGraphic(control, filename, true);
		}

		public static void ExportGraphic(Control control, string filename, bool showDialog){
			if (control == null){
				return;
			}
			ExportGraphic(control, filename, control.Width, control.Height, showDialog);
		}

		private static string ShowDialog(string filename){
			SaveFileDialog dialog = new SaveFileDialog{
				Filter = "Pdf file (*.pdf)|*.pdf|Svg file (*.svg)|*.svg|PNG file (*.png)|*.png|JPEG file (*.jpg)|*.jpg",
				FileName = filename
			};
			return dialog.ShowDialog() == DialogResult.OK ? dialog.FileName : null;
		}

		private static void ExportGraphic(Control control, string filename, int width, int height, bool showDialog){
			if (showDialog){
				filename = ShowDialog(filename);
			}
			if (filename == null){
				return;
			}
			Stream stream = null;
			IGraphics graphics;
			ImageFormat imageFormat = null;
			string extension = System.IO.Path.GetExtension(filename);
			if (!string.IsNullOrEmpty(extension)){
				extension = extension.ToLower();
			}
			switch (extension){
				case ".svg":
					if (File.Exists(filename)){
						try{
							File.Delete(filename);
						} catch (Exception){
							MessageBox.Show("The file " + filename + " is open in another program.");
							return;
						}
					}
					stream = new FileStream(filename, FileMode.CreateNew);
					graphics = new SvgGraphics(stream, width, height);
					break;
				case ".pdf":
					if (File.Exists(filename)){
						try{
							File.Delete(filename);
						} catch (Exception){
							MessageBox.Show("The file " + filename + " is open in another program.");
							return;
						}
					}
					stream = new FileStream(filename, FileMode.CreateNew);
					graphics = new PdfGraphics(stream, width, height);
					break;
				case ".png":
					imageFormat = ImageFormat.Png;
					Graphics g = control.CreateGraphics();
					g.Clip = new Region(new RectangleF(0, 0, width, height));
					graphics = new CGraphics(g);
					break;
				case ".jpg":
					imageFormat = ImageFormat.Jpeg;
					Graphics g2 = control.CreateGraphics();
					g2.Clip = new Region(new RectangleF(0, 0, width, height));
					graphics = new CGraphics(g2);
					break;
				default:
					MessageBox.Show("Could not find specified fileformat " + extension);
					return;
			}
			control.CreateControl();
			if (graphics is CGraphics){
				CGraphics g = (CGraphics) graphics;
				Bitmap bitmap = new Bitmap(width, height, g.Graphics);
				control.DrawToBitmap(bitmap, new Rectangle(0, 0, width, height));
				bitmap.Save(filename, imageFormat);
				bitmap.Dispose();
				graphics.Dispose();
			} else if (control is BasicControl){
				((BasicControl) control).view.Print(graphics, control.Width, control.Height);
			} else{
				DoPaintBackground(graphics, control);
				DoPaint(graphics, control);
			}
			if (stream != null){
				graphics.Dispose();
				stream.Close();
				stream.Dispose();
			}
		}

		public static void DoPaint(IGraphics g, Control control){
			DoPaint(g, control, 0, 0);
		}

		public static void DoPaint(IGraphics g, Control container, int addx, int addy){
			DoPaintBackground(g, container);
			if (container is Label){
				DoPaint(g, (Label) container);
			} else if (container is TextBox){
				DoPaint(g, (TextBox) container);
			} else if (container is RichTextBox){
				DoPaint(g, (RichTextBox) container);
			} else{
				(container as BaseDrawControl)?.DoPaint(g);
				foreach (Control control in container.Controls){
					g.SetClippingMask(control.Width, control.Height, container.Location.X + control.Location.X + addx,
						container.Location.Y + control.Location.Y + addy);
					// Clipping mask: (50,50) 682x306
					Debug.WriteLine("Clipping mask: (" + (container.Location.X + control.Location.X) + "," +
						(container.Location.Y + control.Location.Y) + ") " + control.Width + "x" + control.Height);
					if (control is TableLayoutPanel){
						Debug.WriteLine("TableLayoutPanel");
						DoPaint(g, control as TableLayoutPanel, container.Location.X + addx, container.Location.Y + addy);
					} else if (control is SplitContainer){
						Debug.WriteLine("SplitContainer");
						DoPaint(g, control as SplitContainer);
					} else if (control is SplitterPanel){
						Debug.WriteLine("SplitterPanel");
						DoPaint(g, control as SplitterPanel);
					} else if (control is TextBox){
						Debug.WriteLine("TextBox");
						DoPaint(g, control as TextBox);
					} else if (control is Label){
						Debug.WriteLine("Label");
						DoPaint(g, control as Label);
					} else if (control is RichTextBox){
						Debug.WriteLine("RichTextBox");
						DoPaint(g, control as RichTextBox);
					} else if (control is DataGridView){
						Debug.WriteLine("DataGridView");
						DoPaint(g, control as DataGridView);
					} else if (control is BasicControl){
						Debug.WriteLine("-" + control.GetType().Name);
						((BasicControl) control).view.Print(g, control.Width, control.Height);
					} else if (control is Panel){
						DoPaint(g, control as Panel, container.Location.X + addx, container.Location.Y + addy);
					} else{
						DoPaint(g, control);
					}
				}
			}
		}

		internal static void DoPaint(IGraphics g, TableLayoutPanel table){
			DoPaint(g, table as Control);
		}

		internal static void DoPaint(IGraphics g, SplitterPanel container){
			DoPaint(g, container as Control);
		}

		internal static void DoPaint(IGraphics g, SplitContainer container){
			DoPaint(g, container.Panel1);
			DoPaint(g, container.Panel2);
		}

		internal static void DoPaint(IGraphics g, Panel control, int addx, int addy){
			g.FillRectangle(new Brush2(GraphUtils.ToColor2(control.BackColor) ), 0, 0, control.Width, control.Height);
			if (control.BorderStyle == BorderStyle.FixedSingle){
				g.DrawRectangle(new Pen2(GraphUtils.ToColor2(control.ForeColor)), 0, 0, control.Width, control.Height);
			}
			if (control is TableLayoutPanel){
				DoPaint(g, (Control) control, addx, addy);
			} else{
				foreach (Control c in control.Controls){
					if (c is BasicControl){
						Debug.WriteLine("- BasicControl");
						g.SetClippingMask(c.Width, c.Height, control.Location.X + c.Location.X + addx,
							control.Location.Y + c.Location.Y + addy);
						((BasicControl) c).view.Print(g, c.Width, c.Height);
					} else{
						DoPaint(g, c);
					}
				}
			}
		}

		internal static void DoPaint(IGraphics g, DataGridView grid){
			Pen2 p = new Pen2(GraphUtils.ToColor2(grid.GridColor));
			float x = grid.Location.X;
			float y = grid.Location.Y;
			Brush2 text = new Brush2(GraphUtils.ToColor2(grid.ColumnHeadersDefaultCellStyle.ForeColor));
			Font2 headerFont = GraphUtils.ToFont2(grid.ColumnHeadersDefaultCellStyle.Font) ; 
			StringFormat2 headerformat = new StringFormat2{
				Alignment = StringAlignment2.Near,
				LineAlignment = StringAlignment2.Center
			};
			for (int c = 0; c < grid.Columns.Count; c++){
				Brush2 header = new Brush2(GraphUtils.ToColor2(grid.Columns[c].HeaderCell.Style.BackColor));
				if (header.Color.IsEmpty || header.Color == Color2.Transparent || header.Color == Color2.Black ||
					header.Color.Name == "0"){
					header.Color = GraphUtils.ToColor2(grid.ColumnHeadersDefaultCellStyle.BackColor);
				}
				g.FillRectangle(header, x, y, grid.Columns[c].Width, grid.ColumnHeadersHeight);
				g.DrawRectangle(p, x, y, grid.Columns[c].Width, grid.ColumnHeadersHeight);
				g.DrawString(grid.Columns[c].HeaderText, headerFont, text,
					new Rectangle2(x, y, grid.Columns[c].Width, grid.ColumnHeadersHeight), headerformat);
				x += grid.Columns[c].Width;
			}
			y += grid.ColumnHeadersHeight;
			for (int r = 0; r < grid.Rows.Count; r++){
				x = grid.Location.X;
				for (int c = 0; c < grid.Columns.Count; c++){
					Color2 backcolor = GetBackColor(grid, r, c);
					Color2 forecolor = GetForeColor(grid, r, c);
					if (!backcolor.IsEmpty){
						g.FillRectangle(new Brush2(backcolor), x, y, grid.Columns[c].Width, grid.Rows[r].Height);
					}
					g.DrawRectangle(p, x, y, grid.Columns[c].Width, grid.Rows[r].Height);
					object value = grid.Rows[r].Cells[c].Value;
					if (value != null){
						Font2 font = GraphUtils.ToFont2(grid.DefaultCellStyle.Font) ;
						StringFormat2 cellformat = GetStringFormat(grid.Columns[c].DefaultCellStyle.Alignment);
						string t = value.ToString();
						if (!string.IsNullOrEmpty(grid.Columns[c].DefaultCellStyle.Format)){
							string format = "{0:" + grid.Columns[c].DefaultCellStyle.Format.ToLower() + "}";
							t = string.Format(format, value);
						}
						g.DrawString(t, font, new Brush2(forecolor), new Rectangle2(x, y, grid.Columns[c].Width, grid.Rows[r].Height),
							cellformat);
					}
					x += grid.Columns[c].Width;
				}
				y += grid.Rows[r].Height;
			}
		}

		private static void DoPaint(IGraphics g, TextBox textBox){
			g.FillRectangle(new Brush2(GraphUtils.ToColor2(textBox.BackColor)), textBox.Location.X, textBox.Location.Y,
				textBox.Width - textBox.Margin.Left - textBox.Margin.Right,
				textBox.Height - textBox.Margin.Top - textBox.Margin.Bottom);
			Rectangle2 rect = new Rectangle2(GraphUtils.ToPointF2(textBox.Location) , GraphUtils.ToSize2(textBox.Size));
			StringFormat2 format = new StringFormat2{Alignment = StringAlignment2.Near, LineAlignment = StringAlignment2.Near};
			g.DrawString(textBox.Text, GraphUtils.ToFont2(textBox.Font) , new Brush2(GraphUtils.ToColor2(textBox.ForeColor)), rect, format);
		}

		private static void DoPaint(IGraphics g, RichTextBox textBox){
			g.FillRectangle(new Brush2(GraphUtils.ToColor2(textBox.BackColor)), textBox.Location.X, textBox.Location.Y,
				textBox.Width - textBox.Margin.Left - textBox.Margin.Right,
				textBox.Height - textBox.Margin.Top - textBox.Margin.Bottom);
			string[] lines = textBox.Text.Split('\n');
			float h = 0;
			foreach (string t in lines){
				g.DrawString(t, GraphUtils.ToFont2(textBox.Font), new Brush2(GraphUtils.ToColor2(textBox.ForeColor)), textBox.Location.X, textBox.Location.Y + h);
				h += textBox.Font.Height*0.8f;
			}
		}

		private static void DoPaint(IGraphics g, Label label){
			Rectangle2 rect = new Rectangle2(GraphUtils.ToPointF2(label.Location) , GraphUtils.ToSizeF2(label.Size) );
			g.FillRectangle(new Brush2(GraphUtils.ToColor2(label.BackColor)), rect.X, rect.Y, label.Width - label.Margin.Left - label.Margin.Right,
				label.Height - label.Margin.Top - label.Margin.Bottom);
			StringFormat2 format = new StringFormat2{Alignment = StringAlignment2.Near, LineAlignment = StringAlignment2.Near};
			g.DrawString(label.Text, GraphUtils.ToFont2(label.Font), new Brush2(GraphUtils.ToColor2(label.ForeColor)), rect, format);
		}

		private static StringFormat2 GetStringFormat(DataGridViewContentAlignment alignment){
			StringFormat2 format = new StringFormat2();
			switch (alignment){
				case DataGridViewContentAlignment.BottomCenter:
					format.Alignment = StringAlignment2.Center;
					format.LineAlignment = StringAlignment2.Far;
					break;
				case DataGridViewContentAlignment.BottomLeft:
					format.Alignment = StringAlignment2.Near;
					format.LineAlignment = StringAlignment2.Far;
					break;
				case DataGridViewContentAlignment.BottomRight:
					format.Alignment = StringAlignment2.Far;
					format.LineAlignment = StringAlignment2.Far;
					break;
				case DataGridViewContentAlignment.MiddleCenter:
					format.Alignment = StringAlignment2.Center;
					format.LineAlignment = StringAlignment2.Center;
					break;
				case DataGridViewContentAlignment.MiddleLeft:
					format.Alignment = StringAlignment2.Near;
					format.LineAlignment = StringAlignment2.Center;
					break;
				case DataGridViewContentAlignment.MiddleRight:
					format.Alignment = StringAlignment2.Far;
					format.LineAlignment = StringAlignment2.Center;
					break;
				case DataGridViewContentAlignment.TopCenter:
					format.Alignment = StringAlignment2.Center;
					format.LineAlignment = StringAlignment2.Near;
					break;
				case DataGridViewContentAlignment.TopLeft:
					format.Alignment = StringAlignment2.Near;
					format.LineAlignment = StringAlignment2.Near;
					break;
				case DataGridViewContentAlignment.TopRight:
					format.Alignment = StringAlignment2.Far;
					format.LineAlignment = StringAlignment2.Near;
					break;
				case DataGridViewContentAlignment.NotSet:
					format.Alignment = StringAlignment2.Near;
					format.LineAlignment = StringAlignment2.Center;
					break;
			}
			return format;
		}

		private static Color2 GetBackColor(DataGridView grid, int r, int c){
			if (grid.Rows[r].Cells[c].Selected){
				return GraphUtils.ToColor2(grid.Rows[r].Cells[c].InheritedStyle.SelectionBackColor);
			}
			Color2 backcolor = Color2.Transparent;
			if (!grid.Rows[r].Cells[c].Style.BackColor.IsEmpty){
				backcolor = GraphUtils.ToColor2(grid.Rows[r].Cells[c].Style.BackColor);
			}
			if (!grid.Columns[c].DefaultCellStyle.BackColor.IsEmpty){
				backcolor = GraphUtils.ToColor2(grid.Columns[c].DefaultCellStyle.BackColor);
			}
			return backcolor;
		}

		private static Color2 GetForeColor(DataGridView grid, int r, int c){
			if (grid.Rows[r].Cells[c].Selected){
				return GraphUtils.ToColor2(grid.Rows[r].Cells[c].InheritedStyle.SelectionForeColor);
			}
			Color2 forecolor = Color2.Transparent;
			if (!grid.Rows[r].Cells[c].Style.ForeColor.IsEmpty){
				forecolor = GraphUtils.ToColor2(grid.Rows[r].Cells[c].Style.ForeColor);
			}
			if (!grid.Columns[c].DefaultCellStyle.ForeColor.IsEmpty){
				forecolor = GraphUtils.ToColor2(grid.Columns[c].DefaultCellStyle.ForeColor);
			}
			return forecolor;
		}

		public static void DoPaintBackground(IGraphics g, Control control){
			if (g is CGraphics && (!control.BackColor.IsEmpty && control.BackColor != Color.Transparent)){
				g.FillRectangle(new Brush2(GraphUtils.ToColor2(control.BackColor)), control.Location.X, control.Location.Y, control.Width,
					control.Height);
			}
			(control as BasicControl)?.view.Print(g, control.Width, control.Height);
			if (control is Panel && ((Panel) control).BorderStyle == BorderStyle.FixedSingle){
				g.DrawRectangle(new Pen2(GraphUtils.ToColor2(control.ForeColor)), control.Location.X, control.Location.Y, control.Width, control.Height);
			}
		}
	}
}