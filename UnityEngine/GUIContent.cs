using System;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The contents of a GUI element.</para>
	/// </summary>
	[Serializable]
	[StructLayout(LayoutKind.Sequential)]
	public class GUIContent
	{
		[SerializeField]
		private string m_Text = string.Empty;

		[SerializeField]
		private Texture m_Image;

		[SerializeField]
		private string m_Tooltip = string.Empty;

		private static readonly GUIContent s_Text = new GUIContent();

		private static readonly GUIContent s_Image = new GUIContent();

		private static readonly GUIContent s_TextImage = new GUIContent();

		/// <summary>
		///   <para>Shorthand for empty content.</para>
		/// </summary>
		public static GUIContent none = new GUIContent(string.Empty);

		/// <summary>
		///   <para>Constructor for GUIContent in all shapes and sizes.</para>
		/// </summary>
		public GUIContent()
		{
		}

		/// <summary>
		///   <para>Build a GUIContent object containing only text.</para>
		/// </summary>
		/// <param name="text"></param>
		public GUIContent(string text)
		{
			this.m_Text = text;
		}

		/// <summary>
		///   <para>Build a GUIContent object containing only an image.</para>
		/// </summary>
		/// <param name="image"></param>
		public GUIContent(Texture image)
		{
			this.m_Image = image;
		}

		/// <summary>
		///   <para>Build a GUIContent object containing both text and an image.</para>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="image"></param>
		public GUIContent(string text, Texture image)
		{
			this.m_Text = text;
			this.m_Image = image;
		}

		/// <summary>
		///   <para>Build a GUIContent containing some text. When the user hovers the mouse over it, the global GUI.tooltip is set to the tooltip.</para>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="tooltip"></param>
		public GUIContent(string text, string tooltip)
		{
			this.m_Text = text;
			this.m_Tooltip = tooltip;
		}

		/// <summary>
		///   <para>Build a GUIContent containing an image. When the user hovers the mouse over it, the global GUI.tooltip is set to the tooltip.</para>
		/// </summary>
		/// <param name="image"></param>
		/// <param name="tooltip"></param>
		public GUIContent(Texture image, string tooltip)
		{
			this.m_Image = image;
			this.m_Tooltip = tooltip;
		}

		/// <summary>
		///   <para>Build a GUIContent that contains both text, an image and has a tooltip defined. When the user hovers the mouse over it, the global GUI.tooltip is set to the tooltip.</para>
		/// </summary>
		/// <param name="text"></param>
		/// <param name="image"></param>
		/// <param name="tooltip"></param>
		public GUIContent(string text, Texture image, string tooltip)
		{
			this.m_Text = text;
			this.m_Image = image;
			this.m_Tooltip = tooltip;
		}

		/// <summary>
		///   <para>Build a GUIContent as a copy of another GUIContent.</para>
		/// </summary>
		/// <param name="src"></param>
		public GUIContent(GUIContent src)
		{
			this.m_Text = src.m_Text;
			this.m_Image = src.m_Image;
			this.m_Tooltip = src.m_Tooltip;
		}

		/// <summary>
		///   <para>The text contained.</para>
		/// </summary>
		public string text
		{
			get
			{
				return this.m_Text;
			}
			set
			{
				this.m_Text = value;
			}
		}

		/// <summary>
		///   <para>The icon image contained.</para>
		/// </summary>
		public Texture image
		{
			get
			{
				return this.m_Image;
			}
			set
			{
				this.m_Image = value;
			}
		}

		/// <summary>
		///   <para>The tooltip of this element.</para>
		/// </summary>
		public string tooltip
		{
			get
			{
				return this.m_Tooltip;
			}
			set
			{
				this.m_Tooltip = value;
			}
		}

		internal int hash
		{
			get
			{
				int result = 0;
				if (!string.IsNullOrEmpty(this.m_Text))
				{
					result = this.m_Text.GetHashCode() * 37;
				}
				return result;
			}
		}

		internal static GUIContent Temp(string t)
		{
			GUIContent.s_Text.m_Text = t;
			GUIContent.s_Text.m_Tooltip = string.Empty;
			return GUIContent.s_Text;
		}

		internal static GUIContent Temp(string t, string tooltip)
		{
			GUIContent.s_Text.m_Text = t;
			GUIContent.s_Text.m_Tooltip = tooltip;
			return GUIContent.s_Text;
		}

		internal static GUIContent Temp(Texture i)
		{
			GUIContent.s_Image.m_Image = i;
			GUIContent.s_Image.m_Tooltip = string.Empty;
			return GUIContent.s_Image;
		}

		internal static GUIContent Temp(Texture i, string tooltip)
		{
			GUIContent.s_Image.m_Image = i;
			GUIContent.s_Image.m_Tooltip = tooltip;
			return GUIContent.s_Image;
		}

		internal static GUIContent Temp(string t, Texture i)
		{
			GUIContent.s_TextImage.m_Text = t;
			GUIContent.s_TextImage.m_Image = i;
			return GUIContent.s_TextImage;
		}

		internal static void ClearStaticCache()
		{
			GUIContent.s_Text.m_Text = null;
			GUIContent.s_Text.m_Tooltip = string.Empty;
			GUIContent.s_Image.m_Image = null;
			GUIContent.s_Image.m_Tooltip = string.Empty;
			GUIContent.s_TextImage.m_Text = null;
			GUIContent.s_TextImage.m_Image = null;
		}

		internal static GUIContent[] Temp(string[] texts)
		{
			GUIContent[] array = new GUIContent[texts.Length];
			for (int i = 0; i < texts.Length; i++)
			{
				array[i] = new GUIContent(texts[i]);
			}
			return array;
		}

		internal static GUIContent[] Temp(Texture[] images)
		{
			GUIContent[] array = new GUIContent[images.Length];
			for (int i = 0; i < images.Length; i++)
			{
				array[i] = new GUIContent(images[i]);
			}
			return array;
		}
	}
}
