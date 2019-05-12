using System;
using System.Collections.Generic;
using UnityEngine.UI;

namespace UnityEngine.EventSystems
{
	public static class ExecuteEvents
	{
		private static readonly ExecuteEvents.EventFunction<IPointerEnterHandler> s_PointerEnterHandler = new ExecuteEvents.EventFunction<IPointerEnterHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IPointerExitHandler> s_PointerExitHandler = new ExecuteEvents.EventFunction<IPointerExitHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IPointerDownHandler> s_PointerDownHandler = new ExecuteEvents.EventFunction<IPointerDownHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IPointerUpHandler> s_PointerUpHandler = new ExecuteEvents.EventFunction<IPointerUpHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IPointerClickHandler> s_PointerClickHandler = new ExecuteEvents.EventFunction<IPointerClickHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IInitializePotentialDragHandler> s_InitializePotentialDragHandler = new ExecuteEvents.EventFunction<IInitializePotentialDragHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IBeginDragHandler> s_BeginDragHandler = new ExecuteEvents.EventFunction<IBeginDragHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IDragHandler> s_DragHandler = new ExecuteEvents.EventFunction<IDragHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IEndDragHandler> s_EndDragHandler = new ExecuteEvents.EventFunction<IEndDragHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IDropHandler> s_DropHandler = new ExecuteEvents.EventFunction<IDropHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IScrollHandler> s_ScrollHandler = new ExecuteEvents.EventFunction<IScrollHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IUpdateSelectedHandler> s_UpdateSelectedHandler = new ExecuteEvents.EventFunction<IUpdateSelectedHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<ISelectHandler> s_SelectHandler = new ExecuteEvents.EventFunction<ISelectHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IDeselectHandler> s_DeselectHandler = new ExecuteEvents.EventFunction<IDeselectHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<IMoveHandler> s_MoveHandler = new ExecuteEvents.EventFunction<IMoveHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<ISubmitHandler> s_SubmitHandler = new ExecuteEvents.EventFunction<ISubmitHandler>(ExecuteEvents.Execute);

		private static readonly ExecuteEvents.EventFunction<ICancelHandler> s_CancelHandler = new ExecuteEvents.EventFunction<ICancelHandler>(ExecuteEvents.Execute);

		private static readonly ObjectPool<List<IEventSystemHandler>> s_HandlerListPool = new ObjectPool<List<IEventSystemHandler>>(null, delegate(List<IEventSystemHandler> l)
		{
			l.Clear();
		});

		private static readonly List<Transform> s_InternalTransformList = new List<Transform>(30);

		public static T ValidateEventData<T>(BaseEventData data) where T : class
		{
			if (!(data is T))
			{
				throw new ArgumentException(string.Format("Invalid type: {0} passed to event expecting {1}", data.GetType(), typeof(T)));
			}
			return data as T;
		}

		private static void Execute(IPointerEnterHandler handler, BaseEventData eventData)
		{
			handler.OnPointerEnter(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerExitHandler handler, BaseEventData eventData)
		{
			handler.OnPointerExit(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerDownHandler handler, BaseEventData eventData)
		{
			handler.OnPointerDown(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerUpHandler handler, BaseEventData eventData)
		{
			handler.OnPointerUp(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IPointerClickHandler handler, BaseEventData eventData)
		{
			handler.OnPointerClick(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IInitializePotentialDragHandler handler, BaseEventData eventData)
		{
			handler.OnInitializePotentialDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IBeginDragHandler handler, BaseEventData eventData)
		{
			handler.OnBeginDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IDragHandler handler, BaseEventData eventData)
		{
			handler.OnDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IEndDragHandler handler, BaseEventData eventData)
		{
			handler.OnEndDrag(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IDropHandler handler, BaseEventData eventData)
		{
			handler.OnDrop(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IScrollHandler handler, BaseEventData eventData)
		{
			handler.OnScroll(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
		}

		private static void Execute(IUpdateSelectedHandler handler, BaseEventData eventData)
		{
			handler.OnUpdateSelected(eventData);
		}

		private static void Execute(ISelectHandler handler, BaseEventData eventData)
		{
			handler.OnSelect(eventData);
		}

		private static void Execute(IDeselectHandler handler, BaseEventData eventData)
		{
			handler.OnDeselect(eventData);
		}

		private static void Execute(IMoveHandler handler, BaseEventData eventData)
		{
			handler.OnMove(ExecuteEvents.ValidateEventData<AxisEventData>(eventData));
		}

		private static void Execute(ISubmitHandler handler, BaseEventData eventData)
		{
			handler.OnSubmit(eventData);
		}

		private static void Execute(ICancelHandler handler, BaseEventData eventData)
		{
			handler.OnCancel(eventData);
		}

		public static ExecuteEvents.EventFunction<IPointerEnterHandler> pointerEnterHandler
		{
			get
			{
				return ExecuteEvents.s_PointerEnterHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IPointerExitHandler> pointerExitHandler
		{
			get
			{
				return ExecuteEvents.s_PointerExitHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IPointerDownHandler> pointerDownHandler
		{
			get
			{
				return ExecuteEvents.s_PointerDownHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IPointerUpHandler> pointerUpHandler
		{
			get
			{
				return ExecuteEvents.s_PointerUpHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IPointerClickHandler> pointerClickHandler
		{
			get
			{
				return ExecuteEvents.s_PointerClickHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IInitializePotentialDragHandler> initializePotentialDrag
		{
			get
			{
				return ExecuteEvents.s_InitializePotentialDragHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IBeginDragHandler> beginDragHandler
		{
			get
			{
				return ExecuteEvents.s_BeginDragHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IDragHandler> dragHandler
		{
			get
			{
				return ExecuteEvents.s_DragHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IEndDragHandler> endDragHandler
		{
			get
			{
				return ExecuteEvents.s_EndDragHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IDropHandler> dropHandler
		{
			get
			{
				return ExecuteEvents.s_DropHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IScrollHandler> scrollHandler
		{
			get
			{
				return ExecuteEvents.s_ScrollHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IUpdateSelectedHandler> updateSelectedHandler
		{
			get
			{
				return ExecuteEvents.s_UpdateSelectedHandler;
			}
		}

		public static ExecuteEvents.EventFunction<ISelectHandler> selectHandler
		{
			get
			{
				return ExecuteEvents.s_SelectHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IDeselectHandler> deselectHandler
		{
			get
			{
				return ExecuteEvents.s_DeselectHandler;
			}
		}

		public static ExecuteEvents.EventFunction<IMoveHandler> moveHandler
		{
			get
			{
				return ExecuteEvents.s_MoveHandler;
			}
		}

		public static ExecuteEvents.EventFunction<ISubmitHandler> submitHandler
		{
			get
			{
				return ExecuteEvents.s_SubmitHandler;
			}
		}

		public static ExecuteEvents.EventFunction<ICancelHandler> cancelHandler
		{
			get
			{
				return ExecuteEvents.s_CancelHandler;
			}
		}

		private static void GetEventChain(GameObject root, IList<Transform> eventChain)
		{
			eventChain.Clear();
			if (root == null)
			{
				return;
			}
			Transform transform = root.transform;
			while (transform != null)
			{
				eventChain.Add(transform);
				transform = transform.parent;
			}
		}

		public static bool Execute<T>(GameObject target, BaseEventData eventData, ExecuteEvents.EventFunction<T> functor) where T : IEventSystemHandler
		{
			List<IEventSystemHandler> list = ExecuteEvents.s_HandlerListPool.Get();
			ExecuteEvents.GetEventList<T>(target, list);
			int i = 0;
			while (i < list.Count)
			{
				T handler;
				try
				{
					handler = (T)((object)list[i]);
				}
				catch (Exception innerException)
				{
					IEventSystemHandler eventSystemHandler = list[i];
					Debug.LogException(new Exception(string.Format("Type {0} expected {1} received.", typeof(T).Name, eventSystemHandler.GetType().Name), innerException));
					goto IL_8A;
				}
				goto Block_2;
				IL_8A:
				i++;
				continue;
				Block_2:
				try
				{
					functor(handler, eventData);
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
				goto IL_8A;
			}
			int count = list.Count;
			ExecuteEvents.s_HandlerListPool.Release(list);
			return count > 0;
		}

		public static GameObject ExecuteHierarchy<T>(GameObject root, BaseEventData eventData, ExecuteEvents.EventFunction<T> callbackFunction) where T : IEventSystemHandler
		{
			ExecuteEvents.GetEventChain(root, ExecuteEvents.s_InternalTransformList);
			for (int i = 0; i < ExecuteEvents.s_InternalTransformList.Count; i++)
			{
				Transform transform = ExecuteEvents.s_InternalTransformList[i];
				if (ExecuteEvents.Execute<T>(transform.gameObject, eventData, callbackFunction))
				{
					return transform.gameObject;
				}
			}
			return null;
		}

		private static bool ShouldSendToComponent<T>(Component component) where T : IEventSystemHandler
		{
			if (!(component is T))
			{
				return false;
			}
			Behaviour behaviour = component as Behaviour;
			return !(behaviour != null) || behaviour.isActiveAndEnabled;
		}

		private static void GetEventList<T>(GameObject go, IList<IEventSystemHandler> results) where T : IEventSystemHandler
		{
			if (results == null)
			{
				throw new ArgumentException("Results array is null", "results");
			}
			if (go == null || !go.activeInHierarchy)
			{
				return;
			}
			List<Component> list = ListPool<Component>.Get();
			go.GetComponents<Component>(list);
			for (int i = 0; i < list.Count; i++)
			{
				if (ExecuteEvents.ShouldSendToComponent<T>(list[i]))
				{
					results.Add(list[i] as IEventSystemHandler);
				}
			}
			ListPool<Component>.Release(list);
		}

		public static bool CanHandleEvent<T>(GameObject go) where T : IEventSystemHandler
		{
			List<IEventSystemHandler> list = ExecuteEvents.s_HandlerListPool.Get();
			ExecuteEvents.GetEventList<T>(go, list);
			int count = list.Count;
			ExecuteEvents.s_HandlerListPool.Release(list);
			return count != 0;
		}

		public static GameObject GetEventHandler<T>(GameObject root) where T : IEventSystemHandler
		{
			if (root == null)
			{
				return null;
			}
			Transform transform = root.transform;
			while (transform != null)
			{
				if (ExecuteEvents.CanHandleEvent<T>(transform.gameObject))
				{
					return transform.gameObject;
				}
				transform = transform.parent;
			}
			return null;
		}

		public delegate void EventFunction<T1>(T1 handler, BaseEventData eventData);
	}
}
