using System;

namespace UniRx.InternalUtil
{
	public class ThreadSafeQueueWorker
	{
		private const int MaxArrayLength = 2146435071;

		private const int InitialSize = 16;

		private object gate = new object();

		private bool dequing;

		private int actionListCount;

		private Action<object>[] actionList = new Action<object>[16];

		private object[] actionStates = new object[16];

		private int waitingListCount;

		private Action<object>[] waitingList = new Action<object>[16];

		private object[] waitingStates = new object[16];

		public void Enqueue(Action<object> action, object state)
		{
			object obj = this.gate;
			lock (obj)
			{
				if (this.dequing)
				{
					if (this.waitingList.Length == this.waitingListCount)
					{
						int num = this.waitingListCount * 2;
						if (num > 2146435071)
						{
							num = 2146435071;
						}
						Action<object>[] destinationArray = new Action<object>[num];
						object[] destinationArray2 = new object[num];
						Array.Copy(this.waitingList, destinationArray, this.waitingListCount);
						Array.Copy(this.waitingStates, destinationArray2, this.waitingListCount);
						this.waitingList = destinationArray;
						this.waitingStates = destinationArray2;
					}
					this.waitingList[this.waitingListCount] = action;
					this.waitingStates[this.waitingListCount] = state;
					this.waitingListCount++;
				}
				else
				{
					if (this.actionList.Length == this.actionListCount)
					{
						int num2 = this.actionListCount * 2;
						if (num2 > 2146435071)
						{
							num2 = 2146435071;
						}
						Action<object>[] destinationArray3 = new Action<object>[num2];
						object[] destinationArray4 = new object[num2];
						Array.Copy(this.actionList, destinationArray3, this.actionListCount);
						Array.Copy(this.actionStates, destinationArray4, this.actionListCount);
						this.actionList = destinationArray3;
						this.actionStates = destinationArray4;
					}
					this.actionList[this.actionListCount] = action;
					this.actionStates[this.actionListCount] = state;
					this.actionListCount++;
				}
			}
		}

		public void ExecuteAll(Action<Exception> unhandledExceptionCallback)
		{
			object obj = this.gate;
			lock (obj)
			{
				if (this.actionListCount == 0)
				{
					return;
				}
				this.dequing = true;
			}
			for (int i = 0; i < this.actionListCount; i++)
			{
				Action<object> action = this.actionList[i];
				object obj2 = this.actionStates[i];
				try
				{
					action(obj2);
				}
				catch (Exception obj3)
				{
					unhandledExceptionCallback(obj3);
				}
				finally
				{
					this.actionList[i] = null;
					this.actionStates[i] = null;
				}
			}
			object obj4 = this.gate;
			lock (obj4)
			{
				this.dequing = false;
				Action<object>[] array = this.actionList;
				object[] array2 = this.actionStates;
				this.actionListCount = this.waitingListCount;
				this.actionList = this.waitingList;
				this.actionStates = this.waitingStates;
				this.waitingListCount = 0;
				this.waitingList = array;
				this.waitingStates = array2;
			}
		}
	}
}
