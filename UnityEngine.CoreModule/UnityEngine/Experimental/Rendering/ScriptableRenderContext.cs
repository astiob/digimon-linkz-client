using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	[NativeType("Runtime/Graphics/ScriptableRenderLoop/ScriptableRenderContext.h")]
	public struct ScriptableRenderContext
	{
		private IntPtr m_Ptr;

		internal ScriptableRenderContext(IntPtr ptr)
		{
			this.m_Ptr = ptr;
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void Submit_Internal();

		private void DrawRenderers_Internal(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings)
		{
			ScriptableRenderContext.INTERNAL_CALL_DrawRenderers_Internal(ref this, ref renderers, ref drawSettings, ref filterSettings);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawRenderers_Internal(ref ScriptableRenderContext self, ref FilterResults renderers, ref DrawRendererSettings drawSettings, ref FilterRenderersSettings filterSettings);

		private void DrawRenderers_StateBlock_Internal(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings, RenderStateBlock stateBlock)
		{
			ScriptableRenderContext.INTERNAL_CALL_DrawRenderers_StateBlock_Internal(ref this, ref renderers, ref drawSettings, ref filterSettings, ref stateBlock);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawRenderers_StateBlock_Internal(ref ScriptableRenderContext self, ref FilterResults renderers, ref DrawRendererSettings drawSettings, ref FilterRenderersSettings filterSettings, ref RenderStateBlock stateBlock);

		private void DrawRenderers_StateMap_Internal(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings, Array stateMap, int stateMapLength)
		{
			ScriptableRenderContext.INTERNAL_CALL_DrawRenderers_StateMap_Internal(ref this, ref renderers, ref drawSettings, ref filterSettings, stateMap, stateMapLength);
		}

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void INTERNAL_CALL_DrawRenderers_StateMap_Internal(ref ScriptableRenderContext self, ref FilterResults renderers, ref DrawRendererSettings drawSettings, ref FilterRenderersSettings filterSettings, Array stateMap, int stateMapLength);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DrawShadows_Internal(ref DrawShadowsSettings settings);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ExecuteCommandBuffer_Internal(CommandBuffer commandBuffer);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void ExecuteCommandBufferAsync_Internal(CommandBuffer commandBuffer, ComputeQueueType queueType);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetupCameraProperties_Internal(Camera camera, bool stereoSetup);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StereoEndRender_Internal(Camera camera);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StartMultiEye_Internal(Camera camera);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void StopMultiEye_Internal(Camera camera);

		[GeneratedByOldBindingsGenerator]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void DrawSkybox_Internal(Camera camera);

		internal IntPtr Internal_GetPtr()
		{
			return this.m_Ptr;
		}

		[NativeMethod("BeginRenderPass")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginRenderPassInternal(IntPtr _self, int w, int h, int samples, RenderPassAttachment[] colors, RenderPassAttachment depth);

		[NativeMethod("BeginSubPass")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void BeginSubPassInternal(IntPtr _self, RenderPassAttachment[] colors, RenderPassAttachment[] inputs, bool readOnlyDepth);

		[NativeMethod("EndRenderPass")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void EndRenderPassInternal(IntPtr _self);

		public void Submit()
		{
			this.CheckValid();
			this.Submit_Internal();
		}

		public void DrawRenderers(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings)
		{
			this.CheckValid();
			this.DrawRenderers_Internal(renderers, ref drawSettings, filterSettings);
		}

		public void DrawRenderers(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings, RenderStateBlock stateBlock)
		{
			this.CheckValid();
			this.DrawRenderers_StateBlock_Internal(renderers, ref drawSettings, filterSettings, stateBlock);
		}

		public void DrawRenderers(FilterResults renderers, ref DrawRendererSettings drawSettings, FilterRenderersSettings filterSettings, List<RenderStateMapping> stateMap)
		{
			this.CheckValid();
			this.DrawRenderers_StateMap_Internal(renderers, ref drawSettings, filterSettings, NoAllocHelpers.ExtractArrayFromList(stateMap), stateMap.Count);
		}

		public void DrawShadows(ref DrawShadowsSettings settings)
		{
			this.CheckValid();
			this.DrawShadows_Internal(ref settings);
		}

		public void ExecuteCommandBuffer(CommandBuffer commandBuffer)
		{
			if (commandBuffer == null)
			{
				throw new ArgumentNullException("commandBuffer");
			}
			this.CheckValid();
			this.ExecuteCommandBuffer_Internal(commandBuffer);
		}

		public void ExecuteCommandBufferAsync(CommandBuffer commandBuffer, ComputeQueueType queueType)
		{
			if (commandBuffer == null)
			{
				throw new ArgumentNullException("commandBuffer");
			}
			this.CheckValid();
			this.ExecuteCommandBufferAsync_Internal(commandBuffer, queueType);
		}

		public void SetupCameraProperties(Camera camera)
		{
			this.CheckValid();
			this.SetupCameraProperties_Internal(camera, false);
		}

		public void SetupCameraProperties(Camera camera, bool stereoSetup)
		{
			this.CheckValid();
			this.SetupCameraProperties_Internal(camera, stereoSetup);
		}

		public void StereoEndRender(Camera camera)
		{
			this.CheckValid();
			this.StereoEndRender_Internal(camera);
		}

		public void StartMultiEye(Camera camera)
		{
			this.CheckValid();
			this.StartMultiEye_Internal(camera);
		}

		public void StopMultiEye(Camera camera)
		{
			this.CheckValid();
			this.StopMultiEye_Internal(camera);
		}

		public void DrawSkybox(Camera camera)
		{
			this.CheckValid();
			this.DrawSkybox_Internal(camera);
		}

		internal void CheckValid()
		{
			if (this.m_Ptr.ToInt64() == 0L)
			{
				throw new ArgumentException("Invalid ScriptableRenderContext.  This can be caused by allocating a context in user code.");
			}
		}
	}
}
