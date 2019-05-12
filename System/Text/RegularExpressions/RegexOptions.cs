using System;

namespace System.Text.RegularExpressions
{
	/// <summary>Provides enumerated values to use to set regular expression options.</summary>
	[Flags]
	public enum RegexOptions
	{
		/// <summary>Specifies that no options are set.</summary>
		None = 0,
		/// <summary>Specifies case-insensitive matching.</summary>
		IgnoreCase = 1,
		/// <summary>Multiline mode. Changes the meaning of ^ and $ so they match at the beginning and end, respectively, of any line, and not just the beginning and end of the entire string.</summary>
		Multiline = 2,
		/// <summary>Specifies that the only valid captures are explicitly named or numbered groups of the form (?&lt;name&gt;…). This allows unnamed parentheses to act as noncapturing groups without the syntactic clumsiness of the expression (?:…).</summary>
		ExplicitCapture = 4,
		/// <summary>Specifies single-line mode. Changes the meaning of the dot (.) so it matches every character (instead of every character except \n).</summary>
		Singleline = 16,
		/// <summary>Eliminates unescaped white space from the pattern and enables comments marked with #. However, the <see cref="F:System.Text.RegularExpressions.RegexOptions.IgnorePatternWhitespace" /> value does not affect or eliminate white space in character classes. </summary>
		IgnorePatternWhitespace = 32,
		/// <summary>Specifies that the search will be from right to left instead of from left to right.</summary>
		RightToLeft = 64,
		/// <summary>Enables ECMAScript-compliant behavior for the expression. This value can be used only in conjunction with the <see cref="F:System.Text.RegularExpressions.RegexOptions.IgnoreCase" />, <see cref="F:System.Text.RegularExpressions.RegexOptions.Multiline" />, and <see cref="F:System.Text.RegularExpressions.RegexOptions.Compiled" /> values. The use of this value with any other values results in an exception.</summary>
		ECMAScript = 256,
		/// <summary>Specifies that cultural differences in language is ignored. See Performing Culture-Insensitive Operations in the RegularExpressions Namespace for more information.</summary>
		CultureInvariant = 512
	}
}
