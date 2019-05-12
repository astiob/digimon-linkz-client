using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/AnchoredJoint2D.h")]
	public class AnchoredJoint2D : Joint2D
	{
		public Vector2 anchor
		{
			get
			{
				Vector2 result;
				this.get_anchor_Injected(out result);
				return result;
			}
			set
			{
				this.set_anchor_Injected(ref value);
			}
		}

		public Vector2 connectedAnchor
		{
			get
			{
				Vector2 result;
				this.get_connectedAnchor_Injected(out result);
				return result;
			}
			set
			{
				this.set_connectedAnchor_Injected(ref value);
			}
		}

		public extern bool autoConfigureConnectedAnchor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_anchor_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_anchor_Injected(ref Vector2 value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_connectedAnchor_Injected(out Vector2 ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_connectedAnchor_Injected(ref Vector2 value);
	}
}
