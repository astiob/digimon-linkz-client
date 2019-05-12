using System;
using System.Collections.Generic;

namespace System.Diagnostics
{
	public class ProcessThreadCollectionBase : List<ProcessThread>
	{
		protected ProcessThreadCollectionBase InnerList
		{
			get
			{
				return this;
			}
		}

		public new int Add(ProcessThread thread)
		{
			base.Add(thread);
			return this.Count - 1;
		}
	}
}
