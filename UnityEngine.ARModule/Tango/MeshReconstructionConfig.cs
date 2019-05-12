using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.XR.Tango
{
	[NativeHeader("ARScriptingClasses.h")]
	[UsedByNativeCode]
	internal struct MeshReconstructionConfig
	{
		public double resolution;

		public double minDepth;

		public double maxDepth;

		public int minNumVertices;

		public bool useParallelIntegration;

		public bool generateColor;

		public bool useSpaceClearing;

		public UpdateMethod updateMethod;

		public static MeshReconstructionConfig GetDefault()
		{
			return new MeshReconstructionConfig
			{
				resolution = 0.03,
				minDepth = 0.6,
				maxDepth = 3.5,
				useParallelIntegration = false,
				generateColor = true,
				useSpaceClearing = false,
				minNumVertices = 1,
				updateMethod = UpdateMethod.Traversal
			};
		}
	}
}
