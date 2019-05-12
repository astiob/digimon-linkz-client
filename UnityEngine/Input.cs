using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface into the Input system.</para>
	/// </summary>
	public sealed class Input
	{
		private static Gyroscope m_MainGyro;

		private static LocationService locationServiceInstance;

		private static Compass compassInstance;

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int mainGyroIndex_Internal();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyInt(int key);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyUpInt(int key);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyUpString(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyDownInt(int key);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool GetKeyDownString(string name);

		/// <summary>
		///   <para>Returns the value of the virtual axis identified by axisName.</para>
		/// </summary>
		/// <param name="axisName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAxis(string axisName);

		/// <summary>
		///   <para>Returns the value of the virtual axis identified by axisName with no smoothing filtering applied.</para>
		/// </summary>
		/// <param name="axisName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern float GetAxisRaw(string axisName);

		/// <summary>
		///   <para>Returns true while the virtual button identified by buttonName is held down.</para>
		/// </summary>
		/// <param name="buttonName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButton(string buttonName);

		/// <summary>
		///   <para>This property controls if input sensors should be compensated for screen orientation.</para>
		/// </summary>
		public static extern bool compensateSensors { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("isGyroAvailable property is deprecated. Please use SystemInfo.supportsGyroscope instead.")]
		public static extern bool isGyroAvailable { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns default gyroscope.</para>
		/// </summary>
		public static Gyroscope gyro
		{
			get
			{
				if (Input.m_MainGyro == null)
				{
					Input.m_MainGyro = new Gyroscope(Input.mainGyroIndex_Internal());
				}
				return Input.m_MainGyro;
			}
		}

		/// <summary>
		///   <para>Returns true during the frame the user pressed down the virtual button identified by buttonName.</para>
		/// </summary>
		/// <param name="buttonName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButtonDown(string buttonName);

		/// <summary>
		///   <para>Returns true the first frame the user releases the virtual button identified by buttonName.</para>
		/// </summary>
		/// <param name="buttonName"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetButtonUp(string buttonName);

		/// <summary>
		///   <para>Returns true while the user holds down the key identified by name. Think auto fire.</para>
		/// </summary>
		/// <param name="name"></param>
		public static bool GetKey(string name)
		{
			return Input.GetKeyString(name);
		}

		/// <summary>
		///   <para>Returns true while the user holds down the key identified by the key KeyCode enum parameter.</para>
		/// </summary>
		/// <param name="key"></param>
		public static bool GetKey(KeyCode key)
		{
			return Input.GetKeyInt((int)key);
		}

		/// <summary>
		///   <para>Returns true during the frame the user starts pressing down the key identified by name.</para>
		/// </summary>
		/// <param name="name"></param>
		public static bool GetKeyDown(string name)
		{
			return Input.GetKeyDownString(name);
		}

		/// <summary>
		///   <para>Returns true during the frame the user starts pressing down the key identified by the key KeyCode enum parameter.</para>
		/// </summary>
		/// <param name="key"></param>
		public static bool GetKeyDown(KeyCode key)
		{
			return Input.GetKeyDownInt((int)key);
		}

		/// <summary>
		///   <para>Returns true during the frame the user releases the key identified by name.</para>
		/// </summary>
		/// <param name="name"></param>
		public static bool GetKeyUp(string name)
		{
			return Input.GetKeyUpString(name);
		}

		/// <summary>
		///   <para>Returns true during the frame the user releases the key identified by the key KeyCode enum parameter.</para>
		/// </summary>
		/// <param name="key"></param>
		public static bool GetKeyUp(KeyCode key)
		{
			return Input.GetKeyUpInt((int)key);
		}

		/// <summary>
		///   <para>Returns an array of strings describing the connected joysticks.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetJoystickNames();

		/// <summary>
		///   <para>Returns whether the given mouse button is held down.</para>
		/// </summary>
		/// <param name="button"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButton(int button);

		/// <summary>
		///   <para>Returns true during the frame the user pressed the given mouse button.</para>
		/// </summary>
		/// <param name="button"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonDown(int button);

		/// <summary>
		///   <para>Returns true during the frame the user releases the given mouse button.</para>
		/// </summary>
		/// <param name="button"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool GetMouseButtonUp(int button);

		/// <summary>
		///   <para>Resets all input. After ResetInputAxes all axes return to 0 and all buttons return to 0 for one frame.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResetInputAxes();

		/// <summary>
		///   <para>The current mouse position in pixel coordinates. (Read Only)</para>
		/// </summary>
		public static Vector3 mousePosition
		{
			get
			{
				Vector3 result;
				Input.INTERNAL_get_mousePosition(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_mousePosition(out Vector3 value);

		/// <summary>
		///   <para>The current mouse scroll delta. (Read Only)</para>
		/// </summary>
		public static Vector2 mouseScrollDelta
		{
			get
			{
				Vector2 result;
				Input.INTERNAL_get_mouseScrollDelta(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_mouseScrollDelta(out Vector2 value);

		public static extern bool mousePresent { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Enables/Disables mouse simulation with touches. By default this option is enabled.</para>
		/// </summary>
		public static extern bool simulateMouseWithTouches { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is any key or mouse button currently held down? (Read Only)</para>
		/// </summary>
		public static extern bool anyKey { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns true the first frame the user hits any key or mouse button. (Read Only)</para>
		/// </summary>
		public static extern bool anyKeyDown { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns the keyboard input entered this frame. (Read Only)</para>
		/// </summary>
		public static extern string inputString { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Last measured linear acceleration of a device in three-dimensional space. (Read Only)</para>
		/// </summary>
		public static Vector3 acceleration
		{
			get
			{
				Vector3 result;
				Input.INTERNAL_get_acceleration(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_acceleration(out Vector3 value);

		/// <summary>
		///   <para>Returns list of acceleration measurements which occurred during the last frame. (Read Only) (Allocates temporary variables).</para>
		/// </summary>
		public static AccelerationEvent[] accelerationEvents
		{
			get
			{
				int accelerationEventCount = Input.accelerationEventCount;
				AccelerationEvent[] array = new AccelerationEvent[accelerationEventCount];
				for (int i = 0; i < accelerationEventCount; i++)
				{
					array[i] = Input.GetAccelerationEvent(i);
				}
				return array;
			}
		}

		/// <summary>
		///   <para>Returns specific acceleration measurement which occurred during last frame. (Does not allocate temporary variables).</para>
		/// </summary>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern AccelerationEvent GetAccelerationEvent(int index);

		/// <summary>
		///   <para>Number of acceleration measurements which occurred during last frame.</para>
		/// </summary>
		public static extern int accelerationEventCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns list of objects representing status of all touches during last frame. (Read Only) (Allocates temporary variables).</para>
		/// </summary>
		public static Touch[] touches
		{
			get
			{
				int touchCount = Input.touchCount;
				Touch[] array = new Touch[touchCount];
				for (int i = 0; i < touchCount; i++)
				{
					array[i] = Input.GetTouch(i);
				}
				return array;
			}
		}

		/// <summary>
		///   <para>Returns object representing status of a specific touch. (Does not allocate temporary variables).</para>
		/// </summary>
		/// <param name="index"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Touch GetTouch(int index);

		/// <summary>
		///   <para>Number of touches. Guaranteed not to change throughout the frame. (Read Only)</para>
		/// </summary>
		public static extern int touchCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Property indicating whether keypresses are eaten by a textinput if it has focus (default true).</para>
		/// </summary>
		[Obsolete("eatKeyPressOnTextFieldFocus property is deprecated, and only provided to support legacy behavior.")]
		public static extern bool eatKeyPressOnTextFieldFocus { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Bool value which let's users check if touch pressure is supported.</para>
		/// </summary>
		public static extern bool touchPressureSupported { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Returns whether the device on which application is currently running supports touch input.</para>
		/// </summary>
		public static bool touchSupported
		{
			get
			{
				return true;
			}
		}

		/// <summary>
		///   <para>Property indicating whether the system handles multiple touches.</para>
		/// </summary>
		public static extern bool multiTouchEnabled { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Property for accessing device location (handheld devices only). (Read Only)</para>
		/// </summary>
		public static LocationService location
		{
			get
			{
				if (Input.locationServiceInstance == null)
				{
					Input.locationServiceInstance = new LocationService();
				}
				return Input.locationServiceInstance;
			}
		}

		/// <summary>
		///   <para>Property for accessing compass (handheld devices only). (Read Only)</para>
		/// </summary>
		public static Compass compass
		{
			get
			{
				if (Input.compassInstance == null)
				{
					Input.compassInstance = new Compass();
				}
				return Input.compassInstance;
			}
		}

		/// <summary>
		///   <para>Device physical orientation as reported by OS. (Read Only)</para>
		/// </summary>
		public static extern DeviceOrientation deviceOrientation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("Use ps3 move API instead", true)]
		public static Quaternion GetRotation(int deviceID)
		{
			return Quaternion.identity;
		}

		[Obsolete("Use ps3 move API instead", true)]
		public static Vector3 GetPosition(int deviceID)
		{
			return Vector3.zero;
		}

		/// <summary>
		///   <para>Controls enabling and disabling of IME input composition.</para>
		/// </summary>
		public static extern IMECompositionMode imeCompositionMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The current IME composition string being typed by the user.</para>
		/// </summary>
		public static extern string compositionString { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Does the user have an IME keyboard input source selected?</para>
		/// </summary>
		public static extern bool imeIsSelected { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The current text input position used by IMEs to open windows.</para>
		/// </summary>
		public static Vector2 compositionCursorPos
		{
			get
			{
				Vector2 result;
				Input.INTERNAL_get_compositionCursorPos(out result);
				return result;
			}
			set
			{
				Input.INTERNAL_set_compositionCursorPos(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_compositionCursorPos(out Vector2 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_compositionCursorPos(ref Vector2 value);
	}
}
