using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.UIElements
{
	internal class UIElementsUtility
	{
		private static Stack<IMGUIContainer> s_ContainerStack = new Stack<IMGUIContainer>();

		private static Dictionary<int, Panel> s_UIElementsCache = new Dictionary<int, Panel>();

		private static Event s_EventInstance = new Event();

		private static EventDispatcher s_EventDispatcher;

		[CompilerGenerated]
		private static Action <>f__mg$cache0;

		[CompilerGenerated]
		private static Action <>f__mg$cache1;

		[CompilerGenerated]
		private static Func<int, IntPtr, bool> <>f__mg$cache2;

		[CompilerGenerated]
		private static Action <>f__mg$cache3;

		[CompilerGenerated]
		private static Func<Exception, bool> <>f__mg$cache4;

		static UIElementsUtility()
		{
			Delegate takeCapture = GUIUtility.takeCapture;
			if (UIElementsUtility.<>f__mg$cache0 == null)
			{
				UIElementsUtility.<>f__mg$cache0 = new Action(UIElementsUtility.TakeCapture);
			}
			GUIUtility.takeCapture = (Action)Delegate.Combine(takeCapture, UIElementsUtility.<>f__mg$cache0);
			Delegate releaseCapture = GUIUtility.releaseCapture;
			if (UIElementsUtility.<>f__mg$cache1 == null)
			{
				UIElementsUtility.<>f__mg$cache1 = new Action(UIElementsUtility.ReleaseCapture);
			}
			GUIUtility.releaseCapture = (Action)Delegate.Combine(releaseCapture, UIElementsUtility.<>f__mg$cache1);
			Delegate processEvent = GUIUtility.processEvent;
			if (UIElementsUtility.<>f__mg$cache2 == null)
			{
				UIElementsUtility.<>f__mg$cache2 = new Func<int, IntPtr, bool>(UIElementsUtility.ProcessEvent);
			}
			GUIUtility.processEvent = (Func<int, IntPtr, bool>)Delegate.Combine(processEvent, UIElementsUtility.<>f__mg$cache2);
			Delegate cleanupRoots = GUIUtility.cleanupRoots;
			if (UIElementsUtility.<>f__mg$cache3 == null)
			{
				UIElementsUtility.<>f__mg$cache3 = new Action(UIElementsUtility.CleanupRoots);
			}
			GUIUtility.cleanupRoots = (Action)Delegate.Combine(cleanupRoots, UIElementsUtility.<>f__mg$cache3);
			Delegate endContainerGUIFromException = GUIUtility.endContainerGUIFromException;
			if (UIElementsUtility.<>f__mg$cache4 == null)
			{
				UIElementsUtility.<>f__mg$cache4 = new Func<Exception, bool>(UIElementsUtility.EndContainerGUIFromException);
			}
			GUIUtility.endContainerGUIFromException = (Func<Exception, bool>)Delegate.Combine(endContainerGUIFromException, UIElementsUtility.<>f__mg$cache4);
		}

		internal static IEventDispatcher eventDispatcher
		{
			get
			{
				if (UIElementsUtility.s_EventDispatcher == null)
				{
					UIElementsUtility.s_EventDispatcher = new EventDispatcher();
				}
				return UIElementsUtility.s_EventDispatcher;
			}
		}

		internal static void ClearDispatcher()
		{
			UIElementsUtility.s_EventDispatcher = null;
		}

		private static void TakeCapture()
		{
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				IMGUIContainer imguicontainer = UIElementsUtility.s_ContainerStack.Peek();
				if (imguicontainer.GUIDepth == GUIUtility.Internal_GetGUIDepth())
				{
					if (UIElementsUtility.eventDispatcher.capture != null && UIElementsUtility.eventDispatcher.capture != imguicontainer)
					{
						Debug.Log(string.Format("Should not grab hot control with an active capture (current={0} new={1}", UIElementsUtility.eventDispatcher.capture, imguicontainer));
					}
					UIElementsUtility.eventDispatcher.TakeCapture(imguicontainer);
				}
			}
		}

		private static void ReleaseCapture()
		{
			UIElementsUtility.eventDispatcher.RemoveCapture();
		}

		private static bool ProcessEvent(int instanceID, IntPtr nativeEventPtr)
		{
			Panel panel;
			bool result;
			if (nativeEventPtr != IntPtr.Zero && UIElementsUtility.s_UIElementsCache.TryGetValue(instanceID, out panel))
			{
				UIElementsUtility.s_EventInstance.CopyFromPtr(nativeEventPtr);
				result = UIElementsUtility.DoDispatch(panel);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public static void RemoveCachedPanel(int instanceID)
		{
			UIElementsUtility.s_UIElementsCache.Remove(instanceID);
		}

		private static void CleanupRoots()
		{
			UIElementsUtility.s_EventInstance = null;
			UIElementsUtility.s_EventDispatcher = null;
			UIElementsUtility.s_UIElementsCache = null;
			UIElementsUtility.s_ContainerStack = null;
		}

		private static bool EndContainerGUIFromException(Exception exception)
		{
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
			return GUIUtility.ShouldRethrowException(exception);
		}

		internal static void BeginContainerGUI(GUILayoutUtility.LayoutCache cache, Event evt, IMGUIContainer container)
		{
			if (container.useOwnerObjectGUIState)
			{
				GUIUtility.BeginContainerFromOwner(container.elementPanel.ownerObject);
			}
			else
			{
				GUIUtility.BeginContainer(container.guiState);
			}
			UIElementsUtility.s_ContainerStack.Push(container);
			GUIUtility.s_SkinMode = (int)container.contextType;
			GUIUtility.s_OriginalID = container.elementPanel.ownerObject.GetInstanceID();
			Event.current = evt;
			GUI.enabled = container.enabledInHierarchy;
			GUILayoutUtility.BeginContainer(cache);
			GUIUtility.ResetGlobalState();
			Rect clipRect = container.lastWorldClip;
			if (clipRect.width == 0f || clipRect.height == 0f)
			{
				clipRect = container.worldBound;
			}
			Matrix4x4 lhs = container.worldTransform;
			if (evt.type == EventType.Repaint && container.elementPanel != null && container.elementPanel.stylePainter != null)
			{
				lhs = container.elementPanel.stylePainter.currentTransform;
			}
			GUIClip.SetTransform(lhs * Matrix4x4.Translate(container.layout.position), clipRect);
		}

		internal static void EndContainerGUI()
		{
			if (Event.current.type == EventType.Layout && UIElementsUtility.s_ContainerStack.Count > 0)
			{
				Rect layout = UIElementsUtility.s_ContainerStack.Peek().layout;
				GUILayoutUtility.LayoutFromContainer(layout.width, layout.height);
			}
			GUILayoutUtility.SelectIDList(GUIUtility.s_OriginalID, false);
			GUIContent.ClearStaticCache();
			if (UIElementsUtility.s_ContainerStack.Count > 0)
			{
				GUIUtility.EndContainer();
				UIElementsUtility.s_ContainerStack.Pop();
			}
		}

		internal static ContextType GetGUIContextType()
		{
			return (GUIUtility.s_SkinMode != 0) ? ContextType.Editor : ContextType.Player;
		}

		internal static EventBase CreateEvent(Event systemEvent)
		{
			EventBase pooled;
			switch (systemEvent.type)
			{
			case EventType.MouseDown:
				pooled = MouseEventBase<MouseDownEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseUp:
				pooled = MouseEventBase<MouseUpEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseMove:
				pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
				break;
			case EventType.MouseDrag:
				pooled = MouseEventBase<MouseMoveEvent>.GetPooled(systemEvent);
				break;
			case EventType.KeyDown:
				pooled = KeyboardEventBase<KeyDownEvent>.GetPooled(systemEvent);
				break;
			case EventType.KeyUp:
				pooled = KeyboardEventBase<KeyUpEvent>.GetPooled(systemEvent);
				break;
			case EventType.ScrollWheel:
				pooled = WheelEvent.GetPooled(systemEvent);
				break;
			default:
				pooled = IMGUIEvent.GetPooled(systemEvent);
				break;
			}
			return pooled;
		}

		internal static void ReleaseEvent(EventBase evt)
		{
			long eventTypeId = evt.GetEventTypeId();
			if (eventTypeId == EventBase<MouseMoveEvent>.TypeId())
			{
				EventBase<MouseMoveEvent>.ReleasePooled((MouseMoveEvent)evt);
			}
			else if (eventTypeId == EventBase<MouseDownEvent>.TypeId())
			{
				EventBase<MouseDownEvent>.ReleasePooled((MouseDownEvent)evt);
			}
			else if (eventTypeId == EventBase<MouseUpEvent>.TypeId())
			{
				EventBase<MouseUpEvent>.ReleasePooled((MouseUpEvent)evt);
			}
			else if (eventTypeId == EventBase<WheelEvent>.TypeId())
			{
				EventBase<WheelEvent>.ReleasePooled((WheelEvent)evt);
			}
			else if (eventTypeId == EventBase<KeyDownEvent>.TypeId())
			{
				EventBase<KeyDownEvent>.ReleasePooled((KeyDownEvent)evt);
			}
			else if (eventTypeId == EventBase<KeyUpEvent>.TypeId())
			{
				EventBase<KeyUpEvent>.ReleasePooled((KeyUpEvent)evt);
			}
			else if (eventTypeId == EventBase<IMGUIEvent>.TypeId())
			{
				EventBase<IMGUIEvent>.ReleasePooled((IMGUIEvent)evt);
			}
		}

		private static bool DoDispatch(BaseVisualElementPanel panel)
		{
			bool result;
			if (UIElementsUtility.s_EventInstance.type == EventType.Repaint)
			{
				panel.Repaint(UIElementsUtility.s_EventInstance);
				result = (panel.IMGUIContainersCount > 0);
			}
			else
			{
				panel.ValidateLayout();
				EventBase eventBase = UIElementsUtility.CreateEvent(UIElementsUtility.s_EventInstance);
				Vector2 mousePosition = UIElementsUtility.s_EventInstance.mousePosition;
				UIElementsUtility.s_EventDispatcher.DispatchEvent(eventBase, panel);
				UIElementsUtility.s_EventInstance.mousePosition = mousePosition;
				if (eventBase.isPropagationStopped)
				{
					panel.visualTree.Dirty(ChangeType.Repaint);
				}
				result = eventBase.isPropagationStopped;
				UIElementsUtility.ReleaseEvent(eventBase);
			}
			return result;
		}

		internal static Dictionary<int, Panel>.Enumerator GetPanelsIterator()
		{
			return UIElementsUtility.s_UIElementsCache.GetEnumerator();
		}

		internal static Panel FindOrCreatePanel(ScriptableObject ownerObject, ContextType contextType, IDataWatchService dataWatch = null)
		{
			Panel panel;
			if (!UIElementsUtility.s_UIElementsCache.TryGetValue(ownerObject.GetInstanceID(), out panel))
			{
				panel = new Panel(ownerObject, contextType, dataWatch, UIElementsUtility.eventDispatcher);
				UIElementsUtility.s_UIElementsCache.Add(ownerObject.GetInstanceID(), panel);
			}
			else
			{
				Debug.Assert(contextType == panel.contextType, "Context type mismatch");
			}
			return panel;
		}

		internal static Panel FindOrCreatePanel(ScriptableObject ownerObject)
		{
			return UIElementsUtility.FindOrCreatePanel(ownerObject, UIElementsUtility.GetGUIContextType(), null);
		}
	}
}
