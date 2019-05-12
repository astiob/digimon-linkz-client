using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Physics2D/FixedJoint2D.h")]
	public sealed class FixedJoint2D : AnchoredJoint2D
	{
		public extern float dampingRatio { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float frequency { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern float referenceAngle { [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
