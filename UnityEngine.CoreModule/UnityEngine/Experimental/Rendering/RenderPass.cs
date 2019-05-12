using System;

namespace UnityEngine.Experimental.Rendering
{
	public class RenderPass : IDisposable
	{
		public RenderPass(ScriptableRenderContext ctx, int w, int h, int samples, RenderPassAttachment[] colors, RenderPassAttachment depth = null)
		{
			this.width = w;
			this.height = h;
			this.sampleCount = samples;
			this.colorAttachments = colors;
			this.depthAttachment = depth;
			this.context = ctx;
			ScriptableRenderContext.BeginRenderPassInternal(ctx.Internal_GetPtr(), w, h, samples, colors, depth);
		}

		public RenderPassAttachment[] colorAttachments { get; private set; }

		public RenderPassAttachment depthAttachment { get; private set; }

		public int width { get; private set; }

		public int height { get; private set; }

		public int sampleCount { get; private set; }

		public ScriptableRenderContext context { get; private set; }

		public void Dispose()
		{
			ScriptableRenderContext.EndRenderPassInternal(this.context.Internal_GetPtr());
		}

		public class SubPass : IDisposable
		{
			public SubPass(RenderPass renderPass, RenderPassAttachment[] colors, RenderPassAttachment[] inputs, bool readOnlyDepth = false)
			{
				ScriptableRenderContext.BeginSubPassInternal(renderPass.context.Internal_GetPtr(), (colors == null) ? new RenderPassAttachment[0] : colors, (inputs == null) ? new RenderPassAttachment[0] : inputs, readOnlyDepth);
			}

			public void Dispose()
			{
			}
		}
	}
}
