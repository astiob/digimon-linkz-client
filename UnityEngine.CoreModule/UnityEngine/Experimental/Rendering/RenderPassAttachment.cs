using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	[NativeType("Runtime/Graphics/ScriptableRenderLoop/ScriptableRenderContext.h")]
	public class RenderPassAttachment : Object
	{
		public RenderPassAttachment(RenderTextureFormat fmt)
		{
			RenderPassAttachment.Internal_CreateAttachment(this);
			this.loadAction = RenderBufferLoadAction.DontCare;
			this.storeAction = RenderBufferStoreAction.DontCare;
			this.format = fmt;
			this.loadStoreTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
			this.resolveTarget = new RenderTargetIdentifier(BuiltinRenderTextureType.None);
			this.clearColor = new Color(0f, 0f, 0f, 0f);
			this.clearDepth = 1f;
		}

		public extern RenderBufferLoadAction loadAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

		public extern RenderBufferStoreAction storeAction { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

		public extern RenderTextureFormat format { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

		private RenderTargetIdentifier loadStoreTarget
		{
			get
			{
				RenderTargetIdentifier result;
				this.get_loadStoreTarget_Injected(out result);
				return result;
			}
			set
			{
				this.set_loadStoreTarget_Injected(ref value);
			}
		}

		private RenderTargetIdentifier resolveTarget
		{
			get
			{
				RenderTargetIdentifier result;
				this.get_resolveTarget_Injected(out result);
				return result;
			}
			set
			{
				this.set_resolveTarget_Injected(ref value);
			}
		}

		public Color clearColor
		{
			get
			{
				Color result;
				this.get_clearColor_Injected(out result);
				return result;
			}
			private set
			{
				this.set_clearColor_Injected(ref value);
			}
		}

		public extern float clearDepth { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

		public extern uint clearStencil { [MethodImpl(MethodImplOptions.InternalCall)] get; [MethodImpl(MethodImplOptions.InternalCall)] private set; }

		public void BindSurface(RenderTargetIdentifier tgt, bool loadExistingContents, bool storeResults)
		{
			this.loadStoreTarget = tgt;
			if (loadExistingContents && this.loadAction != RenderBufferLoadAction.Clear)
			{
				this.loadAction = RenderBufferLoadAction.Load;
			}
			if (storeResults)
			{
				if (this.storeAction == RenderBufferStoreAction.StoreAndResolve || this.storeAction == RenderBufferStoreAction.Resolve)
				{
					this.storeAction = RenderBufferStoreAction.StoreAndResolve;
				}
				else
				{
					this.storeAction = RenderBufferStoreAction.Store;
				}
			}
		}

		public void BindResolveSurface(RenderTargetIdentifier tgt)
		{
			this.resolveTarget = tgt;
			if (this.storeAction == RenderBufferStoreAction.StoreAndResolve || this.storeAction == RenderBufferStoreAction.Store)
			{
				this.storeAction = RenderBufferStoreAction.StoreAndResolve;
			}
			else
			{
				this.storeAction = RenderBufferStoreAction.Resolve;
			}
		}

		public void Clear(Color clearCol, float clearDep = 1f, uint clearStenc = 0u)
		{
			this.clearColor = clearCol;
			this.clearDepth = clearDep;
			this.clearStencil = clearStenc;
			this.loadAction = RenderBufferLoadAction.Clear;
		}

		[NativeMethod(Name = "RenderPassAttachment::Internal_CreateAttachment", IsFreeFunction = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Internal_CreateAttachment([Writable] RenderPassAttachment self);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_loadStoreTarget_Injected(out RenderTargetIdentifier ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_loadStoreTarget_Injected(ref RenderTargetIdentifier value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_resolveTarget_Injected(out RenderTargetIdentifier ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_resolveTarget_Injected(ref RenderTargetIdentifier value);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_clearColor_Injected(out Color ret);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_clearColor_Injected(ref Color value);
	}
}
