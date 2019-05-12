using System;
using System.Collections.Generic;

namespace System.Diagnostics
{
	public class ProcessModuleCollectionBase : List<ProcessModule>
	{
		protected ProcessModuleCollectionBase InnerList
		{
			get
			{
				return this;
			}
		}
	}
}
