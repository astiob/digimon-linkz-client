using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies the attributes for a manifest resource.</summary>
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ResourceAttributes
	{
		/// <summary>A mask used to retrieve public manifest resources.</summary>
		Public = 1,
		/// <summary>A mask used to retrieve private manifest resources.</summary>
		Private = 2
	}
}
