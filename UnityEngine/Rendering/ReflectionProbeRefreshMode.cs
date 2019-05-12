using System;

namespace UnityEngine.Rendering
{
	/// <summary>
	///   <para>An enum describing the way a realtime reflection probe refreshes in the Player.</para>
	/// </summary>
	public enum ReflectionProbeRefreshMode
	{
		/// <summary>
		///   <para>Causes the probe to update only on the first frame it becomes visible. The probe will no longer update automatically, however you may subsequently use RenderProbe to refresh the probe
		///
		/// See Also: ReflectionProbe.RenderProbe.</para>
		/// </summary>
		OnAwake,
		/// <summary>
		///   <para>Causes Unity to update the probe's cubemap every frame. 
		/// Note that updating a probe is very costly. Setting this option on too many probes could have a significant negative effect on frame rate. Use time-slicing to help improve performance.
		///
		/// See Also: ReflectionProbeTimeSlicingMode.</para>
		/// </summary>
		EveryFrame,
		/// <summary>
		///   <para>Using this option indicates that the probe will never be automatically updated by Unity. This is useful if you wish to completely control the probe refresh behavior via scripting.
		///
		/// See Also: ReflectionProbe.RenderProbe.</para>
		/// </summary>
		ViaScripting
	}
}
