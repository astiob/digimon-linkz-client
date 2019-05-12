using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Defines the access modes for a dynamic assembly.</summary>
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum AssemblyBuilderAccess
	{
		/// <summary>Represents that the dynamic assembly can be executed, but not saved.</summary>
		Run = 1,
		/// <summary>Represents that the dynamic assembly can be saved, but not executed.</summary>
		Save = 2,
		/// <summary>Represents that the dynamic assembly can be executed and saved.</summary>
		RunAndSave = 3,
		/// <summary>Represents that the dynamic assembly is loaded into the reflection-only context, and cannot be executed.</summary>
		ReflectionOnly = 6
	}
}
