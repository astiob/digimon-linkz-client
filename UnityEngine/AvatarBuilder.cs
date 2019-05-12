using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Class to build avatars from user scripts.</para>
	/// </summary>
	public sealed class AvatarBuilder
	{
		/// <summary>
		///   <para>Create a humanoid avatar.</para>
		/// </summary>
		/// <param name="go">Root object of your transform hierachy.</param>
		/// <param name="monoHumanDescription">Description of the avatar.</param>
		public static Avatar BuildHumanAvatar(GameObject go, HumanDescription monoHumanDescription)
		{
			if (go == null)
			{
				throw new NullReferenceException();
			}
			return AvatarBuilder.BuildHumanAvatarMono(go, monoHumanDescription);
		}

		private static Avatar BuildHumanAvatarMono(GameObject go, HumanDescription monoHumanDescription)
		{
			return AvatarBuilder.INTERNAL_CALL_BuildHumanAvatarMono(go, ref monoHumanDescription);
		}

		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Avatar INTERNAL_CALL_BuildHumanAvatarMono(GameObject go, ref HumanDescription monoHumanDescription);

		/// <summary>
		///   <para>Create a new generic avatar.</para>
		/// </summary>
		/// <param name="go">Root object of your transform hierarchy.</param>
		/// <param name="rootMotionTransformName">Transform name of the root motion transform. If empty no root motion is defined and you must take care of avatar movement yourself.</param>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Avatar BuildGenericAvatar(GameObject go, string rootMotionTransformName);
	}
}
