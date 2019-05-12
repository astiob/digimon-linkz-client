using System;

namespace System
{
	[Serializable]
	internal sealed class OrdinalComparer : StringComparer
	{
		private readonly bool _ignoreCase;

		public OrdinalComparer(bool ignoreCase)
		{
			this._ignoreCase = ignoreCase;
		}

		public override int Compare(string x, string y)
		{
			if (this._ignoreCase)
			{
				return string.CompareOrdinalCaseInsensitiveUnchecked(x, 0, int.MaxValue, y, 0, int.MaxValue);
			}
			return string.CompareOrdinalUnchecked(x, 0, int.MaxValue, y, 0, int.MaxValue);
		}

		public override bool Equals(string x, string y)
		{
			if (this._ignoreCase)
			{
				return this.Compare(x, y) == 0;
			}
			return x == y;
		}

		public override int GetHashCode(string s)
		{
			if (s == null)
			{
				throw new ArgumentNullException("s");
			}
			if (this._ignoreCase)
			{
				return s.GetCaseInsensitiveHashCode();
			}
			return s.GetHashCode();
		}
	}
}
