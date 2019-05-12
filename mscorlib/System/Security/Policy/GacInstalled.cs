using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Security.Policy
{
	/// <summary>Confirms that a code assembly originates in the global assembly cache (GAC) as evidence for policy evaluation. This class cannot be inherited.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class GacInstalled : IBuiltInEvidence, IIdentityPermissionFactory
	{
		int IBuiltInEvidence.GetRequiredSize(bool verbose)
		{
			return 1;
		}

		int IBuiltInEvidence.InitFromBuffer(char[] buffer, int position)
		{
			return position;
		}

		int IBuiltInEvidence.OutputToBuffer(char[] buffer, int position, bool verbose)
		{
			buffer[position] = '\t';
			return position + 1;
		}

		/// <summary>Creates an equivalent copy of the current object.</summary>
		/// <returns>A new <see cref="T:System.Security.Policy.GacInstalled" /> object.</returns>
		public object Copy()
		{
			return new GacInstalled();
		}

		/// <summary>Creates a new identity permission that corresponds to the current object.</summary>
		/// <returns>A new <see cref="T:System.Security.Permissions.GacIdentityPermission" />.</returns>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> from which to construct the identity permission. </param>
		public IPermission CreateIdentityPermission(Evidence evidence)
		{
			return new GacIdentityPermission();
		}

		/// <summary>Indicates whether the current object is equivalent to the specified object.</summary>
		/// <returns>true if <paramref name="o" /> is a <see cref="T:System.Security.Policy.GacInstalled" /> object; otherwise, false.</returns>
		/// <param name="o">The object to compare with the current object. </param>
		public override bool Equals(object o)
		{
			return o != null && o is GacInstalled;
		}

		/// <summary>Returns a hash code for the current object.</summary>
		/// <returns>0 (zero).</returns>
		public override int GetHashCode()
		{
			return 0;
		}

		/// <summary>Returns a string representation of the current  object.</summary>
		/// <returns>A string representation of the current object.</returns>
		public override string ToString()
		{
			SecurityElement securityElement = new SecurityElement(base.GetType().FullName);
			securityElement.AddAttribute("version", "1");
			return securityElement.ToString();
		}
	}
}
