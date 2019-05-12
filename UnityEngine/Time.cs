using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>The interface to get time information from Unity.</para>
	/// </summary>
	public sealed class Time
	{
		/// <summary>
		///   <para>The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game.</para>
		/// </summary>
		public static extern float time { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The time this frame has started (Read Only). This is the time in seconds since the last level has been loaded.</para>
		/// </summary>
		public static extern float timeSinceLevelLoad { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The time in seconds it took to complete the last frame (Read Only).</para>
		/// </summary>
		public static extern float deltaTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The time the latest MonoBehaviour.FixedUpdate has started (Read Only). This is the time in seconds since the start of the game.</para>
		/// </summary>
		public static extern float fixedTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The timeScale-independant time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game.</para>
		/// </summary>
		public static extern float unscaledTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The timeScale-independent time in seconds it took to complete the last frame (Read Only).</para>
		/// </summary>
		public static extern float unscaledDeltaTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The interval in seconds at which physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate) are performed.</para>
		/// </summary>
		public static extern float fixedDeltaTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The maximum time a frame can take. Physics and other fixed frame rate updates (like MonoBehaviour's MonoBehaviour.FixedUpdate).</para>
		/// </summary>
		public static extern float maximumDeltaTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>A smoothed out Time.deltaTime (Read Only).</para>
		/// </summary>
		public static extern float smoothDeltaTime { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The scale at which the time is passing. This can be used for slow motion effects.</para>
		/// </summary>
		public static extern float timeScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The total number of frames that have passed (Read Only).</para>
		/// </summary>
		public static extern int frameCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern int renderedFrameCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The real time in seconds since the game started (Read Only).</para>
		/// </summary>
		public static extern float realtimeSinceStartup { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Slows game playback time to allow screenshots to be saved between frames.</para>
		/// </summary>
		public static extern int captureFramerate { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
