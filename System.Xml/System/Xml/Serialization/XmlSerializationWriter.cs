using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace System.Xml.Serialization
{
	/// <summary>Abstract class used for controlling serialization by the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class. </summary>
	public abstract class XmlSerializationWriter : XmlSerializationGeneratedCode
	{
		private const string xmlNamespace = "http://www.w3.org/2000/xmlns/";

		private const string unexpectedTypeError = "The type {0} was not expected. Use the XmlInclude or SoapInclude attribute to specify types that are not known statically.";

		private ObjectIDGenerator idGenerator;

		private int qnameCount;

		private bool topLevelElement;

		private ArrayList namespaces;

		private XmlWriter writer;

		private Queue referencedElements;

		private Hashtable callbacks;

		private Hashtable serializedObjects;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializationWriter" /> class. </summary>
		protected XmlSerializationWriter()
		{
			this.qnameCount = 0;
			this.serializedObjects = new Hashtable();
		}

		internal void Initialize(XmlWriter writer, XmlSerializerNamespaces nss)
		{
			this.writer = writer;
			if (nss != null)
			{
				this.namespaces = new ArrayList();
				foreach (XmlQualifiedName xmlQualifiedName in nss.ToArray())
				{
					if (xmlQualifiedName.Name != string.Empty && xmlQualifiedName.Namespace != string.Empty)
					{
						this.namespaces.Add(xmlQualifiedName);
					}
				}
			}
		}

		/// <summary>Gets or sets a list of XML qualified name objects that contain the namespaces and prefixes used to produce qualified names in XML documents. </summary>
		/// <returns>An <see cref="T:System.Collections.ArrayList" /> that contains the namespaces and prefix pairs.</returns>
		protected ArrayList Namespaces
		{
			get
			{
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		/// <summary>Gets the <see cref="T:System.Xml.XmlWriter" /> that is being used by the <see cref="T:System.Xml.Serialization.XmlSerializationWriter" />. </summary>
		/// <returns>The <see cref="T:System.Xml.XmlWriter" /> used by the class instance.</returns>
		protected XmlWriter Writer
		{
			get
			{
				return this.writer;
			}
			set
			{
				this.writer = value;
			}
		}

		/// <summary>Stores an implementation of the <see cref="T:System.Xml.Serialization.XmlSerializationWriteCallback" /> delegate and the type it applies to, for a later invocation. </summary>
		/// <param name="type">The <see cref="T:System.Type" /> of objects that are serialized.</param>
		/// <param name="typeName">The name of the type of objects that are serialized.</param>
		/// <param name="typeNs">The namespace of the type of objects that are serialized.</param>
		/// <param name="callback">An instance of the <see cref="T:System.Xml.Serialization.XmlSerializationWriteCallback" /> delegate.</param>
		protected void AddWriteCallback(Type type, string typeName, string typeNs, XmlSerializationWriteCallback callback)
		{
			XmlSerializationWriter.WriteCallbackInfo writeCallbackInfo = new XmlSerializationWriter.WriteCallbackInfo();
			writeCallbackInfo.Type = type;
			writeCallbackInfo.TypeName = typeName;
			writeCallbackInfo.TypeNs = typeNs;
			writeCallbackInfo.Callback = callback;
			if (this.callbacks == null)
			{
				this.callbacks = new Hashtable();
			}
			this.callbacks.Add(type, writeCallbackInfo);
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates an unexpected name for an element that adheres to an XML Schema choice element declaration.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="value">The name that is not valid.</param>
		/// <param name="identifier">The choice element declaration that the name belongs to.</param>
		/// <param name="name">The expected local name of an element.</param>
		/// <param name="ns">The expected namespace of an element.</param>
		protected Exception CreateChoiceIdentifierValueException(string value, string identifier, string name, string ns)
		{
			string message = string.Format("Value '{0}' of the choice identifier '{1}' does not match element '{2}' from namespace '{3}'.", new object[]
			{
				value,
				identifier,
				name,
				ns
			});
			return new InvalidOperationException(message);
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates a failure while writing an array where an XML Schema choice element declaration is applied.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="type">The type being serialized.</param>
		/// <param name="identifier">A name for the choice element declaration.</param>
		protected Exception CreateInvalidChoiceIdentifierValueException(string type, string identifier)
		{
			string message = string.Format("Invalid or missing choice identifier '{0}' of type '{1}'.", identifier, type);
			return new InvalidOperationException(message);
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates that a value for an XML element does not match an enumeration type.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="value">The value that is not valid.</param>
		/// <param name="elementName">The name of the XML element with an invalid value.</param>
		/// <param name="enumValue">The valid value.</param>
		protected Exception CreateMismatchChoiceException(string value, string elementName, string enumValue)
		{
			string message = string.Format("Value of {0} mismatches the type of {1}, you need to set it to {2}.", elementName, value, enumValue);
			return new InvalidOperationException(message);
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates that an XML element that should adhere to the XML Schema any element declaration cannot be processed.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="name">The XML element that cannot be processed.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		protected Exception CreateUnknownAnyElementException(string name, string ns)
		{
			string message = string.Format("The XML element named '{0}' from namespace '{1}' was not expected. The XML element name and namespace must match those provided via XmlAnyElementAttribute(s).", name, ns);
			return new InvalidOperationException(message);
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates that a type being serialized is not being used in a valid manner or is unexpectedly encountered. </summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="o">The object whose type cannot be serialized.</param>
		protected Exception CreateUnknownTypeException(object o)
		{
			return this.CreateUnknownTypeException(o.GetType());
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates that a type being serialized is not being used in a valid manner or is unexpectedly encountered. </summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="type">The type that cannot be serialized.</param>
		protected Exception CreateUnknownTypeException(Type type)
		{
			string message = string.Format("The type {0} may not be used in this context.", type);
			return new InvalidOperationException(message);
		}

		/// <summary>Processes a base-64 byte array.</summary>
		/// <returns>The same byte array that was passed in as an argument.</returns>
		/// <param name="value">A base-64 <see cref="T:System.Byte" /> array.</param>
		protected static byte[] FromByteArrayBase64(byte[] value)
		{
			return value;
		}

		/// <summary>Produces a string from an input hexadecimal byte array.</summary>
		/// <returns>The byte array value converted to a string.</returns>
		/// <param name="value">A hexadecimal byte array to translate to a string.</param>
		protected static string FromByteArrayHex(byte[] value)
		{
			return XmlCustomFormatter.FromByteArrayHex(value);
		}

		/// <summary>Produces a string from an input <see cref="T:System.Char" />.</summary>
		/// <returns>The <see cref="T:System.Char" /> value converted to a string.</returns>
		/// <param name="value">A <see cref="T:System.Char" /> to translate to a string.</param>
		protected static string FromChar(char value)
		{
			return XmlCustomFormatter.FromChar(value);
		}

		/// <summary>Produces a string from a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>A string representation of the <see cref="T:System.DateTime" /> that shows the date but no time.</returns>
		/// <param name="value">A <see cref="T:System.DateTime" /> to translate to a string.</param>
		protected static string FromDate(DateTime value)
		{
			return XmlCustomFormatter.FromDate(value);
		}

		/// <summary>Produces a string from an input <see cref="T:System.DateTime" />.</summary>
		/// <returns>A string representation of the <see cref="T:System.DateTime" /> that shows the date and time.</returns>
		/// <param name="value">A <see cref="T:System.DateTime" /> to translate to a string.</param>
		protected static string FromDateTime(DateTime value)
		{
			return XmlCustomFormatter.FromDateTime(value);
		}

		/// <summary>Produces a string that consists of delimited identifiers that represent the enumeration members that have been set.</summary>
		/// <returns>A string that consists of delimited identifiers, where each represents a member from the set enumerator list.</returns>
		/// <param name="value">The enumeration value as a series of bitwise OR operations.</param>
		/// <param name="values">The enumeration's name values.</param>
		/// <param name="ids">The enumeration's constant values.</param>
		protected static string FromEnum(long value, string[] values, long[] ids)
		{
			return XmlCustomFormatter.FromEnum(value, values, ids);
		}

		/// <summary>Produces a string from a <see cref="T:System.DateTime" /> object.</summary>
		/// <returns>A string representation of the <see cref="T:System.DateTime" /> object that shows the time but no date.</returns>
		/// <param name="value">A <see cref="T:System.DateTime" /> that is translated to a string.</param>
		protected static string FromTime(DateTime value)
		{
			return XmlCustomFormatter.FromTime(value);
		}

		/// <summary>Encodes a valid XML name by replacing characters that are not valid with escape sequences.</summary>
		/// <returns>An encoded string.</returns>
		/// <param name="name">A string to be used as an XML name.</param>
		protected static string FromXmlName(string name)
		{
			return XmlCustomFormatter.FromXmlName(name);
		}

		/// <summary>Encodes a valid XML local name by replacing characters that are not valid with escape sequences.</summary>
		/// <returns>An encoded string.</returns>
		/// <param name="ncName">A string to be used as a local (unqualified) XML name.</param>
		protected static string FromXmlNCName(string ncName)
		{
			return XmlCustomFormatter.FromXmlNCName(ncName);
		}

		/// <summary>Encodes an XML name.</summary>
		/// <returns>An encoded string.</returns>
		/// <param name="nmToken">An XML name to be encoded.</param>
		protected static string FromXmlNmToken(string nmToken)
		{
			return XmlCustomFormatter.FromXmlNmToken(nmToken);
		}

		/// <summary>Encodes a space-delimited sequence of XML names into a single XML name.</summary>
		/// <returns>An encoded string.</returns>
		/// <param name="nmTokens">A space-delimited sequence of XML names to be encoded.</param>
		protected static string FromXmlNmTokens(string nmTokens)
		{
			return XmlCustomFormatter.FromXmlNmTokens(nmTokens);
		}

		/// <summary>Returns an XML qualified name, with invalid characters replaced by escape sequences. </summary>
		/// <returns>An XML qualified name, with invalid characters replaced by escape sequences.</returns>
		/// <param name="xmlQualifiedName">An <see cref="T:System.Xml.XmlQualifiedName" /> that represents the XML to be written.</param>
		protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName)
		{
			if (xmlQualifiedName == null || xmlQualifiedName == XmlQualifiedName.Empty)
			{
				return null;
			}
			return this.GetQualifiedName(xmlQualifiedName.Name, xmlQualifiedName.Namespace);
		}

		private string GetId(object o, bool addToReferencesList)
		{
			if (this.idGenerator == null)
			{
				this.idGenerator = new ObjectIDGenerator();
			}
			bool flag;
			long id = this.idGenerator.GetId(o, out flag);
			return string.Format(CultureInfo.InvariantCulture, "id{0}", new object[]
			{
				id
			});
		}

		private bool AlreadyQueued(object ob)
		{
			if (this.idGenerator == null)
			{
				return false;
			}
			bool flag;
			this.idGenerator.HasId(ob, out flag);
			return !flag;
		}

		private string GetNamespacePrefix(string ns)
		{
			string text = this.Writer.LookupPrefix(ns);
			if (text == null)
			{
				text = string.Format(CultureInfo.InvariantCulture, "q{0}", new object[]
				{
					++this.qnameCount
				});
				this.WriteAttribute("xmlns", text, null, ns);
			}
			return text;
		}

		private string GetQualifiedName(string name, string ns)
		{
			if (ns == string.Empty)
			{
				return name;
			}
			string namespacePrefix = this.GetNamespacePrefix(ns);
			if (namespacePrefix == string.Empty)
			{
				return name;
			}
			return string.Format("{0}:{1}", namespacePrefix, name);
		}

		/// <summary>Initializes instances of the <see cref="T:System.Xml.Serialization.XmlSerializationWriteCallback" /> delegate to serialize SOAP-encoded XML data. </summary>
		protected abstract void InitCallbacks();

		/// <summary>Initializes object references only while serializing a SOAP-encoded SOAP message.</summary>
		protected void TopLevelElement()
		{
			this.topLevelElement = true;
		}

		/// <summary>Instructs an <see cref="T:System.Xml.XmlWriter" /> object to write an XML attribute that has no namespace specified for its name.</summary>
		/// <param name="localName">The local name of the XML attribute.</param>
		/// <param name="value">The value of the XML attribute as a byte array.</param>
		protected void WriteAttribute(string localName, byte[] value)
		{
			this.WriteAttribute(localName, string.Empty, value);
		}

		/// <summary>Instructs the <see cref="T:System.Xml.XmlWriter" /> to write an XML attribute that has no namespace specified for its name. </summary>
		/// <param name="localName">The local name of the XML attribute.</param>
		/// <param name="value">The value of the XML attribute as a string.</param>
		protected void WriteAttribute(string localName, string value)
		{
			this.WriteAttribute(string.Empty, localName, string.Empty, value);
		}

		/// <summary>Instructs an <see cref="T:System.Xml.XmlWriter" /> object to write an XML attribute.</summary>
		/// <param name="localName">The local name of the XML attribute.</param>
		/// <param name="ns">The namespace of the XML attribute.</param>
		/// <param name="value">The value of the XML attribute as a byte array.</param>
		protected void WriteAttribute(string localName, string ns, byte[] value)
		{
			if (value == null)
			{
				return;
			}
			this.Writer.WriteStartAttribute(localName, ns);
			this.WriteValue(value);
			this.Writer.WriteEndAttribute();
		}

		/// <summary>Writes an XML attribute. </summary>
		/// <param name="localName">The local name of the XML attribute.</param>
		/// <param name="ns">The namespace of the XML attribute.</param>
		/// <param name="value">The value of the XML attribute as a string.</param>
		protected void WriteAttribute(string localName, string ns, string value)
		{
			this.WriteAttribute(null, localName, ns, value);
		}

		/// <summary>Writes an XML attribute where the namespace prefix is provided manually. </summary>
		/// <param name="prefix">The namespace prefix to write.</param>
		/// <param name="localName">The local name of the XML attribute.</param>
		/// <param name="ns">The namespace represented by the prefix.</param>
		/// <param name="value">The value of the XML attribute as a string.</param>
		protected void WriteAttribute(string prefix, string localName, string ns, string value)
		{
			if (value == null)
			{
				return;
			}
			this.Writer.WriteAttributeString(prefix, localName, ns, value);
		}

		private void WriteXmlNode(XmlNode node)
		{
			if (node is XmlDocument)
			{
				node = ((XmlDocument)node).DocumentElement;
			}
			node.WriteTo(this.Writer);
		}

		/// <summary>Writes an XML node object within the body of a named XML element.</summary>
		/// <param name="node">The XML node to write, possibly a child XML element.</param>
		/// <param name="name">The local name of the parent XML element to write.</param>
		/// <param name="ns">The namespace of the parent XML element to write.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the object to serialize is null; otherwise, false.</param>
		/// <param name="any">true to indicate that the node, if an XML element, adheres to an XML Schema any element declaration; otherwise, false.</param>
		protected void WriteElementEncoded(XmlNode node, string name, string ns, bool isNullable, bool any)
		{
			if (name != string.Empty)
			{
				if (node == null)
				{
					if (isNullable)
					{
						this.WriteNullTagEncoded(name, ns);
					}
				}
				else
				{
					this.Writer.WriteStartElement(name, ns);
					this.WriteXmlNode(node);
					this.Writer.WriteEndElement();
				}
			}
			else
			{
				this.WriteXmlNode(node);
			}
		}

		/// <summary>Instructs an <see cref="T:System.Xml.XmlWriter" /> object to write an <see cref="T:System.Xml.XmlNode" /> object within the body of a named XML element.</summary>
		/// <param name="node">The XML node to write, possibly a child XML element.</param>
		/// <param name="name">The local name of the parent XML element to write.</param>
		/// <param name="ns">The namespace of the parent XML element to write.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the object to serialize is null; otherwise, false.</param>
		/// <param name="any">true to indicate that the node, if an XML element, adheres to an XML Schema any element declaration; otherwise, false.</param>
		protected void WriteElementLiteral(XmlNode node, string name, string ns, bool isNullable, bool any)
		{
			if (name != string.Empty)
			{
				if (node == null)
				{
					if (isNullable)
					{
						this.WriteNullTagLiteral(name, ns);
					}
				}
				else
				{
					this.Writer.WriteStartElement(name, ns);
					this.WriteXmlNode(node);
					this.Writer.WriteEndElement();
				}
			}
			else
			{
				this.WriteXmlNode(node);
			}
		}

		/// <summary>Writes an XML element with a specified qualified name in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The name to write, using its prefix if namespace-qualified, in the element text.</param>
		protected void WriteElementQualifiedName(string localName, XmlQualifiedName value)
		{
			this.WriteElementQualifiedName(localName, string.Empty, value, null);
		}

		/// <summary>Writes an XML element with a specified qualified name in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The name to write, using its prefix if namespace-qualified, in the element text.</param>
		protected void WriteElementQualifiedName(string localName, string ns, XmlQualifiedName value)
		{
			this.WriteElementQualifiedName(localName, ns, value, null);
		}

		/// <summary>Writes an XML element with a specified qualified name in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The name to write, using its prefix if namespace-qualified, in the element text.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementQualifiedName(string localName, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			this.WriteElementQualifiedName(localName, string.Empty, value, xsiType);
		}

		/// <summary>Writes an XML element with a specified qualified name in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The name to write, using its prefix if namespace-qualified, in the element text.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementQualifiedName(string localName, string ns, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			localName = XmlCustomFormatter.FromXmlNCName(localName);
			this.WriteStartElement(localName, ns);
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			this.Writer.WriteString(this.FromXmlQualifiedName(value));
			this.WriteEndElement();
		}

		/// <summary>Writes an XML element with a specified value in its body. </summary>
		/// <param name="localName">The local name of the XML element to be written without namespace qualification.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementString(string localName, string value)
		{
			this.WriteElementString(localName, string.Empty, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body. </summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementString(string localName, string ns, string value)
		{
			this.WriteElementString(localName, ns, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementString(string localName, string value, XmlQualifiedName xsiType)
		{
			this.WriteElementString(localName, string.Empty, value, xsiType);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementString(string localName, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			if (xsiType != null)
			{
				localName = XmlCustomFormatter.FromXmlNCName(localName);
				this.WriteStartElement(localName, ns);
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
				this.Writer.WriteString(value);
				this.WriteEndElement();
			}
			else
			{
				this.Writer.WriteElementString(localName, ns, value);
			}
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementStringRaw(string localName, byte[] value)
		{
			this.WriteElementStringRaw(localName, string.Empty, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementStringRaw(string localName, string value)
		{
			this.WriteElementStringRaw(localName, string.Empty, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementStringRaw(string localName, byte[] value, XmlQualifiedName xsiType)
		{
			this.WriteElementStringRaw(localName, string.Empty, value, xsiType);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementStringRaw(string localName, string ns, byte[] value)
		{
			this.WriteElementStringRaw(localName, ns, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		protected void WriteElementStringRaw(string localName, string ns, string value)
		{
			this.WriteElementStringRaw(localName, ns, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementStringRaw(string localName, string value, XmlQualifiedName xsiType)
		{
			this.WriteElementStringRaw(localName, string.Empty, value, null);
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementStringRaw(string localName, string ns, byte[] value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				return;
			}
			this.WriteStartElement(localName, ns);
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			if (value.Length > 0)
			{
				this.Writer.WriteBase64(value, 0, value.Length);
			}
			this.WriteEndElement();
		}

		/// <summary>Writes an XML element with a specified value in its body.</summary>
		/// <param name="localName">The local name of the XML element.</param>
		/// <param name="ns">The namespace of the XML element.</param>
		/// <param name="value">The text value of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteElementStringRaw(string localName, string ns, string value, XmlQualifiedName xsiType)
		{
			localName = XmlCustomFormatter.FromXmlNCName(localName);
			this.WriteStartElement(localName, ns);
			if (xsiType != null)
			{
				this.WriteXsiType(xsiType.Name, xsiType.Namespace);
			}
			this.Writer.WriteRaw(value);
			this.WriteEndElement();
		}

		/// <summary>Writes an XML element whose body is empty. </summary>
		/// <param name="name">The local name of the XML element to write.</param>
		protected void WriteEmptyTag(string name)
		{
			this.WriteEmptyTag(name, string.Empty);
		}

		/// <summary>Writes an XML element whose body is empty.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		protected void WriteEmptyTag(string name, string ns)
		{
			name = XmlCustomFormatter.FromXmlName(name);
			this.WriteStartElement(name, ns);
			this.WriteEndElement();
		}

		/// <summary>Writes a &lt;closing&gt; element tag.</summary>
		protected void WriteEndElement()
		{
			this.WriteEndElement(null);
		}

		/// <summary>Writes a closing element tag.</summary>
		/// <param name="o">The object being serialized.</param>
		protected void WriteEndElement(object o)
		{
			if (o != null)
			{
				this.serializedObjects.Remove(o);
			}
			this.Writer.WriteEndElement();
		}

		/// <summary>Writes an id attribute that appears in a SOAP-encoded multiRef element. </summary>
		/// <param name="o">The object being serialized.</param>
		protected void WriteId(object o)
		{
			this.WriteAttribute("id", this.GetId(o, true));
		}

		/// <summary>Writes namespace declaration attributes.</summary>
		/// <param name="xmlns">The XML namespaces to declare.</param>
		protected void WriteNamespaceDeclarations(XmlSerializerNamespaces ns)
		{
			if (ns == null)
			{
				return;
			}
			ICollection values = ns.Namespaces.Values;
			foreach (object obj in values)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
				if (xmlQualifiedName.Namespace != string.Empty && this.Writer.LookupPrefix(xmlQualifiedName.Namespace) != xmlQualifiedName.Name)
				{
					this.WriteAttribute("xmlns", xmlQualifiedName.Name, "http://www.w3.org/2000/xmlns/", xmlQualifiedName.Namespace);
				}
			}
		}

		/// <summary>Writes an XML element whose body contains a valid XML qualified name. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The XML qualified name to write in the body of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteNullableQualifiedNameEncoded(string name, string ns, XmlQualifiedName value, XmlQualifiedName xsiType)
		{
			if (value != null)
			{
				this.WriteElementQualifiedName(name, ns, value, xsiType);
			}
			else
			{
				this.WriteNullTagEncoded(name, ns);
			}
		}

		/// <summary>Writes an XML element whose body contains a valid XML qualified name. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The XML qualified name to write in the body of the XML element.</param>
		protected void WriteNullableQualifiedNameLiteral(string name, string ns, XmlQualifiedName value)
		{
			if (value != null)
			{
				this.WriteElementQualifiedName(name, ns, value);
			}
			else
			{
				this.WriteNullTagLiteral(name, ns);
			}
		}

		/// <summary>Writes an XML element that contains a string as the body. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The string to write in the body of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteNullableStringEncoded(string name, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value != null)
			{
				this.WriteElementString(name, ns, value, xsiType);
			}
			else
			{
				this.WriteNullTagEncoded(name, ns);
			}
		}

		/// <summary>Writes a byte array as the body of an XML element. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The byte array to write in the body of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteNullableStringEncodedRaw(string name, string ns, byte[] value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
			}
			else
			{
				this.WriteElementStringRaw(name, ns, value, xsiType);
			}
		}

		/// <summary>Writes an XML element that contains a string as the body. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The string to write in the body of the XML element.</param>
		/// <param name="xsiType">The name of the XML Schema data type to be written to the xsi:type attribute.</param>
		protected void WriteNullableStringEncodedRaw(string name, string ns, string value, XmlQualifiedName xsiType)
		{
			if (value == null)
			{
				this.WriteNullTagEncoded(name, ns);
			}
			else
			{
				this.WriteElementStringRaw(name, ns, value, xsiType);
			}
		}

		/// <summary>Writes an XML element that contains a string as the body. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The string to write in the body of the XML element.</param>
		protected void WriteNullableStringLiteral(string name, string ns, string value)
		{
			if (value != null)
			{
				this.WriteElementString(name, ns, value, null);
			}
			else
			{
				this.WriteNullTagLiteral(name, ns);
			}
		}

		/// <summary>Writes a byte array as the body of an XML element. <see cref="T:System.Xml.XmlWriter" /> inserts an xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The byte array to write in the body of the XML element.</param>
		protected void WriteNullableStringLiteralRaw(string name, string ns, byte[] value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
			}
			else
			{
				this.WriteElementStringRaw(name, ns, value);
			}
		}

		/// <summary>Writes an XML element that contains a string as the body. <see cref="T:System.Xml.XmlWriter" /> inserts a xsi:nil='true' attribute if the string's value is null.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="value">The string to write in the body of the XML element.</param>
		protected void WriteNullableStringLiteralRaw(string name, string ns, string value)
		{
			if (value == null)
			{
				this.WriteNullTagLiteral(name, ns);
			}
			else
			{
				this.WriteElementStringRaw(name, ns, value);
			}
		}

		/// <summary>Writes an XML element with an xsi:nil='true' attribute.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		protected void WriteNullTagEncoded(string name)
		{
			this.WriteNullTagEncoded(name, string.Empty);
		}

		/// <summary>Writes an XML element with an xsi:nil='true' attribute.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		protected void WriteNullTagEncoded(string name, string ns)
		{
			this.Writer.WriteStartElement(name, ns);
			this.Writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			this.Writer.WriteEndElement();
		}

		/// <summary>Writes an XML element with an xsi:nil='true' attribute.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		protected void WriteNullTagLiteral(string name)
		{
			this.WriteNullTagLiteral(name, string.Empty);
		}

		/// <summary>Writes an XML element with an xsi:nil='true' attribute. </summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		protected void WriteNullTagLiteral(string name, string ns)
		{
			this.WriteStartElement(name, ns);
			this.Writer.WriteAttributeString("nil", "http://www.w3.org/2001/XMLSchema-instance", "true");
			this.WriteEndElement();
		}

		/// <summary>Writes a SOAP message XML element that can contain a reference to a &lt;multiRef&gt; XML element for a given object. </summary>
		/// <param name="n">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized either in the current XML element or a multiRef element that is referenced by the current element.</param>
		protected void WritePotentiallyReferencingElement(string n, string ns, object o)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, null, false, false);
		}

		/// <summary>Writes a SOAP message XML element that can contain a reference to a &lt;multiRef&gt; XML element for a given object. </summary>
		/// <param name="n">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized either in the current XML element or a multiRef element that referenced by the current element.</param>
		/// <param name="ambientType">The type stored in the object's type mapping (as opposed to the object's type found directly through the typeof operation).</param>
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, ambientType, false, false);
		}

		/// <summary>Writes a SOAP message XML element that can contain a reference to a &lt;multiRef&gt; XML element for a given object.</summary>
		/// <param name="n">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized either in the current XML element or a multiRef element that is referenced by the current element.</param>
		/// <param name="ambientType">The type stored in the object's type mapping (as opposed to the object's type found directly through the typeof operation).</param>
		/// <param name="suppressReference">true to serialize the object directly into the XML element rather than make the element reference another element that contains the data; otherwise, false.</param>
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType, bool suppressReference)
		{
			this.WritePotentiallyReferencingElement(n, ns, o, ambientType, suppressReference, false);
		}

		/// <summary>Writes a SOAP message XML element that can contain a reference to a multiRef XML element for a given object.</summary>
		/// <param name="n">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized either in the current XML element or a multiRef element that referenced by the current element.</param>
		/// <param name="ambientType">The type stored in the object's type mapping (as opposed to the object's type found directly through the typeof operation).</param>
		/// <param name="suppressReference">true to serialize the object directly into the XML element rather than make the element reference another element that contains the data; otherwise, false.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the object to serialize is null; otherwise, false.</param>
		protected void WritePotentiallyReferencingElement(string n, string ns, object o, Type ambientType, bool suppressReference, bool isNullable)
		{
			if (o == null)
			{
				if (isNullable)
				{
					this.WriteNullTagEncoded(n, ns);
				}
				return;
			}
			this.WriteStartElement(n, ns, true);
			this.CheckReferenceQueue();
			if (this.callbacks != null && this.callbacks.ContainsKey(o.GetType()))
			{
				XmlSerializationWriter.WriteCallbackInfo writeCallbackInfo = (XmlSerializationWriter.WriteCallbackInfo)this.callbacks[o.GetType()];
				if (o.GetType().IsEnum)
				{
					writeCallbackInfo.Callback(o);
				}
				else if (suppressReference)
				{
					this.Writer.WriteAttributeString("id", this.GetId(o, false));
					if (ambientType != o.GetType())
					{
						this.WriteXsiType(writeCallbackInfo.TypeName, writeCallbackInfo.TypeNs);
					}
					writeCallbackInfo.Callback(o);
				}
				else
				{
					if (!this.AlreadyQueued(o))
					{
						this.referencedElements.Enqueue(o);
					}
					this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
				}
			}
			else
			{
				TypeData typeData = TypeTranslator.GetTypeData(o.GetType());
				if (typeData.SchemaType == SchemaTypes.Primitive)
				{
					this.WriteXsiType(typeData.XmlType, "http://www.w3.org/2001/XMLSchema");
					this.Writer.WriteString(XmlCustomFormatter.ToXmlString(typeData, o));
				}
				else
				{
					if (!this.IsPrimitiveArray(typeData))
					{
						throw new InvalidOperationException("Invalid type: " + o.GetType().FullName);
					}
					if (!this.AlreadyQueued(o))
					{
						this.referencedElements.Enqueue(o);
					}
					this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
				}
			}
			this.WriteEndElement();
		}

		/// <summary>Serializes objects into SOAP-encoded multiRef XML elements in a SOAP message. </summary>
		protected void WriteReferencedElements()
		{
			if (this.referencedElements == null)
			{
				return;
			}
			if (this.callbacks == null)
			{
				return;
			}
			while (this.referencedElements.Count > 0)
			{
				object obj = this.referencedElements.Dequeue();
				TypeData typeData = TypeTranslator.GetTypeData(obj.GetType());
				XmlSerializationWriter.WriteCallbackInfo writeCallbackInfo = (XmlSerializationWriter.WriteCallbackInfo)this.callbacks[obj.GetType()];
				if (writeCallbackInfo != null)
				{
					this.WriteStartElement(writeCallbackInfo.TypeName, writeCallbackInfo.TypeNs, true);
					this.Writer.WriteAttributeString("id", this.GetId(obj, false));
					if (typeData.SchemaType != SchemaTypes.Array)
					{
						this.WriteXsiType(writeCallbackInfo.TypeName, writeCallbackInfo.TypeNs);
					}
					writeCallbackInfo.Callback(obj);
					this.WriteEndElement();
				}
				else if (this.IsPrimitiveArray(typeData))
				{
					this.WriteArray(obj, typeData);
				}
			}
		}

		private bool IsPrimitiveArray(TypeData td)
		{
			return td.SchemaType == SchemaTypes.Array && (td.ListItemTypeData.SchemaType == SchemaTypes.Primitive || td.ListItemType == typeof(object) || this.IsPrimitiveArray(td.ListItemTypeData));
		}

		private void WriteArray(object o, TypeData td)
		{
			TypeData typeData = td;
			int num = -1;
			string text;
			do
			{
				typeData = typeData.ListItemTypeData;
				text = typeData.XmlType;
				num++;
			}
			while (typeData.SchemaType == SchemaTypes.Array);
			while (num-- > 0)
			{
				text += "[]";
			}
			this.WriteStartElement("Array", "http://schemas.xmlsoap.org/soap/encoding/", true);
			this.Writer.WriteAttributeString("id", this.GetId(o, false));
			if (td.SchemaType == SchemaTypes.Array)
			{
				Array array = (Array)o;
				int length = array.Length;
				this.Writer.WriteAttributeString("arrayType", "http://schemas.xmlsoap.org/soap/encoding/", this.GetQualifiedName(text, "http://www.w3.org/2001/XMLSchema") + "[" + length.ToString() + "]");
				for (int i = 0; i < length; i++)
				{
					this.WritePotentiallyReferencingElement("Item", string.Empty, array.GetValue(i), td.ListItemType, false, true);
				}
			}
			this.WriteEndElement();
		}

		/// <summary>Writes a SOAP message XML element that contains a reference to a multiRef element for a given object. </summary>
		/// <param name="n">The local name of the referencing element being written.</param>
		/// <param name="ns">The namespace of the referencing element being written.</param>
		/// <param name="o">The object being serialized.</param>
		protected void WriteReferencingElement(string n, string ns, object o)
		{
			this.WriteReferencingElement(n, ns, o, false);
		}

		/// <summary>Writes a SOAP message XML element that contains a reference to a multiRef element for a given object.</summary>
		/// <param name="n">The local name of the referencing element being written.</param>
		/// <param name="ns">The namespace of the referencing element being written.</param>
		/// <param name="o">The object being serialized.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the object to serialize is null; otherwise, false.</param>
		protected void WriteReferencingElement(string n, string ns, object o, bool isNullable)
		{
			if (o == null)
			{
				if (isNullable)
				{
					this.WriteNullTagEncoded(n, ns);
				}
				return;
			}
			this.CheckReferenceQueue();
			if (!this.AlreadyQueued(o))
			{
				this.referencedElements.Enqueue(o);
			}
			this.Writer.WriteStartElement(n, ns);
			this.Writer.WriteAttributeString("href", "#" + this.GetId(o, true));
			this.Writer.WriteEndElement();
		}

		private void CheckReferenceQueue()
		{
			if (this.referencedElements == null)
			{
				this.referencedElements = new Queue();
				this.InitCallbacks();
			}
		}

		/// <summary>Writes a SOAP 1.2 RPC result element with a specified qualified name in its body.</summary>
		/// <param name="name">The local name of the result body.</param>
		/// <param name="ns">The namespace of the result body.</param>
		[MonoTODO]
		protected void WriteRpcResult(string name, string ns)
		{
			throw new NotImplementedException();
		}

		/// <summary>Writes an object that uses custom XML formatting as an XML element. </summary>
		/// <param name="serializable">An object that implements the <see cref="T:System.Xml.Serialization.IXmlSerializable" /> interface that uses custom XML formatting.</param>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the <see cref="T:System.Xml.Serialization.IXmlSerializable" /> class object is null; otherwise, false.</param>
		protected void WriteSerializable(IXmlSerializable serializable, string name, string ns, bool isNullable)
		{
			this.WriteSerializable(serializable, name, ns, isNullable, true);
		}

		/// <summary>Instructs <see cref="T:System.Xml.XmlNode" /> to write an object that uses custom XML formatting as an XML element. </summary>
		/// <param name="serializable">An object that implements the <see cref="T:System.Xml.Serialization.IXmlSerializable" /> interface that uses custom XML formatting.</param>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="isNullable">true to write an xsi:nil='true' attribute if the <see cref="T:System.Xml.Serialization.IXmlSerializable" /> object is null; otherwise, false.</param>
		/// <param name="wrapped">true to ignore writing the opening element tag; otherwise, false to write the opening element tag.</param>
		protected void WriteSerializable(IXmlSerializable serializable, string name, string ns, bool isNullable, bool wrapped)
		{
			if (serializable == null)
			{
				if (isNullable && wrapped)
				{
					this.WriteNullTagLiteral(name, ns);
				}
				return;
			}
			if (wrapped)
			{
				this.Writer.WriteStartElement(name, ns);
			}
			serializable.WriteXml(this.Writer);
			if (wrapped)
			{
				this.Writer.WriteEndElement();
			}
		}

		/// <summary>Writes the XML declaration if the writer is positioned at the start of an XML document. </summary>
		protected void WriteStartDocument()
		{
			if (this.Writer.WriteState == WriteState.Start)
			{
				this.Writer.WriteStartDocument();
			}
		}

		/// <summary>Writes an opening element tag, including any attributes. </summary>
		/// <param name="name">The local name of the XML element to write.</param>
		protected void WriteStartElement(string name)
		{
			this.WriteStartElement(name, string.Empty, null, false);
		}

		/// <summary>Writes an opening element tag, including any attributes. </summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		protected void WriteStartElement(string name, string ns)
		{
			this.WriteStartElement(name, ns, null, false);
		}

		/// <summary>Writes an opening element tag, including any attributes.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="writePrefixed">true to write the element name with a prefix if none is available for the specified namespace; otherwise, false.</param>
		protected void WriteStartElement(string name, string ns, bool writePrefixed)
		{
			this.WriteStartElement(name, ns, null, writePrefixed);
		}

		/// <summary>Writes an opening element tag, including any attributes.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized as an XML element.</param>
		protected void WriteStartElement(string name, string ns, object o)
		{
			this.WriteStartElement(name, ns, o, false);
		}

		/// <summary>Writes an opening element tag, including any attributes.</summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized as an XML element.</param>
		/// <param name="writePrefixed">true to write the element name with a prefix if none is available for the specified namespace; otherwise, false.</param>
		protected void WriteStartElement(string name, string ns, object o, bool writePrefixed)
		{
			this.WriteStartElement(name, ns, o, writePrefixed, this.namespaces);
		}

		/// <summary>Writes an opening element tag, including any attributes. </summary>
		/// <param name="name">The local name of the XML element to write.</param>
		/// <param name="ns">The namespace of the XML element to write.</param>
		/// <param name="o">The object being serialized as an XML element.</param>
		/// <param name="writePrefixed">true to write the element name with a prefix if none is available for the specified namespace; otherwise, false.</param>
		/// <param name="xmlns">An instance of the <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> class that contains prefix and namespace pairs to be used in the generated XML.</param>
		protected void WriteStartElement(string name, string ns, object o, bool writePrefixed, XmlSerializerNamespaces xmlns)
		{
			if (xmlns == null)
			{
				throw new ArgumentNullException("xmlns");
			}
			this.WriteStartElement(name, ns, o, writePrefixed, xmlns.ToArray());
		}

		private void WriteStartElement(string name, string ns, object o, bool writePrefixed, ICollection namespaces)
		{
			if (o != null)
			{
				if (this.serializedObjects.Contains(o))
				{
					throw new InvalidOperationException("A circular reference was detected while serializing an object of type " + o.GetType().Name);
				}
				this.serializedObjects[o] = o;
			}
			string text = null;
			if (this.topLevelElement && ns != null && ns.Length != 0)
			{
				foreach (object obj in namespaces)
				{
					XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)obj;
					if (xmlQualifiedName.Namespace == ns)
					{
						text = xmlQualifiedName.Name;
						writePrefixed = true;
						break;
					}
				}
			}
			if (writePrefixed && ns != string.Empty)
			{
				name = XmlCustomFormatter.FromXmlName(name);
				if (text == null)
				{
					text = this.Writer.LookupPrefix(ns);
				}
				if (text == null || text.Length == 0)
				{
					text = "q" + ++this.qnameCount;
				}
				this.Writer.WriteStartElement(text, name, ns);
			}
			else
			{
				this.Writer.WriteStartElement(name, ns);
			}
			if (this.topLevelElement)
			{
				if (namespaces != null)
				{
					foreach (object obj2 in namespaces)
					{
						XmlQualifiedName xmlQualifiedName2 = (XmlQualifiedName)obj2;
						string text2 = this.Writer.LookupPrefix(xmlQualifiedName2.Namespace);
						if (text2 == null || text2.Length == 0)
						{
							this.WriteAttribute("xmlns", xmlQualifiedName2.Name, "http://www.w3.org/2000/xmlns/", xmlQualifiedName2.Namespace);
						}
					}
				}
				this.topLevelElement = false;
			}
		}

		/// <summary>Writes an XML element whose text body is a value of a simple XML Schema data type. </summary>
		/// <param name="name">The local name of the element to write.</param>
		/// <param name="ns">The namespace of the element to write.</param>
		/// <param name="o">The object to be serialized in the element body.</param>
		/// <param name="xsiType">true if the XML element explicitly specifies the text value's type using the xsi:type attribute; otherwise, false.</param>
		protected void WriteTypedPrimitive(string name, string ns, object o, bool xsiType)
		{
			TypeData typeData = TypeTranslator.GetTypeData(o.GetType());
			if (typeData.SchemaType != SchemaTypes.Primitive)
			{
				throw new InvalidOperationException(string.Format("The type of the argument object '{0}' is not primitive.", typeData.FullTypeName));
			}
			if (name == null)
			{
				ns = ((!typeData.IsXsdType) ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema");
				name = typeData.XmlType;
			}
			else
			{
				name = XmlCustomFormatter.FromXmlName(name);
			}
			this.Writer.WriteStartElement(name, ns);
			string value;
			if (o is XmlQualifiedName)
			{
				value = this.FromXmlQualifiedName((XmlQualifiedName)o);
			}
			else
			{
				value = XmlCustomFormatter.ToXmlString(typeData, o);
			}
			if (xsiType)
			{
				if (typeData.SchemaType != SchemaTypes.Primitive)
				{
					throw new InvalidOperationException(string.Format("The type {0} was not expected. Use the XmlInclude or SoapInclude attribute to specify types that are not known statically.", o.GetType().FullName));
				}
				this.WriteXsiType(typeData.XmlType, (!typeData.IsXsdType) ? "http://microsoft.com/wsdl/types/" : "http://www.w3.org/2001/XMLSchema");
			}
			this.WriteValue(value);
			this.Writer.WriteEndElement();
		}

		/// <summary>Writes a base-64 byte array.</summary>
		/// <param name="value">The byte array to write.</param>
		protected void WriteValue(byte[] value)
		{
			this.Writer.WriteBase64(value, 0, value.Length);
		}

		/// <summary>Writes a specified string.</summary>
		/// <param name="value">The string to write.</param>
		protected void WriteValue(string value)
		{
			if (value != null)
			{
				this.Writer.WriteString(value);
			}
		}

		/// <summary>Writes the specified <see cref="T:System.Xml.XmlNode" /> as an XML attribute.</summary>
		/// <param name="node">An <see cref="T:System.Xml.XmlAttribute" /> object.</param>
		protected void WriteXmlAttribute(XmlNode node)
		{
			this.WriteXmlAttribute(node, null);
		}

		/// <summary>Writes the specified <see cref="T:System.Xml.XmlNode" /> object as an XML attribute.</summary>
		/// <param name="node">An <see cref="T:System.Xml.XmlNode" /> of <see cref="T:System.Xml.XmlAttribute" /> type.</param>
		/// <param name="container">An <see cref="T:System.Xml.Schema.XmlSchemaObject" /> object (or null) used to generate a qualified name value for an arrayType attribute from the Web Services Description Language (WSDL) namespace ("http://schemas.xmlsoap.org/wsdl/").</param>
		protected void WriteXmlAttribute(XmlNode node, object container)
		{
			XmlAttribute xmlAttribute = node as XmlAttribute;
			if (xmlAttribute == null)
			{
				throw new InvalidOperationException("The node must be either type XmlAttribute or a derived type.");
			}
			if (xmlAttribute.NamespaceURI == "http://schemas.xmlsoap.org/wsdl/" && xmlAttribute.LocalName == "arrayType")
			{
				string str;
				string ns;
				string str2;
				TypeTranslator.ParseArrayType(xmlAttribute.Value, out str, out ns, out str2);
				string qualifiedName = this.GetQualifiedName(str + str2, ns);
				this.WriteAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, qualifiedName);
				return;
			}
			this.WriteAttribute(xmlAttribute.Prefix, xmlAttribute.LocalName, xmlAttribute.NamespaceURI, xmlAttribute.Value);
		}

		/// <summary>Writes an xsi:type attribute for an XML element that is being serialized into a document. </summary>
		/// <param name="name">The local name of an XML Schema data type.</param>
		/// <param name="ns">The namespace of an XML Schema data type.</param>
		protected void WriteXsiType(string name, string ns)
		{
			if (ns != null && ns != string.Empty)
			{
				this.WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", this.GetQualifiedName(name, ns));
			}
			else
			{
				this.WriteAttribute("type", "http://www.w3.org/2001/XMLSchema-instance", name);
			}
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> has been invalidly applied to a member; only members that are of type <see cref="T:System.Xml.XmlNode" />, or derived from <see cref="T:System.Xml.XmlNode" />, are valid.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="o">The object that represents the invalid member.</param>
		protected Exception CreateInvalidAnyTypeException(object o)
		{
			if (o == null)
			{
				return new InvalidOperationException("null is invalid as anyType in XmlSerializer");
			}
			return this.CreateInvalidAnyTypeException(o.GetType());
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> that indicates the <see cref="T:System.Xml.Serialization.XmlAnyElementAttribute" /> has been invalidly applied to a member; only members that are of type <see cref="T:System.Xml.XmlNode" />, or derived from <see cref="T:System.Xml.XmlNode" />, are valid.</summary>
		/// <returns>An <see cref="T:System.InvalidOperationException" /> exception.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> that is invalid.</param>
		protected Exception CreateInvalidAnyTypeException(Type t)
		{
			return new InvalidOperationException(string.Format("An object of type '{0}' is invalid as anyType in XmlSerializer", t));
		}

		/// <summary>Creates an <see cref="T:System.InvalidOperationException" /> for an invalid enumeration value.</summary>
		/// <returns>An <see cref="T:System.ComponentModel.InvalidEnumArgumentException" />.</returns>
		/// <param name="value">An object that represents the invalid enumeration.</param>
		/// <param name="typeName">The XML type name.</param>
		protected Exception CreateInvalidEnumValueException(object value, string typeName)
		{
			return new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "'{0}' is not a valid value for {1}.", new object[]
			{
				value,
				typeName
			}));
		}

		/// <summary>Takes a numeric enumeration value and the names and constants from the enumerator list for the enumeration and returns a string that consists of delimited identifiers that represent the enumeration members that have been set.</summary>
		/// <returns>A string that consists of delimited identifiers, where each item is one of the values set by the bitwise operation.</returns>
		/// <param name="value">The enumeration value as a series of bitwise OR operations.</param>
		/// <param name="values">The values of the enumeration.</param>
		/// <param name="ids">The constants of the enumeration.</param>
		/// <param name="typeName">The name of the type </param>
		protected static string FromEnum(long value, string[] values, long[] ids, string typeName)
		{
			return XmlCustomFormatter.FromEnum(value, values, ids, typeName);
		}

		/// <summary>Produces a string that can be written as an XML qualified name, with invalid characters replaced by escape sequences. </summary>
		/// <returns>An XML qualified name, with invalid characters replaced by escape sequences.</returns>
		/// <param name="xmlQualifiedName">An <see cref="T:System.Xml.XmlQualifiedName" /> that represents the XML to be written.</param>
		/// <param name="ignoreEmpty">true to ignore empty spaces in the string; otherwise, false.</param>
		[MonoTODO]
		protected string FromXmlQualifiedName(XmlQualifiedName xmlQualifiedName, bool ignoreEmpty)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets a dynamically generated assembly by name.</summary>
		/// <returns>A dynamically generated assembly.</returns>
		/// <param name="assemblyFullName">The full name of the assembly.</param>
		[MonoTODO]
		protected static Assembly ResolveDynamicAssembly(string assemblyFullName)
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets or sets a value that indicates whether the <see cref="M:System.Xml.XmlConvert.EncodeName(System.String)" /> method is used to write valid XML.</summary>
		/// <returns>true if the <see cref="M:System.Xml.Serialization.XmlSerializationWriter.FromXmlQualifiedName(System.Xml.XmlQualifiedName)" /> method returns an encoded name; otherwise, false.</returns>
		[MonoTODO]
		protected bool EscapeName
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		private class WriteCallbackInfo
		{
			public Type Type;

			public string TypeName;

			public string TypeNs;

			public XmlSerializationWriteCallback Callback;
		}
	}
}
