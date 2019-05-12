using System;

namespace UnityEngine.XR.Tango
{
	internal struct SegmentGenerationResult
	{
		public GridIndex gridIndex;

		public MeshFilter meshFilter;

		public MeshCollider meshCollider;

		public bool success;

		public double elapsedTimeSeconds;
	}
}
