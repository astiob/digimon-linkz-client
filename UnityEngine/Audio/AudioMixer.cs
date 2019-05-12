using System;
using System.Runtime.CompilerServices;

namespace UnityEngine.Audio
{
	/// <summary>
	///   <para>AudioMixer asset.</para>
	/// </summary>
	public class AudioMixer : Object
	{
		internal AudioMixer()
		{
		}

		/// <summary>
		///   <para>Routing target.</para>
		/// </summary>
		public extern AudioMixerGroup outputAudioMixerGroup { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Connected groups in the mixer form a path from the mixer's master group to the leaves. This path has the format "Master GroupChild of Master GroupGrandchild of Master Group", so to find the grandchild group in this example, a valid search string would be for instance "randchi" which would return exactly one group while "hild" or "oup/" would return 2 different groups.</para>
		/// </summary>
		/// <param name="subPath">Sub-string of the paths to be matched.</param>
		/// <returns>
		///   <para>Groups in the mixer whose paths match the specified search path.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AudioMixerGroup[] FindMatchingGroups(string subPath);

		/// <summary>
		///   <para>The name must be an exact match.</para>
		/// </summary>
		/// <param name="name">Name of snapshot object to be returned.</param>
		/// <returns>
		///   <para>The snapshot identified by the name.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern AudioMixerSnapshot FindSnapshot(string name);

		private void TransitionToSnapshot(AudioMixerSnapshot snapshot, float timeToReach)
		{
			if (snapshot == null)
			{
				throw new ArgumentException("null Snapshot passed to AudioMixer.TransitionToSnapshot of AudioMixer '" + base.name + "'");
			}
			if (snapshot.audioMixer != this)
			{
				throw new ArgumentException(string.Concat(new string[]
				{
					"Snapshot '",
					snapshot.name,
					"' passed to AudioMixer.TransitionToSnapshot is not a snapshot from AudioMixer '",
					base.name,
					"'"
				}));
			}
			snapshot.TransitionTo(timeToReach);
		}

		/// <summary>
		///   <para>Transitions to a weighted mixture of the snapshots specified. This can be used for games that specify the game state as a continuum between states or for interpolating snapshots from a triangulated map location.</para>
		/// </summary>
		/// <param name="snapshots">The set of snapshots to be mixed.</param>
		/// <param name="weights">The mix weights for the snapshots specified.</param>
		/// <param name="timeToReach">Relative time after which the mixture should be reached from any current state.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void TransitionToSnapshots(AudioMixerSnapshot[] snapshots, float[] weights, float timeToReach);

		/// <summary>
		///   <para>Sets the value of the exposed parameter specified. When a parameter is exposed, it is not controlled by mixer snapshots and can therefore only be changed via this function.</para>
		/// </summary>
		/// <param name="name">Name of exposed parameter.</param>
		/// <param name="value">New value of exposed parameter.</param>
		/// <returns>
		///   <para>Returns false if the exposed parameter was not found or snapshots are currently being edited.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool SetFloat(string name, float value);

		/// <summary>
		///   <para>Resets an exposed parameter to its initial value.</para>
		/// </summary>
		/// <param name="name">Exposed parameter.</param>
		/// <returns>
		///   <para>Returns false if the parameter was not found or could not be set.</para>
		/// </returns>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ClearFloat(string name);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool GetFloat(string name, out float value);
	}
}
