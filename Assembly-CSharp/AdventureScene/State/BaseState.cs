using System;

namespace AdventureScene.State
{
	public abstract class BaseState
	{
		protected ResultCode resultCode;

		public ResultCode GetResultCode()
		{
			return this.resultCode;
		}

		public abstract bool Update();
	}
}
