using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Scripting;

namespace Firebase.Unity
{
	[Preserve]
	internal class UnitySynchronizationContext : SynchronizationContext
	{
		private static UnitySynchronizationContext _instance = null;

		private Queue<Tuple<SendOrPostCallback, object>> queue;

		private UnitySynchronizationContext.SynchronizationContextBehavoir behavior;

		private int mainThreadId;

		private const int Timeout = 15000;

		private static Dictionary<int, ManualResetEvent> signalDictionary = new Dictionary<int, ManualResetEvent>();

		private UnitySynchronizationContext(GameObject gameObject)
		{
			this.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			this.behavior = gameObject.AddComponent<UnitySynchronizationContext.SynchronizationContextBehavoir>();
			this.queue = this.behavior.CallbackQueue;
		}

		public static UnitySynchronizationContext Instance
		{
			get
			{
				if (UnitySynchronizationContext._instance == null)
				{
					throw new InvalidOperationException("SyncContext not initialized.");
				}
				return UnitySynchronizationContext._instance;
			}
		}

		public static void Create(GameObject gameObject)
		{
			if (UnitySynchronizationContext._instance == null)
			{
				UnitySynchronizationContext._instance = new UnitySynchronizationContext(gameObject);
			}
		}

		public static void Destroy()
		{
			UnitySynchronizationContext._instance = null;
		}

		private ManualResetEvent GetThreadEvent()
		{
			object obj = UnitySynchronizationContext.signalDictionary;
			ManualResetEvent manualResetEvent;
			lock (obj)
			{
				if (!UnitySynchronizationContext.signalDictionary.TryGetValue(Thread.CurrentThread.ManagedThreadId, out manualResetEvent))
				{
					manualResetEvent = new ManualResetEvent(false);
					UnitySynchronizationContext.signalDictionary[Thread.CurrentThread.ManagedThreadId] = manualResetEvent;
				}
			}
			manualResetEvent.Reset();
			return manualResetEvent;
		}

		public void PostCoroutine(Func<IEnumerator> coroutine)
		{
			this.Post(delegate(object x)
			{
				Func<IEnumerator> func = (Func<IEnumerator>)x;
				this.behavior.StartCoroutine(func());
			}, coroutine);
		}

		private IEnumerator SignaledCoroutine(Func<IEnumerator> coroutine, ManualResetEvent newSignal)
		{
			yield return coroutine();
			newSignal.Set();
			yield break;
		}

		public void SendCoroutine(Func<IEnumerator> coroutine, int timeout = 15000)
		{
			if (this.mainThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				this.behavior.StartCoroutine(coroutine());
			}
			else
			{
				ManualResetEvent newSignal = this.GetThreadEvent();
				this.PostCoroutine(() => this.SignaledCoroutine(coroutine, newSignal));
				newSignal.WaitOne(timeout);
			}
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			object obj = this.queue;
			lock (obj)
			{
				this.queue.Enqueue(new Tuple<SendOrPostCallback, object>(d, state));
			}
		}

		public override void Send(SendOrPostCallback d, object state)
		{
			if (this.mainThreadId == Thread.CurrentThread.ManagedThreadId)
			{
				d(state);
			}
			else
			{
				ManualResetEvent newSignal = this.GetThreadEvent();
				this.Post(delegate(object x)
				{
					try
					{
						d(x);
					}
					catch (Exception ex)
					{
						Debug.Log(ex.ToString());
					}
					newSignal.Set();
				}, state);
				newSignal.WaitOne(15000);
			}
		}

		private class SynchronizationContextBehavoir : MonoBehaviour
		{
			private Queue<Tuple<SendOrPostCallback, object>> callbackQueue;

			public Queue<Tuple<SendOrPostCallback, object>> CallbackQueue
			{
				get
				{
					if (this.callbackQueue == null)
					{
						this.callbackQueue = new Queue<Tuple<SendOrPostCallback, object>>();
					}
					return this.callbackQueue;
				}
			}

			[Preserve]
			private IEnumerator Start()
			{
				for (;;)
				{
					Tuple<SendOrPostCallback, object> entry = null;
					object obj = this.CallbackQueue;
					lock (obj)
					{
						if (this.CallbackQueue.Count > 0)
						{
							entry = this.CallbackQueue.Dequeue();
						}
					}
					if (entry != null && entry.Item1 != null)
					{
						try
						{
							entry.Item1(entry.Item2);
						}
						catch (Exception ex)
						{
							Debug.Log(ex.ToString());
						}
					}
					yield return null;
				}
				yield break;
			}
		}
	}
}
