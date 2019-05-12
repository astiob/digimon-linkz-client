using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("Runtime/Graphics/Mesh/SkinnedMeshRenderer.h")]
	public class SkinnedMeshRenderer : Renderer
	{
		public extern Transform[] bones { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern SkinQuality quality { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool updateWhenOffscreen { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern Transform rootBone { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("Mesh")]
		public extern Mesh sharedMesh { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("SkinnedMeshMotionVectors")]
		public extern bool skinnedMotionVectors { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern float GetBlendShapeWeight(int index);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetBlendShapeWeight(int index, float value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeMesh(Mesh mesh);

		[FreeFunction(Name = "SkinnedMeshRendererScripting::GetLocalAABB", HasExplicitThis = true)]
		private Bounds GetLocalAABB()
		{
			Bounds result;
			this.GetLocalAABB_Injected(out result);
			return result;
		}

		private void SetLocalAABB(Bounds b)
		{
			this.SetLocalAABB_Injected(ref b);
		}

		public Bounds localBounds
		{
			get
			{
				return this.GetLocalAABB();
			}
			set
			{
				this.SetLocalAABB(value);
			}
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLocalAABB_Injected(out Bounds ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLocalAABB_Injected(ref Bounds b);
	}
}
