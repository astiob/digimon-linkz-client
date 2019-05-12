using System;
using System.Collections.Generic;

namespace System.ComponentModel
{
	internal sealed class WeakObjectWrapperComparer : EqualityComparer<WeakObjectWrapper>
	{
		public override bool Equals(WeakObjectWrapper x, WeakObjectWrapper y)
		{
			if (x == null && y == null)
			{
				return false;
			}
			if (x == null || y == null)
			{
				return false;
			}
			WeakReference weak = x.Weak;
			WeakReference weak2 = y.Weak;
			return (weak.IsAlive || weak2.IsAlive) && weak.Target == weak2.Target;
		}

		public override int GetHashCode(WeakObjectWrapper obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.TargetHashCode;
		}
	}
}
