using System;

namespace Firebase
{
	public enum DependencyStatus
	{
		Available,
		UnavailableDisabled,
		UnavailableInvalid,
		UnavilableMissing,
		UnavailablePermission,
		UnavailableUpdaterequired,
		UnavailableUpdating,
		UnavailableOther
	}
}
