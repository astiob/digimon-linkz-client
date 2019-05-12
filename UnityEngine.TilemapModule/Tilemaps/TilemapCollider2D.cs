using System;
using UnityEngine.Bindings;

namespace UnityEngine.Tilemaps
{
	[NativeType(Header = "Modules/Tilemap/Public/TilemapCollider2D.h")]
	[RequireComponent(typeof(Tilemap))]
	public sealed class TilemapCollider2D : Collider2D
	{
	}
}
