using System;
using System.Runtime.Serialization;

namespace System.Runtime.CompilerServices
{
	/// <summary>Wraps an exception that does not derive from the <see cref="T:System.Exception" /> class. This class cannot be inherited.</summary>
	[Serializable]
	public sealed class RuntimeWrappedException : Exception
	{
		private object wrapped_exception;

		private RuntimeWrappedException()
		{
		}

		/// <summary>Gets the object that was wrapped by the <see cref="T:System.Runtime.CompilerServices.RuntimeWrappedException" /> object.</summary>
		/// <returns>The object that was wrapped by the <see cref="T:System.Runtime.CompilerServices.RuntimeWrappedException" /> object.</returns>
		public object WrappedException
		{
			get
			{
				return this.wrapped_exception;
			}
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with information about the exception.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> object that contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null.</exception>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("WrappedException", this.wrapped_exception);
		}
	}
}
