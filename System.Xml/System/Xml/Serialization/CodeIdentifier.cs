using System;
using System.Globalization;

namespace System.Xml.Serialization
{
	/// <summary>Provides static methods to convert input text into names for code entities.</summary>
	public class CodeIdentifier
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.CodeIdentifier" /> class. </summary>
		[Obsolete("Design mistake. It only contains static methods.")]
		public CodeIdentifier()
		{
		}

		/// <summary>Produces a camel-case string from an input string. </summary>
		/// <returns>A camel-case version of the parameter string.</returns>
		/// <param name="identifier">The name of a code entity, such as a method parameter, typically taken from an XML element or attribute name.</param>
		public static string MakeCamel(string identifier)
		{
			string text = CodeIdentifier.MakeValid(identifier);
			return char.ToLower(text[0], CultureInfo.InvariantCulture) + text.Substring(1);
		}

		/// <summary>Produces a Pascal-case string from an input string. </summary>
		/// <returns>A Pascal-case version of the parameter string.</returns>
		/// <param name="identifier">The name of a code entity, such as a method parameter, typically taken from an XML element or attribute name.</param>
		public static string MakePascal(string identifier)
		{
			string text = CodeIdentifier.MakeValid(identifier);
			return char.ToUpper(text[0], CultureInfo.InvariantCulture) + text.Substring(1);
		}

		/// <summary>Produces a valid code entity name from an input string. </summary>
		/// <returns>A string that can be used as a code identifier, such as the name of a method parameter.</returns>
		/// <param name="identifier">The name of a code entity, such as a method parameter, typically taken from an XML element or attribute name.</param>
		public static string MakeValid(string identifier)
		{
			if (identifier == null)
			{
				throw new NullReferenceException();
			}
			if (identifier.Length == 0)
			{
				return "Item";
			}
			string text = string.Empty;
			if (!char.IsLetter(identifier[0]) && identifier[0] != '_')
			{
				text = "Item";
			}
			foreach (char c in identifier)
			{
				if (char.IsLetterOrDigit(c) || c == '_')
				{
					text += c;
				}
			}
			if (text.Length > 400)
			{
				text = text.Substring(0, 400);
			}
			return text;
		}
	}
}
