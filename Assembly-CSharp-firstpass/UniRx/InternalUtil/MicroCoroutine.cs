using System;
using System.Collections;
using System.Collections.Generic;

namespace UniRx.InternalUtil
{
	public class MicroCoroutine
	{
		private const int InitialSize = 16;

		private readonly object runningAndQueueLock = new object();

		private readonly object arrayLock = new object();

		private readonly Action<Exception> unhandledExceptionCallback;

		private int tail;

		private bool running;

		private IEnumerator[] coroutines = new IEnumerator[16];

		private Queue<IEnumerator> waitQueue = new Queue<IEnumerator>();

		public MicroCoroutine(Action<Exception> unhandledExceptionCallback)
		{
			this.unhandledExceptionCallback = unhandledExceptionCallback;
		}

		public void AddCoroutine(IEnumerator enumerator)
		{
			object obj = this.runningAndQueueLock;
			lock (obj)
			{
				if (this.running)
				{
					this.waitQueue.Enqueue(enumerator);
					return;
				}
			}
			object obj2 = this.arrayLock;
			lock (obj2)
			{
				if (this.coroutines.Length == this.tail)
				{
					Array.Resize<IEnumerator>(ref this.coroutines, checked(this.tail * 2));
				}
				this.coroutines[this.tail++] = enumerator;
			}
		}

		public void Run()
		{
			object obj = this.runningAndQueueLock;
			lock (obj)
			{
				this.running = true;
			}
			object obj2 = this.arrayLock;
			lock (obj2)
			{
				int num = this.tail - 1;
				int i = 0;
				while (i < this.coroutines.Length)
				{
					IEnumerator enumerator = this.coroutines[i];
					if (enumerator != null)
					{
						try
						{
							if (enumerator.MoveNext())
							{
								goto IL_132;
							}
							this.coroutines[i] = null;
						}
						catch (Exception obj3)
						{
							this.coroutines[i] = null;
							try
							{
								this.unhandledExceptionCallback(obj3);
							}
							catch
							{
							}
						}
						goto IL_9A;
					}
					goto IL_9A;
					IL_132:
					i++;
					continue;
					IL_9A:
					while (i < num)
					{
						IEnumerator enumerator2 = this.coroutines[num];
						if (enumerator2 != null)
						{
							try
							{
								if (!enumerator2.MoveNext())
								{
									this.coroutines[num] = null;
									num--;
									continue;
								}
								this.coroutines[i] = enumerator2;
								this.coroutines[num] = null;
								num--;
								goto IL_12D;
							}
							catch (Exception obj4)
							{
								this.coroutines[num] = null;
								num--;
								try
								{
									this.unhandledExceptionCallback(obj4);
								}
								catch
								{
								}
								continue;
							}
							goto IL_116;
							IL_12D:
							goto IL_132;
						}
						IL_116:
						num--;
					}
					this.tail = i;
					break;
				}
				object obj5 = this.runningAndQueueLock;
				lock (obj5)
				{
					this.running = false;
					while (this.waitQueue.Count != 0)
					{
						if (this.coroutines.Length == this.tail)
						{
							Array.Resize<IEnumerator>(ref this.coroutines, checked(this.tail * 2));
						}
						this.coroutines[this.tail++] = this.waitQueue.Dequeue();
					}
				}
			}
		}
	}
}
