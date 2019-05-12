using System;

namespace UnityEngine.Analytics
{
	[EnumCase(EnumCase.Styles.Snake)]
	public enum ShareType
	{
		None,
		TextOnly,
		Image,
		Video,
		Invite,
		Achievement
	}
}
