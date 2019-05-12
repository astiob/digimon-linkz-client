using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
	[RequireComponent(typeof(Transform))]
	[NativeHeader("Runtime/Camera/HaloManager.h")]
	internal sealed class Halo : Behaviour
	{
	}
}
