using System;
using System.Runtime.InteropServices;

namespace System
{
	/// <summary>Defines the base class for all context-bound classes.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public abstract class ContextBoundObject : MarshalByRefObject
	{
	}
}
