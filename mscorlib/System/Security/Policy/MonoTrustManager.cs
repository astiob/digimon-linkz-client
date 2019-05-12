using System;

namespace System.Security.Policy
{
	internal class MonoTrustManager : IApplicationTrustManager, ISecurityEncodable
	{
		private const string tag = "IApplicationTrustManager";

		public ApplicationTrust DetermineApplicationTrust(ActivationContext activationContext, TrustManagerContext context)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			return null;
		}

		public void FromXml(SecurityElement e)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (e.Tag != "IApplicationTrustManager")
			{
				throw new ArgumentException("e", Locale.GetText("Invalid XML tag."));
			}
		}

		public SecurityElement ToXml()
		{
			SecurityElement securityElement = new SecurityElement("IApplicationTrustManager");
			securityElement.AddAttribute("class", typeof(MonoTrustManager).AssemblyQualifiedName);
			securityElement.AddAttribute("version", "1");
			return securityElement;
		}
	}
}
