using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class that can be used to generate text for rendering.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class TextGenerator : IDisposable
	{
		internal IntPtr m_Ptr;

		private string m_LastString;

		private TextGenerationSettings m_LastSettings;

		private bool m_HasGenerated;

		private bool m_LastValid;

		private readonly List<UIVertex> m_Verts;

		private readonly List<UICharInfo> m_Characters;

		private readonly List<UILineInfo> m_Lines;

		private bool m_CachedVerts;

		private bool m_CachedCharacters;

		private bool m_CachedLines;

		/// <summary>
		///   <para>Create a TextGenerator.</para>
		/// </summary>
		/// <param name="initialCapacity"></param>
		public TextGenerator() : this(50)
		{
		}

		/// <summary>
		///   <para>Create a TextGenerator.</para>
		/// </summary>
		/// <param name="initialCapacity"></param>
		public TextGenerator(int initialCapacity)
		{
			this.m_Verts = new List<UIVertex>((initialCapacity + 1) * 4);
			this.m_Characters = new List<UICharInfo>(initialCapacity + 1);
			this.m_Lines = new List<UILineInfo>(20);
			this.Init();
		}

		void IDisposable.Dispose()
		{
			this.Dispose_cpp();
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Dispose_cpp();

		internal bool Populate_Internal(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, VerticalWrapMode verticalOverFlow, HorizontalWrapMode horizontalOverflow, bool updateBounds, TextAnchor anchor, Vector2 extents, Vector2 pivot, bool generateOutOfBounds)
		{
			return this.Populate_Internal_cpp(str, font, color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, (int)verticalOverFlow, (int)horizontalOverflow, updateBounds, anchor, extents.x, extents.y, pivot.x, pivot.y, generateOutOfBounds);
		}

		internal bool Populate_Internal_cpp(string str, Font font, Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds)
		{
			return TextGenerator.INTERNAL_CALL_Populate_Internal_cpp(this, str, font, ref color, fontSize, scaleFactor, lineSpacing, style, richText, resizeTextForBestFit, resizeTextMinSize, resizeTextMaxSize, verticalOverFlow, horizontalOverflow, updateBounds, anchor, extentsX, extentsY, pivotX, pivotY, generateOutOfBounds);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_Populate_Internal_cpp(TextGenerator self, string str, Font font, ref Color color, int fontSize, float scaleFactor, float lineSpacing, FontStyle style, bool richText, bool resizeTextForBestFit, int resizeTextMinSize, int resizeTextMaxSize, int verticalOverFlow, int horizontalOverflow, bool updateBounds, TextAnchor anchor, float extentsX, float extentsY, float pivotX, float pivotY, bool generateOutOfBounds);

		/// <summary>
		///   <para>Extents of the generated text in rect format.</para>
		/// </summary>
		public Rect rectExtents
		{
			get
			{
				Rect result;
				this.INTERNAL_get_rectExtents(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_rectExtents(out Rect value);

		/// <summary>
		///   <para>Number of vertices generated.</para>
		/// </summary>
		public extern int vertexCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetVerticesInternal(object vertices);

		/// <summary>
		///   <para>Returns the current UILineInfo.</para>
		/// </summary>
		/// <returns>
		///   <para>Vertices.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern UIVertex[] GetVerticesArray();

		/// <summary>
		///   <para>The number of characters that have been generated.</para>
		/// </summary>
		public extern int characterCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of characters that have been generated and are included in the visible lines.</para>
		/// </summary>
		public int characterCountVisible
		{
			get
			{
				return (!string.IsNullOrEmpty(this.m_LastString)) ? Mathf.Min(this.m_LastString.Length, Mathf.Max(0, (this.vertexCount - 4) / 4)) : 0;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetCharactersInternal(object characters);

		/// <summary>
		///   <para>Returns the current UICharInfo.</para>
		/// </summary>
		/// <returns>
		///   <para>Character information.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern UICharInfo[] GetCharactersArray();

		/// <summary>
		///   <para>Number of text lines generated.</para>
		/// </summary>
		public extern int lineCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLinesInternal(object lines);

		/// <summary>
		///   <para>Returns the current UILineInfo.</para>
		/// </summary>
		/// <returns>
		///   <para>Line information.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern UILineInfo[] GetLinesArray();

		/// <summary>
		///   <para>The size of the font that was found if using best fit mode.</para>
		/// </summary>
		public extern int fontSizeUsedForBestFit { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		~TextGenerator()
		{
			((IDisposable)this).Dispose();
		}

		private TextGenerationSettings ValidatedSettings(TextGenerationSettings settings)
		{
			if (settings.font != null && settings.font.dynamic)
			{
				return settings;
			}
			if (settings.fontSize != 0 || settings.fontStyle != FontStyle.Normal)
			{
				Debug.LogWarning("Font size and style overrides are only supported for dynamic fonts.");
				settings.fontSize = 0;
				settings.fontStyle = FontStyle.Normal;
			}
			if (settings.resizeTextForBestFit)
			{
				Debug.LogWarning("BestFit is only suppoerted for dynamic fonts.");
				settings.resizeTextForBestFit = false;
			}
			return settings;
		}

		/// <summary>
		///   <para>Mark the text generator as invalid. This will force a full text generation the next time Populate is called.</para>
		/// </summary>
		public void Invalidate()
		{
			this.m_HasGenerated = false;
		}

		public void GetCharacters(List<UICharInfo> characters)
		{
			this.GetCharactersInternal(characters);
		}

		public void GetLines(List<UILineInfo> lines)
		{
			this.GetLinesInternal(lines);
		}

		public void GetVertices(List<UIVertex> vertices)
		{
			this.GetVerticesInternal(vertices);
		}

		/// <summary>
		///   <para>Given a string and settings, returns the preferred width for a container that would hold this text.</para>
		/// </summary>
		/// <param name="str">Generation text.</param>
		/// <param name="settings">Settings for generation.</param>
		/// <returns>
		///   <para>Preferred width.</para>
		/// </returns>
		public float GetPreferredWidth(string str, TextGenerationSettings settings)
		{
			settings.horizontalOverflow = HorizontalWrapMode.Overflow;
			settings.verticalOverflow = VerticalWrapMode.Overflow;
			settings.updateBounds = true;
			this.Populate(str, settings);
			return this.rectExtents.width;
		}

		/// <summary>
		///   <para>Given a string and settings, returns the preferred height for a container that would hold this text.</para>
		/// </summary>
		/// <param name="str">Generation text.</param>
		/// <param name="settings">Settings for generation.</param>
		/// <returns>
		///   <para>Preferred height.</para>
		/// </returns>
		public float GetPreferredHeight(string str, TextGenerationSettings settings)
		{
			settings.verticalOverflow = VerticalWrapMode.Overflow;
			settings.updateBounds = true;
			this.Populate(str, settings);
			return this.rectExtents.height;
		}

		/// <summary>
		///   <para>Will generate the vertices and other data for the given string with the given settings.</para>
		/// </summary>
		/// <param name="str">String to generate.</param>
		/// <param name="settings">Settings.</param>
		public bool Populate(string str, TextGenerationSettings settings)
		{
			if (this.m_HasGenerated && str == this.m_LastString && settings.Equals(this.m_LastSettings))
			{
				return this.m_LastValid;
			}
			return this.PopulateAlways(str, settings);
		}

		private bool PopulateAlways(string str, TextGenerationSettings settings)
		{
			this.m_LastString = str;
			this.m_HasGenerated = true;
			this.m_CachedVerts = false;
			this.m_CachedCharacters = false;
			this.m_CachedLines = false;
			this.m_LastSettings = settings;
			TextGenerationSettings textGenerationSettings = this.ValidatedSettings(settings);
			this.m_LastValid = this.Populate_Internal(str, textGenerationSettings.font, textGenerationSettings.color, textGenerationSettings.fontSize, textGenerationSettings.scaleFactor, textGenerationSettings.lineSpacing, textGenerationSettings.fontStyle, textGenerationSettings.richText, textGenerationSettings.resizeTextForBestFit, textGenerationSettings.resizeTextMinSize, textGenerationSettings.resizeTextMaxSize, textGenerationSettings.verticalOverflow, textGenerationSettings.horizontalOverflow, textGenerationSettings.updateBounds, textGenerationSettings.textAnchor, textGenerationSettings.generationExtents, textGenerationSettings.pivot, textGenerationSettings.generateOutOfBounds);
			return this.m_LastValid;
		}

		/// <summary>
		///   <para>Array of generated vertices.</para>
		/// </summary>
		public IList<UIVertex> verts
		{
			get
			{
				if (!this.m_CachedVerts)
				{
					this.GetVertices(this.m_Verts);
					this.m_CachedVerts = true;
				}
				return this.m_Verts;
			}
		}

		/// <summary>
		///   <para>Array of generated characters.</para>
		/// </summary>
		public IList<UICharInfo> characters
		{
			get
			{
				if (!this.m_CachedCharacters)
				{
					this.GetCharacters(this.m_Characters);
					this.m_CachedCharacters = true;
				}
				return this.m_Characters;
			}
		}

		/// <summary>
		///   <para>Information about each generated text line.</para>
		/// </summary>
		public IList<UILineInfo> lines
		{
			get
			{
				if (!this.m_CachedLines)
				{
					this.GetLines(this.m_Lines);
					this.m_CachedLines = true;
				}
				return this.m_Lines;
			}
		}
	}
}
