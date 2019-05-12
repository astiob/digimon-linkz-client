using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	[UsedByNativeCode]
	public struct ScriptableCullingParameters
	{
		private int m_IsOrthographic;

		private LODParameters m_LodParameters;

		private ScriptableCullingParameters.<m_CullingPlanes>__FixedBuffer4 m_CullingPlanes;

		private int m_CullingPlaneCount;

		private int m_CullingMask;

		private long m_SceneMask;

		private ScriptableCullingParameters.<m_LayerFarCullDistances>__FixedBuffer5 m_LayerFarCullDistances;

		private int m_LayerCull;

		private Matrix4x4 m_CullingMatrix;

		private Vector3 m_Position;

		private float m_shadowDistance;

		private int m_CullingFlags;

		private ReflectionProbeSortOptions m_ReflectionProbeSortOptions;

		private CameraProperties m_CameraProperties;

		public Matrix4x4 cullStereoView;

		public Matrix4x4 cullStereoProj;

		public float cullStereoSeparation;

		private int padding2;

		public int cullingPlaneCount
		{
			get
			{
				return this.m_CullingPlaneCount;
			}
			set
			{
				if (value < 0 || value > 10)
				{
					throw new IndexOutOfRangeException("Invalid plane count (0 <= count <= 10)");
				}
				this.m_CullingPlaneCount = value;
			}
		}

		public bool isOrthographic
		{
			get
			{
				return Convert.ToBoolean(this.m_IsOrthographic);
			}
			set
			{
				this.m_IsOrthographic = Convert.ToInt32(value);
			}
		}

		public LODParameters lodParameters
		{
			get
			{
				return this.m_LodParameters;
			}
			set
			{
				this.m_LodParameters = value;
			}
		}

		public int cullingMask
		{
			get
			{
				return this.m_CullingMask;
			}
			set
			{
				this.m_CullingMask = value;
			}
		}

		public long sceneMask
		{
			get
			{
				return this.m_SceneMask;
			}
			set
			{
				this.m_SceneMask = value;
			}
		}

		public int layerCull
		{
			get
			{
				return this.m_LayerCull;
			}
			set
			{
				this.m_LayerCull = value;
			}
		}

		public Matrix4x4 cullingMatrix
		{
			get
			{
				return this.m_CullingMatrix;
			}
			set
			{
				this.m_CullingMatrix = value;
			}
		}

		public Vector3 position
		{
			get
			{
				return this.m_Position;
			}
			set
			{
				this.m_Position = value;
			}
		}

		public float shadowDistance
		{
			get
			{
				return this.m_shadowDistance;
			}
			set
			{
				this.m_shadowDistance = value;
			}
		}

		public int cullingFlags
		{
			get
			{
				return this.m_CullingFlags;
			}
			set
			{
				this.m_CullingFlags = value;
			}
		}

		public ReflectionProbeSortOptions reflectionProbeSortOptions
		{
			get
			{
				return this.m_ReflectionProbeSortOptions;
			}
			set
			{
				this.m_ReflectionProbeSortOptions = value;
			}
		}

		public CameraProperties cameraProperties
		{
			get
			{
				return this.m_CameraProperties;
			}
			set
			{
				this.m_CameraProperties = value;
			}
		}

		public unsafe float GetLayerCullDistance(int layerIndex)
		{
			if (layerIndex < 0 || layerIndex >= 32)
			{
				throw new IndexOutOfRangeException("Invalid layer index");
			}
			fixed (float* ptr = &this.m_LayerFarCullDistances.FixedElementField)
			{
				return ptr[(IntPtr)layerIndex * 4];
			}
		}

		public unsafe void SetLayerCullDistance(int layerIndex, float distance)
		{
			if (layerIndex < 0 || layerIndex >= 32)
			{
				throw new IndexOutOfRangeException("Invalid layer index");
			}
			fixed (float* ptr = &this.m_LayerFarCullDistances.FixedElementField)
			{
				ptr[(IntPtr)layerIndex * 4] = distance;
			}
		}

		public unsafe Plane GetCullingPlane(int index)
		{
			if (index < 0 || index >= this.cullingPlaneCount || index >= 10)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				return new Plane(new Vector3(ptr[(IntPtr)(index * 4) * 4], ptr[(IntPtr)(index * 4 + 1) * 4], ptr[(IntPtr)(index * 4 + 2) * 4]), ptr[(IntPtr)(index * 4 + 3) * 4]);
			}
		}

		public unsafe void SetCullingPlane(int index, Plane plane)
		{
			if (index < 0 || index >= this.cullingPlaneCount || index >= 10)
			{
				throw new IndexOutOfRangeException("Invalid plane index");
			}
			fixed (float* ptr = &this.m_CullingPlanes.FixedElementField)
			{
				ptr[(IntPtr)(index * 4) * 4] = plane.normal.x;
				ptr[(IntPtr)(index * 4 + 1) * 4] = plane.normal.y;
				ptr[(IntPtr)(index * 4 + 2) * 4] = plane.normal.z;
				ptr[(IntPtr)(index * 4 + 3) * 4] = plane.distance;
			}
		}

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 160)]
		public struct <m_CullingPlanes>__FixedBuffer4
		{
			public float FixedElementField;
		}

		[UnsafeValueType]
		[CompilerGenerated]
		[StructLayout(LayoutKind.Sequential, Size = 128)]
		public struct <m_LayerFarCullDistances>__FixedBuffer5
		{
			public float FixedElementField;
		}
	}
}
