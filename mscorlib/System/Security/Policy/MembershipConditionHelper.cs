using System;

namespace System.Security.Policy
{
	internal sealed class MembershipConditionHelper
	{
		private static readonly string XmlTag = "IMembershipCondition";

		internal static int CheckSecurityElement(SecurityElement se, string parameterName, int minimumVersion, int maximumVersion)
		{
			if (se == null)
			{
				throw new ArgumentNullException(parameterName);
			}
			if (se.Tag != MembershipConditionHelper.XmlTag)
			{
				string message = string.Format(Locale.GetText("Invalid tag {0}, expected {1}."), se.Tag, MembershipConditionHelper.XmlTag);
				throw new ArgumentException(message, parameterName);
			}
			int result = minimumVersion;
			string text = se.Attribute("version");
			if (text != null)
			{
				try
				{
					result = int.Parse(text);
				}
				catch (Exception innerException)
				{
					string text2 = Locale.GetText("Couldn't parse version from '{0}'.");
					text2 = string.Format(text2, text);
					throw new ArgumentException(text2, parameterName, innerException);
				}
			}
			return result;
		}

		internal static SecurityElement Element(Type type, int version)
		{
			SecurityElement securityElement = new SecurityElement(MembershipConditionHelper.XmlTag);
			securityElement.AddAttribute("class", type.FullName + ", " + type.Assembly.ToString().Replace('"', '\''));
			securityElement.AddAttribute("version", version.ToString());
			return securityElement;
		}
	}
}
