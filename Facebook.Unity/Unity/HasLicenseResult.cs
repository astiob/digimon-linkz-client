using System;

namespace Facebook.Unity
{
	internal class HasLicenseResult : ResultBase, IHasLicenseResult, IResult
	{
		public HasLicenseResult(ResultContainer resultContainer) : base(resultContainer)
		{
			bool hasLicense;
			if (this.ResultDictionary != null && this.ResultDictionary.TryGetValue("has_license", out hasLicense))
			{
				this.HasLicense = hasLicense;
			}
		}

		public bool HasLicense { get; private set; }
	}
}
