using System;

namespace Neptune.Common
{
	public interface INpPermission
	{
		void OnRequestPermissionsResult(ManifestPermission permission, PermisionState state);
	}
}
