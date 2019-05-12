using System;
using System.Runtime.ConstrainedExecution;

namespace System.Runtime
{
	/// <summary>Check for sufficient memory resources prior to execution. This class cannot be inherited.</summary>
	public sealed class MemoryFailPoint : CriticalFinalizerObject, IDisposable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.MemoryFailPoint" /> class, specifying the amount of memory required for successful execution. </summary>
		/// <param name="sizeInMegabytes">The required memory size in megabytes.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">The specified memory size is negative.</exception>
		/// <exception cref="T:System.InsufficientMemoryException">There is insufficient memory to begin execution of the code protected by the gate.</exception>
		[MonoTODO]
		public MemoryFailPoint(int sizeInMegabytes)
		{
			throw new NotImplementedException();
		}

		~MemoryFailPoint()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Runtime.MemoryFailPoint" />. </summary>
		[MonoTODO]
		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
