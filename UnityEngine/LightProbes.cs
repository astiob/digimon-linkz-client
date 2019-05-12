using System;
using System.Runtime.CompilerServices;
using UnityEngine.Rendering;

namespace UnityEngine
{
	/// <summary>
	///   <para>Stores light probes for the scene.</para>
	/// </summary>
	public sealed class LightProbes : Object
	{
		public static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe)
		{
			LightProbes.INTERNAL_CALL_GetInterpolatedProbe(ref position, renderer, out probe);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_GetInterpolatedProbe(ref Vector3 position, Renderer renderer, out SphericalHarmonicsL2 probe);

		/// <summary>
		///   <para>Positions of the baked light probes (Read Only).</para>
		/// </summary>
		public extern Vector3[] positions { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Coefficients of baked light probes.</para>
		/// </summary>
		public extern SphericalHarmonicsL2[] bakedProbes { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The number of light probes (Read Only).</para>
		/// </summary>
		public extern int count { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>The number of cells space is divided into (Read Only).</para>
		/// </summary>
		public extern int cellCount { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("GetInterpolatedLightProbe has been deprecated. Please use the static GetInterpolatedProbe instead.", true)]
		public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients)
		{
		}

		[Obsolete("coefficients property has been deprecated. Please use bakedProbes instead.", true)]
		public float[] coefficients
		{
			get
			{
				return new float[0];
			}
			set
			{
			}
		}
	}
}
