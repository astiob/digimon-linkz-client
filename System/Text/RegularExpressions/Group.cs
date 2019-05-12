using System;

namespace System.Text.RegularExpressions
{
	/// <summary>Represents the results from a single capturing group. </summary>
	[Serializable]
	public class Group : Capture
	{
		internal static Group Fail = new Group();

		private bool success;

		private CaptureCollection captures;

		internal Group(string text, int index, int length, int n_caps) : base(text, index, length)
		{
			this.success = true;
			this.captures = new CaptureCollection(n_caps);
			this.captures.SetValue(this, n_caps - 1);
		}

		internal Group(string text, int index, int length) : base(text, index, length)
		{
			this.success = true;
		}

		internal Group() : base(string.Empty)
		{
			this.success = false;
			this.captures = new CaptureCollection(0);
		}

		/// <summary>Returns a Group object equivalent to the one supplied that is safe to share between multiple threads.</summary>
		/// <returns>A regular expression Group object. </returns>
		/// <param name="inner">The input <see cref="T:System.Text.RegularExpressions.Group" /> object.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="inner" /> is null.</exception>
		[MonoTODO("not thread-safe")]
		public static Group Synchronized(Group inner)
		{
			if (inner == null)
			{
				throw new ArgumentNullException("inner");
			}
			return inner;
		}

		/// <summary>Gets a collection of all the captures matched by the capturing group, in innermost-leftmost-first order (or innermost-rightmost-first order if the regular expression is modified with the <see cref="F:System.Text.RegularExpressions.RegexOptions.RightToLeft" /> option). The collection may have zero or more items.</summary>
		/// <returns>The collection of substrings matched by the group.</returns>
		public CaptureCollection Captures
		{
			get
			{
				return this.captures;
			}
		}

		/// <summary>Gets a value indicating whether the match is successful.</summary>
		/// <returns>true if the match is successful; otherwise, false.</returns>
		public bool Success
		{
			get
			{
				return this.success;
			}
		}
	}
}
