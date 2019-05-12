using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	/// <summary>Defines the details of how a method is implemented.</summary>
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplOptions
	{
		/// <summary>Specifies that the method is implemented in unmanaged code.</summary>
		Unmanaged = 4,
		/// <summary>Specifies that the method is declared, but its implementation is provided elsewhere.</summary>
		ForwardRef = 16,
		/// <summary>Specifies an internal call. An internal call is a call to a method that is implemented within the common language runtime itself.</summary>
		InternalCall = 4096,
		/// <summary>Specifies that the method can be executed by only one thread at a time.  Static methods lock on the type, whereas instance methods lock on the instance. Only one thread can execute in any of the instance functions, and only one thread can execute in any of a class's static functions.</summary>
		Synchronized = 32,
		/// <summary>Specifies that the method cannot be inlined.</summary>
		NoInlining = 8,
		/// <summary>Specifies that the method signature is exported exactly as declared.</summary>
		PreserveSig = 128,
		/// <summary>Specifies that the method is not optimized by the just-in-time (JIT) compiler or by native code generation (see Ngen.exe) when debugging possible code generation problems.</summary>
		NoOptimization = 64
	}
}
