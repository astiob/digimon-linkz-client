using System;

namespace System.Net
{
	/// <summary>Provides the base interface to load and execute scripts for automatic proxy detection.</summary>
	public interface IWebProxyScript
	{
		/// <summary>Closes a script.</summary>
		void Close();

		/// <summary>Loads a script.</summary>
		/// <returns>A <see cref="T:System.Boolean" /> indicating whether the script was successfully loaded.</returns>
		/// <param name="scriptLocation">Internal only.</param>
		/// <param name="script">Internal only.</param>
		/// <param name="helperType">Internal only.</param>
		bool Load(System.Uri scriptLocation, string Script, Type helperType);

		/// <summary>Runs a script.</summary>
		/// <returns>A <see cref="T:System.String" />.</returns>
		/// <param name="url">Internal only.</param>
		/// <param name="host">Internal only.</param>
		string Run(string url, string host);
	}
}
