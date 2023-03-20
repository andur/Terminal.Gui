﻿using NStack;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Terminal.Gui.Configuration;

namespace Terminal.Gui {
	/// <summary>
	/// Describes the thickness of a frame around a rectangle. Four <see cref="int"/> values describe
	///  the <see cref="Left"/>, <see cref="Top"/>, <see cref="Right"/>, and <see cref="Bottom"/> sides
	///  of the rectangle, respectively. Provides a helper API (<see cref="Draw(Rect, string)"/> for
	///  drawing a frame with the specified thickness.
	/// </summary>
	public class Thickness : IEquatable<Thickness> {
		private int validate (int width)
		{
			if (width < 0) {
				throw new ArgumentException ("Thickness widths cannot be negative.");
			}
			return width;
		}

		/// <summary>
		/// Gets or sets the width of the left side of the rectangle.
		/// </summary>
		[JsonInclude]
		public int Left;

		/// <summary>
		/// Gets or sets the width of the upper side of the rectangle.
		/// </summary>
		[JsonInclude]
		public int Top;

		/// <summary>
		/// Gets or sets the width of the right side of the rectangle.
		/// </summary>
		[JsonInclude]
		public int Right;

		/// <summary>
		/// Gets or sets the width of the lower side of the rectangle.
		/// </summary>
		[JsonInclude]
		public int Bottom;

		/// <summary>
		/// Initializes a new instance of the <see cref="Thickness"/> class with all widths
		/// set to 0.
		/// </summary>
		public Thickness () { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Thickness"/> class with a uniform width to each side.
		/// </summary>
		/// <param name="width"></param>
		public Thickness (int width) : this (width, width, width, width) { }

		/// <summary>
		/// Initializes a new instance of the <see cref="Thickness"/> class that has specific
		///  widths applied to each side of the rectangle.
		/// </summary>
		/// <param name="left"></param>
		/// <param name="top"></param>
		/// <param name="right"></param>
		/// <param name="bottom"></param>
		public Thickness (int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		/// <summary>
		/// Gets the total width of the left and right sides of the rectangle. Sets the height of the left and right sides of the rectangle to half the specified value.
		/// </summary>
		public int Vertical {
			get {
				return Top + Bottom;
			}
			set {
				Top = Bottom = value / 2;
			}
		}

		/// <summary>
		/// Gets the total width of the top and bottom sides of the rectangle. Sets the width of the top and bottom sides of the rectangle to half the specified value.
		/// </summary>
		public int Horizontal {
			get {
				return Left + Right;
			}
			set {
				Left = Right = value / 2;
			}
		}

		//public virtual void OnChanged()
		//{
		//	Changed?.Invoke (this, new ThicknessEventArgs () { Thickness = this });
		//}
		//public event EventHandler<ThicknessEventArgs> Changed;

		/// <summary>
		/// Returns a rectangle describing the location and size of the inner area of <paramref name="rect"/>
		/// with the thickness widths subracted. The height and width of the retunred rect may be zero.
		/// </summary>
		/// <param name="rect">The source rectangle</param>
		/// <returns></returns>
		public Rect GetInnerRect (Rect rect)
		{
			var width = rect.Size.Width - (Left + Right);
			var height = rect.Size.Height - (Top + Bottom);
			var size = new Size (Math.Max (0, width), Math.Max (0, height));
			return new Rect (new Point (rect.X + Left, rect.Y + Top), size);
		}

		/// <summary>
		/// Draws the <see cref="Thickness"/> rectangle with an optional diagnostics label.
		/// </summary>
		/// <remarks>
		/// If <see cref="ConsoleDriver.DiagnosticFlags"/> is set to <see cref="ConsoleDriver.DiagnosticFlags.FramePadding"/> then
		/// 'T', 'L', 'R', and 'B' glyphs will be used instead of space. If <see cref="ConsoleDriver.DiagnosticFlags"/>
		/// is set to <see cref="ConsoleDriver.DiagnosticFlags.FrameRuler"/> then a ruler will be drawn on the outer edge of the
		/// Thickness.
		/// </remarks>
		/// <param name="rect">The location and size of the rectangle that bounds the thickness rectangle, in 
		/// screen coordinates.</param>
		/// <param name="label">The diagnostics label to draw on the bottom of the <see cref="Bottom"/>.</param>
		/// <returns>The inner rectangle remaining to be drawn.</returns>
		public Rect Draw (Rect rect, string label = null)
		{
			if (rect.Size.Width < 1 || rect.Size.Height < 1) {
				return Rect.Empty;
			}

			System.Rune clearChar = ' ';
			System.Rune leftChar = clearChar;
			System.Rune rightChar = clearChar;
			System.Rune topChar = clearChar;
			System.Rune bottomChar = clearChar;

			if ((ConsoleDriver.Diagnostics & ConsoleDriver.DiagnosticFlags.FramePadding) == ConsoleDriver.DiagnosticFlags.FramePadding) {
				leftChar = 'L';
				rightChar = 'R';
				topChar = 'T';
				bottomChar = 'B';
				if (!string.IsNullOrEmpty (label)) {
					leftChar = rightChar = bottomChar = topChar = label [0];
				}
			}

			ustring hrule = ustring.Empty;
			ustring vrule = ustring.Empty;
			if ((ConsoleDriver.Diagnostics & ConsoleDriver.DiagnosticFlags.FrameRuler) == ConsoleDriver.DiagnosticFlags.FrameRuler) {

				string h = "0123456789";
				hrule = h.Repeat ((int)Math.Ceiling ((double)(rect.Width) / (double)h.Length)) [0..(rect.Width)];
				string v = "0123456789";
				vrule = v.Repeat ((int)Math.Ceiling ((double)(rect.Height * 2) / (double)v.Length)) [0..(rect.Height * 2)];
			};

			// Draw the Top side
			if (Top > 0) {
				Application.Driver.FillRect (new Rect (rect.X, rect.Y, rect.Width, Math.Min (rect.Height, Top)), topChar);
			}

			// Draw the Left side
			if (Left > 0) {
				Application.Driver.FillRect (new Rect (rect.X, rect.Y, Math.Min (rect.Width, Left), rect.Height), leftChar);
			}

			// Draw the Right side			
			if (Right > 0) {
				Application.Driver.FillRect (new Rect (Math.Max (0, rect.X + rect.Width - Right), rect.Y, Math.Min (rect.Width, Right), rect.Height), rightChar);
			}

			// Draw the Bottom side
			if (Bottom > 0) {
				Application.Driver.FillRect (new Rect (rect.X, rect.Y + Math.Max (0, rect.Height - Bottom), rect.Width, Bottom), bottomChar);
			}

			// TODO: This should be moved to LineCanvas as a new BorderStyle.Ruler
			if ((ConsoleDriver.Diagnostics & ConsoleDriver.DiagnosticFlags.FrameRuler) == ConsoleDriver.DiagnosticFlags.FrameRuler) {
				// Top
				Application.Driver.Move (rect.X, rect.Y);
				Application.Driver.AddStr (hrule);
				//Left
				for (var r = rect.Y; r < rect.Y + rect.Height; r++) {
					Application.Driver.Move (rect.X, r);
					Application.Driver.AddRune (vrule [r - rect.Y]);
				}
				// Bottom
				Application.Driver.Move (rect.X, rect.Y + rect.Height - Bottom + 1);
				Application.Driver.AddStr (hrule);
				// Right
				for (var r = rect.Y + 1; r < rect.Y + rect.Height; r++) {
					Application.Driver.Move (rect.X + rect.Width - Right + 1, r);
					Application.Driver.AddRune (vrule [r - rect.Y]);
				}
			}

			if ((ConsoleDriver.Diagnostics & ConsoleDriver.DiagnosticFlags.FramePadding) == ConsoleDriver.DiagnosticFlags.FramePadding) {
				// Draw the diagnostics label on the bottom
				var tf = new TextFormatter () {
					Text = label == null ? string.Empty : $"{label} {this}",
					Alignment = TextAlignment.Centered,
					VerticalAlignment = VerticalTextAlignment.Bottom
				};
				tf.Draw (rect, Application.Driver.CurrentAttribute, Application.Driver.CurrentAttribute, rect, false);
			}

			return GetInnerRect (rect);

		}

		// TODO: add operator overloads
		/// <summary>
		/// Gets an empty thickness.
		/// </summary>
		public static Thickness Empty => new Thickness (0);

		/// <inheritdoc/>
		public override bool Equals (object obj)
		{
			//Check for null and compare run-time types.
			if ((obj == null) || !this.GetType ().Equals (obj.GetType ())) {
				return false;
			} else {
				return Equals ((Thickness)obj);
			}
		}

		/// <summary>Returns the thickness widths of the Thickness formatted as a string.</summary>
		/// <returns>The thickness widths as a string.</returns>
		public override string ToString ()
		{
			return $"(Left={Left},Top={Top},Right={Right},Bottom={Bottom})";
		}

		// IEquitable
		/// <inheritdoc/>
		public bool Equals (Thickness other)
		{
			return other is not null &&
			       Left == other.Left &&
			       Right == other.Right &&
			       Top == other.Top &&
			       Bottom == other.Bottom;
		}

		/// <inheritdoc/>
		public override int GetHashCode ()
		{
			int hashCode = 1380952125;
			hashCode = hashCode * -1521134295 + Left.GetHashCode ();
			hashCode = hashCode * -1521134295 + Right.GetHashCode ();
			hashCode = hashCode * -1521134295 + Top.GetHashCode ();
			hashCode = hashCode * -1521134295 + Bottom.GetHashCode ();
			return hashCode;
		}

		/// <inheritdoc/>
		public static bool operator == (Thickness left, Thickness right)
		{
			return EqualityComparer<Thickness>.Default.Equals (left, right);
		}

		/// <inheritdoc/>
		public static bool operator != (Thickness left, Thickness right)
		{
			return !(left == right);
		}
	}

	internal static class StringExtensions {
		public static string Repeat (this string instr, int n)
		{
			if (n <= 0) {
				return null;
			}

			if (string.IsNullOrEmpty (instr) || n == 1) {
				return instr;
			}

			return new StringBuilder (instr.Length * n)
				.Insert (0, instr, n)
				.ToString ();
		}
	}
}