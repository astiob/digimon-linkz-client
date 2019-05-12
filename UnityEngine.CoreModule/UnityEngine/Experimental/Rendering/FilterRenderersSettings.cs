using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct FilterRenderersSettings
	{
		private RenderQueueRange m_RenderQueueRange;

		private int m_LayerMask;

		public FilterRenderersSettings(bool initializeValues = false)
		{
			this = default(FilterRenderersSettings);
			if (initializeValues)
			{
				this.m_RenderQueueRange = RenderQueueRange.all;
				this.m_LayerMask = -1;
			}
		}

		public RenderQueueRange renderQueueRange
		{
			get
			{
				return this.m_RenderQueueRange;
			}
			set
			{
				this.m_RenderQueueRange = value;
			}
		}

		public int layerMask
		{
			get
			{
				return this.m_LayerMask;
			}
			set
			{
				this.m_LayerMask = value;
			}
		}
	}
}
