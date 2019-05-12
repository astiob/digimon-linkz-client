using System;
using System.Collections.Generic;

namespace UnityEngine.Experimental.UIElements
{
	internal class EventDispatcher : IEventDispatcher
	{
		private VisualElement m_TopElementUnderMouse;

		public IEventHandler capture { get; set; }

		public void ReleaseCapture(IEventHandler handler)
		{
			Debug.Assert(handler == this.capture, "Element releasing capture does not have capture");
			this.capture = null;
		}

		public void RemoveCapture()
		{
			if (this.capture != null)
			{
				this.capture.OnLostCapture();
			}
			this.capture = null;
		}

		public void TakeCapture(IEventHandler handler)
		{
			if (this.capture != handler)
			{
				if (GUIUtility.hotControl != 0)
				{
					Debug.Log("Should not be capturing when there is a hotcontrol");
				}
				else
				{
					this.RemoveCapture();
					this.capture = handler;
				}
			}
		}

		private void DispatchMouseEnterMouseLeave(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, IMouseEvent triggerEvent)
		{
			if (previousTopElementUnderMouse != currentTopElementUnderMouse)
			{
				int i = 0;
				VisualElement visualElement;
				for (visualElement = previousTopElementUnderMouse; visualElement != null; visualElement = visualElement.shadow.parent)
				{
					i++;
				}
				int j = 0;
				VisualElement visualElement2;
				for (visualElement2 = currentTopElementUnderMouse; visualElement2 != null; visualElement2 = visualElement2.shadow.parent)
				{
					j++;
				}
				visualElement = previousTopElementUnderMouse;
				visualElement2 = currentTopElementUnderMouse;
				while (i > j)
				{
					MouseLeaveEvent pooled = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent);
					pooled.target = visualElement;
					this.DispatchEvent(pooled, visualElement.panel);
					EventBase<MouseLeaveEvent>.ReleasePooled(pooled);
					i--;
					visualElement = visualElement.shadow.parent;
				}
				List<VisualElement> list = new List<VisualElement>(j);
				while (j > i)
				{
					list.Add(visualElement2);
					j--;
					visualElement2 = visualElement2.shadow.parent;
				}
				while (visualElement != visualElement2)
				{
					MouseLeaveEvent pooled2 = MouseEventBase<MouseLeaveEvent>.GetPooled(triggerEvent);
					pooled2.target = visualElement;
					this.DispatchEvent(pooled2, visualElement.panel);
					EventBase<MouseLeaveEvent>.ReleasePooled(pooled2);
					list.Add(visualElement2);
					visualElement = visualElement.shadow.parent;
					visualElement2 = visualElement2.shadow.parent;
				}
				for (int k = list.Count - 1; k >= 0; k--)
				{
					MouseEnterEvent pooled3 = MouseEventBase<MouseEnterEvent>.GetPooled(triggerEvent);
					pooled3.target = list[k];
					this.DispatchEvent(pooled3, list[k].panel);
					EventBase<MouseEnterEvent>.ReleasePooled(pooled3);
				}
			}
		}

		private void DispatchMouseOverMouseOut(VisualElement previousTopElementUnderMouse, VisualElement currentTopElementUnderMouse, IMouseEvent triggerEvent)
		{
			if (previousTopElementUnderMouse != currentTopElementUnderMouse)
			{
				if (previousTopElementUnderMouse != null)
				{
					MouseOutEvent pooled = MouseEventBase<MouseOutEvent>.GetPooled(triggerEvent);
					pooled.target = previousTopElementUnderMouse;
					this.DispatchEvent(pooled, previousTopElementUnderMouse.panel);
					EventBase<MouseOutEvent>.ReleasePooled(pooled);
				}
				if (currentTopElementUnderMouse != null)
				{
					MouseOverEvent pooled2 = MouseEventBase<MouseOverEvent>.GetPooled(triggerEvent);
					pooled2.target = currentTopElementUnderMouse;
					this.DispatchEvent(pooled2, currentTopElementUnderMouse.panel);
					EventBase<MouseOverEvent>.ReleasePooled(pooled2);
				}
			}
		}

		public void DispatchEvent(EventBase evt, IPanel panel)
		{
			Event imguiEvent = evt.imguiEvent;
			if (imguiEvent == null || imguiEvent.type != EventType.Repaint)
			{
				bool flag = false;
				VisualElement visualElement = this.capture as VisualElement;
				if (visualElement != null && visualElement.panel == null)
				{
					Debug.Log(string.Format("Capture has no panel, forcing removal (capture={0} eventType={1})", this.capture, (imguiEvent == null) ? "null" : imguiEvent.type.ToString()));
					this.RemoveCapture();
					visualElement = null;
				}
				if ((evt is IMouseEvent || imguiEvent != null) && this.capture != null)
				{
					if (panel != null)
					{
						if (visualElement != null && visualElement.panel.contextType != panel.contextType)
						{
							return;
						}
					}
					flag = true;
					evt.dispatch = true;
					evt.target = this.capture;
					evt.currentTarget = this.capture;
					evt.propagationPhase = PropagationPhase.AtTarget;
					this.capture.HandleEvent(evt);
					evt.propagationPhase = PropagationPhase.None;
					evt.currentTarget = null;
					evt.dispatch = false;
				}
				if (!evt.isPropagationStopped)
				{
					if (evt is IKeyboardEvent)
					{
						if (panel.focusController.focusedElement != null)
						{
							IMGUIContainer imguicontainer = panel.focusController.focusedElement as IMGUIContainer;
							flag = true;
							if (imguicontainer != null)
							{
								if (imguicontainer.HandleIMGUIEvent(evt.imguiEvent))
								{
									evt.StopPropagation();
									evt.PreventDefault();
								}
							}
							else
							{
								evt.target = panel.focusController.focusedElement;
								EventDispatcher.PropagateEvent(evt);
							}
						}
						else
						{
							evt.target = panel.visualTree;
							EventDispatcher.PropagateEvent(evt);
							flag = false;
						}
					}
					else if (evt.GetEventTypeId() == EventBase<MouseEnterEvent>.TypeId() || evt.GetEventTypeId() == EventBase<MouseLeaveEvent>.TypeId())
					{
						Debug.Assert(evt.target != null);
						flag = true;
						EventDispatcher.PropagateEvent(evt);
					}
					else if (evt is IMouseEvent || (imguiEvent != null && (imguiEvent.type == EventType.ContextClick || imguiEvent.type == EventType.MouseEnterWindow || imguiEvent.type == EventType.MouseLeaveWindow || imguiEvent.type == EventType.DragUpdated || imguiEvent.type == EventType.DragPerform || imguiEvent.type == EventType.DragExited)))
					{
						VisualElement topElementUnderMouse = this.m_TopElementUnderMouse;
						if (imguiEvent != null && imguiEvent.type == EventType.MouseLeaveWindow)
						{
							this.m_TopElementUnderMouse = null;
							this.DispatchMouseEnterMouseLeave(topElementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
							this.DispatchMouseOverMouseOut(topElementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
						}
						else if (evt is IMouseEvent || imguiEvent != null)
						{
							if (evt.target == null)
							{
								if (evt is IMouseEvent)
								{
									this.m_TopElementUnderMouse = panel.Pick((evt as IMouseEvent).localMousePosition);
								}
								else if (imguiEvent != null)
								{
									this.m_TopElementUnderMouse = panel.Pick(imguiEvent.mousePosition);
								}
								evt.target = this.m_TopElementUnderMouse;
							}
							if (evt.target != null)
							{
								flag = true;
								EventDispatcher.PropagateEvent(evt);
							}
							if (evt.GetEventTypeId() == EventBase<MouseMoveEvent>.TypeId())
							{
								this.DispatchMouseEnterMouseLeave(topElementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
								this.DispatchMouseOverMouseOut(topElementUnderMouse, this.m_TopElementUnderMouse, evt as IMouseEvent);
							}
						}
					}
					else if (imguiEvent != null && (imguiEvent.type == EventType.ExecuteCommand || imguiEvent.type == EventType.ValidateCommand))
					{
						IMGUIContainer imguicontainer2 = panel.focusController.focusedElement as IMGUIContainer;
						if (imguicontainer2 != null)
						{
							flag = true;
							if (imguicontainer2.HandleIMGUIEvent(evt.imguiEvent))
							{
								evt.StopPropagation();
								evt.PreventDefault();
							}
						}
						else if (panel.focusController.focusedElement != null)
						{
							flag = true;
							evt.target = panel.focusController.focusedElement;
							EventDispatcher.PropagateEvent(evt);
						}
					}
					else if (evt is IPropagatableEvent)
					{
						Debug.Assert(evt.target != null);
						flag = true;
						EventDispatcher.PropagateEvent(evt);
					}
				}
				if (!evt.isPropagationStopped && imguiEvent != null)
				{
					if (!flag || (imguiEvent != null && (imguiEvent.type == EventType.MouseEnterWindow || imguiEvent.type == EventType.MouseLeaveWindow || imguiEvent.type == EventType.Used)))
					{
						EventDispatcher.PropagateToIMGUIContainer(panel.visualTree, evt, visualElement);
					}
				}
				if (evt.target == null)
				{
					evt.target = panel.visualTree;
				}
				EventDispatcher.ExecuteDefaultAction(evt);
			}
		}

		private static void PropagateToIMGUIContainer(VisualElement root, EventBase evt, VisualElement capture)
		{
			IMGUIContainer imguicontainer = root as IMGUIContainer;
			if (imguicontainer != null && (evt.imguiEvent.type == EventType.Used || root != capture))
			{
				if (imguicontainer.HandleIMGUIEvent(evt.imguiEvent))
				{
					evt.StopPropagation();
					evt.PreventDefault();
				}
			}
			else if (root != null)
			{
				for (int i = 0; i < root.shadow.childCount; i++)
				{
					EventDispatcher.PropagateToIMGUIContainer(root.shadow[i], evt, capture);
					if (evt.isPropagationStopped)
					{
						break;
					}
				}
			}
		}

		private static void PropagateEvent(EventBase evt)
		{
			if (!evt.dispatch)
			{
				EventDispatcher.PropagationPaths propagationPaths = EventDispatcher.BuildPropagationPath(evt.target as VisualElement);
				evt.dispatch = true;
				if (evt.capturable && propagationPaths.capturePath.Count > 0)
				{
					evt.propagationPhase = PropagationPhase.Capture;
					for (int i = propagationPaths.capturePath.Count - 1; i >= 0; i--)
					{
						if (evt.isPropagationStopped)
						{
							break;
						}
						evt.currentTarget = propagationPaths.capturePath[i];
						evt.currentTarget.HandleEvent(evt);
					}
				}
				if (!evt.isPropagationStopped)
				{
					evt.propagationPhase = PropagationPhase.AtTarget;
					evt.currentTarget = evt.target;
					evt.currentTarget.HandleEvent(evt);
				}
				if (evt.bubbles && propagationPaths.bubblePath.Count > 0)
				{
					evt.propagationPhase = PropagationPhase.BubbleUp;
					for (int j = 0; j < propagationPaths.bubblePath.Count; j++)
					{
						if (evt.isPropagationStopped)
						{
							break;
						}
						evt.currentTarget = propagationPaths.bubblePath[j];
						evt.currentTarget.HandleEvent(evt);
					}
				}
				evt.dispatch = false;
				evt.propagationPhase = PropagationPhase.None;
				evt.currentTarget = null;
			}
		}

		private static void ExecuteDefaultAction(EventBase evt)
		{
			if (!evt.isDefaultPrevented && evt.target != null)
			{
				evt.dispatch = true;
				evt.currentTarget = evt.target;
				evt.propagationPhase = PropagationPhase.DefaultAction;
				evt.currentTarget.HandleEvent(evt);
				evt.propagationPhase = PropagationPhase.None;
				evt.currentTarget = null;
				evt.dispatch = false;
			}
		}

		private static EventDispatcher.PropagationPaths BuildPropagationPath(VisualElement elem)
		{
			EventDispatcher.PropagationPaths propagationPaths = new EventDispatcher.PropagationPaths(16);
			EventDispatcher.PropagationPaths result;
			if (elem == null)
			{
				result = propagationPaths;
			}
			else
			{
				while (elem.shadow.parent != null)
				{
					if (elem.shadow.parent.enabledInHierarchy)
					{
						if (elem.shadow.parent.HasCaptureHandlers())
						{
							propagationPaths.capturePath.Add(elem.shadow.parent);
						}
						if (elem.shadow.parent.HasBubbleHandlers())
						{
							propagationPaths.bubblePath.Add(elem.shadow.parent);
						}
					}
					elem = elem.shadow.parent;
				}
				result = propagationPaths;
			}
			return result;
		}

		private struct PropagationPaths
		{
			public List<VisualElement> capturePath;

			public List<VisualElement> bubblePath;

			public PropagationPaths(int initialSize)
			{
				this.capturePath = new List<VisualElement>(initialSize);
				this.bubblePath = new List<VisualElement>(initialSize);
			}
		}
	}
}
