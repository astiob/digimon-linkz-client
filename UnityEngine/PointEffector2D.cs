using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Applies forces to attract/repulse against a point.</para>
	/// </summary>
	public sealed class PointEffector2D : Effector2D
	{
		/// <summary>
		///   <para>The magnitude of the force to be applied.</para>
		/// </summary>
		public extern float forceMagnitude { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The variation of the magnitude of the force to be applied.</para>
		/// </summary>
		public extern float forceVariation { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The scale applied to the calculated distance between source and target.</para>
		/// </summary>
		public extern float distanceScale { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The linear drag to apply to rigid-bodies.</para>
		/// </summary>
		public extern float drag { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The angular drag to apply to rigid-bodies.</para>
		/// </summary>
		public extern float angularDrag { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The source which is used to calculate the centroid point of the effector.  The distance from the target is defined from this point.</para>
		/// </summary>
		public extern EffectorSelection2D forceSource { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The target for where the effector applies any force.</para>
		/// </summary>
		public extern EffectorSelection2D forceTarget { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The mode used to apply the effector force.</para>
		/// </summary>
		public extern EffectorForceMode2D forceMode { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
