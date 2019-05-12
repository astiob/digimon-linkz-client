using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventCallbackList
	{
		private List<EventCallbackFunctorBase> m_List;

		public EventCallbackList()
		{
			this.m_List = new List<EventCallbackFunctorBase>();
			this.capturingCallbackCount = 0;
			this.bubblingCallbackCount = 0;
		}

		public EventCallbackList(EventCallbackList source)
		{
			this.m_List = new List<EventCallbackFunctorBase>(source.m_List);
			this.capturingCallbackCount = 0;
			this.bubblingCallbackCount = 0;
		}

		public int capturingCallbackCount { get; private set; }

		public int bubblingCallbackCount { get; private set; }

		public bool Contains(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			for (int i = 0; i < this.m_List.Count; i++)
			{
				if (this.m_List[i].IsEquivalentTo(eventTypeId, callback, phase))
				{
					return true;
				}
			}
			return false;
		}

		public bool Remove(long eventTypeId, Delegate callback, CallbackPhase phase)
		{
			for (int i = 0; i < this.m_List.Count; i++)
			{
				if (this.m_List[i].IsEquivalentTo(eventTypeId, callback, phase))
				{
					this.m_List.RemoveAt(i);
					if (phase == CallbackPhase.CaptureAndTarget)
					{
						this.capturingCallbackCount--;
					}
					else if (phase == CallbackPhase.TargetAndBubbleUp)
					{
						this.bubblingCallbackCount--;
					}
					return true;
				}
			}
			return false;
		}

		public void Add(EventCallbackFunctorBase item)
		{
			this.m_List.Add(item);
			if (item.phase == CallbackPhase.CaptureAndTarget)
			{
				this.capturingCallbackCount++;
			}
			else if (item.phase == CallbackPhase.TargetAndBubbleUp)
			{
				this.bubblingCallbackCount++;
			}
		}

		public void AddRange(EventCallbackList list)
		{
			this.m_List.AddRange(list.m_List);
			foreach (EventCallbackFunctorBase eventCallbackFunctorBase in list.m_List)
			{
				if (eventCallbackFunctorBase.phase == CallbackPhase.CaptureAndTarget)
				{
					this.capturingCallbackCount++;
				}
				else if (eventCallbackFunctorBase.phase == CallbackPhase.TargetAndBubbleUp)
				{
					this.bubblingCallbackCount++;
				}
			}
		}

		public int Count
		{
			get
			{
				return this.m_List.Count;
			}
		}

		public EventCallbackFunctorBase this[int i]
		{
			get
			{
				return this.m_List[i];
			}
			set
			{
				this.m_List[i] = value;
			}
		}

		public void Clear()
		{
			this.m_List.Clear();
			this.capturingCallbackCount = 0;
			this.bubblingCallbackCount = 0;
		}
	}
}
