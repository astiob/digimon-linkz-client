using System;
using System.Runtime.CompilerServices;

namespace UnityEngine
{
	/// <summary>
	///   <para>A heightmap based collider.</para>
	/// </summary>
	public sealed class TerrainCollider : Collider
	{
		/// <summary>
		///   <para>The terrain that stores the heightmap.</para>
		/// </summary>
		public extern TerrainData terrainData { [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] get; [WrapperlessIcall] [MethodImpl(MethodImplOptions.InternalCall)] set; }
	}
}
