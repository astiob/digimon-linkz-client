using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Indicates a serialization surrogate selector class.</summary>
	[ComVisible(true)]
	public interface ISurrogateSelector
	{
		/// <summary>Specifies the next <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> for surrogates to examine if the current instance does not have a surrogate for the specified type and assembly in the specified context.</summary>
		/// <param name="selector">The next surrogate selector to examine. </param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		void ChainSelector(ISurrogateSelector selector);

		/// <summary>Returns the next surrogate selector in the chain.</summary>
		/// <returns>The next surrogate selector in the chain or null.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		ISurrogateSelector GetNextSelector();

		/// <summary>Finds the surrogate that represents the specified object's type, starting with the specified surrogate selector for the specified serialization context.</summary>
		/// <returns>The appropriate surrogate for the given type in the given context.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> of object (class) that needs a surrogate. </param>
		/// <param name="context">The source or destination context for the current serialization. </param>
		/// <param name="selector">When this method returns, contains a <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> that holds a reference to the surrogate selector where the appropriate surrogate was found. This parameter is passed uninitialized. </param>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		ISerializationSurrogate GetSurrogate(Type type, StreamingContext context, out ISurrogateSelector selector);
	}
}
