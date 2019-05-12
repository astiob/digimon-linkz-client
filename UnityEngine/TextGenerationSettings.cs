using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>A struct that stores the settings for TextGeneration.</para>
	/// </summary>
	public struct TextGenerationSettings
	{
		/// <summary>
		///   <para>Font to use for generation.</para>
		/// </summary>
		public Font font;

		/// <summary>
		///   <para>The base color for the text generation.</para>
		/// </summary>
		public Color color;

		/// <summary>
		///   <para>Font size.</para>
		/// </summary>
		public int fontSize;

		/// <summary>
		///   <para>The line spacing multiplier.</para>
		/// </summary>
		public float lineSpacing;

		/// <summary>
		///   <para>Allow rich text markup in generation.</para>
		/// </summary>
		public bool richText;

		/// <summary>
		///   <para>A scale factor for the text. This is useful if the Text is on a Canvas and the canvas is scaled.</para>
		/// </summary>
		public float scaleFactor;

		/// <summary>
		///   <para>Font style.</para>
		/// </summary>
		public FontStyle fontStyle;

		/// <summary>
		///   <para>How is the generated text anchored.</para>
		/// </summary>
		public TextAnchor textAnchor;

		/// <summary>
		///   <para>Should the text be resized to fit the configured bounds?</para>
		/// </summary>
		public bool resizeTextForBestFit;

		/// <summary>
		///   <para>Minimum size for resized text.</para>
		/// </summary>
		public int resizeTextMinSize;

		/// <summary>
		///   <para>Maximum size for resized text.</para>
		/// </summary>
		public int resizeTextMaxSize;

		/// <summary>
		///   <para>Should the text generator update the bounds from the generated text.</para>
		/// </summary>
		public bool updateBounds;

		/// <summary>
		///   <para>What happens to text when it reaches the bottom generation bounds.</para>
		/// </summary>
		public VerticalWrapMode verticalOverflow;

		/// <summary>
		///   <para>What happens to text when it reaches the horizontal generation bounds.</para>
		/// </summary>
		public HorizontalWrapMode horizontalOverflow;

		/// <summary>
		///   <para>Extents that the generator will attempt to fit the text in.</para>
		/// </summary>
		public Vector2 generationExtents;

		/// <summary>
		///   <para>Generated vertices are offset by the pivot.</para>
		/// </summary>
		public Vector2 pivot;

		/// <summary>
		///   <para>Continue to generate characters even if the text runs out of bounds.</para>
		/// </summary>
		public bool generateOutOfBounds;

		private bool CompareColors(Color left, Color right)
		{
			return Mathf.Approximately(left.r, right.r) && Mathf.Approximately(left.g, right.g) && Mathf.Approximately(left.b, right.b) && Mathf.Approximately(left.a, right.a);
		}

		private bool CompareVector2(Vector2 left, Vector2 right)
		{
			return Mathf.Approximately(left.x, right.x) && Mathf.Approximately(left.y, right.y);
		}

		public bool Equals(TextGenerationSettings other)
		{
			return this.CompareColors(this.color, other.color) && this.fontSize == other.fontSize && Mathf.Approximately(this.scaleFactor, other.scaleFactor) && this.resizeTextMinSize == other.resizeTextMinSize && this.resizeTextMaxSize == other.resizeTextMaxSize && Mathf.Approximately(this.lineSpacing, other.lineSpacing) && this.fontStyle == other.fontStyle && this.richText == other.richText && this.textAnchor == other.textAnchor && this.resizeTextForBestFit == other.resizeTextForBestFit && this.resizeTextMinSize == other.resizeTextMinSize && this.resizeTextMaxSize == other.resizeTextMaxSize && this.resizeTextForBestFit == other.resizeTextForBestFit && this.updateBounds == other.updateBounds && this.horizontalOverflow == other.horizontalOverflow && this.verticalOverflow == other.verticalOverflow && this.CompareVector2(this.generationExtents, other.generationExtents) && this.CompareVector2(this.pivot, other.pivot) && this.font == other.font;
		}
	}
}
