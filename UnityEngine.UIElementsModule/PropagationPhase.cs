using System;

namespace UnityEngine.Experimental.UIElements
{
	public enum PropagationPhase
	{
		None,
		Capture,
		AtTarget,
		BubbleUp,
		DefaultAction
	}
}
