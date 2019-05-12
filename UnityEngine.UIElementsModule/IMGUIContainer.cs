using System;

namespace UnityEngine.Experimental.UIElements
{
	public class IMGUIContainer : VisualElement
	{
		private readonly Action m_OnGUIHandler;

		private ObjectGUIState m_ObjectGUIState;

		internal bool useOwnerObjectGUIState;

		private GUILayoutUtility.LayoutCache m_Cache = null;

		private bool lostFocus = false;

		private bool receivedFocus = false;

		private FocusChangeDirection focusChangeDirection = FocusChangeDirection.unspecified;

		private bool hasFocusableControls = false;

		private int newKeyboardFocusControlID = 0;

		private IMGUIContainer.GUIGlobals m_GUIGlobals;

		public IMGUIContainer(Action onGUIHandler)
		{
			this.m_OnGUIHandler = onGUIHandler;
			this.contextType = ContextType.Editor;
			base.focusIndex = 0;
		}

		internal ObjectGUIState guiState
		{
			get
			{
				Debug.Assert(!this.useOwnerObjectGUIState);
				if (this.m_ObjectGUIState == null)
				{
					this.m_ObjectGUIState = new ObjectGUIState();
				}
				return this.m_ObjectGUIState;
			}
		}

		internal Rect lastWorldClip { get; set; }

		private GUILayoutUtility.LayoutCache cache
		{
			get
			{
				if (this.m_Cache == null)
				{
					this.m_Cache = new GUILayoutUtility.LayoutCache();
				}
				return this.m_Cache;
			}
		}

		public ContextType contextType { get; set; }

		internal int GUIDepth { get; private set; }

		public override bool canGrabFocus
		{
			get
			{
				return base.canGrabFocus && this.hasFocusableControls;
			}
		}

		internal override void DoRepaint(IStylePainter painter)
		{
			base.DoRepaint();
			this.lastWorldClip = painter.currentWorldClip;
			this.HandleIMGUIEvent(painter.repaintEvent);
		}

		internal override void ChangePanel(BaseVisualElementPanel p)
		{
			if (base.elementPanel != null)
			{
				base.elementPanel.IMGUIContainersCount--;
			}
			base.ChangePanel(p);
			if (base.elementPanel != null)
			{
				base.elementPanel.IMGUIContainersCount++;
			}
		}

		private void SaveGlobals()
		{
			this.m_GUIGlobals.matrix = GUI.matrix;
			this.m_GUIGlobals.color = GUI.color;
			this.m_GUIGlobals.contentColor = GUI.contentColor;
			this.m_GUIGlobals.backgroundColor = GUI.backgroundColor;
			this.m_GUIGlobals.enabled = GUI.enabled;
			this.m_GUIGlobals.changed = GUI.changed;
			this.m_GUIGlobals.displayIndex = Event.current.displayIndex;
		}

		private void RestoreGlobals()
		{
			GUI.matrix = this.m_GUIGlobals.matrix;
			GUI.color = this.m_GUIGlobals.color;
			GUI.contentColor = this.m_GUIGlobals.contentColor;
			GUI.backgroundColor = this.m_GUIGlobals.backgroundColor;
			GUI.enabled = this.m_GUIGlobals.enabled;
			GUI.changed = this.m_GUIGlobals.changed;
			Event.current.displayIndex = this.m_GUIGlobals.displayIndex;
		}

		private void DoOnGUI(Event evt)
		{
			if (this.m_OnGUIHandler != null && base.panel != null)
			{
				int num = GUIClip.Internal_GetCount();
				this.SaveGlobals();
				UIElementsUtility.BeginContainerGUI(this.cache, evt, this);
				if (this.lostFocus)
				{
					GUIUtility.keyboardControl = 0;
					if (this.focusController != null)
					{
						this.focusController.imguiKeyboardControl = 0;
					}
					this.lostFocus = false;
				}
				if (this.receivedFocus)
				{
					if (this.focusChangeDirection != FocusChangeDirection.unspecified && this.focusChangeDirection != FocusChangeDirection.none)
					{
						if (this.focusChangeDirection == VisualElementFocusChangeDirection.left)
						{
							GUIUtility.SetKeyboardControlToLastControlId();
						}
						else if (this.focusChangeDirection == VisualElementFocusChangeDirection.right)
						{
							GUIUtility.SetKeyboardControlToFirstControlId();
						}
					}
					this.receivedFocus = false;
					this.focusChangeDirection = FocusChangeDirection.unspecified;
					if (this.focusController != null)
					{
						this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
					}
				}
				this.GUIDepth = GUIUtility.Internal_GetGUIDepth();
				EventType type = Event.current.type;
				bool flag = false;
				try
				{
					this.m_OnGUIHandler();
				}
				catch (Exception exception)
				{
					if (type != EventType.Layout)
					{
						throw;
					}
					flag = GUIUtility.IsExitGUIException(exception);
					if (!flag)
					{
						Debug.LogException(exception);
					}
				}
				finally
				{
					int num2 = GUIUtility.CheckForTabEvent(evt);
					if (this.focusController != null)
					{
						if (num2 < 0)
						{
							KeyDownEvent keyDownEvent = null;
							if (num2 == -1)
							{
								keyDownEvent = KeyboardEventBase<KeyDownEvent>.GetPooled('\t', KeyCode.Tab, EventModifiers.None);
							}
							else if (num2 == -2)
							{
								keyDownEvent = KeyboardEventBase<KeyDownEvent>.GetPooled('\t', KeyCode.Tab, EventModifiers.Shift);
							}
							Focusable focusedElement = this.focusController.focusedElement;
							this.focusController.SwitchFocusOnEvent(keyDownEvent);
							EventBase<KeyDownEvent>.ReleasePooled(keyDownEvent);
							if (focusedElement == this)
							{
								if (this.focusController.focusedElement == this)
								{
									if (num2 == -2)
									{
										GUIUtility.SetKeyboardControlToLastControlId();
									}
									else if (num2 == -1)
									{
										GUIUtility.SetKeyboardControlToFirstControlId();
									}
									this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
									this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
								}
								else
								{
									GUIUtility.keyboardControl = 0;
									this.focusController.imguiKeyboardControl = 0;
								}
							}
						}
						else if (num2 > 0)
						{
							this.focusController.imguiKeyboardControl = GUIUtility.keyboardControl;
							this.newKeyboardFocusControlID = GUIUtility.keyboardControl;
						}
						else if (num2 == 0 && type == EventType.MouseDown)
						{
							this.focusController.SyncIMGUIFocus(this);
						}
					}
					this.hasFocusableControls = GUIUtility.HasFocusableControls();
				}
				EventType type2 = Event.current.type;
				UIElementsUtility.EndContainerGUI();
				this.RestoreGlobals();
				if (!flag)
				{
					if (type2 != EventType.Ignore && type2 != EventType.Used)
					{
						int num3 = GUIClip.Internal_GetCount();
						if (num3 > num)
						{
							Debug.LogError("GUI Error: You are pushing more GUIClips than you are popping. Make sure they are balanced)");
						}
						else if (num3 < num)
						{
							Debug.LogError("GUI Error: You are popping more GUIClips than you are pushing. Make sure they are balanced)");
						}
					}
				}
				while (GUIClip.Internal_GetCount() > num)
				{
					GUIClip.Internal_Pop();
				}
				if (type2 == EventType.Used)
				{
					base.Dirty(ChangeType.Repaint);
				}
			}
		}

		public override void HandleEvent(EventBase evt)
		{
			base.HandleEvent(evt);
			if (evt.propagationPhase != PropagationPhase.DefaultAction)
			{
				if (evt.imguiEvent != null)
				{
					if (!evt.isPropagationStopped)
					{
						if (this.m_OnGUIHandler != null && base.elementPanel != null && base.elementPanel.IMGUIEventInterests.WantsEvent(evt.imguiEvent.type))
						{
							if (this.HandleIMGUIEvent(evt.imguiEvent))
							{
								evt.StopPropagation();
								evt.PreventDefault();
							}
						}
					}
				}
			}
		}

		internal bool HandleIMGUIEvent(Event e)
		{
			EventType type = e.type;
			e.type = EventType.Layout;
			this.DoOnGUI(e);
			e.type = type;
			this.DoOnGUI(e);
			if (this.newKeyboardFocusControlID > 0)
			{
				this.newKeyboardFocusControlID = 0;
				this.HandleIMGUIEvent(new Event
				{
					type = EventType.ExecuteCommand,
					commandName = "NewKeyboardFocus"
				});
			}
			bool result;
			if (e.type == EventType.Used)
			{
				result = true;
			}
			else
			{
				if (e.type == EventType.MouseUp && this.HasCapture())
				{
					GUIUtility.hotControl = 0;
				}
				if (base.elementPanel == null)
				{
					GUIUtility.ExitGUI();
				}
				result = false;
			}
			return result;
		}

		protected internal override void ExecuteDefaultAction(EventBase evt)
		{
			if (evt.GetEventTypeId() == EventBase<BlurEvent>.TypeId())
			{
				BlurEvent blurEvent = evt as BlurEvent;
				if (blurEvent.relatedTarget == null || !blurEvent.relatedTarget.canGrabFocus)
				{
					this.lostFocus = true;
				}
			}
			else if (evt.GetEventTypeId() == EventBase<FocusEvent>.TypeId())
			{
				FocusEvent focusEvent = evt as FocusEvent;
				this.receivedFocus = true;
				this.focusChangeDirection = focusEvent.direction;
			}
		}

		protected internal override Vector2 DoMeasure(float desiredWidth, VisualElement.MeasureMode widthMode, float desiredHeight, VisualElement.MeasureMode heightMode)
		{
			float num = float.NaN;
			float num2 = float.NaN;
			if (widthMode != VisualElement.MeasureMode.Exactly || heightMode != VisualElement.MeasureMode.Exactly)
			{
				this.DoOnGUI(new Event
				{
					type = EventType.Layout
				});
				num = this.m_Cache.topLevel.minWidth;
				num2 = this.m_Cache.topLevel.minHeight;
			}
			if (widthMode != VisualElement.MeasureMode.Exactly)
			{
				if (widthMode == VisualElement.MeasureMode.AtMost)
				{
					num = Mathf.Min(num, desiredWidth);
				}
			}
			else
			{
				num = desiredWidth;
			}
			if (heightMode != VisualElement.MeasureMode.Exactly)
			{
				if (heightMode == VisualElement.MeasureMode.AtMost)
				{
					num2 = Mathf.Min(num2, desiredHeight);
				}
			}
			else
			{
				num2 = desiredHeight;
			}
			return new Vector2(num, num2);
		}

		private struct GUIGlobals
		{
			public Matrix4x4 matrix;

			public Color color;

			public Color contentColor;

			public Color backgroundColor;

			public bool enabled;

			public bool changed;

			public int displayIndex;
		}
	}
}
