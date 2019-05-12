using System;
using System.Collections;

namespace System.IO
{
	internal class ParentInotifyData
	{
		public bool IncludeSubdirs;

		public bool Enabled;

		public ArrayList children;

		public InotifyData data;
	}
}
