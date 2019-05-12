using System;

namespace System.Runtime.Remoting.Proxies
{
	internal class TransparentProxy
	{
		public RealProxy _rp;

		private IntPtr _class;

		private bool _custom_type_info;
	}
}
