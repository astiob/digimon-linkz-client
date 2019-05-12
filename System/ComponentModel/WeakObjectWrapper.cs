using System;

namespace System.ComponentModel
{
	internal sealed class WeakObjectWrapper
	{
		public WeakObjectWrapper(object target)
		{
			this.TargetHashCode = target.GetHashCode();
			this.Weak = new WeakReference(target);
		}

		public int TargetHashCode { get; private set; }

		public WeakReference Weak { get; private set; }
	}
}
