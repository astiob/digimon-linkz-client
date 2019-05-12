using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Represents a display resolution.</para>
	/// </summary>
	public struct Resolution
	{
		private int m_Width;

		private int m_Height;

		private int m_RefreshRate;

		/// <summary>
		///   <para>Resolution width in pixels.</para>
		/// </summary>
		public int width
		{
			get
			{
				return this.m_Width;
			}
			set
			{
				this.m_Width = value;
			}
		}

		/// <summary>
		///   <para>Resolution height in pixels.</para>
		/// </summary>
		public int height
		{
			get
			{
				return this.m_Height;
			}
			set
			{
				this.m_Height = value;
			}
		}

		/// <summary>
		///   <para>Resolution's vertical refresh rate in Hz.</para>
		/// </summary>
		public int refreshRate
		{
			get
			{
				return this.m_RefreshRate;
			}
			set
			{
				this.m_RefreshRate = value;
			}
		}

		/// <summary>
		///   <para>Returns a nicely formatted string of the resolution.</para>
		/// </summary>
		/// <returns>
		///   <para>A string with the format "width x height @ refreshRateHz".</para>
		/// </returns>
		public override string ToString()
		{
			return UnityString.Format("{0} x {1} @ {2}Hz", new object[]
			{
				this.m_Width,
				this.m_Height,
				this.m_RefreshRate
			});
		}
	}
}
