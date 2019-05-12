using System;

namespace System.Runtime.Remoting.Channels
{
	[Serializable]
	internal class CrossAppDomainData
	{
		private object _ContextID;

		private int _DomainID;

		private string _processGuid;

		internal CrossAppDomainData(int domainId)
		{
			this._ContextID = 0;
			this._DomainID = domainId;
			this._processGuid = RemotingConfiguration.ProcessId;
		}

		internal int DomainID
		{
			get
			{
				return this._DomainID;
			}
		}

		internal string ProcessID
		{
			get
			{
				return this._processGuid;
			}
		}
	}
}
