using System;
using System.Runtime.CompilerServices;
using UnityEngine.Internal;

namespace UnityEngine
{
	/// <summary>
	///   <para>Interface into functionality unique to handheld devices.</para>
	/// </summary>
	public sealed class Handheld
	{
		/// <summary>
		///   <para>Plays a full-screen movie.</para>
		/// </summary>
		/// <param name="path">Filesystem path to the movie file.</param>
		/// <param name="bgColor">Background color.</param>
		/// <param name="controlMode">How the playback controls are to be displayed.</param>
		/// <param name="scalingMode">How the movie is to be scaled to fit the screen.</param>
		public static bool PlayFullScreenMovie(string path, [DefaultValue("Color.black")] Color bgColor, [DefaultValue("FullScreenMovieControlMode.Full")] FullScreenMovieControlMode controlMode, [DefaultValue("FullScreenMovieScalingMode.AspectFit")] FullScreenMovieScalingMode scalingMode)
		{
			return Handheld.INTERNAL_CALL_PlayFullScreenMovie(path, ref bgColor, controlMode, scalingMode);
		}

		/// <summary>
		///   <para>Plays a full-screen movie.</para>
		/// </summary>
		/// <param name="path">Filesystem path to the movie file.</param>
		/// <param name="bgColor">Background color.</param>
		/// <param name="controlMode">How the playback controls are to be displayed.</param>
		/// <param name="scalingMode">How the movie is to be scaled to fit the screen.</param>
		[ExcludeFromDocs]
		public static bool PlayFullScreenMovie(string path, Color bgColor, FullScreenMovieControlMode controlMode)
		{
			FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
			return Handheld.INTERNAL_CALL_PlayFullScreenMovie(path, ref bgColor, controlMode, scalingMode);
		}

		/// <summary>
		///   <para>Plays a full-screen movie.</para>
		/// </summary>
		/// <param name="path">Filesystem path to the movie file.</param>
		/// <param name="bgColor">Background color.</param>
		/// <param name="controlMode">How the playback controls are to be displayed.</param>
		/// <param name="scalingMode">How the movie is to be scaled to fit the screen.</param>
		[ExcludeFromDocs]
		public static bool PlayFullScreenMovie(string path, Color bgColor)
		{
			FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
			FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
			return Handheld.INTERNAL_CALL_PlayFullScreenMovie(path, ref bgColor, controlMode, scalingMode);
		}

		/// <summary>
		///   <para>Plays a full-screen movie.</para>
		/// </summary>
		/// <param name="path">Filesystem path to the movie file.</param>
		/// <param name="bgColor">Background color.</param>
		/// <param name="controlMode">How the playback controls are to be displayed.</param>
		/// <param name="scalingMode">How the movie is to be scaled to fit the screen.</param>
		[ExcludeFromDocs]
		public static bool PlayFullScreenMovie(string path)
		{
			FullScreenMovieScalingMode scalingMode = FullScreenMovieScalingMode.AspectFit;
			FullScreenMovieControlMode controlMode = FullScreenMovieControlMode.Full;
			Color black = Color.black;
			return Handheld.INTERNAL_CALL_PlayFullScreenMovie(path, ref black, controlMode, scalingMode);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool INTERNAL_CALL_PlayFullScreenMovie(string path, ref Color bgColor, FullScreenMovieControlMode controlMode, FullScreenMovieScalingMode scalingMode);

		/// <summary>
		///   <para>Triggers device vibration.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Vibrate();

		/// <summary>
		///   <para>Determines whether or not a 32-bit display buffer will be used.</para>
		/// </summary>
		[Obsolete("Property Handheld.use32BitDisplayBuffer has been deprecated. Modifying it has no effect, use PlayerSettings instead.")]
		public static extern bool use32BitDisplayBuffer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void SetActivityIndicatorStyleImpl(int style);

		/// <summary>
		///   <para>Sets the desired activity indicator style.</para>
		/// </summary>
		/// <param name="style"></param>
		public static void SetActivityIndicatorStyle(AndroidActivityIndicatorStyle style)
		{
			Handheld.SetActivityIndicatorStyleImpl((int)style);
		}

		/// <summary>
		///   <para>Gets the current activity indicator style.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetActivityIndicatorStyle();

		/// <summary>
		///   <para>Starts os activity indicator.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StartActivityIndicator();

		/// <summary>
		///   <para>Stops os activity indicator.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void StopActivityIndicator();

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearShaderCache();
	}
}
