using System;
using UnityEngine;

namespace UniRx.Diagnostics
{
	public class UnityDebugSink : IObserver<LogEntry>
	{
		public void OnCompleted()
		{
		}

		public void OnError(Exception error)
		{
		}

		public void OnNext(LogEntry value)
		{
			object context = value.Context;
			switch (value.LogType)
			{
			case LogType.Error:
				if (context == null)
				{
					global::Debug.LogError(value.Message);
				}
				else
				{
					global::Debug.LogError(value.Message, value.Context);
				}
				break;
			case LogType.Warning:
				if (context == null)
				{
					global::Debug.LogWarning(value.Message);
				}
				else
				{
					global::Debug.LogWarning(value.Message, value.Context);
				}
				break;
			case LogType.Log:
				if (context == null)
				{
					global::Debug.Log(value.Message);
				}
				else
				{
					global::Debug.Log(value.Message, value.Context);
				}
				break;
			case LogType.Exception:
				if (context == null)
				{
					global::Debug.LogException(value.Exception);
				}
				else
				{
					global::Debug.LogException(value.Exception, value.Context);
				}
				break;
			}
		}
	}
}
