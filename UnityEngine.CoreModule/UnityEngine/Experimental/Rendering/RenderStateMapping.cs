using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct RenderStateMapping
	{
		private int m_RenderTypeID;

		private RenderStateBlock m_StateBlock;

		public RenderStateMapping(string renderType, RenderStateBlock stateBlock)
		{
			this.m_RenderTypeID = Shader.TagToID(renderType);
			this.m_StateBlock = stateBlock;
		}

		public RenderStateMapping(RenderStateBlock stateBlock)
		{
			this = new RenderStateMapping(null, stateBlock);
		}

		public string renderType
		{
			get
			{
				return Shader.IDToTag(this.m_RenderTypeID);
			}
			set
			{
				this.m_RenderTypeID = Shader.TagToID(value);
			}
		}

		public RenderStateBlock stateBlock
		{
			get
			{
				return this.m_StateBlock;
			}
			set
			{
				this.m_StateBlock = value;
			}
		}
	}
}
