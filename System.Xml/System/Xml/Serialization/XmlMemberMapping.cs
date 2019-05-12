using System;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	/// <summary>Maps a code entity in a .NET Framework Web service method to an element in a Web Services Description Language (WSDL) message.</summary>
	public class XmlMemberMapping
	{
		private XmlTypeMapMember _mapMember;

		private string _elementName;

		private string _memberName;

		private string _namespace;

		private string _typeNamespace;

		private XmlSchemaForm _form;

		internal XmlMemberMapping(string memberName, string defaultNamespace, XmlTypeMapMember mapMem, bool encodedFormat)
		{
			this._mapMember = mapMem;
			this._memberName = memberName;
			if (mapMem is XmlTypeMapMemberAnyElement)
			{
				XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = (XmlTypeMapMemberAnyElement)mapMem;
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)xmlTypeMapMemberAnyElement.ElementInfo[xmlTypeMapMemberAnyElement.ElementInfo.Count - 1];
				this._elementName = xmlTypeMapElementInfo.ElementName;
				this._namespace = xmlTypeMapElementInfo.Namespace;
				if (xmlTypeMapElementInfo.MappedType != null)
				{
					this._typeNamespace = xmlTypeMapElementInfo.MappedType.Namespace;
				}
				else
				{
					this._typeNamespace = string.Empty;
				}
			}
			else if (mapMem is XmlTypeMapMemberElement)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)((XmlTypeMapMemberElement)mapMem).ElementInfo[0];
				this._elementName = xmlTypeMapElementInfo2.ElementName;
				if (encodedFormat)
				{
					this._namespace = defaultNamespace;
					if (xmlTypeMapElementInfo2.MappedType != null)
					{
						this._typeNamespace = string.Empty;
					}
					else
					{
						this._typeNamespace = xmlTypeMapElementInfo2.DataTypeNamespace;
					}
				}
				else
				{
					this._namespace = xmlTypeMapElementInfo2.Namespace;
					if (xmlTypeMapElementInfo2.MappedType != null)
					{
						this._typeNamespace = xmlTypeMapElementInfo2.MappedType.Namespace;
					}
					else
					{
						this._typeNamespace = string.Empty;
					}
					this._form = xmlTypeMapElementInfo2.Form;
				}
			}
			else
			{
				this._elementName = this._memberName;
				this._namespace = string.Empty;
			}
			if (this._form == XmlSchemaForm.None)
			{
				this._form = XmlSchemaForm.Qualified;
			}
		}

		/// <summary>Gets or sets a value that indicates whether the .NET Framework type maps to an XML element or attribute of any type. </summary>
		/// <returns>true, if the type maps to an XML any element or attribute; otherwise, false.</returns>
		public bool Any
		{
			get
			{
				return this._mapMember is XmlTypeMapMemberAnyElement;
			}
		}

		/// <summary>Gets the unqualified name of the XML element declaration that applies to this mapping. </summary>
		/// <returns>The unqualified name of the XML element declaration that applies to this mapping.</returns>
		public string ElementName
		{
			get
			{
				return this._elementName;
			}
		}

		/// <summary>Gets the name of the Web service method member that is represented by this mapping. </summary>
		/// <returns>The name of the Web service method member represented by this mapping.</returns>
		public string MemberName
		{
			get
			{
				return this._memberName;
			}
		}

		/// <summary>Gets the XML namespace that applies to this mapping. </summary>
		/// <returns>The XML namespace that applies to this mapping.</returns>
		public string Namespace
		{
			get
			{
				return this._namespace;
			}
		}

		/// <summary>Gets the fully qualified type name of the .NET Framework type for this mapping. </summary>
		/// <returns>The fully qualified type name of the .NET Framework type for this mapping.</returns>
		public string TypeFullName
		{
			get
			{
				return this._mapMember.TypeData.FullTypeName;
			}
		}

		/// <summary>Gets the type name of the .NET Framework type for this mapping. </summary>
		/// <returns>The type name of the .NET Framework type for this mapping.</returns>
		public string TypeName
		{
			get
			{
				return this._mapMember.TypeData.XmlType;
			}
		}

		/// <summary>Gets the namespace of the .NET Framework type for this mapping.</summary>
		/// <returns>The namespace of the .NET Framework type for this mapping.</returns>
		public string TypeNamespace
		{
			get
			{
				return this._typeNamespace;
			}
		}

		internal XmlTypeMapMember TypeMapMember
		{
			get
			{
				return this._mapMember;
			}
		}

		internal XmlSchemaForm Form
		{
			get
			{
				return this._form;
			}
		}

		/// <summary>Gets the XML element name as it appears in the service description document.</summary>
		/// <returns>The XML element name.</returns>
		public string XsdElementName
		{
			get
			{
				return this._mapMember.Name;
			}
		}

		/// <summary>Gets a value that indicates whether the accompanying field in the .NET Framework type has a value specified.</summary>
		/// <returns>true, if the accompanying field has a value specified; otherwise, false.</returns>
		public bool CheckSpecified
		{
			get
			{
				return this._mapMember.IsOptionalValueType;
			}
		}
	}
}
