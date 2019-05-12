using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Marks a server channel sink provider as a server formatter sink provider.</summary>
	[ComVisible(true)]
	public interface IServerFormatterSinkProvider : IServerChannelSinkProvider
	{
	}
}
