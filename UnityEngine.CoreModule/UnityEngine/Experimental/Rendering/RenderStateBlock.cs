using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct RenderStateBlock
	{
		private BlendState m_BlendState;

		private RasterState m_RasterState;

		private DepthState m_DepthState;

		private StencilState m_StencilState;

		private int m_StencilReference;

		private RenderStateMask m_Mask;

		public RenderStateBlock(RenderStateMask mask)
		{
			this.m_BlendState = BlendState.Default;
			this.m_RasterState = RasterState.Default;
			this.m_DepthState = DepthState.Default;
			this.m_StencilState = StencilState.Default;
			this.m_StencilReference = 0;
			this.m_Mask = mask;
		}

		public BlendState blendState
		{
			get
			{
				return this.m_BlendState;
			}
			set
			{
				this.m_BlendState = value;
			}
		}

		public RasterState rasterState
		{
			get
			{
				return this.m_RasterState;
			}
			set
			{
				this.m_RasterState = value;
			}
		}

		public DepthState depthState
		{
			get
			{
				return this.m_DepthState;
			}
			set
			{
				this.m_DepthState = value;
			}
		}

		public StencilState stencilState
		{
			get
			{
				return this.m_StencilState;
			}
			set
			{
				this.m_StencilState = value;
			}
		}

		public int stencilReference
		{
			get
			{
				return this.m_StencilReference;
			}
			set
			{
				this.m_StencilReference = value;
			}
		}

		public RenderStateMask mask
		{
			get
			{
				return this.m_Mask;
			}
			set
			{
				this.m_Mask = value;
			}
		}
	}
}
