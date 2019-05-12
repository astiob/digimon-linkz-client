using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Retrieves the mapping of an interface into the actual methods on a class that implements that interface.</summary>
	[ComVisible(true)]
	public struct InterfaceMapping
	{
		/// <summary>Shows the methods that are defined on the interface.</summary>
		[ComVisible(true)]
		public MethodInfo[] InterfaceMethods;

		/// <summary>Shows the type that represents the interface.</summary>
		[ComVisible(true)]
		public Type InterfaceType;

		/// <summary>Shows the methods that implement the interface.</summary>
		[ComVisible(true)]
		public MethodInfo[] TargetMethods;

		/// <summary>Represents the type that was used to create the interface mapping.</summary>
		[ComVisible(true)]
		public Type TargetType;
	}
}
