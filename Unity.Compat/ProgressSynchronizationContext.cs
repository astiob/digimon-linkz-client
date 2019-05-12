using System;
using System.Threading;

namespace System
{
	internal static class ProgressSynchronizationContext
	{
		internal static readonly SynchronizationContext SharedContext = new SynchronizationContext();
	}
}
