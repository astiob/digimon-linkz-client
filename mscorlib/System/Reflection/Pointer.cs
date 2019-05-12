using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.Reflection
{
	/// <summary>Provides a wrapper class for pointers.</summary>
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class Pointer : ISerializable
	{
		private unsafe void* data;

		private Type type;

		private Pointer()
		{
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name, fusion log, and additional exception information.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			throw new NotSupportedException("Pointer deserializatioon not supported.");
		}

		/// <summary>Boxes the supplied unmanaged memory pointer and the type associated with that pointer into a managed <see cref="T:System.Reflection.Pointer" /> wrapper object. The value and the type are saved so they can be accessed from the native code during an invocation.</summary>
		/// <returns>A pointer object.</returns>
		/// <param name="ptr">The supplied unmanaged memory pointer. </param>
		/// <param name="type">The type associated with the <paramref name="ptr" /> parameter. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="type" /> is not a pointer. </exception>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> is null. </exception>
		public unsafe static object Box(void* ptr, Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!type.IsPointer)
			{
				throw new ArgumentException("type");
			}
			return new Pointer
			{
				data = ptr,
				type = type
			};
		}

		/// <summary>Returns the stored pointer.</summary>
		/// <returns>This method returns void.</returns>
		/// <param name="ptr">The stored pointer. </param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="ptr" /> is not a pointer. </exception>
		public unsafe static void* Unbox(object ptr)
		{
			Pointer pointer = ptr as Pointer;
			if (pointer == null)
			{
				throw new ArgumentException("ptr");
			}
			return pointer.data;
		}
	}
}
