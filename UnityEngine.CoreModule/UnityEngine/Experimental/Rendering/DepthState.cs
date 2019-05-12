using System;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering
{
	public struct DepthState
	{
		private byte m_WriteEnabled;

		private sbyte m_CompareFunction;

		public DepthState(bool writeEnabled = true, CompareFunction compareFunction = CompareFunction.Less)
		{
			this.m_WriteEnabled = Convert.ToByte(writeEnabled);
			this.m_CompareFunction = (sbyte)compareFunction;
		}

		public static DepthState Default
		{
			get
			{
				return new DepthState(true, CompareFunction.Less);
			}
		}

		public bool writeEnabled
		{
			get
			{
				return Convert.ToBoolean(this.m_WriteEnabled);
			}
			set
			{
				this.m_WriteEnabled = Convert.ToByte(value);
			}
		}

		public CompareFunction compareFunction
		{
			get
			{
				return (CompareFunction)this.m_CompareFunction;
			}
			set
			{
				this.m_CompareFunction = (sbyte)value;
			}
		}
	}
}
