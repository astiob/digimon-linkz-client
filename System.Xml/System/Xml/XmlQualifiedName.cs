using System;

namespace System.Xml
{
	/// <summary>Represents an XML qualified name.</summary>
	[Serializable]
	public class XmlQualifiedName
	{
		/// <summary>Provides an empty <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		public static readonly XmlQualifiedName Empty = new XmlQualifiedName();

		private readonly string name;

		private readonly string ns;

		private readonly int hash;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlQualifiedName" /> class.</summary>
		public XmlQualifiedName() : this(string.Empty, string.Empty)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlQualifiedName" /> class with the specified name.</summary>
		/// <param name="name">The local name to use as the name of the <see cref="T:System.Xml.XmlQualifiedName" /> object. </param>
		public XmlQualifiedName(string name) : this(name, string.Empty)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlQualifiedName" /> class with the specified name and namespace.</summary>
		/// <param name="name">The local name to use as the name of the <see cref="T:System.Xml.XmlQualifiedName" /> object. </param>
		/// <param name="ns">The namespace for the <see cref="T:System.Xml.XmlQualifiedName" /> object. </param>
		public XmlQualifiedName(string name, string ns)
		{
			this.name = ((name != null) ? name : string.Empty);
			this.ns = ((ns != null) ? ns : string.Empty);
			this.hash = (this.name.GetHashCode() ^ this.ns.GetHashCode());
		}

		/// <summary>Gets a value indicating whether the <see cref="T:System.Xml.XmlQualifiedName" /> is empty.</summary>
		/// <returns>true if name and namespace are empty strings; otherwise, false.</returns>
		public bool IsEmpty
		{
			get
			{
				return this.name.Length == 0 && this.ns.Length == 0;
			}
		}

		/// <summary>Gets a string representation of the qualified name of the <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		/// <returns>A string representation of the qualified name or String.Empty if a name is not defined for the object.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets a string representation of the namespace of the <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		/// <returns>A string representation of the namespace or String.Empty if a namespace is not defined for the object.</returns>
		public string Namespace
		{
			get
			{
				return this.ns;
			}
		}

		/// <summary>Determines whether the specified <see cref="T:System.Xml.XmlQualifiedName" /> object is equal to the current <see cref="T:System.Xml.XmlQualifiedName" /> object. </summary>
		/// <returns>true if the two are the same instance object; otherwise, false.</returns>
		/// <param name="other">The <see cref="T:System.Xml.XmlQualifiedName" /> to compare. </param>
		public override bool Equals(object other)
		{
			return this == other as XmlQualifiedName;
		}

		/// <summary>Returns the hash code for the <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		/// <returns>A hash code for this object.</returns>
		public override int GetHashCode()
		{
			return this.hash;
		}

		/// <summary>Returns the string value of the <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		/// <returns>The string value of the <see cref="T:System.Xml.XmlQualifiedName" /> in the format of namespace:localname. If the object does not have a namespace defined, this method returns just the local name.</returns>
		public override string ToString()
		{
			if (this.ns == string.Empty)
			{
				return this.name;
			}
			return this.ns + ":" + this.name;
		}

		/// <summary>Returns the string value of the <see cref="T:System.Xml.XmlQualifiedName" />.</summary>
		/// <returns>The string value of the <see cref="T:System.Xml.XmlQualifiedName" /> in the format of namespace:localname. If the object does not have a namespace defined, this method returns just the local name.</returns>
		/// <param name="name">The name of the object. </param>
		/// <param name="ns">The namespace of the object. </param>
		public static string ToString(string name, string ns)
		{
			if (ns == string.Empty)
			{
				return name;
			}
			return ns + ":" + name;
		}

		internal static XmlQualifiedName Parse(string name, IXmlNamespaceResolver resolver)
		{
			return XmlQualifiedName.Parse(name, resolver, false);
		}

		internal static XmlQualifiedName Parse(string name, IXmlNamespaceResolver resolver, bool considerDefaultNamespace)
		{
			int num = name.IndexOf(':');
			if (num < 0 && !considerDefaultNamespace)
			{
				return new XmlQualifiedName(name);
			}
			string prefix = (num >= 0) ? name.Substring(0, num) : string.Empty;
			string text = (num >= 0) ? name.Substring(num + 1) : name;
			string text2 = resolver.LookupNamespace(prefix);
			if (text2 == null)
			{
				throw new ArgumentException("Invalid qualified name.");
			}
			return new XmlQualifiedName(text, text2);
		}

		internal static XmlQualifiedName Parse(string name, XmlReader reader)
		{
			int num = name.IndexOf(':');
			if (num < 0)
			{
				return new XmlQualifiedName(name);
			}
			string text = reader.LookupNamespace(name.Substring(0, num));
			if (text == null)
			{
				throw new ArgumentException("Invalid qualified name.");
			}
			return new XmlQualifiedName(name.Substring(num + 1), text);
		}

		/// <summary>Compares two <see cref="T:System.Xml.XmlQualifiedName" /> objects.</summary>
		/// <returns>true if the two objects have the same name and namespace values; otherwise, false.</returns>
		/// <param name="a">An <see cref="T:System.Xml.XmlQualifiedName" /> to compare. </param>
		/// <param name="b">An <see cref="T:System.Xml.XmlQualifiedName" /> to compare. </param>
		public static bool operator ==(XmlQualifiedName a, XmlQualifiedName b)
		{
			return a == b || (a != null && b != null && (a.hash == b.hash && a.name == b.name) && a.ns == b.ns);
		}

		/// <summary>Compares two <see cref="T:System.Xml.XmlQualifiedName" /> objects.</summary>
		/// <returns>true if the name and namespace values for the two objects differ; otherwise, false.</returns>
		/// <param name="a">An <see cref="T:System.Xml.XmlQualifiedName" /> to compare. </param>
		/// <param name="b">An <see cref="T:System.Xml.XmlQualifiedName" /> to compare. </param>
		public static bool operator !=(XmlQualifiedName a, XmlQualifiedName b)
		{
			return !(a == b);
		}
	}
}
