using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>General settings for how the GUI behaves.</para>
	/// </summary>
	[Serializable]
	public sealed class GUISettings
	{
		[SerializeField]
		private bool m_DoubleClickSelectsWord = true;

		[SerializeField]
		private bool m_TripleClickSelectsLine = true;

		[SerializeField]
		private Color m_CursorColor = Color.white;

		[SerializeField]
		private float m_CursorFlashSpeed = -1f;

		[SerializeField]
		private Color m_SelectionColor = new Color(0.5f, 0.5f, 1f);

		/// <summary>
		///   <para>Should double-clicking select words in text fields.</para>
		/// </summary>
		public bool doubleClickSelectsWord
		{
			get
			{
				return this.m_DoubleClickSelectsWord;
			}
			set
			{
				this.m_DoubleClickSelectsWord = value;
			}
		}

		/// <summary>
		///   <para>Should triple-clicking select whole text in text fields.</para>
		/// </summary>
		public bool tripleClickSelectsLine
		{
			get
			{
				return this.m_TripleClickSelectsLine;
			}
			set
			{
				this.m_TripleClickSelectsLine = value;
			}
		}

		/// <summary>
		///   <para>The color of the cursor in text fields.</para>
		/// </summary>
		public Color cursorColor
		{
			get
			{
				return this.m_CursorColor;
			}
			set
			{
				this.m_CursorColor = value;
			}
		}

		/// <summary>
		///   <para>The speed of text field cursor flashes.</para>
		/// </summary>
		public float cursorFlashSpeed
		{
			get
			{
				if (this.m_CursorFlashSpeed >= 0f)
				{
					return this.m_CursorFlashSpeed;
				}
				return GUISettings.Internal_GetCursorFlashSpeed();
			}
			set
			{
				this.m_CursorFlashSpeed = value;
			}
		}

		/// <summary>
		///   <para>The color of the selection rect in text fields.</para>
		/// </summary>
		public Color selectionColor
		{
			get
			{
				return this.m_SelectionColor;
			}
			set
			{
				this.m_SelectionColor = value;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern float Internal_GetCursorFlashSpeed();
	}
}
