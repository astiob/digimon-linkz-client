using System;

namespace System.IO
{
	internal struct InotifyEvent
	{
		public static readonly InotifyEvent Default = default(InotifyEvent);

		public int WatchDescriptor;

		public InotifyMask Mask;

		public string Name;

		public override string ToString()
		{
			return string.Format("[Descriptor: {0} Mask: {1} Name: {2}]", this.WatchDescriptor, this.Mask, this.Name);
		}
	}
}
