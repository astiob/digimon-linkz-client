using System;

namespace UnityEngine.Experimental.Rendering
{
	public struct RenderQueueRange
	{
		public int min;

		public int max;

		public static RenderQueueRange all
		{
			get
			{
				return new RenderQueueRange
				{
					min = 0,
					max = 5000
				};
			}
		}

		public static RenderQueueRange opaque
		{
			get
			{
				return new RenderQueueRange
				{
					min = 0,
					max = 2500
				};
			}
		}

		public static RenderQueueRange transparent
		{
			get
			{
				return new RenderQueueRange
				{
					min = 2501,
					max = 5000
				};
			}
		}
	}
}
