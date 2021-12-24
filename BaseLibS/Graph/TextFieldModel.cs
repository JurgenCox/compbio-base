﻿using System;
using System.Collections.Generic;
using BaseLibS.Graph.Scroll;
using BaseLibS.Util;
namespace BaseLibS.Graph {
	public class TextFieldModel : ISimpleScrollableControlModel {
		//TODO
		//Invoke text changed
		//english and other keyboards
		//some special chars
		private const string carriageReturn = "\r\n";
		private readonly List<string> lines = new List<string>();
		private readonly HashSet<char> forbiddenChars = new HashSet<char>();
		private string copyBuffer;
		private int lastEditedLine = -1;
		private string undoBuffer;
		private readonly Brush2 textBrush = Brushes2.Black;
		private readonly Brush2 selectionBrush = Brushes2.LightBlue;
		private readonly Pen2 cursorPen = Pens2.BlueViolet;
		private readonly Font2 font = new Font2("Courier New", 10F);
		private int cursorPosLine;
		private int cursorPosChar;
		private int selectedLine = -1;
		private bool[] lineIsSelected;
		private int selectionStartLine = -1;
		private int selectionStartChar = -1;
		private int selectionEndLine = -1;
		private int selectionEndChar = -1;
		private ISimpleScrollableControl control;
		private int lineHeight = 15;
		public int OffsetX { get; set; }
		public int OffsetY { get; set; }
		public bool HasBoundingBox { get; set; } = false;
		public event EventHandler TextChanged;

		public TextFieldModel() : this("") {
		}
		public TextFieldModel(string text){
			Text = text;
		}
		public TextFieldModel(string[] lines) {
			if (MultiLine) {
				Lines = lines;
			} else {
				Lines = lines.Length == 0 ? new[] { "" } : new[] { lines[0] };
			}
		}
		public string[] Lines {
			set {
				lines.Clear();
				foreach (string t in value) {
					lines.Add(t == null ? "" : t.Trim());
				}
			}
		}
		public string Text{
			set{
				if (value == null) {
					value = "";
				}
				string[] x = value.Split(new[] { carriageReturn }, StringSplitOptions.RemoveEmptyEntries);
				if (!MultiLine && x.Length > 1) {
					x = new[] { x[0] };
				}
				Lines = x;
			}
			get => StringUtils.Concat(carriageReturn, lines);
		}
		public IEnumerable<char> ForbiddenChars {
			set {
				forbiddenChars.Clear();
				foreach (char c in value) {
					forbiddenChars.Add(c);
				}
			}
		}
		public Color2 TextColor {
			get => textBrush.Color;
			set => textBrush.Color = value;
		}
		public TextFieldSelectionMode SelectionMode { get; set; } = TextFieldSelectionMode.Chars;
		public int LineHeight {
			get => lineHeight;
			set {
				lineHeight = value;
				font.Size = lineHeight * 0.6667f;
			}
		}
		public bool MultiLine { get; set; } = false;
		public bool Editable { get; set; } = true;
		public void ProcessCmdKey(Keys2 keyData) {
			switch (keyData) {
				case Keys2.Shift | Keys2.Left:
					if (selectionStartLine == -1) {
						selectionStartLine = cursorPosLine;
						selectionStartChar = cursorPosChar;
					}
					selectionEndLine = cursorPosLine;
					selectionEndChar = Math.Max(cursorPosChar - 1, 0);
					MoveCursorLeft();
					break;
				case Keys2.Left:
					if (selectionStartLine != -1) {
						selectionStartLine = -1;
						selectionStartChar = -1;
						selectionEndLine = -1;
						selectionEndChar = -1;
					}
					MoveCursorLeft();
					break;
				case Keys2.Shift | Keys2.Right:
					if (selectionStartLine == -1) {
						selectionStartLine = cursorPosLine;
						selectionStartChar = cursorPosChar;
					}
					selectionEndLine = cursorPosLine;
					string currentString = lines[cursorPosLine];
					selectionEndChar = Math.Min(cursorPosChar + 1, currentString.Length);
					MoveCursorRight();
					break;
				case Keys2.Right:
					if (selectionStartLine != -1) {
						selectionStartLine = -1;
						selectionStartChar = -1;
						selectionEndLine = -1;
						selectionEndChar = -1;
					}
					MoveCursorRight();
					break;
				case Keys2.Shift | Keys2.Up:
				case Keys2.Up:
					MoveCursorUp();
					break;
				case Keys2.Shift | Keys2.Down:
				case Keys2.Down:
					MoveCursorDown();
					break;
				case Keys2.Delete:
					DeleteSelectedChars();
					break;
				case Keys2.Back:
					if (selectionStartLine >= 0) {
						DeleteSelectedChars();
					} else {
						DeletePreviousChar();
					}
					break;
				case Keys2.Control | Keys2.A:
					SelectAll();
					break;
				case Keys2.Control | Keys2.C:
					copyBuffer = GetSelectedString();
					break;
				case Keys2.Control | Keys2.X:
					copyBuffer = GetSelectedString();
					DeleteSelectedChars();
					break;
				case Keys2.Control | Keys2.V:
					if (!string.IsNullOrEmpty(copyBuffer)) {
						AddAtCursor(copyBuffer);
					}
					break;
				case Keys2.Control | Keys2.Z:
					if (lastEditedLine != -1 && !string.IsNullOrEmpty(undoBuffer)) {
						(lines[lastEditedLine], undoBuffer) = (undoBuffer, lines[lastEditedLine]);
						selectionStartLine = -1;
						selectionEndLine = -1;
					}
					break;
				default:
					char c = KeyData.GetChar(keyData);
					if (c != 0 && !forbiddenChars.Contains(c)) {
						DeleteSelectedChars();
						AddAtCursor("" + c);
					}
					break;
			}
			control.Invalidate(true);
		}
		private string GetSelectedString() {
			if (selectionStartLine == -1 || selectionStartLine != selectionEndLine) {
				return "";
			}
			string currentLine = lines[selectionStartLine];
			return currentLine.Substring(Math.Min(selectionStartChar, selectionEndChar),
				Math.Abs(selectionStartChar - selectionEndChar));
		}
		private void SelectAll() {
			if (cursorPosLine < 0) {
				return;
			}
			selectionStartLine = cursorPosLine;
			selectionEndLine = cursorPosLine;
			selectionStartChar = 0;
			selectionEndChar = lines[selectionStartLine].Length;
		}
		private void AddAtCursor(string c) {
			string currentLine = lines[cursorPosLine];
			if (lastEditedLine == -1) {
				undoBuffer = currentLine;
				lastEditedLine = cursorPosLine;
			}
			string before = currentLine.Substring(0, cursorPosChar);
			string after = currentLine.Substring(cursorPosChar);
			lines[cursorPosLine] = before + c + after;
			cursorPosChar += c.Length;
		}
		private void DeleteSelectedChars() {
			if (selectionStartLine == -1 || selectionStartLine != selectionEndLine) {
				return;
			}
			string currentLine = lines[selectionStartLine];
			if (lastEditedLine == -1) {
				undoBuffer = currentLine;
				lastEditedLine = selectionEndLine;
			}
			string before = currentLine.Substring(0, Math.Min(selectionStartChar, selectionEndChar));
			string after = currentLine.Substring(Math.Max(selectionStartChar, selectionEndChar));
			lines[selectionStartLine] = before + after;
			if (selectionEndChar > selectionStartChar) {
				cursorPosChar -= Math.Abs(selectionStartChar - selectionEndChar);
				cursorPosChar = Math.Max(0, cursorPosChar);
			}
			selectionStartLine = -1;
			selectionEndLine = -1;
		}
		private void DeletePreviousChar() {
			if (cursorPosLine == -1 || cursorPosChar == 0) {
				return;
			}
			string currentLine = lines[cursorPosLine];
			if (lastEditedLine == -1) {
				undoBuffer = currentLine;
				lastEditedLine = cursorPosLine;
			}
			string before = currentLine.Substring(0, cursorPosChar - 1);
			string after = currentLine.Substring(cursorPosChar);
			lines[cursorPosLine] = before + after;
			cursorPosChar -= 1;
		}
		private void MoveCursorLeft() {
			if (cursorPosChar == 0) {
				CursorPreviousLineEnd();
				return;
			}
			cursorPosChar--;
		}
		private void MoveCursorRight() {
			string currentString = lines[cursorPosLine];
			if (cursorPosChar >= currentString.Length) {
				CursorNextLineBeginning();
				return;
			}
			cursorPosChar++;
		}
		private void CursorNextLineBeginning() {
			if (cursorPosLine >= lines.Count - 1) {
				return;
			}
			cursorPosLine++;
			cursorPosChar = 0;
		}
		private void CursorPreviousLineEnd() {
			if (cursorPosLine == 0) {
				return;
			}
			cursorPosLine--;
			cursorPosChar = lines[cursorPosLine].Length;
		}
		private void MoveCursorUp() {
			if (cursorPosLine == 0) {
				return;
			}
			cursorPosLine--;
			cursorPosChar = Math.Min(cursorPosChar, lines[cursorPosLine].Length);
		}
		private void MoveCursorDown() {
			if (cursorPosLine == lines.Count - 1) {
				return;
			}
			cursorPosLine++;
			cursorPosChar = Math.Min(cursorPosChar, lines[cursorPosLine].Length);
		}
		public void InvalidateBackgroundImages() {
		}
		public void OnSizeChanged() {
		}
		public void Register(ISimpleScrollableControl control1) {
			control = control1;
			control.HasOverview = false;
			control.HasZoomButtons = false;
			control.HorizontalScrollbarMode = ScrollBarMode.Never;
			control.VerticalScrollbarMode = ScrollBarMode.Never;
			control.TotalHeight = () => lineHeight * lines.Count + OffsetY;
			control.TotalWidth = () => {
				int w = 100;
				foreach (string line in lines) {
					int w1 = (int)(lineHeight * 0.57 * line.Length);
					w = Math.Max(w, w1);
				}
				return w + OffsetX;
			};
			control.OnPaintMainView = (g, x, y, width, height, isOverview) => {
				PaintSelection(g, x, y);
				PaintText(g, x, y);
				if (Editable) {
					PaintCursor(g, x, y);
				}
				if (HasBoundingBox && !isOverview) {
					PaintBoundingBox(g, width, height);
				}
			};
			control.OnMouseIsDownMainView = args => {
				(int row, int col) = GetPlotPos(args.X, args.Y);
				if (row >= 0) {
					cursorPosLine = row;
					cursorPosChar = col;
					selectionStartLine = row;
					selectionStartChar = col;
				}
			};
			control.OnMouseDraggedMainView = args => {
				(int row, int col) = GetPlotPos(args.X, args.Y);
				if (row >= 0) {
					selectionEndLine = row;
					selectionEndChar = col;
				}
			};
		}
		private void PaintBoundingBox(IGraphics g, int width, int height) {
			g.DrawLine(Pens2.Black, 0, 0, width, 0);
			g.DrawLine(Pens2.Black, 0, height - 1, width, height - 1);
			g.DrawLine(Pens2.Black, 0, 0, 0, height);
			g.DrawLine(Pens2.Black, width - 1, 0, width - 1, height);
		}
		private (int, int) GetPlotPos(int x, int y) {
			int row = ((y - OffsetY) / LineHeight);
			row = Math.Min(row, lines.Count - 1);
			row = Math.Max(row, 0);
			string line = lines[row];
			int col = (int)((x - OffsetX) * 1.8 / LineHeight);
			col = Math.Min(col, line.Length);
			col = Math.Max(col, 0);
			return (row, col);
		}
		public void PaintSelection(IGraphics g, int x, int y) {
			switch (SelectionMode) {
				case TextFieldSelectionMode.Chars:
					if (selectionStartLine == -1) {
						return;
					}
					if (selectionStartLine == selectionEndLine) {
						string currentLine = lines[selectionStartLine];
						string s1 = selectionStartChar == currentLine.Length
							? currentLine
							: currentLine.Substring(0, selectionStartChar);
						s1 = s1.Replace(' ', '_');
						int pos1 = (int)Math.Round(g.MeasureString(s1, font).Width);
						pos1 -= 2;
						string s2 = selectionEndChar == currentLine.Length
							? currentLine
							: currentLine.Substring(0, selectionEndChar);
						s2 = s2.Replace(' ', '_');
						int pos2 = (int)Math.Round(g.MeasureString(s2, font).Width);
						pos2 -= 2;
						g.FillRectangle(selectionBrush, OffsetX + Math.Min(pos1, pos2) - x, OffsetY + lineHeight * selectionStartLine - y,
							Math.Abs(pos2 - pos1), lineHeight);
					}
					break;
				case TextFieldSelectionMode.SingleLines:
					if (selectedLine == -1) {
						return;
					}
					g.FillRectangle(selectionBrush, OffsetX, OffsetY + lineHeight * selectedLine - y, 199, lineHeight);
					break;
				case TextFieldSelectionMode.MultipleLines:
					if (lineIsSelected == null) {
						return;
					}
					for (int i = 0; i < lineIsSelected.Length; i++) {
						if (lineIsSelected[i]) {
							g.FillRectangle(selectionBrush, OffsetX, OffsetY + lineHeight * i - y, 199, lineHeight);
						}
					}
					break;
			}
		}
		public void PaintText(IGraphics g, int x, int y) {
			for (int i = 0; i < lines.Count; i++) {
				if (!MultiLine && i > 0) {
					break;
				}
				g.DrawString(lines[i], font, textBrush, OffsetX - x, OffsetY + i * lineHeight - y);
			}
		}
		public void PaintCursor(IGraphics g, int x, int y) {
			int pos = 1;
			if (cursorPosChar > 0) {
				string currentLine = lines[cursorPosLine];
				string s = cursorPosChar == currentLine.Length ? currentLine : currentLine.Substring(0, cursorPosChar);
				s = s.Replace(' ', '_');
				pos = (int)Math.Round(g.MeasureString(s, font).Width);
				pos -= 2;
			}
			g.DrawLine(cursorPen, OffsetX + pos - x, OffsetY + cursorPosLine * lineHeight - y, OffsetX + pos - x,
				OffsetY + cursorPosLine * lineHeight + lineHeight - y);
		}
	}
}