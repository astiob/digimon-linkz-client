using System;

namespace System
{
	internal class MonoAsyncCall
	{
		private object msg;

		private IntPtr cb_method;

		private object cb_target;

		private object state;

		private object res;

		private object out_args;

		private long wait_event;
	}
}
