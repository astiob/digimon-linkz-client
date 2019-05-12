using System;

namespace System.Runtime.Remoting.Messaging
{
	[Serializable]
	internal class CallContextRemotingData : ICloneable
	{
		private string _logicalCallID;

		public string LogicalCallID
		{
			get
			{
				return this._logicalCallID;
			}
			set
			{
				this._logicalCallID = value;
			}
		}

		public object Clone()
		{
			return new CallContextRemotingData
			{
				_logicalCallID = this._logicalCallID
			};
		}
	}
}
