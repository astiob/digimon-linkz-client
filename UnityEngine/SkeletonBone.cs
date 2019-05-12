using System;

namespace UnityEngine
{
	/// <summary>
	///   <para>Details of the Transform name mapped to a model's skeleton bone and its default position and rotation in the T-pose.</para>
	/// </summary>
	public struct SkeletonBone
	{
		/// <summary>
		///   <para>The name of the Transform mapped to the bone.</para>
		/// </summary>
		public string name;

		/// <summary>
		///   <para>The T-pose position of the bone in local space.</para>
		/// </summary>
		public Vector3 position;

		/// <summary>
		///   <para>The T-pose rotation of the bone in local space.</para>
		/// </summary>
		public Quaternion rotation;

		/// <summary>
		///   <para>The T-pose scaling of the bone in local space.</para>
		/// </summary>
		public Vector3 scale;

		public int transformModified;
	}
}
