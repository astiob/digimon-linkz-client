using System;

namespace UnityEngine.Analytics
{
	[EnumCase(EnumCase.Styles.Lower)]
	public enum AuthorizationNetwork
	{
		None,
		Internal,
		Facebook,
		Twitter,
		Google,
		GameCenter
	}
}
