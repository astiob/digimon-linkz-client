using System;

namespace System.Xml.Schema
{
	/// <summary>Infers an XML Schema Definition Language (XSD) schema from an XML document. The <see cref="T:System.Xml.Schema.XmlSchemaInference" /> class cannot be inherited.</summary>
	[MonoTODO]
	public sealed class XmlSchemaInference
	{
		private XmlSchemaInference.InferenceOption occurrence;

		private XmlSchemaInference.InferenceOption typeInference;

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaInference.InferenceOption" /> value that affects schema occurrence declarations inferred from the XML document.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaInference.InferenceOption" /> object.</returns>
		public XmlSchemaInference.InferenceOption Occurrence
		{
			get
			{
				return this.occurrence;
			}
			set
			{
				this.occurrence = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Xml.Schema.XmlSchemaInference.InferenceOption" /> value that affects types inferred from the XML document.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaInference.InferenceOption" /> object.</returns>
		public XmlSchemaInference.InferenceOption TypeInference
		{
			get
			{
				return this.typeInference;
			}
			set
			{
				this.typeInference = value;
			}
		}

		/// <summary>Infers an XML Schema Definition Language (XSD) schema from the XML document contained in the <see cref="T:System.Xml.XmlReader" /> object specified.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object containing the inferred schemas.</returns>
		/// <param name="instanceDocument">An <see cref="T:System.Xml.XmlReader" /> object containing the XML document to infer a schema from.</param>
		/// <exception cref="T:System.Xml.XmlException">The XML document is not well-formed.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaInferenceException">The <see cref="T:System.Xml.XmlReader" /> object is not positioned on the root node or on an element. An error occurs during the schema inference process.</exception>
		public XmlSchemaSet InferSchema(XmlReader xmlReader)
		{
			return this.InferSchema(xmlReader, new XmlSchemaSet());
		}

		/// <summary>Infers an XML Schema Definition Language (XSD) schema from the XML document contained in the <see cref="T:System.Xml.XmlReader" /> object specified, and refines the inferred schema using an existing schema in the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object specified with the same target namespace.</summary>
		/// <returns>An <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object containing the inferred schemas.</returns>
		/// <param name="instanceDocument">An <see cref="T:System.Xml.XmlReader" /> object containing the XML document to infer a schema from.</param>
		/// <param name="schemas">An <see cref="T:System.Xml.Schema.XmlSchemaSet" /> object containing an existing schema used to refine the inferred schema.</param>
		/// <exception cref="T:System.Xml.XmlException">The XML document is not well-formed.</exception>
		/// <exception cref="T:System.Xml.Schema.XmlSchemaInferenceException">The <see cref="T:System.Xml.XmlReader" /> object is not positioned on the root node or on an element. An error occurs during the schema inference process.</exception>
		public XmlSchemaSet InferSchema(XmlReader xmlReader, XmlSchemaSet schemas)
		{
			return XsdInference.Process(xmlReader, schemas, this.occurrence == XmlSchemaInference.InferenceOption.Relaxed, this.typeInference == XmlSchemaInference.InferenceOption.Relaxed);
		}

		/// <summary>Affects occurrence and type information inferred by the <see cref="T:System.Xml.Schema.XmlSchemaInference" /> class for elements and attributes in an XML document. </summary>
		public enum InferenceOption
		{
			/// <summary>Indicates that a more restrictive schema declaration should be inferred for a particular element or attribute.</summary>
			Restricted,
			/// <summary>Indicates that a less restrictive schema declaration should be inferred for a particular element or attribute.</summary>
			Relaxed
		}
	}
}
