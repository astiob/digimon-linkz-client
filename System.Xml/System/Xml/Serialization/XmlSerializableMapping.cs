using System;
using System.Reflection;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class XmlSerializableMapping : XmlTypeMapping
	{
		private XmlSchema _schema;

		private XmlSchemaComplexType _schemaType;

		private XmlQualifiedName _schemaTypeName;

		internal XmlSerializableMapping(string elementName, string ns, TypeData typeData, string xmlType, string xmlTypeNamespace) : base(elementName, ns, typeData, xmlType, xmlTypeNamespace)
		{
			XmlSchemaProviderAttribute xmlSchemaProviderAttribute = (XmlSchemaProviderAttribute)Attribute.GetCustomAttribute(typeData.Type, typeof(XmlSchemaProviderAttribute));
			if (xmlSchemaProviderAttribute != null)
			{
				string methodName = xmlSchemaProviderAttribute.MethodName;
				MethodInfo method = typeData.Type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
				if (method == null)
				{
					throw new InvalidOperationException(string.Format("Type '{0}' must implement public static method '{1}'", typeData.Type, methodName));
				}
				if (!typeof(XmlQualifiedName).IsAssignableFrom(method.ReturnType) && !typeof(XmlSchemaComplexType).IsAssignableFrom(method.ReturnType))
				{
					throw new InvalidOperationException(string.Format("Method '{0}' indicated by XmlSchemaProviderAttribute must have its return type as XmlQualifiedName", methodName));
				}
				XmlSchemaSet xmlSchemaSet = new XmlSchemaSet();
				object obj = method.Invoke(null, new object[]
				{
					xmlSchemaSet
				});
				this._schemaTypeName = XmlQualifiedName.Empty;
				if (obj == null)
				{
					return;
				}
				if (obj is XmlSchemaComplexType)
				{
					this._schemaType = (XmlSchemaComplexType)obj;
					if (!this._schemaType.QualifiedName.IsEmpty)
					{
						this._schemaTypeName = this._schemaType.QualifiedName;
					}
					else
					{
						this._schemaTypeName = new XmlQualifiedName(xmlType, xmlTypeNamespace);
					}
				}
				else
				{
					if (!(obj is XmlQualifiedName))
					{
						throw new InvalidOperationException(string.Format("Method {0}.{1}() specified by XmlSchemaProviderAttribute has invalid signature: return type must be compatible with System.Xml.XmlQualifiedName.", typeData.Type.Name, methodName));
					}
					this._schemaTypeName = (XmlQualifiedName)obj;
				}
				base.UpdateRoot(new XmlQualifiedName(this._schemaTypeName.Name, base.Namespace ?? this._schemaTypeName.Namespace));
				base.XmlTypeNamespace = this._schemaTypeName.Namespace;
				base.XmlType = this._schemaTypeName.Name;
				if (!this._schemaTypeName.IsEmpty && xmlSchemaSet.Count > 0)
				{
					XmlSchema[] array = new XmlSchema[xmlSchemaSet.Count];
					xmlSchemaSet.CopyTo(array, 0);
					this._schema = array[0];
				}
				return;
			}
			else
			{
				IXmlSerializable xmlSerializable = (IXmlSerializable)Activator.CreateInstance(typeData.Type, true);
				try
				{
					this._schema = xmlSerializable.GetSchema();
				}
				catch (Exception)
				{
				}
				if (this._schema != null && (this._schema.Id == null || this._schema.Id.Length == 0))
				{
					throw new InvalidOperationException("Schema Id is missing. The schema returned from " + typeData.Type.FullName + ".GetSchema() must have an Id.");
				}
				return;
			}
		}

		internal XmlSchema Schema
		{
			get
			{
				return this._schema;
			}
		}

		internal XmlSchemaType SchemaType
		{
			get
			{
				return this._schemaType;
			}
		}

		internal XmlQualifiedName SchemaTypeName
		{
			get
			{
				return this._schemaTypeName;
			}
		}
	}
}
