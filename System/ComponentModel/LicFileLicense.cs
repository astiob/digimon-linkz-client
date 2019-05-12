using System;

namespace System.ComponentModel
{
	internal class LicFileLicense : License
	{
		private string _key;

		public LicFileLicense(string key)
		{
			this._key = key;
		}

		public override string LicenseKey
		{
			get
			{
				return this._key;
			}
		}

		public override void Dispose()
		{
		}
	}
}
