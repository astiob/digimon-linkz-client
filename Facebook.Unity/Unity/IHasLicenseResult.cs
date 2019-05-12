using System;

namespace Facebook.Unity
{
	public interface IHasLicenseResult : IResult
	{
		bool HasLicense { get; }
	}
}
