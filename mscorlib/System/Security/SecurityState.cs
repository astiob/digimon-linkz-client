using System;

namespace System.Security
{
	/// <summary>Provides a base class for requesting the security status of an action from the <see cref="T:System.AppDomainManager" /> object.</summary>
	public abstract class SecurityState
	{
		/// <summary>When overridden in a derived class, ensures that the state that is represented by <see cref="T:System.Security.SecurityState" /> is available on the host.</summary>
		public abstract void EnsureState();

		/// <summary>Gets a value that indicates whether the state for this implementation of the <see cref="T:System.Security.SecurityState" /> class is available on the current host. </summary>
		/// <returns>true if the state is available; otherwise, false. </returns>
		public bool IsStateAvailable()
		{
			AppDomainManager domainManager = AppDomain.CurrentDomain.DomainManager;
			return domainManager != null && domainManager.CheckSecuritySettings(this);
		}
	}
}
