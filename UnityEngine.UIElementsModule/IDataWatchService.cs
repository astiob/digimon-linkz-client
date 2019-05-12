using System;

namespace UnityEngine.Experimental.UIElements
{
	internal interface IDataWatchService
	{
		IDataWatchHandle AddWatch(Object watched, Action<Object> onDataChanged);

		void RemoveWatch(IDataWatchHandle handle);

		void ForceDirtyNextPoll(Object obj);
	}
}
