using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>The ClothSkinningCoefficient struct is used to set up how a Cloth component is allowed to move with respect to the SkinnedMeshRenderer it is attached to.</para>
	/// </summary>
	public struct ClothSkinningCoefficient
	{
		/// <summary>
		///   <para>Distance a vertex is allowed to travel from the skinned mesh vertex position.</para>
		/// </summary>
		public float maxDistance;

		/// <summary>
		///   <para>Definition of a sphere a vertex is not allowed to enter. This allows collision against the animated cloth.</para>
		/// </summary>
		public float collisionSphereDistance;
	}
}
