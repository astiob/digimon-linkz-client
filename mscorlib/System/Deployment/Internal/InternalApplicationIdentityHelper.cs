using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal
{
	/// <summary>Provides access to internal properties of an <see cref="T:System.ApplicationIdentity" /> object.</summary>
	[ComVisible(false)]
	public static class InternalApplicationIdentityHelper
	{
		[MonoTODO("2.0 SP1 member")]
		public static object GetActivationContextData(ActivationContext appInfo)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets an IDefinitionAppId Interface representing the unique identifier of an <see cref="T:System.ApplicationIdentity" /> object.</summary>
		/// <returns>The unique identifier held by the <see cref="T:System.ApplicationIdentity" /> object.</returns>
		/// <param name="id">The object from which to extract the identifier.</param>
		[MonoTODO]
		public static object GetInternalAppId(ApplicationIdentity id)
		{
			throw new NotImplementedException();
		}
	}
}
