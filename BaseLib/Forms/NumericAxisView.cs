using System;
using System.Collections.Generic;
using BaseLibS.Graph;
using BaseLibS.Graph.Axis;
using BaseLibS.Graph.Base;
using BaseLibS.Num;

namespace BaseLib.Forms{
	public delegate void ZoomChangeHandler(object sender, double min, double max);

	public class NumericAxisView : BasicView{
		public event ZoomChangeHandler OnZoomChange;
		private const int spacePerNumber = 110;
		public Font2 numbersFont = new Font2("Arial", 8, FontStyle2.Regular);
		public Font2 labelFont = new Font2("Arial Unicode MS", 9, FontStyle2.Regular);
		private double dragStart = double.NaN;
		private double zeroPoint = double.NaN;
		private float indicator1 = -1;
		private float indicator2 = -1;
		public AxisPositioning Positioning { get; set; }
		public AxisZoomType ZoomType { get; set; }
		public AxisMouseMode MouseMode { get; set; }
		public Color2 IndicatorColor { get; set; }
		public double TotalMin { get; set; }
		public double TotalMax { get; set; }
		public double ZoomMin { get; set; }
		public double ZoomMax { get; set; }
		public bool Reverse { get; set; }
		public bool IsLogarithmic { get; set; }
		public bool Configurable { get; set; }
		private int MaxNumIntegerDigits { get; }
		public int MajorTickLength { get; set; }
		public int MinorTickLength { get; set; }
		public float MajorTickLineWidth { get; set; }
		public float MinorTickLineWidth { get; set; }
		public float LineWidth { get; set; }
		public string Text { get; set; }

		public NumericAxisView(){
			ForeColor = Color2.Black;
			IndicatorColor = Color2.FromArgb(30, 251, 173, 73);
			IsLogarithmic = false;
			Reverse = false;
			TotalMax = double.NaN;
			TotalMin = double.NaN;
			ZeroPoint = double.NaN;
			ZoomMax = double.NaN;
			ZoomMin = double.NaN;
			MaxNumIntegerDigits = 4;
			LineWidth = 1F;
			ZoomType = AxisZoomType.None;
			MinorTickLineWidth = 1;
			MajorTickLineWidth = 1;
			MinorTickLength = 3;
			MajorTickLength = 6;
			IsLogarithmic = false;
			ZoomMax = double.NaN;
			ZoomMin = double.NaN;
			TotalMax = double.NaN;
			TotalMin = double.NaN;
			MouseMode = AxisMouseMode.Zoom;
		}

		public bool Zoomable => true;
		private double VisibleMin => ZoomType == AxisZoomType.Zoom ? ZoomMin : TotalMin;
		private double VisibleMax => ZoomType == AxisZoomType.Zoom ? ZoomMax : TotalMax;

		public double ZeroPoint{
			get => zeroPoint;
			set{
				zeroPoint = value;
				Invalidate();
			}
		}

		private int Sign => Reverse ? -1 : 1;

		internal float GetMinIndicator(int length){
			if (indicator1 != -1 && indicator2 != -1){
				return Math.Min(indicator1, indicator2);
			}
			return Reverse ? ModelToView(ZoomMax, length) : ModelToView(ZoomMin, length);
		}

		internal float GetMaxIndicator(int length){
			if (indicator1 != -1 && indicator2 != -1){
				return Math.Max(indicator1, indicator2);
			}
			return Reverse ? ModelToView(ZoomMin, length) : ModelToView(ZoomMax, length);
		}

		private float GetDeltaIndicator(int length){
			if (indicator1 != -1 && indicator2 != -1){
				return Math.Abs(indicator1 - indicator2);
			}
			return (ModelToView(ZoomMax, length) - ModelToView(ZoomMin, length))*(Reverse ? -1 : 1);
		}

		public int GetLength(int width, int height){
			return IsHorizontal() ? width : height;
		}

		public bool IsHorizontal(){
			return Positioning == AxisPositioning.Top || Positioning == AxisPositioning.Bottom;
		}

		public float ModelToView(double val, int width, int height){
			return ModelToView(val, GetLength(width, height));
		}

		public float ModelToView(double val, int length){
			float x = (float) Math.Round((val - VisibleMin)/(VisibleMax - VisibleMin)*length);
			if (Reverse){
				x = length - 1 - x;
			}
			return x;
		}

		public double ViewToModel(float x, int length){
			if (Reverse){
				x = length - 1 - x;
			}
			return VisibleMin + x*(VisibleMax - VisibleMin)/length;
		}

		public double ViewToModel(float x, int width, int height){
			return ViewToModel(x, GetLength(width, height));
		}

		private void PaintIndicator(IGraphics g, int width, int height){
			Brush2 indicatorBrush = new Brush2(IndicatorColor);
			Pen2 indicatorPen = new Pen2(ForeColor);
			float len = GetDeltaIndicator(GetLength(width, height));
			if (IsHorizontal()){
				if (len >= 1){
					g.FillRectangle(indicatorBrush, GetMinIndicator(GetLength(width, height)), 0, len, height - 1);
					g.DrawRectangle(indicatorPen, GetMinIndicator(GetLength(width, height)), 0, len, height - 1);
				} else{
					g.DrawLine(indicatorPen, GetMinIndicator(GetLength(width, height)), 0, GetMinIndicator(GetLength(width, height)),
						height - 1);
				}
			} else{
				if (len >= 1){
					g.FillRectangle(indicatorBrush, 0, GetMinIndicator(GetLength(width, height)), width - 1, len);
					g.DrawRectangle(indicatorPen, 0, GetMinIndicator(GetLength(width, height)), width - 1, len);
				} else{
					g.DrawLine(indicatorPen, 0, GetMinIndicator(GetLength(width, height)), width - 1,
						GetMinIndicator(GetLength(width, height)));
				}
			}
		}

		public void WidenRange(double min, double max, bool fullZoom){
			bool changed = false;
			if (double.IsNaN(TotalMin)){
				TotalMin = min;
				changed = true;
			} else{
				if (min < TotalMin){
					TotalMin = min;
				}
			}
			if (double.IsNaN(ZoomMin) || fullZoom){
				ZoomMin = TotalMin;
				changed = true;
			}
			if (double.IsNaN(TotalMax)){
				TotalMax = max;
				changed = true;
			} else{
				if (max > TotalMax){
					TotalMax = max;
				}
			}
			if (double.IsNaN(ZoomMax) || fullZoom){
				ZoomMax = TotalMax;
				changed = true;
			}
			Invalidate();
			if (changed){
				FireZoomChanged();
			}
		}

		public void UpdateRange(double min, double max, bool fullZoom){
			bool changed = false;
			if (double.IsNaN(TotalMin)){
				TotalMin = min;
				changed = true;
			} else{
				TotalMin = min;
			}
			if (double.IsNaN(ZoomMin) || fullZoom){
				ZoomMin = TotalMin;
				changed = true;
			}
			if (double.IsNaN(TotalMax)){
				TotalMax = max;
				changed = true;
			} else{
				TotalMax = max;
			}
			if (double.IsNaN(ZoomMax) || fullZoom){
				ZoomMax = TotalMax;
				changed = true;
			}
			Invalidate();
			if (changed){
				FireZoomChanged();
			}
		}

		public override void OnPaint(IGraphics g, int width, int height){
			OnPaint(g, 0, 0, width, height);
		}

		public void OnPaint(IGraphics g, int xm, int ym, int width, int height){
			if (!Visible){
				return;
			}
			if (TotalMax == double.NegativeInfinity){
				return;
			}
			if ((indicator1 != -1 && indicator2 != -1) || (ZoomType == AxisZoomType.Indicate && !IsFullZoom())){
				if (IsValid()){
					PaintIndicator(g, width, height);
				}
			}
			Pen2 forePen = new Pen2(ForeColor, LineWidth);
			Pen2 majorTicPen = new Pen2(ForeColor, MajorTickLineWidth);
			Pen2 minorTicPen = new Pen2(ForeColor, MinorTickLineWidth);
			Brush2 brush = new Brush2(ForeColor);
			g.SmoothingMode = SmoothingMode2.AntiAlias;
			string label = Text ?? "";
			float x0;
			float y0;
			int decade = 0;
			double[][] tics = null;
			double max = 0;
			if (IsValid()){
				decade = (int) Math.Floor(Math.Log(Math.Max(Math.Abs(VisibleMax), Math.Abs(VisibleMin)))/Math.Log(10));
				if (decade > 0 && decade < MaxNumIntegerDigits){
					decade = 0;
				}
				if (decade != 0 && !IsLogarithmic){
					label += " [1" + ToSuperscript(decade) + ']';
				}
				tics = GetTics(GetLength(width, height));
				if (tics == null){
					return;
				}
				max = Math.Max(Math.Abs(ArrayUtils.Max(tics[0])), Math.Abs(ArrayUtils.Min(tics[0])));
			}
			Font2 font = labelFont;
			while (g.MeasureString(label, font).Width > Math.Max(width, height)*0.95f && font.Size > 5f){
				font = new Font2(font.Name, font.Size - 0.5f, font.Style);
			}
			switch (Positioning){
				case AxisPositioning.Top:
					y0 = height - 1;
					g.DrawLine(forePen, xm, ym + y0, xm + width, ym + y0);
					if (IsValid() && tics != null){
						int previousstringEnd = -Sign*int.MaxValue;
						for (int i = 0; i < tics[0].Length; i++){
							x0 = ModelToView(tics[0][i], width, height);
							g.DrawLine(majorTicPen, xm + x0, ym + y0, xm + x0, ym + y0 - MajorTickLength);
							string s = GetTicLabel(tics[0][i], decade, max);
							int w = (int) g.MeasureString(s, numbersFont).Width;
							int pos = (int) (x0 - w/2.0);
							pos = Math.Max(-2, pos);
							pos = Math.Min(width - w + 2, pos);
							if (Sign*pos > Sign*previousstringEnd){
								DrawString(g, s, numbersFont, brush, xm + pos + 1, ym + y0 - MajorTickLength - numbersFont.Height);
								previousstringEnd = pos + Sign*w;
							}
						}
						for (int i = 0; i < tics[1].Length; i++){
							x0 = ModelToView(tics[1][i], width, height);
							g.DrawLine(minorTicPen, xm + x0, ym + y0, xm + x0, ym + y0 - MinorTickLength);
						}
					}
					DrawString(g, label, font, brush, xm + (width)/2 - (int) g.MeasureString(label, font).Width/2,
						ym + y0 - MajorTickLength - labelFont.Height - 12);
					break;
				case AxisPositioning.Left:
					x0 = width - 1;
					g.DrawLine(forePen, xm + x0, ym, xm + x0, ym + height);
					if (IsValid() && tics != null){
						int previousstringEnd = -Sign*int.MaxValue;
						for (int i = 0; i < tics[0].Length; i++){
							y0 = ModelToView(tics[0][i], width, height);
							g.DrawLine(majorTicPen, xm + x0, ym + y0, xm + x0 - MajorTickLength, ym + y0);
							string s = GetTicLabel(tics[0][i], decade, max);
							int w = (int) g.MeasureString(s, numbersFont).Width;
							int pos = (int) (y0 + w/2.0) + 1;
							pos = Math.Max(w - 2, pos);
							pos = Math.Min(height + 2, pos);
							if (Sign*pos > Sign*previousstringEnd){
								g.RotateTransform(-90);
								DrawString(g, s, numbersFont, brush, -pos - ym, xm + x0 - MajorTickLength - numbersFont.Height);
								g.RotateTransform(90);
								previousstringEnd = pos + Sign*w;
							}
						}
						for (int i = 0; i < tics[1].Length; i++){
							y0 = ModelToView(tics[1][i], width, height);
							g.DrawLine(minorTicPen, xm + x0, ym + y0, xm + x0 - MinorTickLength, ym + y0);
						}
					}
					g.RotateTransform(-90);
					float x = -height/2 - (int) g.MeasureString(label, font).Width/2;
					float y = x0 - MajorTickLength - labelFont.Height - numbersFont.Height - 10;
					if (y < 0){
						y = 0;
					}
					DrawString(g, label, font, brush, -ym + x, xm + y - 2);
					g.RotateTransform(90);
					break;
				case AxisPositioning.Bottom:
					y0 = 0;
					g.DrawLine(forePen, xm, ym + y0, xm + width, ym + y0);
					if (IsValid() && tics != null){
						int previousstringEnd = -Sign*int.MaxValue;
						for (int i = 0; i < tics[0].Length; i++){
							x0 = ModelToView(tics[0][i], width, height);
							g.DrawLine(majorTicPen, xm + x0, ym + y0, xm + x0, ym + y0 + MajorTickLength);
							string s = GetTicLabel(tics[0][i], decade, max);
							int w = (int) g.MeasureString(s, numbersFont).Width;
							int pos = (int) (x0 - w/2.0);
							pos = Math.Max(-2, pos);
							pos = Math.Min(width - w + 2, pos);
							if (Sign*pos > Sign*previousstringEnd){
								DrawString(g, s, numbersFont, brush, xm + pos + 1, ym + y0 + 1 + MajorTickLength - 1);
								previousstringEnd = pos + Sign*w;
							}
						}
						for (int i = 0; i < tics[1].Length; i++){
							x0 = ModelToView(tics[1][i], width, height);
							g.DrawLine(minorTicPen, xm + x0, ym + y0, xm + x0, ym + y0 + MinorTickLength);
						}
					}
					DrawString(g, label, font, brush, xm + (width)/2 - (int) g.MeasureString(label, font).Width/2,
						ym + y0 + MajorTickLength + 12);
					break;
				case AxisPositioning.Right:
					x0 = 0;
					g.DrawLine(forePen, xm + x0, ym, xm + x0, ym + height);
					if (IsValid() && tics != null){
						int previousstringEnd = -Sign*int.MaxValue;
						for (int i = 0; i < tics[0].Length; i++){
							y0 = ModelToView(tics[0][i], width, height);
							g.DrawLine(majorTicPen, xm + x0, ym + y0, xm + x0 + MajorTickLength, ym + y0);
							string s = GetTicLabel(tics[0][i], decade, max);
							int w = (int) g.MeasureString(s, numbersFont).Width;
							int pos = (int) (y0 - w/2.0);
							pos = Math.Max(-2, pos);
							pos = Math.Min(height - w + 2, pos);
							if (Sign*pos > Sign*previousstringEnd){
								g.RotateTransform(90);
								DrawString(g, s, numbersFont, brush, ym + pos + 1, -xm - MajorTickLength - numbersFont.Height*0.99f);
								g.RotateTransform(-90);
								previousstringEnd = pos + Sign*w;
							}
						}
						for (int i = 0; i < tics[1].Length; i++){
							y0 = ModelToView(tics[1][i], width, height);
							g.DrawLine(minorTicPen, xm + x0, ym + y0, xm + x0 + MinorTickLength, ym + y0);
						}
					}
					g.RotateTransform(90);
					DrawString(g, label, font, brush, ym + height/2 - (int) g.MeasureString(label, font).Width/2,
						-xm - MajorTickLength - numbersFont.Height - labelFont.Height - 3);
					g.RotateTransform(-90);
					break;
			}
		}

		private static void DrawString(IGraphics g, string s, Font2 font, Brush2 brush, float x, float y){
			try{
				g.DrawString(s, font, brush, x, y);
			} catch (Exception){}
		}

		public void SetZoomFromView(int minInd, int maxInd, int length){
			if (GetMaxIndicator(length) != minInd){
				double min = ViewToModel(Reverse ? maxInd : minInd, length);
				double max = ViewToModel(Reverse ? minInd : maxInd, length);
				if ((max - min)/Math.Max(Math.Abs(max), Math.Abs(min)) > 1e-10){
					if (min < TotalMin){
						double delta = max - min;
						if (delta > TotalMax - TotalMin){
							delta = TotalMax - TotalMin;
						}
						ZoomMin = TotalMin;
						ZoomMax = TotalMin + delta;
					} else if (max > TotalMax){
						double delta = max - min;
						if (delta > TotalMax - TotalMin){
							delta = TotalMax - TotalMin;
						}
						ZoomMin = TotalMax - delta;
						ZoomMax = TotalMax;
					} else{
						ZoomMin = min;
						ZoomMax = max;
					}
					FireZoomChanged();
				}
				Invalidate();
			}
		}

		public void SetZoom(double min, double max){
			if ((max - min)/Math.Max(Math.Abs(max), Math.Abs(min)) > 1e-10){
				ZoomMin = !double.IsNaN(TotalMin) ? Math.Max(min, TotalMin) : min;
				ZoomMax = !double.IsNaN(TotalMax) ? Math.Min(max, TotalMax) : max;
				FireZoomChanged();
				Invalidate();
			}
		}

		public void SetZoomNoFire(double min, double max){
			if ((max - min)/Math.Max(Math.Abs(max), Math.Abs(min)) > 1e-10){
				ZoomMin = Math.Max(min, TotalMin);
				ZoomMax = Math.Min(max, TotalMax);
				Invalidate();
			}
		}

		internal void Center(BasicMouseEventArgs e){
			if (IsFullZoom()){
				return;
			}
			double delta = (ZoomMax - ZoomMin)*0.5;
			double center = ViewToModel(IsHorizontal() ? e.X : e.Y, e.Width, e.Height);
			ZoomMin = center - delta;
			ZoomMax = center + delta;
			MoveZoomIntoRange();
			Invalidate();
		}

		internal void MoveZoomIntoRange(){
			if (ZoomMax - ZoomMin > TotalMax - TotalMin){
				ZoomMax = TotalMax;
				ZoomMin = TotalMin;
				return;
			}
			if (ZoomMin < TotalMin){
				ZoomMax += TotalMin - ZoomMin;
				ZoomMin = TotalMin;
			}
			if (ZoomMax > TotalMax){
				ZoomMin += TotalMax - ZoomMax;
				ZoomMax = TotalMax;
			}
		}

		internal bool IsFullZoom(){
			return ZoomMin == TotalMin && ZoomMax == TotalMax;
		}

		private string GetTicLabel(double ticval, int decade, double max){
			if (IsLogarithmic){
				int exp = (int) Math.Round(ticval/Math.Log(10.0));
				if (exp == -1){
					return "0.1";
				}
				if (exp == 0){
					return "1";
				}
				if (exp == 1){
					return "10";
				}
				return "1" + ToSuperscript(exp);
			}
			return NumUtils.RoundSignificantDigits2(ticval/Math.Pow(10.0, decade), 7, max/Math.Pow(10.0, decade));
		}

		private static string ToSuperscript(int round){
			return "e" + round;
		}

		public double[][] GetTics(int width, int height){
			return GetTics(GetLength(width, height));
		}

		public double[][] GetTics(int length){
			float minMajorTics = (float) length/spacePerNumber;
			if (IsLogarithmic){
				minMajorTics *= 5;
				double step = Math.Ceiling((VisibleMax - VisibleMin)/Math.Log(10.0)/minMajorTics)*Math.Log(10.0);
				double val = Math.Ceiling(VisibleMin/step)*step;
				double minorval = val - step;
				double[] r = new double[10*(int) minMajorTics];
				int count = 0;
				while (val <= VisibleMax){
					r[count++] = val;
					val += step;
				}
				List<double> s = new List<double>();
				Array.Resize(ref r, count);
				double[] majors = r;
				double[] minors;
				int b = (int) Math.Round(step/Math.Log(10.0));
				if (b > 1){
					for (int i = 0; i < majors.Length + 1; i++){
						for (int j = 0; j < b - 1; j++){
							minorval += step/b;
							if (minorval >= VisibleMin && minorval <= VisibleMax){
								s.Add(minorval);
							}
						}
						minorval += step/b;
					}
					minors = s.ToArray();
				} else{
					for (int i = 0; i < majors.Length + 1; i++){
						for (int j = 2; j < 10; j++){
							double x = minorval + Math.Log(j);
							if (x >= VisibleMin && x <= VisibleMax){
								s.Add(x);
							}
						}
						minorval += step;
					}
					minors = s.ToArray();
				}
				return new[]{majors, minors};
			} else{
				//int exp = (int)Math.Floor(Math.Log(Math.Max(Math.Abs(VisibleMax), Math.Abs(VisibleMin))) / Math.Log(10));
				//minMajorTics = Length / (e * 25);
				double a = (VisibleMax - VisibleMin)/minMajorTics;
				int exp = (int) Math.Floor(Math.Log(a)/Math.Log(10));
				double step;
				if (exp > 0){
					step = Math.Round(Math.Pow(10, exp));
				} else if (exp == 0){
					step = 1;
				} else{
					step = 1.0/Math.Round(Math.Pow(10, -exp));
				}
				double c = (VisibleMax - VisibleMin)/step;
				if (c >= 10*minMajorTics){
					step *= 10;
				} else if (c >= 5*minMajorTics){
					step *= 5;
				} else if (c >= 2*minMajorTics){
					step *= 2;
				}
				long s = 0;
				if (!double.IsInfinity(step)){
					s = (long) (step/10);
					while (s > 10){
						s = s/10;
					}
					if (s <= 2){
						s *= 2;
					}
				}
				double val = Math.Ceiling(VisibleMin/step)*step;
				double minorval = val - step;
				if (minorval < VisibleMin){
					if (VisibleMin == 0){
						minorval = VisibleMin;
					} else{
						while (minorval < VisibleMin){
							minorval += step/s;
						}
					}
				}
				List<double> majors = new List<double>();
				while (val <= VisibleMax){
					majors.Add(val);
					val += step;
				}
				List<double> minors = new List<double>();
				step = step/s;
				while (minorval <= VisibleMax){
					minors.Add(minorval);
					minorval += step;
				}
				return new[]{majors.ToArray(), minors.ToArray()};
			}
		}

		internal bool IsValid(){
			return !double.IsNaN(TotalMin) && !double.IsNaN(TotalMax) && VisibleMax != 0;
		}

		internal void FireZoomChanged(){
			OnZoomChange?.Invoke(this, ZoomMin, ZoomMax);
		}

		public void ZoomIn(int length){
			SetZoomFromView(length/3, length*2/3, length);
		}

		public void ZoomOut(int length){
			SetZoomFromView(-length/3, length*4/3, length);
		}

		public void MoveDown(int length){
			int min = -length/3;
			int max = length*2/3;
			SetZoomFromView(min, max, length);
		}

		public void MoveUp(int length){
			int min = length/3;
			int max = length*4/3;
			SetZoomFromView(min, max, length);
		}

		public override void OnMouseDragged(BasicMouseEventArgs e){
			if (!IsValid() || ZoomType == AxisZoomType.None){
				return;
			}
			indicator2 = IsHorizontal() ? e.X : e.Y;
			indicator2 = Math.Max(0, indicator2);
			indicator2 = Math.Min(GetLength(e.Width, e.Height), indicator2);
			switch (MouseMode){
				case AxisMouseMode.Zoom:
					Invalidate();
					break;
				case AxisMouseMode.Move:
					switch (ZoomType){
						case AxisZoomType.Indicate:
							Center(e);
							break;
						case AxisZoomType.Zoom:
							if (IsFullZoom()){
								return;
							}
							double current = ViewToModel(IsHorizontal() ? e.X : e.Y, e.Width, e.Height);
							double delta = current - dragStart;
							ZoomMin -= delta;
							ZoomMax -= delta;
							MoveZoomIntoRange();
							Invalidate();
							break;
					}
					break;
			}
		}

		public override void OnMouseIsDown(BasicMouseEventArgs e){
			if (!Visible){
				return;
			}
			if (!IsValid()){
				return;
			}
			if (e.IsMainButton){
				if (ZoomType == AxisZoomType.None){
					return;
				}
				switch (MouseMode){
					case AxisMouseMode.Zoom:
						indicator1 = IsHorizontal() ? e.X : e.Y;
						indicator2 = indicator1;
						break;
					case AxisMouseMode.Move:
						switch (ZoomType){
							case AxisZoomType.Indicate:
								Center(e);
								break;
							case AxisZoomType.Zoom:
								if (IsFullZoom()){
									return;
								}
								dragStart = ViewToModel(IsHorizontal() ? e.X : e.Y, e.Width, e.Height);
								break;
						}
						break;
				}
			} else{
				ZeroPoint = ViewToModel(IsHorizontal() ? e.X : e.Y, e.Width, e.Height);
			}
		}

		public override void OnMouseIsUp(BasicMouseEventArgs e){
			if (Configurable && !e.IsMainButton){
				NumericAxisPropertiesForm f = new NumericAxisPropertiesForm(Text, IsLogarithmic ? Math.Exp(ZoomMin) : ZoomMin,
					IsLogarithmic ? Math.Exp(ZoomMax) : ZoomMax);
				f.ShowDialog();
				if (f.Ok){
					Text = f.Title;
					double newMin = IsLogarithmic ? Math.Log(f.MinValue) : f.MinValue;
					double newMax = IsLogarithmic ? Math.Log(f.MaxValue) : f.MaxValue;
					if (!double.IsNaN(newMin) && !double.IsNaN(newMax) && newMin < newMax){
						ZoomMin = newMin;
						ZoomMax = newMax;
						TotalMin = newMin;
						TotalMax = newMax;
						FireZoomChanged();
						Invalidate();
					}
				}
				f.Dispose();
				return;
			}
			if (!IsValid()){
				return;
			}
			if (e.IsMainButton){
				if (ZoomType == AxisZoomType.None){
					return;
				}
				switch (MouseMode){
					case AxisMouseMode.Zoom:
						SetZoomFromView((int) GetMinIndicator(GetLength(e.Width, e.Height)),
							(int) GetMaxIndicator(GetLength(e.Width, e.Height)), GetLength(e.Width, e.Height));
						break;
					case AxisMouseMode.Move:
						switch (ZoomType){
							case AxisZoomType.Indicate:
								if (IsFullZoom()){
									return;
								}
								FireZoomChanged();
								break;
							case AxisZoomType.Zoom:
								if (IsFullZoom()){
									return;
								}
								if (indicator1 == indicator2){
									Center(e);
									FireZoomChanged();
									break;
								}
								Invalidate();
								FireZoomChanged();
								break;
						}
						break;
				}
				indicator1 = -1;
				indicator2 = -1;
			}
		}

		public void FullZoom(){
			ZoomMin = TotalMin;
			ZoomMax = TotalMax;
			FireZoomChanged();
			Invalidate();
		}

		public override void OnMouseDoubleClick(BasicMouseEventArgs e){
			if (!Visible){
				return;
			}
			if (!IsValid()){
				return;
			}
			if (e.IsMainButton){
				if (ZoomType == AxisZoomType.None){
					return;
				}
				switch (MouseMode){
					case AxisMouseMode.Zoom:
						FullZoom();
						break;
				}
			} else{
				ZeroPoint = double.NaN;
			}
		}
	}
}