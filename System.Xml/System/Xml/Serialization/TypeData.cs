using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class TypeData
	{
		private Type type;

		private string elementName;

		private SchemaTypes sType;

		private Type listItemType;

		private string typeName;

		private string fullTypeName;

		private string csharpName;

		private string csharpFullName;

		private TypeData listItemTypeData;

		private TypeData listTypeData;

		private TypeData mappedType;

		private XmlSchemaPatternFacet facet;

		private bool hasPublicConstructor;

		private bool nullableOverride;

		private static Hashtable keywordsTable;

		private static string[] keywords = new string[]
		{
			"abstract",
			"event",
			"new",
			"struct",
			"as",
			"explicit",
			"null",
			"switch",
			"base",
			"extern",
			"this",
			"false",
			"operator",
			"throw",
			"break",
			"finally",
			"out",
			"true",
			"fixed",
			"override",
			"try",
			"case",
			"params",
			"typeof",
			"catch",
			"for",
			"private",
			"foreach",
			"protected",
			"checked",
			"goto",
			"public",
			"unchecked",
			"class",
			"if",
			"readonly",
			"unsafe",
			"const",
			"implicit",
			"ref",
			"continue",
			"in",
			"return",
			"using",
			"virtual",
			"default",
			"interface",
			"sealed",
			"volatile",
			"delegate",
			"internal",
			"do",
			"is",
			"sizeof",
			"while",
			"lock",
			"stackalloc",
			"else",
			"static",
			"enum",
			"namespace",
			"object",
			"bool",
			"byte",
			"float",
			"uint",
			"char",
			"ulong",
			"ushort",
			"decimal",
			"int",
			"sbyte",
			"short",
			"double",
			"long",
			"string",
			"void",
			"partial",
			"yield",
			"where"
		};

		public TypeData(Type type, string elementName, bool isPrimitive) : this(type, elementName, isPrimitive, null, null)
		{
		}

		public TypeData(Type type, string elementName, bool isPrimitive, TypeData mappedType, XmlSchemaPatternFacet facet)
		{
			this.hasPublicConstructor = true;
			base..ctor();
			if (type.IsGenericTypeDefinition)
			{
				throw new InvalidOperationException("Generic type definition cannot be used in serialization. Only specific generic types can be used.");
			}
			this.mappedType = mappedType;
			this.facet = facet;
			this.type = type;
			this.typeName = type.Name;
			this.fullTypeName = type.FullName.Replace('+', '.');
			if (isPrimitive)
			{
				this.sType = SchemaTypes.Primitive;
			}
			else if (type.IsEnum)
			{
				this.sType = SchemaTypes.Enum;
			}
			else if (typeof(IXmlSerializable).IsAssignableFrom(type))
			{
				this.sType = SchemaTypes.XmlSerializable;
			}
			else if (typeof(XmlNode).IsAssignableFrom(type))
			{
				this.sType = SchemaTypes.XmlNode;
			}
			else if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
			{
				this.sType = SchemaTypes.Array;
			}
			else
			{
				this.sType = SchemaTypes.Class;
			}
			if (this.IsListType)
			{
				this.elementName = TypeTranslator.GetArrayName(this.ListItemTypeData.XmlType);
			}
			else
			{
				this.elementName = elementName;
			}
			if (this.sType == SchemaTypes.Array || this.sType == SchemaTypes.Class)
			{
				this.hasPublicConstructor = (!type.IsInterface && (type.IsArray || type.GetConstructor(Type.EmptyTypes) != null || type.IsAbstract || type.IsValueType));
			}
		}

		internal TypeData(string typeName, string fullTypeName, string xmlType, SchemaTypes schemaType, TypeData listItemTypeData)
		{
			this.hasPublicConstructor = true;
			base..ctor();
			this.elementName = xmlType;
			this.typeName = typeName;
			this.fullTypeName = fullTypeName.Replace('+', '.');
			this.listItemTypeData = listItemTypeData;
			this.sType = schemaType;
			this.hasPublicConstructor = true;
		}

		public string TypeName
		{
			get
			{
				return this.typeName;
			}
		}

		public string XmlType
		{
			get
			{
				return this.elementName;
			}
		}

		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		public string FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
		}

		public string CSharpName
		{
			get
			{
				if (this.csharpName == null)
				{
					this.csharpName = ((this.Type != null) ? TypeData.ToCSharpName(this.Type, false) : this.TypeName);
				}
				return this.csharpName;
			}
		}

		public string CSharpFullName
		{
			get
			{
				if (this.csharpFullName == null)
				{
					this.csharpFullName = ((this.Type != null) ? TypeData.ToCSharpName(this.Type, true) : this.TypeName);
				}
				return this.csharpFullName;
			}
		}

		public static string ToCSharpName(Type type, bool full)
		{
			if (type.IsArray)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(TypeData.ToCSharpName(type.GetElementType(), full));
				stringBuilder.Append('[');
				int arrayRank = type.GetArrayRank();
				for (int i = 1; i < arrayRank; i++)
				{
					stringBuilder.Append(',');
				}
				stringBuilder.Append(']');
				return stringBuilder.ToString();
			}
			if (type.IsGenericType && !type.IsGenericTypeDefinition)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(TypeData.ToCSharpName(type.GetGenericTypeDefinition(), full));
				stringBuilder2.Append('<');
				foreach (Type type2 in type.GetGenericArguments())
				{
					stringBuilder2.Append(TypeData.ToCSharpName(type2, full)).Append(',');
				}
				stringBuilder2.Length--;
				stringBuilder2.Append('>');
				return stringBuilder2.ToString();
			}
			string text = (!full) ? type.Name : type.FullName;
			text = text.Replace('+', '.');
			int num = text.IndexOf('`');
			text = ((num <= 0) ? text : text.Substring(0, num));
			if (TypeData.IsKeyword(text))
			{
				return "@" + text;
			}
			return text;
		}

		private static bool IsKeyword(string name)
		{
			if (TypeData.keywordsTable == null)
			{
				Hashtable hashtable = new Hashtable();
				foreach (string text in TypeData.keywords)
				{
					hashtable[text] = text;
				}
				TypeData.keywordsTable = hashtable;
			}
			return TypeData.keywordsTable.Contains(name);
		}

		public SchemaTypes SchemaType
		{
			get
			{
				return this.sType;
			}
		}

		public bool IsListType
		{
			get
			{
				return this.SchemaType == SchemaTypes.Array;
			}
		}

		public bool IsComplexType
		{
			get
			{
				return this.SchemaType == SchemaTypes.Class || this.SchemaType == SchemaTypes.Array || this.SchemaType == SchemaTypes.Enum || this.SchemaType == SchemaTypes.XmlNode || this.SchemaType == SchemaTypes.XmlSerializable || !this.IsXsdType;
			}
		}

		public bool IsValueType
		{
			get
			{
				if (this.type != null)
				{
					return this.type.IsValueType;
				}
				return this.sType == SchemaTypes.Primitive || this.sType == SchemaTypes.Enum;
			}
		}

		public bool NullableOverride
		{
			get
			{
				return this.nullableOverride;
			}
		}

		public bool IsNullable
		{
			get
			{
				return this.nullableOverride || !this.IsValueType || (this.type != null && this.type.IsGenericType && this.type.GetGenericTypeDefinition() == typeof(Nullable<>));
			}
			set
			{
				this.nullableOverride = value;
			}
		}

		public TypeData ListItemTypeData
		{
			get
			{
				if (this.listItemTypeData == null && this.type != null)
				{
					this.listItemTypeData = TypeTranslator.GetTypeData(this.ListItemType);
				}
				return this.listItemTypeData;
			}
		}

		public Type ListItemType
		{
			get
			{
				if (this.type == null)
				{
					throw new InvalidOperationException("Property ListItemType is not supported for custom types");
				}
				if (this.listItemType != null)
				{
					return this.listItemType;
				}
				Type type = null;
				if (this.SchemaType != SchemaTypes.Array)
				{
					throw new InvalidOperationException(this.Type.FullName + " is not a collection");
				}
				if (this.type.IsArray)
				{
					this.listItemType = this.type.GetElementType();
				}
				else if (typeof(ICollection).IsAssignableFrom(this.type) || (type = this.GetGenericListItemType(this.type)) != null)
				{
					if (typeof(IDictionary).IsAssignableFrom(this.type))
					{
						throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "The type {0} is not supported because it implements IDictionary.", new object[]
						{
							this.type.FullName
						}));
					}
					if (type != null)
					{
						this.listItemType = type;
					}
					else
					{
						PropertyInfo indexerProperty = TypeData.GetIndexerProperty(this.type);
						if (indexerProperty == null)
						{
							throw new InvalidOperationException("You must implement a default accessor on " + this.type.FullName + " because it inherits from ICollection");
						}
						this.listItemType = indexerProperty.PropertyType;
					}
					if (this.type.GetMethod("Add", new Type[]
					{
						this.listItemType
					}) == null)
					{
						throw TypeData.CreateMissingAddMethodException(this.type, "ICollection", this.listItemType);
					}
				}
				else
				{
					MethodInfo method = this.type.GetMethod("GetEnumerator", Type.EmptyTypes);
					if (method == null)
					{
						method = this.type.GetMethod("System.Collections.IEnumerable.GetEnumerator", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);
					}
					PropertyInfo property = method.ReturnType.GetProperty("Current");
					if (property == null)
					{
						this.listItemType = typeof(object);
					}
					else
					{
						this.listItemType = property.PropertyType;
					}
					if (this.type.GetMethod("Add", new Type[]
					{
						this.listItemType
					}) == null)
					{
						throw TypeData.CreateMissingAddMethodException(this.type, "IEnumerable", this.listItemType);
					}
				}
				return this.listItemType;
			}
		}

		public TypeData ListTypeData
		{
			get
			{
				if (this.listTypeData != null)
				{
					return this.listTypeData;
				}
				this.listTypeData = new TypeData(this.TypeName + "[]", this.FullTypeName + "[]", TypeTranslator.GetArrayName(this.XmlType), SchemaTypes.Array, this);
				return this.listTypeData;
			}
		}

		public bool IsXsdType
		{
			get
			{
				return this.mappedType == null;
			}
		}

		public TypeData MappedType
		{
			get
			{
				return (this.mappedType == null) ? this : this.mappedType;
			}
		}

		public XmlSchemaPatternFacet XmlSchemaPatternFacet
		{
			get
			{
				return this.facet;
			}
		}

		public bool HasPublicConstructor
		{
			get
			{
				return this.hasPublicConstructor;
			}
		}

		public static PropertyInfo GetIndexerProperty(Type collectionType)
		{
			PropertyInfo[] properties = collectionType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
			foreach (PropertyInfo propertyInfo in properties)
			{
				ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
				if (indexParameters != null && indexParameters.Length == 1 && indexParameters[0].ParameterType == typeof(int))
				{
					return propertyInfo;
				}
			}
			return null;
		}

		private static InvalidOperationException CreateMissingAddMethodException(Type type, string inheritFrom, Type argumentType)
		{
			return new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "To be XML serializable, types which inherit from {0} must have an implementation of Add({1}) at all levels of their inheritance hierarchy. {2} does not implement Add({1}).", new object[]
			{
				inheritFrom,
				argumentType.FullName,
				type.FullName
			}));
		}

		private Type GetGenericListItemType(Type type)
		{
			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
			{
				return type.GetGenericArguments()[0];
			}
			foreach (Type type2 in type.GetInterfaces())
			{
				Type genericListItemType;
				if ((genericListItemType = this.GetGenericListItemType(type2)) != null)
				{
					return genericListItemType;
				}
			}
			return null;
		}
	}
}
