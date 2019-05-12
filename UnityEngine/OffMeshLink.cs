using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>Link allowing movement outside the planar navigation mesh.</para>
	/// </summary>
	public sealed class OffMeshLink : Component
	{
		/// <summary>
		///   <para>Is link active.</para>
		/// </summary>
		public extern bool activated { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Is link occupied. (Read Only)</para>
		/// </summary>
		public extern bool occupied { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		/// <summary>
		///   <para>Modify pathfinding cost for the link.</para>
		/// </summary>
		public extern float costOverride { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Can link be traversed in both directions.</para>
		/// </summary>
		public extern bool biDirectional { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Explicitly update the link endpoints.</para>
		/// </summary>
		[WrapperlessIcall]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void UpdatePositions();

		/// <summary>
		///   <para>NavMeshLayer for this OffMeshLink component.</para>
		/// </summary>
		[Obsolete("Use area instead.")]
		public extern int navMeshLayer { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>NavMesh area index for this OffMeshLink component.</para>
		/// </summary>
		public extern int area { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>Automatically update endpoints.</para>
		/// </summary>
		public extern bool autoUpdatePositions { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The transform representing link start position.</para>
		/// </summary>
		public extern Transform startTransform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		/// <summary>
		///   <para>The transform representing link end position.</para>
		/// </summary>
		public extern Transform endTransform { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
