using System;
using System.Globalization;

namespace System
{
	[Serializable]
	internal sealed class CultureAwareComparer : StringComparer
	{
		private readonly bool _ignoreCase;

		private readonly CompareInfo _compareInfo;

		public CultureAwareComparer(CultureInfo ci, bool ignore_case)
		{
			this._compareInfo = ci.CompareInfo;
			this._ignoreCase = ignore_case;
		}

		public override int Compare(string x, string y)
		{
			CompareOptions options = (!this._ignoreCase) ? CompareOptions.None : CompareOptions.IgnoreCase;
			return this._compareInfo.Compare(x, y, options);
		}

		public override bool Equals(string x, string y)
		{
			return this.Compare(x, y) == 0;
		}

		public override int GetHashCode(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			CompareOptions options = (!this._ignoreCase) ? CompareOptions.None : CompareOptions.IgnoreCase;
			return this._compareInfo.GetSortKey(s, options).GetHashCode();
		}
	}
}
