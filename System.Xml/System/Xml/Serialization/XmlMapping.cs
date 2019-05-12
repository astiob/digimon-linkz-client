using System;
using System.Collections;

namespace System.Xml.Serialization
{
	/// <summary>Supports mappings between .NET Framework types and XML Schema data types. </summary>
	public abstract class XmlMapping
	{
		private ObjectMap map;

		private ArrayList relatedMaps;

		private SerializationFormat format;

		private SerializationSource source;

		internal string _elementName;

		internal string _namespace;

		private string key;

		internal XmlMapping()
		{
		}

		internal XmlMapping(string elementName, string ns)
		{
			this._elementName = elementName;
			this._namespace = ns;
		}

		/// <summary>Gets the name of the XSD element of the mapping.</summary>
		/// <returns>The XSD element name.</returns>
		[MonoTODO]
		public string XsdElementName
		{
			get
			{
				return this._elementName;
			}
		}

		/// <summary>Get the name of the mapped element.</summary>
		/// <returns>The name of the mapped element.</returns>
		public string ElementName
		{
			get
			{
				return this._elementName;
			}
		}

		/// <summary>Gets the namespace of the mapped element.</summary>
		/// <returns>The namespace of the mapped element.</returns>
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
		}

		/// <summary>Sets the key used to look up the mapping.</summary>
		/// <param name="key">A <see cref="T:System.String" /> that contains the lookup key.</param>
		public void SetKey(string key)
		{
			this.key = key;
		}

		internal string GetKey()
		{
			return this.key;
		}

		internal ObjectMap ObjectMap
		{
			get
			{
				return this.map;
			}
			set
			{
				this.map = value;
			}
		}

		internal ArrayList RelatedMaps
		{
			get
			{
				return this.relatedMaps;
			}
			set
			{
				this.relatedMaps = value;
			}
		}

		internal SerializationFormat Format
		{
			get
			{
				return this.format;
			}
			set
			{
				this.format = value;
			}
		}

		internal SerializationSource Source
		{
			get
			{
				return this.source;
			}
			set
			{
				this.source = value;
			}
		}
	}
}
