using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct BlendState
	{
		private RenderTargetBlendState m_BlendState0;

		private RenderTargetBlendState m_BlendState1;

		private RenderTargetBlendState m_BlendState2;

		private RenderTargetBlendState m_BlendState3;

		private RenderTargetBlendState m_BlendState4;

		private RenderTargetBlendState m_BlendState5;

		private RenderTargetBlendState m_BlendState6;

		private RenderTargetBlendState m_BlendState7;

		private byte m_SeparateMRTBlendStates;

		private byte m_AlphaToMask;

		private short m_Padding;

		public BlendState(bool separateMRTBlend = false, bool alphaToMask = false)
		{
			this.m_BlendState0 = RenderTargetBlendState.Default;
			this.m_BlendState1 = RenderTargetBlendState.Default;
			this.m_BlendState2 = RenderTargetBlendState.Default;
			this.m_BlendState3 = RenderTargetBlendState.Default;
			this.m_BlendState4 = RenderTargetBlendState.Default;
			this.m_BlendState5 = RenderTargetBlendState.Default;
			this.m_BlendState6 = RenderTargetBlendState.Default;
			this.m_BlendState7 = RenderTargetBlendState.Default;
			this.m_SeparateMRTBlendStates = Convert.ToByte(separateMRTBlend);
			this.m_AlphaToMask = Convert.ToByte(alphaToMask);
			this.m_Padding = 0;
		}

		public static BlendState Default
		{
			get
			{
				return new BlendState(false, false);
			}
		}

		public bool separateMRTBlendStates
		{
			get
			{
				return Convert.ToBoolean(this.m_SeparateMRTBlendStates);
			}
			set
			{
				this.m_SeparateMRTBlendStates = Convert.ToByte(value);
			}
		}

		public bool alphaToMask
		{
			get
			{
				return Convert.ToBoolean(this.m_AlphaToMask);
			}
			set
			{
				this.m_AlphaToMask = Convert.ToByte(value);
			}
		}

		public RenderTargetBlendState blendState0
		{
			get
			{
				return this.m_BlendState0;
			}
			set
			{
				this.m_BlendState0 = value;
			}
		}

		public RenderTargetBlendState blendState1
		{
			get
			{
				return this.m_BlendState1;
			}
			set
			{
				this.m_BlendState1 = value;
			}
		}

		public RenderTargetBlendState blendState2
		{
			get
			{
				return this.m_BlendState2;
			}
			set
			{
				this.m_BlendState2 = value;
			}
		}

		public RenderTargetBlendState blendState3
		{
			get
			{
				return this.m_BlendState3;
			}
			set
			{
				this.m_BlendState3 = value;
			}
		}

		public RenderTargetBlendState blendState4
		{
			get
			{
				return this.m_BlendState4;
			}
			set
			{
				this.m_BlendState4 = value;
			}
		}

		public RenderTargetBlendState blendState5
		{
			get
			{
				return this.m_BlendState5;
			}
			set
			{
				this.m_BlendState5 = value;
			}
		}

		public RenderTargetBlendState blendState6
		{
			get
			{
				return this.m_BlendState6;
			}
			set
			{
				this.m_BlendState6 = value;
			}
		}

		public RenderTargetBlendState blendState7
		{
			get
			{
				return this.m_BlendState7;
			}
			set
			{
				this.m_BlendState7 = value;
			}
		}
	}
}
