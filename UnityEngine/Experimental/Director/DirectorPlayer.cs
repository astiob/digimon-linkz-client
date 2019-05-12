using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Experimental.Director
{
	/// <summary>
	///   <para>The DirectorPlayer is the base class for all components capable of playing a Experimental.Director.Playable tree.</para>
	/// </summary>
	public class DirectorPlayer : Behaviour
	{
		public void Play(Playable playable, object customData)
		{
			this.PlayInternal(playable, customData);
		}

		/// <summary>
		///   <para>Starts playing a Experimental.Director.Playable tree.</para>
		/// </summary>
		/// <param name="playable">The root Experimental.Director.Playable in the tree.</param>
		public void Play(Playable playable)
		{
			this.PlayInternal(playable, null);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void PlayInternal(Playable playable, object customData);

		/// <summary>
		///   <para>Stop the playback of the Player and Experimental.Director.Playable.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Stop();

		/// <summary>
		///   <para>Sets the Player's local time.</para>
		/// </summary>
		/// <param name="time">The new local time.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTime(double time);

		/// <summary>
		///   <para>Returns the Player's current local time.</para>
		/// </summary>
		/// <returns>
		///   <para>Current local time.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double GetTime();

		/// <summary>
		///   <para>Specifies the way the Player's will increment when it is playing.</para>
		/// </summary>
		/// <param name="mode"></param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetTimeUpdateMode(DirectorUpdateMode mode);

		/// <summary>
		///   <para>Returns the current Experimental.Director.DirectorUpdateMode.</para>
		/// </summary>
		/// <returns>
		///   <para>Current update mode for this player.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern DirectorUpdateMode GetTimeUpdateMode();
	}
}
