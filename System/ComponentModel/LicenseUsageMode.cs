using System;

namespace System.ComponentModel
{
	/// <summary>Specifies when the <see cref="T:System.ComponentModel.License" /> can be used.</summary>
	public enum LicenseUsageMode
	{
		/// <summary>Used during design time by a visual designer or the compiler.</summary>
		Designtime = 1,
		/// <summary>Used during runtime.</summary>
		Runtime = 0
	}
}
