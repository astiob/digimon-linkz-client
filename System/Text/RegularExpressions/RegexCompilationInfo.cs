using System;

namespace System.Text.RegularExpressions
{
	/// <summary>Provides information about a regular expression that is used to compile a regular expression to a stand-alone assembly. </summary>
	[Serializable]
	public class RegexCompilationInfo
	{
		private string pattern;

		private string name;

		private string nspace;

		private RegexOptions options;

		private bool isPublic;

		/// <summary>Initializes a new instance of the <see cref="T:System.Text.RegularExpressions.RegexCompilationInfo" /> class that contains information about a regular expression to be included in an assembly. </summary>
		/// <param name="pattern">The regular expression to compile. </param>
		/// <param name="options">The regular expression options to use when compiling the regular expression. </param>
		/// <param name="name">The name of the type that represents the compiled regular expression. </param>
		/// <param name="fullnamespace">The namespace to which the new type belongs. </param>
		/// <param name="ispublic">true to make the compiled regular expression publicly visible; otherwise, false. </param>
		public RegexCompilationInfo(string pattern, RegexOptions options, string name, string fullnamespace, bool ispublic)
		{
			this.Pattern = pattern;
			this.Options = options;
			this.Name = name;
			this.Namespace = fullnamespace;
			this.IsPublic = ispublic;
		}

		/// <summary>Gets or sets a value that indicates whether the compiled regular expression has public visibility.</summary>
		/// <returns>true if the regular expression has public visibility; otherwise, false.</returns>
		public bool IsPublic
		{
			get
			{
				return this.isPublic;
			}
			set
			{
				this.isPublic = value;
			}
		}

		/// <summary>Gets or sets the name of the type that represents the compiled regular expression.</summary>
		/// <returns>The name of the new type.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value for this property is null.</exception>
		/// <exception cref="T:System.ArgumentException">The value for this property is an empty string.</exception>
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Name");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException("Name");
				}
				this.name = value;
			}
		}

		/// <summary>Gets or sets the namespace to which the new type belongs.</summary>
		/// <returns>The namespace of the new type.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value for this property is null.</exception>
		public string Namespace
		{
			get
			{
				return this.nspace;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Namespace");
				}
				this.nspace = value;
			}
		}

		/// <summary>Gets or sets the options to use when compiling the regular expression.</summary>
		/// <returns>A bitwise combination of the enumeration values.</returns>
		public RegexOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		/// <summary>Gets or sets the regular expression to compile.</summary>
		/// <returns>The regular expression to compile.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value for this property is null.</exception>
		public string Pattern
		{
			get
			{
				return this.pattern;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("pattern");
				}
				this.pattern = value;
			}
		}
	}
}
