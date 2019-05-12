using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Contains a constant used to specify an infinite amount of time. This class cannot be inherited. </summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public static class Timeout
	{
		/// <summary>A constant used to specify an infinite waiting period. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int Infinite = -1;
	}
}
