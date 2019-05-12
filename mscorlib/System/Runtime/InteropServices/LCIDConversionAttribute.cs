using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Indicates that a method's unmanaged signature expects a locale identifier (LCID) parameter.</summary>
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Method, Inherited = false)]
	public sealed class LCIDConversionAttribute : Attribute
	{
		private int id;

		/// <summary>Initializes a new instance of the LCIDConversionAttribute class with the position of the LCID in the unmanaged signature.</summary>
		/// <param name="lcid">Indicates the position of the LCID argument in the unmanaged signature, where 0 is the first argument. </param>
		public LCIDConversionAttribute(int lcid)
		{
			this.id = lcid;
		}

		/// <summary>Gets the position of the LCID argument in the unmanaged signature.</summary>
		/// <returns>The position of the LCID argument in the unmanaged signature, where 0 is the first argument.</returns>
		public int Value
		{
			get
			{
				return this.id;
			}
		}
	}
}
