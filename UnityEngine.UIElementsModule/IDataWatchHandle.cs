using System;

namespace UnityEngine.Experimental.UIElements
{
	public interface IDataWatchHandle : IDisposable
	{
		Object watched { get; }

		bool disposed { get; }
	}
}
