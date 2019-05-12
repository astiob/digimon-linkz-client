using System;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.XR.WSA
{
	[MovedFrom("UnityEngine.VR.WSA")]
	public enum PositionalLocatorState
	{
		Unavailable,
		OrientationOnly,
		Activating,
		Active,
		Inhibited
	}
}
