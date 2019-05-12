using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Specifies the resource location.</summary>
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum ResourceLocation
	{
		/// <summary>Specifies an embedded (that is, non-linked) resource.</summary>
		Embedded = 1,
		/// <summary>Specifies that the resource is contained in another assembly.</summary>
		ContainedInAnotherAssembly = 2,
		/// <summary>Specifies that the resource is contained in the manifest file.</summary>
		ContainedInManifestFile = 4
	}
}
