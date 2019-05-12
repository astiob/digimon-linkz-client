using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Joint is the base class for all joints.</para>
	/// </summary>
	public class Joint : Component
	{
		/// <summary>
		///   <para>A reference to another rigidbody this joint connects to.</para>
		/// </summary>
		public extern Rigidbody connectedBody { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The Direction of the axis around which the body is constrained.</para>
		/// </summary>
		public Vector3 axis
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_axis(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_axis(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_axis(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_axis(ref Vector3 value);

		/// <summary>
		///   <para>The Position of the anchor around which the joints motion is constrained.</para>
		/// </summary>
		public Vector3 anchor
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_anchor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_anchor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_anchor(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_anchor(ref Vector3 value);

		/// <summary>
		///   <para>Position of the anchor relative to the connected Rigidbody.</para>
		/// </summary>
		public Vector3 connectedAnchor
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_connectedAnchor(out result);
				return result;
			}
			set
			{
				this.INTERNAL_set_connectedAnchor(ref value);
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_connectedAnchor(out Vector3 value);

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_set_connectedAnchor(ref Vector3 value);

		/// <summary>
		///   <para>Should the connectedAnchor be calculated automatically?</para>
		/// </summary>
		public extern bool autoConfigureConnectedAnchor { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The force that needs to be applied for this joint to break.</para>
		/// </summary>
		public extern float breakForce { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The torque that needs to be applied for this joint to break.</para>
		/// </summary>
		public extern float breakTorque { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Enable collision between bodies connected with the joint.</para>
		/// </summary>
		public extern bool enableCollision { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Toggle preprocessing for this joint.</para>
		/// </summary>
		public extern bool enablePreprocessing { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
