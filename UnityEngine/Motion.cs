using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Base class for AnimationClips and BlendTrees.</para>
	/// </summary>
	public class Motion : Object
	{
		public extern float averageDuration { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern float averageAngularSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public Vector3 averageSpeed
		{
			get
			{
				Vector3 result;
				this.INTERNAL_get_averageSpeed(out result);
				return result;
			}
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void INTERNAL_get_averageSpeed(out Vector3 value);

		public extern float apparentSpeed { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool isLooping { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool legacy { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public extern bool isHumanMotion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[Obsolete("ValidateIfRetargetable is not supported anymore. Use isHumanMotion instead.", true)]
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool ValidateIfRetargetable(bool val);

		[Obsolete("isAnimatorMotion is not supported anymore. Use !legacy instead.", true)]
		public extern bool isAnimatorMotion { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
