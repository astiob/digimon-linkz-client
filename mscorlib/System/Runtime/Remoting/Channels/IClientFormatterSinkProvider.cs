using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Marks a client channel sink provider as a client formatter sink provider.</summary>
	[ComVisible(true)]
	public interface IClientFormatterSinkProvider : IClientChannelSinkProvider
	{
	}
}
