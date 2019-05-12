using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[NativeHeader("Runtime/Graphics/QualitySettings.h")]
	[StaticAccessor("GetQualitySettings()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Misc/PlayerSettings.h")]
	public sealed class QualitySettings : Object
	{
		public static extern string[] names { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern int GetQualityLevel();

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetQualityLevel(int index, [DefaultValue("true")] bool applyExpensiveChanges);

		[ExcludeFromDocs]
		public static void SetQualityLevel(int index)
		{
			bool applyExpensiveChanges = true;
			QualitySettings.SetQualityLevel(index, applyExpensiveChanges);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IncreaseLevel([DefaultValue("false")] bool applyExpensiveChanges);

		[ExcludeFromDocs]
		public static void IncreaseLevel()
		{
			bool applyExpensiveChanges = false;
			QualitySettings.IncreaseLevel(applyExpensiveChanges);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void DecreaseLevel([DefaultValue("false")] bool applyExpensiveChanges);

		[ExcludeFromDocs]
		public static void DecreaseLevel()
		{
			bool applyExpensiveChanges = false;
			QualitySettings.DecreaseLevel(applyExpensiveChanges);
		}

		public static Vector3 shadowCascade4Split
		{
			get
			{
				Vector3 result;
				QualitySettings.INTERNAL_get_shadowCascade4Split(out result);
				return result;
			}
			set
			{
				QualitySettings.INTERNAL_set_shadowCascade4Split(ref value);
			}
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_get_shadowCascade4Split(out Vector3 value);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_set_shadowCascade4Split(ref Vector3 value);

		public static extern AnisotropicFiltering anisotropicFiltering { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int maxQueuedFrames { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern BlendWeights blendWeights { [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] get; [GeneratedByOldBindingsGenerator] [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[Obsolete("Use GetQualityLevel and SetQualityLevel", false)]
		public static QualityLevel currentLevel
		{
			get
			{
				return (QualityLevel)QualitySettings.GetQualityLevel();
			}
			set
			{
				QualitySettings.SetQualityLevel((int)value, true);
			}
		}

		public static extern int pixelLightCount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("ShadowQuality")]
		public static extern ShadowQuality shadows { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern ShadowProjection shadowProjection { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int shadowCascades { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern float shadowDistance { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern ShadowResolution shadowResolution { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern ShadowmaskMode shadowmaskMode { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern float shadowNearPlaneOffset { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern float shadowCascade2Split { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		[NativeProperty("LODBias")]
		public static extern float lodBias { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int masterTextureLimit { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int maximumLODLevel { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int particleRaycastBudget { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool softParticles { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool softVegetation { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int vSyncCount { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int antiAliasing { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int asyncUploadTimeSlice { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern int asyncUploadBufferSize { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool realtimeReflectionProbes { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern bool billboardsFaceCameraPosition { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern float resolutionScalingFixedDPIFactor { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] set; }

		public static extern ColorSpace desiredColorSpace { [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)] [NativeName("GetColorSpace")] [MethodImpl(MethodImplOptions.InternalCall)] get; }

		public static extern ColorSpace activeColorSpace { [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)] [NativeName("GetColorSpace")] [MethodImpl(MethodImplOptions.InternalCall)] get; }
	}
}
