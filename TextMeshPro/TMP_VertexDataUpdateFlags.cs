using System;

namespace TMPro
{
	public enum TMP_VertexDataUpdateFlags
	{
		None,
		Vertices,
		Uv0,
		Uv2 = 4,
		Uv4 = 8,
		Colors32 = 16,
		All = 255
	}
}
