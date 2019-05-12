using System;
using System.Runtime.InteropServices;

namespace System.Security
{
	/// <summary>Defines the methods that convert permission object state to and from XML element representation.</summary>
	[ComVisible(true)]
	public interface ISecurityEncodable
	{
		/// <summary>Reconstructs a security object with a specified state from an XML encoding.</summary>
		/// <param name="e">The XML encoding to use to reconstruct the security object. </param>
		void FromXml(SecurityElement e);

		/// <summary>Creates an XML encoding of the security object and its current state.</summary>
		/// <returns>An XML encoding of the security object, including any state information.</returns>
		SecurityElement ToXml();
	}
}
