using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A UnityGUI event.</para>
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public sealed class Event
	{
		[NonSerialized]
		internal IntPtr m_Ptr;

		private static Event s_Current;

		private static Event s_MasterEvent;

		public Event()
		{
			this.Init();
		}

		public Event(Event other)
		{
			if (other == null)
			{
				throw new ArgumentException("Event to copy from is null.");
			}
			this.InitCopy(other);
		}

		private Event(IntPtr ptr)
		{
			this.InitPtr(ptr);
		}

		~Event()
		{
			this.Cleanup();
		}

		/// <summary>
		///   <para>The mouse position.</para>
		/// </summary>
		public Vector2 mousePosition
		{
			get
			{
				Vector2 result;
				this.Internal_GetMousePosition(out result);
				return result;
			}
			set
			{
				this.Internal_SetMousePosition(value);
			}
		}

		/// <summary>
		///   <para>The relative movement of the mouse compared to last event.</para>
		/// </summary>
		public Vector2 delta
		{
			get
			{
				Vector2 result;
				this.Internal_GetMouseDelta(out result);
				return result;
			}
			set
			{
				this.Internal_SetMouseDelta(value);
			}
		}

		[Obsolete("Use HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);", true)]
		public Ray mouseRay
		{
			get
			{
				return new Ray(Vector3.up, Vector3.up);
			}
			set
			{
			}
		}

		/// <summary>
		///   <para>Is Shift held down? (Read Only)</para>
		/// </summary>
		public bool shift
		{
			get
			{
				return (this.modifiers & EventModifiers.Shift) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.Shift;
				}
				else
				{
					this.modifiers |= EventModifiers.Shift;
				}
			}
		}

		/// <summary>
		///   <para>Is Control key held down? (Read Only)</para>
		/// </summary>
		public bool control
		{
			get
			{
				return (this.modifiers & EventModifiers.Control) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.Control;
				}
				else
				{
					this.modifiers |= EventModifiers.Control;
				}
			}
		}

		/// <summary>
		///   <para>Is Alt/Option key held down? (Read Only)</para>
		/// </summary>
		public bool alt
		{
			get
			{
				return (this.modifiers & EventModifiers.Alt) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.Alt;
				}
				else
				{
					this.modifiers |= EventModifiers.Alt;
				}
			}
		}

		/// <summary>
		///   <para>Is Command/Windows key held down? (Read Only)</para>
		/// </summary>
		public bool command
		{
			get
			{
				return (this.modifiers & EventModifiers.Command) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.Command;
				}
				else
				{
					this.modifiers |= EventModifiers.Command;
				}
			}
		}

		/// <summary>
		///   <para>Is Caps Lock on? (Read Only)</para>
		/// </summary>
		public bool capsLock
		{
			get
			{
				return (this.modifiers & EventModifiers.CapsLock) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.CapsLock;
				}
				else
				{
					this.modifiers |= EventModifiers.CapsLock;
				}
			}
		}

		/// <summary>
		///   <para>Is the current keypress on the numeric keyboard? (Read Only)</para>
		/// </summary>
		public bool numeric
		{
			get
			{
				return (this.modifiers & EventModifiers.Numeric) != EventModifiers.None;
			}
			set
			{
				if (!value)
				{
					this.modifiers &= ~EventModifiers.Shift;
				}
				else
				{
					this.modifiers |= EventModifiers.Shift;
				}
			}
		}

		/// <summary>
		///   <para>Is the current keypress a function key? (Read Only)</para>
		/// </summary>
		public bool functionKey
		{
			get
			{
				return (this.modifiers & EventModifiers.FunctionKey) != EventModifiers.None;
			}
		}

		/// <summary>
		///   <para>The current event that's being processed right now.</para>
		/// </summary>
		public static Event current
		{
			get
			{
				return Event.s_Current;
			}
			set
			{
				if (value != null)
				{
					Event.s_Current = value;
				}
				else
				{
					Event.s_Current = Event.s_MasterEvent;
				}
				Event.Internal_SetNativeEvent(Event.s_Current.m_Ptr);
			}
		}

		private static void Internal_MakeMasterEventCurrent()
		{
			if (Event.s_MasterEvent == null)
			{
				Event.s_MasterEvent = new Event();
			}
			Event.s_Current = Event.s_MasterEvent;
			Event.Internal_SetNativeEvent(Event.s_MasterEvent.m_Ptr);
		}

		/// <summary>
		///   <para>Is this event a keyboard event? (Read Only)</para>
		/// </summary>
		public bool isKey
		{
			get
			{
				EventType type = this.type;
				return type == EventType.KeyDown || type == EventType.KeyUp;
			}
		}

		/// <summary>
		///   <para>Is this event a mouse event? (Read Only)</para>
		/// </summary>
		public bool isMouse
		{
			get
			{
				EventType type = this.type;
				return type == EventType.MouseMove || type == EventType.MouseDown || type == EventType.MouseUp || type == EventType.MouseDrag;
			}
		}

		/// <summary>
		///   <para>Create a keyboard event.</para>
		/// </summary>
		/// <param name="key"></param>
		public static Event KeyboardEvent(string key)
		{
			Event @event = new Event();
			@event.type = EventType.KeyDown;
			if (string.IsNullOrEmpty(key))
			{
				return @event;
			}
			int num = 0;
			bool flag;
			do
			{
				flag = true;
				if (num >= key.Length)
				{
					break;
				}
				char c = key[num];
				switch (c)
				{
				case '#':
					@event.modifiers |= EventModifiers.Shift;
					num++;
					break;
				default:
					if (c != '^')
					{
						flag = false;
					}
					else
					{
						@event.modifiers |= EventModifiers.Control;
						num++;
					}
					break;
				case '%':
					@event.modifiers |= EventModifiers.Command;
					num++;
					break;
				case '&':
					@event.modifiers |= EventModifiers.Alt;
					num++;
					break;
				}
			}
			while (flag);
			string text = key.Substring(num, key.Length - num).ToLower();
			string text2 = text;
			switch (text2)
			{
			case "[0]":
				@event.character = '0';
				@event.keyCode = KeyCode.Keypad0;
				return @event;
			case "[1]":
				@event.character = '1';
				@event.keyCode = KeyCode.Keypad1;
				return @event;
			case "[2]":
				@event.character = '2';
				@event.keyCode = KeyCode.Keypad2;
				return @event;
			case "[3]":
				@event.character = '3';
				@event.keyCode = KeyCode.Keypad3;
				return @event;
			case "[4]":
				@event.character = '4';
				@event.keyCode = KeyCode.Keypad4;
				return @event;
			case "[5]":
				@event.character = '5';
				@event.keyCode = KeyCode.Keypad5;
				return @event;
			case "[6]":
				@event.character = '6';
				@event.keyCode = KeyCode.Keypad6;
				return @event;
			case "[7]":
				@event.character = '7';
				@event.keyCode = KeyCode.Keypad7;
				return @event;
			case "[8]":
				@event.character = '8';
				@event.keyCode = KeyCode.Keypad8;
				return @event;
			case "[9]":
				@event.character = '9';
				@event.keyCode = KeyCode.Keypad9;
				return @event;
			case "[.]":
				@event.character = '.';
				@event.keyCode = KeyCode.KeypadPeriod;
				return @event;
			case "[/]":
				@event.character = '/';
				@event.keyCode = KeyCode.KeypadDivide;
				return @event;
			case "[-]":
				@event.character = '-';
				@event.keyCode = KeyCode.KeypadMinus;
				return @event;
			case "[+]":
				@event.character = '+';
				@event.keyCode = KeyCode.KeypadPlus;
				return @event;
			case "[=]":
				@event.character = '=';
				@event.keyCode = KeyCode.KeypadEquals;
				return @event;
			case "[equals]":
				@event.character = '=';
				@event.keyCode = KeyCode.KeypadEquals;
				return @event;
			case "[enter]":
				@event.character = '\n';
				@event.keyCode = KeyCode.KeypadEnter;
				return @event;
			case "up":
				@event.keyCode = KeyCode.UpArrow;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "down":
				@event.keyCode = KeyCode.DownArrow;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "left":
				@event.keyCode = KeyCode.LeftArrow;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "right":
				@event.keyCode = KeyCode.RightArrow;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "insert":
				@event.keyCode = KeyCode.Insert;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "home":
				@event.keyCode = KeyCode.Home;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "end":
				@event.keyCode = KeyCode.End;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "pgup":
				@event.keyCode = KeyCode.PageDown;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "page up":
				@event.keyCode = KeyCode.PageUp;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "pgdown":
				@event.keyCode = KeyCode.PageUp;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "page down":
				@event.keyCode = KeyCode.PageDown;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "backspace":
				@event.keyCode = KeyCode.Backspace;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "delete":
				@event.keyCode = KeyCode.Delete;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "tab":
				@event.keyCode = KeyCode.Tab;
				return @event;
			case "f1":
				@event.keyCode = KeyCode.F1;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f2":
				@event.keyCode = KeyCode.F2;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f3":
				@event.keyCode = KeyCode.F3;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f4":
				@event.keyCode = KeyCode.F4;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f5":
				@event.keyCode = KeyCode.F5;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f6":
				@event.keyCode = KeyCode.F6;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f7":
				@event.keyCode = KeyCode.F7;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f8":
				@event.keyCode = KeyCode.F8;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f9":
				@event.keyCode = KeyCode.F9;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f10":
				@event.keyCode = KeyCode.F10;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f11":
				@event.keyCode = KeyCode.F11;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f12":
				@event.keyCode = KeyCode.F12;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f13":
				@event.keyCode = KeyCode.F13;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f14":
				@event.keyCode = KeyCode.F14;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "f15":
				@event.keyCode = KeyCode.F15;
				@event.modifiers |= EventModifiers.FunctionKey;
				return @event;
			case "[esc]":
				@event.keyCode = KeyCode.Escape;
				return @event;
			case "return":
				@event.character = '\n';
				@event.keyCode = KeyCode.Return;
				@event.modifiers &= ~EventModifiers.FunctionKey;
				return @event;
			case "space":
				@event.keyCode = KeyCode.Space;
				@event.character = ' ';
				@event.modifiers &= ~EventModifiers.FunctionKey;
				return @event;
			}
			if (text.Length != 1)
			{
				try
				{
					@event.keyCode = (KeyCode)((int)Enum.Parse(typeof(KeyCode), text, true));
				}
				catch (ArgumentException)
				{
					Debug.LogError(UnityString.Format("Unable to find key name that matches '{0}'", new object[]
					{
						text
					}));
				}
			}
			else
			{
				@event.character = text.ToLower()[0];
				@event.keyCode = (KeyCode)@event.character;
				if (@event.modifiers != EventModifiers.None)
				{
					@event.character = '\0';
				}
			}
			return @event;
		}

		public override int GetHashCode()
		{
			int num = 1;
			if (this.isKey)
			{
				num = (int)((ushort)this.keyCode);
			}
			if (this.isMouse)
			{
				num = this.mousePosition.GetHashCode();
			}
			return num * 37 | (int)this.modifiers;
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != base.GetType())
			{
				return false;
			}
			Event @event = (Event)obj;
			if (this.type != @event.type || (this.modifiers & ~EventModifiers.CapsLock) != (@event.modifiers & ~EventModifiers.CapsLock))
			{
				return false;
			}
			if (this.isKey)
			{
				return this.keyCode == @event.keyCode;
			}
			return this.isMouse && this.mousePosition == @event.mousePosition;
		}

		public override string ToString()
		{
			if (this.isKey)
			{
				if (this.character == '\0')
				{
					return UnityString.Format("Event:{0}   Character:\\0   Modifiers:{1}   KeyCode:{2}", new object[]
					{
						this.type,
						this.modifiers,
						this.keyCode
					});
				}
				return string.Concat(new object[]
				{
					"Event:",
					this.type,
					"   Character:",
					(int)this.character,
					"   Modifiers:",
					this.modifiers,
					"   KeyCode:",
					this.keyCode
				});
			}
			else
			{
				if (this.isMouse)
				{
					return UnityString.Format("Event: {0}   Position: {1} Modifiers: {2}", new object[]
					{
						this.type,
						this.mousePosition,
						this.modifiers
					});
				}
				if (this.type == EventType.ExecuteCommand || this.type == EventType.ValidateCommand)
				{
					return UnityString.Format("Event: {0}  \"{1}\"", new object[]
					{
						this.type,
						this.commandName
					});
				}
				return string.Empty + this.type;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Init();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Cleanup();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InitCopy(Event other);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void InitPtr(IntPtr ptr);

		public extern EventType rawType { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The type of event.</para>
		/// </summary>
		public extern EventType type { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Get a filtered event type for a given control ID.</para>
		/// </summary>
		/// <param name="controlID">The ID of the control you are querying from.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern EventType GetTypeForControl(int controlID);

		private void Internal_SetMousePosition(Vector2 value)
		{
			Event.INTERNAL_CALL_Internal_SetMousePosition(this, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_SetMousePosition(Event self, ref Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetMousePosition(out Vector2 value);

		private void Internal_SetMouseDelta(Vector2 value)
		{
			Event.INTERNAL_CALL_Internal_SetMouseDelta(this, ref value);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_Internal_SetMouseDelta(Event self, ref Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Internal_GetMouseDelta(out Vector2 value);

		/// <summary>
		///   <para>Which mouse button was pressed.</para>
		/// </summary>
		public extern int button { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Which modifier keys are held down.</para>
		/// </summary>
		public extern EventModifiers modifiers { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float pressure { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>How many consecutive mouse clicks have we received.</para>
		/// </summary>
		public extern int clickCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The character typed.</para>
		/// </summary>
		public extern char character { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The name of an ExecuteCommand or ValidateCommand Event.</para>
		/// </summary>
		public extern string commandName { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The raw key code for keyboard events.</para>
		/// </summary>
		public extern KeyCode keyCode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_SetNativeEvent(IntPtr ptr);

		/// <summary>
		///   <para>Use this event.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Use();

		/// <summary>
		///   <para>Get the next queued [Event] from the event system.</para>
		/// </summary>
		/// <param name="outEvent">Next Event.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool PopEvent(Event outEvent);

		/// <summary>
		///   <para>Returns the current number of events that are stored in the event queue.</para>
		/// </summary>
		/// <returns>
		///   <para>Current number of events currently in the event queue.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetEventCount();
	}
}
