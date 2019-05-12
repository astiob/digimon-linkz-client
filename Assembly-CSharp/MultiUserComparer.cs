using System;
using System.Collections.Generic;

internal class MultiUserComparer : IEqualityComparer<MultiUser>
{
	public bool Equals(MultiUser x, MultiUser y)
	{
		return object.ReferenceEquals(x, y) || (!object.ReferenceEquals(x, null) && !object.ReferenceEquals(y, null) && x.userName == y.userName && x.userId == y.userId);
	}

	public int GetHashCode(MultiUser multiUser)
	{
		if (object.ReferenceEquals(multiUser, null))
		{
			return 0;
		}
		int num = (multiUser.userName != null) ? multiUser.userName.GetHashCode() : 0;
		int hashCode = multiUser.userId.GetHashCode();
		return num ^ hashCode;
	}
}
