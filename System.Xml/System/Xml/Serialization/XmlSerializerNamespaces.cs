using System;
using System.Collections.Specialized;

namespace System.Xml.Serialization
{
	/// <summary>Contains the XML namespaces and prefixes that the <see cref="T:System.Xml.Serialization.XmlSerializer" /> uses to generate qualified names in an XML-document instance.</summary>
	public class XmlSerializerNamespaces
	{
		private ListDictionary namespaces;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> class.</summary>
		public XmlSerializerNamespaces()
		{
			this.namespaces = new ListDictionary();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> class.</summary>
		/// <param name="namespaces">An array of <see cref="T:System.Xml.XmlQualifiedName" /> objects. </param>
		public XmlSerializerNamespaces(XmlQualifiedName[] namespaces) : this()
		{
			foreach (XmlQualifiedName xmlQualifiedName in namespaces)
			{
				this.namespaces[xmlQualifiedName.Name] = xmlQualifiedName;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> class, using the specified instance of XmlSerializerNamespaces containing the collection of prefix and namespace pairs.</summary>
		/// <param name="namespaces">An instance of the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" />containing the namespace and prefix pairs. </param>
		public XmlSerializerNamespaces(XmlSerializerNamespaces namespaces) : this(namespaces.ToArray())
		{
		}

		/// <summary>Adds a prefix and namespace pair to an <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> object.</summary>
		/// <param name="prefix">The prefix associated with an XML namespace. </param>
		/// <param name="ns">An XML namespace. </param>
		public void Add(string prefix, string ns)
		{
			XmlQualifiedName xmlQualifiedName = new XmlQualifiedName(prefix, ns);
			this.namespaces[xmlQualifiedName.Name] = xmlQualifiedName;
		}

		/// <summary>Gets the array of prefix and namespace pairs in an <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> object.</summary>
		/// <returns>An array of <see cref="T:System.Xml.XmlQualifiedName" /> objects that are used as qualified names in an XML document.</returns>
		public XmlQualifiedName[] ToArray()
		{
			XmlQualifiedName[] array = new XmlQualifiedName[this.namespaces.Count];
			this.namespaces.Values.CopyTo(array, 0);
			return array;
		}

		/// <summary>Gets the number of prefix and namespace pairs in the collection.</summary>
		/// <returns>The number of prefix and namespace pairs in the collection.</returns>
		public int Count
		{
			get
			{
				return this.namespaces.Count;
			}
		}

		internal string GetPrefix(string Ns)
		{
			foreach (object obj in this.namespaces.Keys)
			{
				string text = (string)obj;
				if (Ns == ((XmlQualifiedName)this.namespaces[text]).Namespace)
				{
					return text;
				}
			}
			return null;
		}

		internal ListDictionary Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}
	}
}
