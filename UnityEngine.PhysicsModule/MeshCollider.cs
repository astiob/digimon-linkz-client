using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[RequiredByNativeCode]
	public sealed class MeshCollider : Collider
	{
		public extern Mesh sharedMesh { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool convex { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern MeshColliderCookingOptions cookingOptions { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public bool inflateMesh
		{
			get
			{
				return (this.cookingOptions & MeshColliderCookingOptions.InflateConvexMesh) != MeshColliderCookingOptions.None;
			}
			set
			{
				MeshColliderCookingOptions meshColliderCookingOptions = this.cookingOptions & ~MeshColliderCookingOptions.InflateConvexMesh;
				if (value)
				{
					meshColliderCookingOptions |= MeshColliderCookingOptions.InflateConvexMesh;
				}
				this.cookingOptions = meshColliderCookingOptions;
			}
		}

		public extern float skinWidth { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Configuring smooth sphere collisions is no longer needed. PhysX3 has a better behaviour in place.")]
		public bool smoothSphereCollisions
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
	}
}
