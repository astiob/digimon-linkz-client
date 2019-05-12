using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Provides a base implementation for channel sinks that want to expose a dictionary interface to their properties.</summary>
	[ComVisible(true)]
	public abstract class BaseChannelSinkWithProperties : BaseChannelObjectWithProperties
	{
	}
}
