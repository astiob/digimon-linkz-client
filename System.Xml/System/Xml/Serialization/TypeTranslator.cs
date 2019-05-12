using System;
using System.Collections;
using System.Globalization;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class TypeTranslator
	{
		private static Hashtable nameCache = new Hashtable();

		private static Hashtable primitiveTypes;

		private static Hashtable primitiveArrayTypes = Hashtable.Synchronized(new Hashtable());

		private static Hashtable nullableTypes;

		static TypeTranslator()
		{
			TypeTranslator.nameCache = Hashtable.Synchronized(TypeTranslator.nameCache);
			TypeTranslator.nameCache.Add(typeof(bool), new TypeData(typeof(bool), "boolean", true));
			TypeTranslator.nameCache.Add(typeof(short), new TypeData(typeof(short), "short", true));
			TypeTranslator.nameCache.Add(typeof(ushort), new TypeData(typeof(ushort), "unsignedShort", true));
			TypeTranslator.nameCache.Add(typeof(int), new TypeData(typeof(int), "int", true));
			TypeTranslator.nameCache.Add(typeof(uint), new TypeData(typeof(uint), "unsignedInt", true));
			TypeTranslator.nameCache.Add(typeof(long), new TypeData(typeof(long), "long", true));
			TypeTranslator.nameCache.Add(typeof(ulong), new TypeData(typeof(ulong), "unsignedLong", true));
			TypeTranslator.nameCache.Add(typeof(float), new TypeData(typeof(float), "float", true));
			TypeTranslator.nameCache.Add(typeof(double), new TypeData(typeof(double), "double", true));
			TypeTranslator.nameCache.Add(typeof(DateTime), new TypeData(typeof(DateTime), "dateTime", true));
			TypeTranslator.nameCache.Add(typeof(decimal), new TypeData(typeof(decimal), "decimal", true));
			TypeTranslator.nameCache.Add(typeof(XmlQualifiedName), new TypeData(typeof(XmlQualifiedName), "QName", true));
			TypeTranslator.nameCache.Add(typeof(string), new TypeData(typeof(string), "string", true));
			XmlSchemaPatternFacet xmlSchemaPatternFacet = new XmlSchemaPatternFacet();
			xmlSchemaPatternFacet.Value = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
			TypeTranslator.nameCache.Add(typeof(Guid), new TypeData(typeof(Guid), "guid", true, (TypeData)TypeTranslator.nameCache[typeof(string)], xmlSchemaPatternFacet));
			TypeTranslator.nameCache.Add(typeof(byte), new TypeData(typeof(byte), "unsignedByte", true));
			TypeTranslator.nameCache.Add(typeof(sbyte), new TypeData(typeof(sbyte), "byte", true));
			TypeTranslator.nameCache.Add(typeof(char), new TypeData(typeof(char), "char", true, (TypeData)TypeTranslator.nameCache[typeof(ushort)], null));
			TypeTranslator.nameCache.Add(typeof(object), new TypeData(typeof(object), "anyType", false));
			TypeTranslator.nameCache.Add(typeof(byte[]), new TypeData(typeof(byte[]), "base64Binary", true));
			TypeTranslator.nameCache.Add(typeof(XmlNode), new TypeData(typeof(XmlNode), "XmlNode", false));
			TypeTranslator.nameCache.Add(typeof(XmlElement), new TypeData(typeof(XmlElement), "XmlElement", false));
			TypeTranslator.primitiveTypes = new Hashtable();
			ICollection values = TypeTranslator.nameCache.Values;
			foreach (object obj in values)
			{
				TypeData typeData = (TypeData)obj;
				TypeTranslator.primitiveTypes.Add(typeData.XmlType, typeData);
			}
			TypeTranslator.primitiveTypes.Add("date", new TypeData(typeof(DateTime), "date", true));
			TypeTranslator.primitiveTypes.Add("time", new TypeData(typeof(DateTime), "time", true));
			TypeTranslator.primitiveTypes.Add("timePeriod", new TypeData(typeof(DateTime), "timePeriod", true));
			TypeTranslator.primitiveTypes.Add("gDay", new TypeData(typeof(string), "gDay", true));
			TypeTranslator.primitiveTypes.Add("gMonthDay", new TypeData(typeof(string), "gMonthDay", true));
			TypeTranslator.primitiveTypes.Add("gYear", new TypeData(typeof(string), "gYear", true));
			TypeTranslator.primitiveTypes.Add("gYearMonth", new TypeData(typeof(string), "gYearMonth", true));
			TypeTranslator.primitiveTypes.Add("month", new TypeData(typeof(DateTime), "month", true));
			TypeTranslator.primitiveTypes.Add("NMTOKEN", new TypeData(typeof(string), "NMTOKEN", true));
			TypeTranslator.primitiveTypes.Add("NMTOKENS", new TypeData(typeof(string), "NMTOKENS", true));
			TypeTranslator.primitiveTypes.Add("Name", new TypeData(typeof(string), "Name", true));
			TypeTranslator.primitiveTypes.Add("NCName", new TypeData(typeof(string), "NCName", true));
			TypeTranslator.primitiveTypes.Add("language", new TypeData(typeof(string), "language", true));
			TypeTranslator.primitiveTypes.Add("integer", new TypeData(typeof(string), "integer", true));
			TypeTranslator.primitiveTypes.Add("positiveInteger", new TypeData(typeof(string), "positiveInteger", true));
			TypeTranslator.primitiveTypes.Add("nonPositiveInteger", new TypeData(typeof(string), "nonPositiveInteger", true));
			TypeTranslator.primitiveTypes.Add("negativeInteger", new TypeData(typeof(string), "negativeInteger", true));
			TypeTranslator.primitiveTypes.Add("nonNegativeInteger", new TypeData(typeof(string), "nonNegativeInteger", true));
			TypeTranslator.primitiveTypes.Add("ENTITIES", new TypeData(typeof(string), "ENTITIES", true));
			TypeTranslator.primitiveTypes.Add("ENTITY", new TypeData(typeof(string), "ENTITY", true));
			TypeTranslator.primitiveTypes.Add("hexBinary", new TypeData(typeof(byte[]), "hexBinary", true));
			TypeTranslator.primitiveTypes.Add("ID", new TypeData(typeof(string), "ID", true));
			TypeTranslator.primitiveTypes.Add("IDREF", new TypeData(typeof(string), "IDREF", true));
			TypeTranslator.primitiveTypes.Add("IDREFS", new TypeData(typeof(string), "IDREFS", true));
			TypeTranslator.primitiveTypes.Add("NOTATION", new TypeData(typeof(string), "NOTATION", true));
			TypeTranslator.primitiveTypes.Add("token", new TypeData(typeof(string), "token", true));
			TypeTranslator.primitiveTypes.Add("normalizedString", new TypeData(typeof(string), "normalizedString", true));
			TypeTranslator.primitiveTypes.Add("anyURI", new TypeData(typeof(string), "anyURI", true));
			TypeTranslator.primitiveTypes.Add("base64", new TypeData(typeof(byte[]), "base64", true));
			TypeTranslator.primitiveTypes.Add("duration", new TypeData(typeof(string), "duration", true));
			TypeTranslator.nullableTypes = Hashtable.Synchronized(new Hashtable());
			foreach (object obj2 in TypeTranslator.primitiveTypes)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
				TypeData typeData2 = (TypeData)dictionaryEntry.Value;
				TypeData typeData3 = new TypeData(typeData2.Type, typeData2.XmlType, true);
				typeData3.IsNullable = true;
				TypeTranslator.nullableTypes.Add(dictionaryEntry.Key, typeData3);
			}
		}

		public static TypeData GetTypeData(Type type)
		{
			return TypeTranslator.GetTypeData(type, null);
		}

		public static TypeData GetTypeData(Type runtimeType, string xmlDataType)
		{
			Type type = runtimeType;
			bool flag = false;
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				flag = true;
				type = type.GetGenericArguments()[0];
				TypeData typeData = TypeTranslator.GetTypeData(type);
				if (typeData != null)
				{
					TypeData typeData2 = (TypeData)TypeTranslator.nullableTypes[typeData.XmlType];
					if (typeData2 == null)
					{
						typeData2 = new TypeData(type, typeData.XmlType, false);
						typeData2.IsNullable = true;
						TypeTranslator.nullableTypes[typeData.XmlType] = typeData2;
					}
					return typeData2;
				}
			}
			if (xmlDataType != null && xmlDataType.Length != 0)
			{
				TypeData primitiveTypeData = TypeTranslator.GetPrimitiveTypeData(xmlDataType);
				if (!type.IsArray || type == primitiveTypeData.Type)
				{
					return primitiveTypeData;
				}
				TypeData typeData3 = (TypeData)TypeTranslator.primitiveArrayTypes[xmlDataType];
				if (typeData3 != null)
				{
					return typeData3;
				}
				if (primitiveTypeData.Type == type.GetElementType())
				{
					typeData3 = new TypeData(type, TypeTranslator.GetArrayName(primitiveTypeData.XmlType), false);
					TypeTranslator.primitiveArrayTypes[xmlDataType] = typeData3;
					return typeData3;
				}
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot convert values of type '",
					type.GetElementType(),
					"' to '",
					xmlDataType,
					"'"
				}));
			}
			else
			{
				TypeData typeData4 = TypeTranslator.nameCache[runtimeType] as TypeData;
				if (typeData4 != null)
				{
					return typeData4;
				}
				string text;
				if (type.IsArray)
				{
					string xmlType = TypeTranslator.GetTypeData(type.GetElementType()).XmlType;
					text = TypeTranslator.GetArrayName(xmlType);
				}
				else if (type.IsGenericType && !type.IsGenericTypeDefinition)
				{
					text = XmlConvert.EncodeLocalName(type.Name.Substring(0, type.Name.IndexOf('`'))) + "Of";
					foreach (Type type2 in type.GetGenericArguments())
					{
						text += ((!type2.IsArray && !type2.IsGenericType) ? CodeIdentifier.MakePascal(XmlConvert.EncodeLocalName(type2.Name)) : TypeTranslator.GetTypeData(type2).XmlType);
					}
				}
				else
				{
					text = XmlConvert.EncodeLocalName(type.Name);
				}
				typeData4 = new TypeData(type, text, false);
				if (flag)
				{
					typeData4.IsNullable = true;
				}
				TypeTranslator.nameCache[runtimeType] = typeData4;
				return typeData4;
			}
		}

		public static bool IsPrimitive(Type type)
		{
			return TypeTranslator.GetTypeData(type).SchemaType == SchemaTypes.Primitive;
		}

		public static TypeData GetPrimitiveTypeData(string typeName)
		{
			return TypeTranslator.GetPrimitiveTypeData(typeName, false);
		}

		public static TypeData GetPrimitiveTypeData(string typeName, bool nullable)
		{
			TypeData typeData = (TypeData)TypeTranslator.primitiveTypes[typeName];
			if (typeData != null && !typeData.Type.IsValueType)
			{
				return typeData;
			}
			Hashtable hashtable = (!nullable || TypeTranslator.nullableTypes == null) ? TypeTranslator.primitiveTypes : TypeTranslator.nullableTypes;
			typeData = (TypeData)hashtable[typeName];
			if (typeData == null)
			{
				throw new NotSupportedException("Data type '" + typeName + "' not supported");
			}
			return typeData;
		}

		public static TypeData FindPrimitiveTypeData(string typeName)
		{
			return (TypeData)TypeTranslator.primitiveTypes[typeName];
		}

		public static TypeData GetDefaultPrimitiveTypeData(TypeData primType)
		{
			if (primType.SchemaType == SchemaTypes.Primitive)
			{
				TypeData typeData = TypeTranslator.GetTypeData(primType.Type, null);
				if (typeData != primType)
				{
					return typeData;
				}
			}
			return primType;
		}

		public static bool IsDefaultPrimitiveTpeData(TypeData primType)
		{
			return TypeTranslator.GetDefaultPrimitiveTypeData(primType) == primType;
		}

		public static TypeData CreateCustomType(string typeName, string fullTypeName, string xmlType, SchemaTypes schemaType, TypeData listItemTypeData)
		{
			return new TypeData(typeName, fullTypeName, xmlType, schemaType, listItemTypeData);
		}

		public static string GetArrayName(string elemName)
		{
			return "ArrayOf" + char.ToUpper(elemName[0], CultureInfo.InvariantCulture) + elemName.Substring(1);
		}

		public static string GetArrayName(string elemName, int dimensions)
		{
			string text = TypeTranslator.GetArrayName(elemName);
			while (dimensions > 1)
			{
				text = "ArrayOf" + text;
				dimensions--;
			}
			return text;
		}

		public static void ParseArrayType(string arrayType, out string type, out string ns, out string dimensions)
		{
			int num = arrayType.LastIndexOf(":");
			if (num == -1)
			{
				ns = string.Empty;
			}
			else
			{
				ns = arrayType.Substring(0, num);
			}
			int num2 = arrayType.IndexOf("[", num + 1);
			if (num2 == -1)
			{
				throw new InvalidOperationException("Cannot parse WSDL array type: " + arrayType);
			}
			type = arrayType.Substring(num + 1, num2 - num - 1);
			dimensions = arrayType.Substring(num2);
		}
	}
}
