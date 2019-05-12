using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[NativeHeader("Modules/Terrain/Public/Tree.h")]
	public sealed class Tree : Component
	{
		[NativeProperty("TreeData")]
		public extern ScriptableObject data { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public extern bool hasSpeedTreeWind { [NativeMethod("HasSpeedTreeWind")] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
