using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;

namespace System.Xml.Serialization
{
	/// <summary>Serializes and deserializes objects into and from XML documents. The <see cref="T:System.Xml.Serialization.XmlSerializer" /> enables you to control how objects are encoded into XML.</summary>
	public class XmlSerializer
	{
		internal const string WsdlNamespace = "http://schemas.xmlsoap.org/wsdl/";

		internal const string EncodingNamespace = "http://schemas.xmlsoap.org/soap/encoding/";

		internal const string WsdlTypesNamespace = "http://microsoft.com/wsdl/types/";

		private static int generationThreshold;

		private static bool backgroundGeneration = true;

		private static bool deleteTempFiles = true;

		private static bool generatorFallback = true;

		private bool customSerializer;

		private XmlMapping typeMapping;

		private XmlSerializer.SerializerData serializerData;

		private static Hashtable serializerTypes = new Hashtable();

		private XmlAttributeEventHandler onUnknownAttribute;

		private XmlElementEventHandler onUnknownElement;

		private XmlNodeEventHandler onUnknownNode;

		private UnreferencedObjectEventHandler onUnreferencedObject;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class.</summary>
		protected XmlSerializer()
		{
			this.customSerializer = true;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML documents, and deserialize XML documents into objects of the specified type.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize. </param>
		public XmlSerializer(Type type) : this(type, null, null, null, null)
		{
		}

		/// <summary>Initializes an instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class using an object that maps one type to another.</summary>
		/// <param name="xmlTypeMapping">An <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> that maps one type to another. </param>
		public XmlSerializer(XmlTypeMapping xmlTypeMapping)
		{
			this.typeMapping = xmlTypeMapping;
		}

		internal XmlSerializer(XmlMapping mapping, XmlSerializer.SerializerData data)
		{
			this.typeMapping = mapping;
			this.serializerData = data;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML documents, and deserialize XML documents into objects of the specified type. Specifies the default namespace for all the XML elements.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize. </param>
		/// <param name="defaultNamespace">The default namespace to use for all the XML elements. </param>
		public XmlSerializer(Type type, string defaultNamespace) : this(type, null, null, null, defaultNamespace)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML documents, and deserialize XML documents into object of a specified type. If a property or field returns an array, the <paramref name="extraTypes" /> parameter specifies objects that can be inserted into the array.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize. </param>
		/// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize. </param>
		public XmlSerializer(Type type, Type[] extraTypes) : this(type, null, extraTypes, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML documents, and deserialize XML documents into objects of the specified type. Each object to be serialized can itself contain instances of classes, which this overload can override with other classes.</summary>
		/// <param name="type">The type of the object to serialize. </param>
		/// <param name="overrides">An <see cref="T:System.Xml.Serialization.XmlAttributeOverrides" />. </param>
		public XmlSerializer(Type type, XmlAttributeOverrides overrides) : this(type, overrides, null, null, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML documents, and deserialize an XML document into object of the specified type. It also specifies the class to use as the XML root element.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize. </param>
		/// <param name="root">An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> that represents the XML root element. </param>
		public XmlSerializer(Type type, XmlRootAttribute root) : this(type, null, null, root, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of type <see cref="T:System.Object" /> into XML document instances, and deserialize XML document instances into objects of type <see cref="T:System.Object" />. Each object to be serialized can itself contain instances of classes, which this overload overrides with other classes. This overload also specifies the default namespace for all the XML elements and the class to use as the XML root element.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize. </param>
		/// <param name="overrides">An <see cref="T:System.Xml.Serialization.XmlAttributeOverrides" /> that extends or overrides the behavior of the class specified in the <paramref name="type" /> parameter. </param>
		/// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize. </param>
		/// <param name="root">An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> that defines the XML root element properties. </param>
		/// <param name="defaultNamespace">The default namespace of all XML elements in the XML document. </param>
		public XmlSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			XmlReflectionImporter xmlReflectionImporter = new XmlReflectionImporter(overrides, defaultNamespace);
			if (extraTypes != null)
			{
				foreach (Type type2 in extraTypes)
				{
					xmlReflectionImporter.IncludeType(type2);
				}
			}
			this.typeMapping = xmlReflectionImporter.ImportTypeMapping(type, root, defaultNamespace);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class that can serialize objects of the specified type into XML document instances, and deserialize XML document instances into objects of the specified type. This overload allows you to supply other types that can be encountered during a serialization or deserialization operation, as well as a default namespace for all XML elements, the class to use as the XML root element, its location, and credentials required for access.</summary>
		/// <param name="type">The type of the object that this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can serialize.</param>
		/// <param name="overrides">An <see cref="T:System.Xml.Serialization.XmlAttributeOverrides" /> that extends or overrides the behavior of the class specified in the <paramref name="type" /> parameter.</param>
		/// <param name="extraTypes">A <see cref="T:System.Type" /> array of additional object types to serialize.</param>
		/// <param name="root">An <see cref="T:System.Xml.Serialization.XmlRootAttribute" /> that defines the XML root element properties.</param>
		/// <param name="defaultNamespace">The default namespace of all XML elements in the XML document.</param>
		/// <param name="location">The location of the types.</param>
		/// <param name="evidence">An instance of the <see cref="T:System.Security.Policy.Evidence" /> class that contains credentials required to access types.</param>
		[MonoTODO]
		public XmlSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location, Evidence evidence)
		{
		}

		static XmlSerializer()
		{
			string text = null;
			XmlSerializer.generationThreshold = -1;
			XmlSerializer.backgroundGeneration = false;
			XmlSerializer.deleteTempFiles = (text == null || text == "no");
		}

		/// <summary>Occurs when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> encounters an XML attribute of unknown type during deserialization.</summary>
		public event XmlAttributeEventHandler UnknownAttribute
		{
			add
			{
				this.onUnknownAttribute = (XmlAttributeEventHandler)Delegate.Combine(this.onUnknownAttribute, value);
			}
			remove
			{
				this.onUnknownAttribute = (XmlAttributeEventHandler)Delegate.Remove(this.onUnknownAttribute, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> encounters an XML element of unknown type during deserialization.</summary>
		public event XmlElementEventHandler UnknownElement
		{
			add
			{
				this.onUnknownElement = (XmlElementEventHandler)Delegate.Combine(this.onUnknownElement, value);
			}
			remove
			{
				this.onUnknownElement = (XmlElementEventHandler)Delegate.Remove(this.onUnknownElement, value);
			}
		}

		/// <summary>Occurs when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> encounters an XML node of unknown type during deserialization.</summary>
		public event XmlNodeEventHandler UnknownNode
		{
			add
			{
				this.onUnknownNode = (XmlNodeEventHandler)Delegate.Combine(this.onUnknownNode, value);
			}
			remove
			{
				this.onUnknownNode = (XmlNodeEventHandler)Delegate.Remove(this.onUnknownNode, value);
			}
		}

		/// <summary>Occurs during deserialization of a SOAP-encoded XML stream, when the <see cref="T:System.Xml.Serialization.XmlSerializer" /> encounters a recognized type that is not used or is unreferenced.</summary>
		public event UnreferencedObjectEventHandler UnreferencedObject
		{
			add
			{
				this.onUnreferencedObject = (UnreferencedObjectEventHandler)Delegate.Combine(this.onUnreferencedObject, value);
			}
			remove
			{
				this.onUnreferencedObject = (UnreferencedObjectEventHandler)Delegate.Remove(this.onUnreferencedObject, value);
			}
		}

		internal XmlMapping Mapping
		{
			get
			{
				return this.typeMapping;
			}
		}

		internal virtual void OnUnknownAttribute(XmlAttributeEventArgs e)
		{
			if (this.onUnknownAttribute != null)
			{
				this.onUnknownAttribute(this, e);
			}
		}

		internal virtual void OnUnknownElement(XmlElementEventArgs e)
		{
			if (this.onUnknownElement != null)
			{
				this.onUnknownElement(this, e);
			}
		}

		internal virtual void OnUnknownNode(XmlNodeEventArgs e)
		{
			if (this.onUnknownNode != null)
			{
				this.onUnknownNode(this, e);
			}
		}

		internal virtual void OnUnreferencedObject(UnreferencedObjectEventArgs e)
		{
			if (this.onUnreferencedObject != null)
			{
				this.onUnreferencedObject(this, e);
			}
		}

		/// <summary>Gets a value that indicates whether this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can deserialize a specified XML document.</summary>
		/// <returns>true if this <see cref="T:System.Xml.Serialization.XmlSerializer" /> can deserialize the object that the <see cref="T:System.Xml.XmlReader" /> points to; otherwise, false.</returns>
		/// <param name="xmlReader">An <see cref="T:System.Xml.XmlReader" /> that points to the document to deserialize. </param>
		public virtual bool CanDeserialize(XmlReader xmlReader)
		{
			xmlReader.MoveToContent();
			return this.typeMapping is XmlMembersMapping || ((XmlTypeMapping)this.typeMapping).ElementName == xmlReader.LocalName;
		}

		/// <summary>Returns an object used to read the XML document to be serialized.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlSerializationReader" /> used to read the XML document.</returns>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		protected virtual XmlSerializationReader CreateReader()
		{
			throw new NotImplementedException();
		}

		/// <summary>When overridden in a derived class, returns a writer used to serialize the object.</summary>
		/// <returns>An instance that implements the <see cref="T:System.Xml.Serialization.XmlSerializationWriter" /> class.</returns>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		protected virtual XmlSerializationWriter CreateWriter()
		{
			throw new NotImplementedException();
		}

		/// <summary>Deserializes the XML document contained by the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> being deserialized.</returns>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> that contains the XML document to deserialize. </param>
		public object Deserialize(Stream stream)
		{
			return this.Deserialize(new XmlTextReader(stream)
			{
				Normalization = true,
				WhitespaceHandling = WhitespaceHandling.Significant
			});
		}

		/// <summary>Deserializes the XML document contained by the specified <see cref="T:System.IO.TextReader" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> being deserialized.</returns>
		/// <param name="textReader">The <see cref="T:System.IO.TextReader" /> that contains the XML document to deserialize. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during deserialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public object Deserialize(TextReader textReader)
		{
			return this.Deserialize(new XmlTextReader(textReader)
			{
				Normalization = true,
				WhitespaceHandling = WhitespaceHandling.Significant
			});
		}

		/// <summary>Deserializes the XML document contained by the specified <see cref="T:System.xml.XmlReader" />.</summary>
		/// <returns>The <see cref="T:System.Object" /> being deserialized.</returns>
		/// <param name="xmlReader">The <see cref="T:System.xml.XmlReader" /> that contains the XML document to deserialize. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during deserialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public object Deserialize(XmlReader xmlReader)
		{
			XmlSerializationReader xmlSerializationReader;
			if (this.customSerializer)
			{
				xmlSerializationReader = this.CreateReader();
			}
			else
			{
				xmlSerializationReader = this.CreateReader(this.typeMapping);
			}
			xmlSerializationReader.Initialize(xmlReader, this);
			return this.Deserialize(xmlSerializationReader);
		}

		/// <summary>Deserializes the XML document contained by the specified <see cref="T:System.Xml.Serialization.XmlSerializationReader" />.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="reader">The <see cref="T:System.Xml.Serialization.XmlSerializationReader" /> that contains the XML document to deserialize. </param>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		protected virtual object Deserialize(XmlSerializationReader reader)
		{
			if (this.customSerializer)
			{
				throw new NotImplementedException();
			}
			object result;
			try
			{
				if (reader is XmlSerializationReaderInterpreter)
				{
					result = ((XmlSerializationReaderInterpreter)reader).ReadRoot();
				}
				else
				{
					result = this.serializerData.ReaderMethod.Invoke(reader, null);
				}
			}
			catch (Exception ex)
			{
				if (ex is InvalidOperationException || ex is InvalidCastException)
				{
					throw new InvalidOperationException("There is an error in XML document.", ex);
				}
				throw;
			}
			return result;
		}

		/// <summary>Returns an array of <see cref="T:System.Xml.Serialization.XmlSerializer" /> objects created from an array of <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> objects.</summary>
		/// <returns>An array of <see cref="T:System.Xml.Serialization.XmlSerializer" /> objects.</returns>
		/// <param name="mappings">An array of <see cref="T:System.Xml.Serialization.XmlTypeMapping" /> that maps one type to another. </param>
		public static XmlSerializer[] FromMappings(XmlMapping[] mappings)
		{
			XmlSerializer[] array = new XmlSerializer[mappings.Length];
			XmlSerializer.SerializerData[] array2 = new XmlSerializer.SerializerData[mappings.Length];
			XmlSerializer.GenerationBatch generationBatch = new XmlSerializer.GenerationBatch();
			generationBatch.Maps = mappings;
			generationBatch.Datas = array2;
			for (int i = 0; i < mappings.Length; i++)
			{
				if (mappings[i] != null)
				{
					XmlSerializer.SerializerData serializerData = new XmlSerializer.SerializerData();
					serializerData.Batch = generationBatch;
					array[i] = new XmlSerializer(mappings[i], serializerData);
					array2[i] = serializerData;
				}
			}
			return array;
		}

		/// <summary>Returns an array of <see cref="T:System.Xml.Serialization.XmlSerializer" /> objects created from an array of types.</summary>
		/// <returns>An array of <see cref="T:System.Xml.Serialization.XmlSerializer" /> objects.</returns>
		/// <param name="types">An array of <see cref="T:System.Type" /> objects. </param>
		public static XmlSerializer[] FromTypes(Type[] mappings)
		{
			XmlSerializer[] array = new XmlSerializer[mappings.Length];
			for (int i = 0; i < mappings.Length; i++)
			{
				array[i] = new XmlSerializer(mappings[i]);
			}
			return array;
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.Xml.Serialization.XmlSerializationWriter" />.</summary>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <param name="writer">The <see cref="T:System.Xml.Serialization.XmlSerializationWriter" /> used to write the XML document. </param>
		/// <exception cref="T:System.NotImplementedException">Any attempt is made to access the method when the method is not overridden in a descendant class. </exception>
		protected virtual void Serialize(object o, XmlSerializationWriter writer)
		{
			if (this.customSerializer)
			{
				throw new NotImplementedException();
			}
			if (writer is XmlSerializationWriterInterpreter)
			{
				((XmlSerializationWriterInterpreter)writer).WriteRoot(o);
			}
			else
			{
				this.serializerData.WriterMethod.Invoke(writer, new object[]
				{
					o
				});
			}
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.IO.Stream" />.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public void Serialize(Stream stream, object o)
		{
			this.Serialize(new XmlTextWriter(stream, Encoding.Default)
			{
				Formatting = Formatting.Indented
			}, o, null);
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.IO.TextWriter" />.</summary>
		/// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		public void Serialize(TextWriter textWriter, object o)
		{
			this.Serialize(new XmlTextWriter(textWriter)
			{
				Formatting = Formatting.Indented
			}, o, null);
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="xmlWriter">The <see cref="T:System.xml.XmlWriter" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public void Serialize(XmlWriter xmlWriter, object o)
		{
			this.Serialize(xmlWriter, o, null);
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.IO.Stream" />that references the specified namespaces.</summary>
		/// <param name="stream">The <see cref="T:System.IO.Stream" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <param name="namespaces">The <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> referenced by the object. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public void Serialize(Stream stream, object o, XmlSerializerNamespaces namespaces)
		{
			this.Serialize(new XmlTextWriter(stream, Encoding.Default)
			{
				Formatting = Formatting.Indented
			}, o, namespaces);
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.IO.TextWriter" /> and references the specified namespaces.</summary>
		/// <param name="textWriter">The <see cref="T:System.IO.TextWriter" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <param name="namespaces">The <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> that contains namespaces for the generated XML document. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public void Serialize(TextWriter textWriter, object o, XmlSerializerNamespaces namespaces)
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(textWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			this.Serialize(xmlTextWriter, o, namespaces);
			xmlTextWriter.Flush();
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.Xml.XmlWriter" /> and references the specified namespaces.</summary>
		/// <param name="xmlWriter">The <see cref="T:System.xml.XmlWriter" /> used to write the XML document. </param>
		/// <param name="o">The <see cref="T:System.Object" /> to serialize. </param>
		/// <param name="namespaces">The <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> referenced by the object. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		public void Serialize(XmlWriter writer, object o, XmlSerializerNamespaces namespaces)
		{
			try
			{
				XmlSerializationWriter xmlSerializationWriter;
				if (this.customSerializer)
				{
					xmlSerializationWriter = this.CreateWriter();
				}
				else
				{
					xmlSerializationWriter = this.CreateWriter(this.typeMapping);
				}
				if (namespaces == null || namespaces.Count == 0)
				{
					namespaces = new XmlSerializerNamespaces();
					namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
					namespaces.Add("xsd", "http://www.w3.org/2001/XMLSchema");
				}
				xmlSerializationWriter.Initialize(writer, namespaces);
				this.Serialize(o, xmlSerializationWriter);
				writer.Flush();
			}
			catch (Exception innerException)
			{
				if (innerException is TargetInvocationException)
				{
					innerException = innerException.InnerException;
				}
				if (innerException is InvalidOperationException || innerException is InvalidCastException)
				{
					throw new InvalidOperationException("There was an error generating the XML document.", innerException);
				}
				throw;
			}
		}

		/// <summary>Deserializes the object using the data contained by the specified <see cref="T:System.Xml.XmlReader" />.</summary>
		/// <returns>The object being deserialized.</returns>
		/// <param name="xmlReader">An instance of the <see cref="T:System.Xml.XmlReader" /> class used to read the document.</param>
		/// <param name="encodingStyle">The encoding used.</param>
		/// <param name="events">An instance of the <see cref="T:System.Xml.Serialization.XmlDeserializationEvents" /> class. </param>
		[MonoTODO]
		public object Deserialize(XmlReader xmlReader, string encodingStyle, XmlDeserializationEvents events)
		{
			throw new NotImplementedException();
		}

		/// <summary>Deserializes the XML document contained by the specified <see cref="T:System.xml.XmlReader" /> and encoding style.</summary>
		/// <returns>The deserialized object.</returns>
		/// <param name="xmlReader">The <see cref="T:System.xml.XmlReader" /> that contains the XML document to deserialize. </param>
		/// <param name="encodingStyle">The encoding style of the serialized XML. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during deserialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		[MonoTODO]
		public object Deserialize(XmlReader xmlReader, string encodingStyle)
		{
			throw new NotImplementedException();
		}

		/// <summary>Deserializes an XML document contained by the specified <see cref="T:System.Xml.XmlReader" /> and allows the overriding of events that occur during deserialization.</summary>
		/// <returns>The <see cref="T:System.Object" /> being deserialized.</returns>
		/// <param name="xmlReader">The <see cref="T:System.Xml.XmlReader" /> that contains the document to deserialize.</param>
		/// <param name="events">An instance of the <see cref="T:System.Xml.Serialization.XmlDeserializationEvents" /> class. </param>
		[MonoTODO]
		public object Deserialize(XmlReader xmlReader, XmlDeserializationEvents events)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns an instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class created from mappings of one XML type to another.</summary>
		/// <returns>An instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class.</returns>
		/// <param name="mappings">An array of <see cref="T:System.Xml.Serialization.XmlMapping" /> objects used to map one type to another.</param>
		/// <param name="evidence">An instance of the <see cref="T:System.Security.Policy.Evidence" /> class that contains host and assembly data presented to the common language runtime policy system.</param>
		[MonoTODO]
		public static XmlSerializer[] FromMappings(XmlMapping[] mappings, Evidence evidence)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns an instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class from the specified mappings.</summary>
		/// <returns>An instance of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class.</returns>
		/// <param name="mappings">An array of <see cref="T:System.Xml.Serialization.XmlMapping" /> objects.</param>
		/// <param name="type">The <see cref="T:System.Type" /> of the deserialized object.</param>
		[MonoTODO]
		public static XmlSerializer[] FromMappings(XmlMapping[] mappings, Type type)
		{
			throw new NotImplementedException();
		}

		/// <summary>Returns the name of the assembly that contains one or more versions of the <see cref="T:System.Xml.Serialization.XmlSerializer" /> especially created to serialize or deserialize the specified type.</summary>
		/// <returns>The name of the assembly that contains an <see cref="T:System.Xml.Serialization.XmlSerializer" /> for the type.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> you are deserializing.</param>
		public static string GetXmlSerializerAssemblyName(Type type)
		{
			return type.Assembly.GetName().Name + ".XmlSerializers";
		}

		/// <summary>Returns the name of the assembly that contains the serializer for the specified type in the specified namespace.</summary>
		/// <returns>The name of the assembly that contains specially built serializers.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> you are interested in.</param>
		/// <param name="defaultNamespace">The namespace of the type.</param>
		public static string GetXmlSerializerAssemblyName(Type type, string defaultNamespace)
		{
			return XmlSerializer.GetXmlSerializerAssemblyName(type) + "." + defaultNamespace.GetHashCode();
		}

		/// <summary>Serializes the specified object and writes the XML document to a file using the specified <see cref="T:System.Xml.XmlWriter" /> and references the specified namespaces and encoding style.</summary>
		/// <param name="xmlWriter">The <see cref="T:System.xml.XmlWriter" /> used to write the XML document. </param>
		/// <param name="o">The object to serialize. </param>
		/// <param name="namespaces">The <see cref="T:System.Xml.Serialization.XmlSerializerNamespaces" /> referenced by the object. </param>
		/// <param name="encodingStyle">The encoding style of the serialized XML. </param>
		/// <exception cref="T:System.InvalidOperationException">An error occurred during serialization. The original exception is available using the <see cref="P:System.Exception.InnerException" /> property. </exception>
		[MonoTODO]
		public void Serialize(XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle)
		{
			throw new NotImplementedException();
		}

		/// <summary>Serializes the specified <see cref="T:System.Object" /> and writes the XML document to a file using the specified <see cref="T:System.Xml.XmlWriter" />, XML namespaces, and encoding. </summary>
		/// <param name="xmlWriter">The <see cref="T:System.Xml.XmlWriter" /> used to write the XML document.</param>
		/// <param name="o">The object to serialize.</param>
		/// <param name="namespaces">An instance of the XmlSerializaerNamespaces that contains namespaces and prefixes to use.</param>
		/// <param name="encodingStyle">The encoding used in the document.</param>
		/// <param name="id">For SOAP encoded messages, the base used to generate id attributes. </param>
		[MonoNotSupported("")]
		public void Serialize(XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle, string id)
		{
			throw new NotImplementedException();
		}

		private XmlSerializationWriter CreateWriter(XmlMapping typeMapping)
		{
			lock (this)
			{
				if (this.serializerData != null)
				{
					XmlSerializer.SerializerData obj = this.serializerData;
					XmlSerializationWriter xmlSerializationWriter;
					lock (obj)
					{
						xmlSerializationWriter = this.serializerData.CreateWriter();
					}
					if (xmlSerializationWriter != null)
					{
						return xmlSerializationWriter;
					}
				}
			}
			if (!typeMapping.Source.CanBeGenerated || XmlSerializer.generationThreshold == -1)
			{
				return new XmlSerializationWriterInterpreter(typeMapping);
			}
			this.CheckGeneratedTypes(typeMapping);
			lock (this)
			{
				XmlSerializer.SerializerData obj2 = this.serializerData;
				XmlSerializationWriter xmlSerializationWriter;
				lock (obj2)
				{
					xmlSerializationWriter = this.serializerData.CreateWriter();
				}
				if (xmlSerializationWriter != null)
				{
					return xmlSerializationWriter;
				}
				if (!XmlSerializer.generatorFallback)
				{
					throw new InvalidOperationException("Error while generating serializer");
				}
			}
			return new XmlSerializationWriterInterpreter(typeMapping);
		}

		private XmlSerializationReader CreateReader(XmlMapping typeMapping)
		{
			lock (this)
			{
				if (this.serializerData != null)
				{
					XmlSerializer.SerializerData obj = this.serializerData;
					XmlSerializationReader xmlSerializationReader;
					lock (obj)
					{
						xmlSerializationReader = this.serializerData.CreateReader();
					}
					if (xmlSerializationReader != null)
					{
						return xmlSerializationReader;
					}
				}
			}
			if (!typeMapping.Source.CanBeGenerated || XmlSerializer.generationThreshold == -1)
			{
				return new XmlSerializationReaderInterpreter(typeMapping);
			}
			this.CheckGeneratedTypes(typeMapping);
			lock (this)
			{
				XmlSerializer.SerializerData obj2 = this.serializerData;
				XmlSerializationReader xmlSerializationReader;
				lock (obj2)
				{
					xmlSerializationReader = this.serializerData.CreateReader();
				}
				if (xmlSerializationReader != null)
				{
					return xmlSerializationReader;
				}
				if (!XmlSerializer.generatorFallback)
				{
					throw new InvalidOperationException("Error while generating serializer");
				}
			}
			return new XmlSerializationReaderInterpreter(typeMapping);
		}

		private void CheckGeneratedTypes(XmlMapping typeMapping)
		{
			throw new NotImplementedException();
		}

		private void GenerateSerializersAsync(XmlSerializer.GenerationBatch batch)
		{
			throw new NotImplementedException();
		}

		private void RunSerializerGeneration(object obj)
		{
			throw new NotImplementedException();
		}

		private XmlSerializer.GenerationBatch LoadFromSatelliteAssembly(XmlSerializer.GenerationBatch batch)
		{
			return batch;
		}

		internal class SerializerData
		{
			public int UsageCount;

			public Type ReaderType;

			public MethodInfo ReaderMethod;

			public Type WriterType;

			public MethodInfo WriterMethod;

			public XmlSerializer.GenerationBatch Batch;

			public XmlSerializerImplementation Implementation;

			public XmlSerializationReader CreateReader()
			{
				if (this.ReaderType != null)
				{
					return (XmlSerializationReader)Activator.CreateInstance(this.ReaderType);
				}
				if (this.Implementation != null)
				{
					return this.Implementation.Reader;
				}
				return null;
			}

			public XmlSerializationWriter CreateWriter()
			{
				if (this.WriterType != null)
				{
					return (XmlSerializationWriter)Activator.CreateInstance(this.WriterType);
				}
				if (this.Implementation != null)
				{
					return this.Implementation.Writer;
				}
				return null;
			}
		}

		internal class GenerationBatch
		{
			public bool Done;

			public XmlMapping[] Maps;

			public XmlSerializer.SerializerData[] Datas;
		}
	}
}
