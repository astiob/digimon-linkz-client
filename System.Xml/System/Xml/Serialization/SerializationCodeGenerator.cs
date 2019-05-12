using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Schema;

namespace System.Xml.Serialization
{
	internal class SerializationCodeGenerator
	{
		private XmlMapping _typeMap;

		private SerializationFormat _format;

		private TextWriter _writer;

		private int _tempVarId;

		private int _indent;

		private Hashtable _uniqueNames;

		private int _methodId;

		private SerializerInfo _config;

		private ArrayList _mapsToGenerate;

		private ArrayList _fixupCallbacks;

		private ArrayList _referencedTypes;

		private GenerationResult[] _results;

		private GenerationResult _result;

		private XmlMapping[] _xmlMaps;

		private CodeIdentifiers classNames;

		private ArrayList _listsToFill;

		private Hashtable _hookVariables;

		private Stack _hookContexts;

		private Stack _hookOpenHooks;

		public SerializationCodeGenerator(XmlMapping[] xmlMaps) : this(xmlMaps, null)
		{
		}

		public SerializationCodeGenerator(XmlMapping[] xmlMaps, SerializerInfo config)
		{
			this._uniqueNames = new Hashtable();
			this._mapsToGenerate = new ArrayList();
			this._referencedTypes = new ArrayList();
			this.classNames = new CodeIdentifiers();
			this._listsToFill = new ArrayList();
			base..ctor();
			this._xmlMaps = xmlMaps;
			this._config = config;
		}

		public SerializationCodeGenerator(XmlMapping xmlMap, SerializerInfo config)
		{
			this._uniqueNames = new Hashtable();
			this._mapsToGenerate = new ArrayList();
			this._referencedTypes = new ArrayList();
			this.classNames = new CodeIdentifiers();
			this._listsToFill = new ArrayList();
			base..ctor();
			this._xmlMaps = new XmlMapping[]
			{
				xmlMap
			};
			this._config = config;
		}

		public static void Generate(string configFileName, string outputPath)
		{
			SerializationCodeGeneratorConfiguration serializationCodeGeneratorConfiguration = null;
			StreamReader streamReader = new StreamReader(configFileName);
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(new XmlReflectionImporter
				{
					AllowPrivateTypes = true
				}.ImportTypeMapping(typeof(SerializationCodeGeneratorConfiguration)));
				serializationCodeGeneratorConfiguration = (SerializationCodeGeneratorConfiguration)xmlSerializer.Deserialize(streamReader);
			}
			finally
			{
				streamReader.Close();
			}
			if (outputPath == null)
			{
				outputPath = string.Empty;
			}
			CodeIdentifiers codeIdentifiers = new CodeIdentifiers();
			if (serializationCodeGeneratorConfiguration.Serializers != null)
			{
				foreach (SerializerInfo serializerInfo in serializationCodeGeneratorConfiguration.Serializers)
				{
					Type type;
					if (serializerInfo.Assembly != null)
					{
						Assembly assembly;
						try
						{
							assembly = Assembly.Load(serializerInfo.Assembly);
						}
						catch
						{
							assembly = Assembly.LoadFrom(serializerInfo.Assembly);
						}
						type = assembly.GetType(serializerInfo.ClassName, true);
					}
					else
					{
						type = Type.GetType(serializerInfo.ClassName);
					}
					if (type == null)
					{
						throw new InvalidOperationException("Type " + serializerInfo.ClassName + " not found");
					}
					string text = serializerInfo.OutFileName;
					if (text == null || text.Length == 0)
					{
						int num = serializerInfo.ClassName.LastIndexOf(".");
						if (num != -1)
						{
							text = serializerInfo.ClassName.Substring(num + 1);
						}
						else
						{
							text = serializerInfo.ClassName;
						}
						text = codeIdentifiers.AddUnique(text, type) + "Serializer.cs";
					}
					StreamWriter streamWriter = new StreamWriter(Path.Combine(outputPath, text));
					try
					{
						XmlTypeMapping xmlMap;
						if (serializerInfo.SerializationFormat == SerializationFormat.Literal)
						{
							XmlReflectionImporter xmlReflectionImporter = new XmlReflectionImporter();
							xmlMap = xmlReflectionImporter.ImportTypeMapping(type);
						}
						else
						{
							SoapReflectionImporter soapReflectionImporter = new SoapReflectionImporter();
							xmlMap = soapReflectionImporter.ImportTypeMapping(type);
						}
						SerializationCodeGenerator serializationCodeGenerator = new SerializationCodeGenerator(xmlMap, serializerInfo);
						serializationCodeGenerator.GenerateSerializers(streamWriter);
					}
					finally
					{
						streamWriter.Close();
					}
				}
			}
		}

		public void GenerateSerializers(TextWriter writer)
		{
			this._writer = writer;
			this._results = new GenerationResult[this._xmlMaps.Length];
			this.WriteLine("// It is automatically generated");
			this.WriteLine("using System;");
			this.WriteLine("using System.Xml;");
			this.WriteLine("using System.Xml.Schema;");
			this.WriteLine("using System.Xml.Serialization;");
			this.WriteLine("using System.Text;");
			this.WriteLine("using System.Collections;");
			this.WriteLine("using System.Globalization;");
			if (this._config != null && this._config.NamespaceImports != null && this._config.NamespaceImports.Length > 0)
			{
				foreach (string str in this._config.NamespaceImports)
				{
					this.WriteLine("using " + str + ";");
				}
			}
			this.WriteLine(string.Empty);
			string text = null;
			string text2 = null;
			string text3 = null;
			string text4 = null;
			string text5 = null;
			if (this._config != null)
			{
				text = this._config.ReaderClassName;
				text2 = this._config.WriterClassName;
				text3 = this._config.BaseSerializerClassName;
				text4 = this._config.ImplementationClassName;
				text5 = this._config.Namespace;
			}
			if (text == null || text.Length == 0)
			{
				text = "GeneratedReader";
			}
			if (text2 == null || text2.Length == 0)
			{
				text2 = "GeneratedWriter";
			}
			if (text3 == null || text3.Length == 0)
			{
				text3 = "BaseXmlSerializer";
			}
			if (text4 == null || text4.Length == 0)
			{
				text4 = "XmlSerializerContract";
			}
			text = this.GetUniqueClassName(text);
			text2 = this.GetUniqueClassName(text2);
			text3 = this.GetUniqueClassName(text3);
			text4 = this.GetUniqueClassName(text4);
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			for (int j = 0; j < this._xmlMaps.Length; j++)
			{
				this._typeMap = this._xmlMaps[j];
				if (this._typeMap != null)
				{
					this._result = (hashtable2[this._typeMap] as GenerationResult);
					if (this._result != null)
					{
						this._results[j] = this._result;
					}
					else
					{
						this._result = new GenerationResult();
						this._results[j] = this._result;
						hashtable2[this._typeMap] = this._result;
						string str2;
						if (this._typeMap is XmlTypeMapping)
						{
							str2 = CodeIdentifier.MakeValid(((XmlTypeMapping)this._typeMap).TypeData.CSharpName);
						}
						else
						{
							str2 = ((XmlMembersMapping)this._typeMap).ElementName;
						}
						this._result.ReaderClassName = text;
						this._result.WriterClassName = text2;
						this._result.BaseSerializerClassName = text3;
						this._result.ImplementationClassName = text4;
						if (text5 == null || text5.Length == 0)
						{
							this._result.Namespace = "Mono.GeneratedSerializers." + this._typeMap.Format;
						}
						else
						{
							this._result.Namespace = text5;
						}
						this._result.WriteMethodName = this.GetUniqueName("rwo", this._typeMap, "WriteRoot_" + str2);
						this._result.ReadMethodName = this.GetUniqueName("rro", this._typeMap, "ReadRoot_" + str2);
						this._result.Mapping = this._typeMap;
						ArrayList arrayList = (ArrayList)hashtable[this._result.Namespace];
						if (arrayList == null)
						{
							arrayList = new ArrayList();
							hashtable[this._result.Namespace] = arrayList;
						}
						arrayList.Add(this._result);
					}
				}
			}
			foreach (object obj in hashtable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				ArrayList arrayList2 = (ArrayList)dictionaryEntry.Value;
				this.WriteLine("namespace " + dictionaryEntry.Key);
				this.WriteLineInd("{");
				if (this._config == null || !this._config.NoReader)
				{
					this.GenerateReader(text, arrayList2);
				}
				this.WriteLine(string.Empty);
				if (this._config == null || !this._config.NoWriter)
				{
					this.GenerateWriter(text2, arrayList2);
				}
				this.WriteLine(string.Empty);
				this.GenerateContract(arrayList2);
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
			}
		}

		public GenerationResult[] GenerationResults
		{
			get
			{
				return this._results;
			}
		}

		public ArrayList ReferencedTypes
		{
			get
			{
				return this._referencedTypes;
			}
		}

		private void UpdateGeneratedTypes(ArrayList list)
		{
			for (int i = 0; i < list.Count; i++)
			{
				XmlTypeMapping xmlTypeMapping = list[i] as XmlTypeMapping;
				if (xmlTypeMapping != null && !this._referencedTypes.Contains(xmlTypeMapping.TypeData.Type))
				{
					this._referencedTypes.Add(xmlTypeMapping.TypeData.Type);
				}
			}
		}

		private static string ToCSharpFullName(Type type)
		{
			return TypeData.ToCSharpName(type, true);
		}

		public void GenerateContract(ArrayList generatedMaps)
		{
			if (generatedMaps.Count == 0)
			{
				return;
			}
			GenerationResult generationResult = (GenerationResult)generatedMaps[0];
			string baseSerializerClassName = generationResult.BaseSerializerClassName;
			string text = (this._config != null && this._config.GenerateAsInternal) ? "internal" : "public";
			this.WriteLine(string.Empty);
			this.WriteLine(text + " class " + baseSerializerClassName + " : System.Xml.Serialization.XmlSerializer");
			this.WriteLineInd("{");
			this.WriteLineInd("protected override System.Xml.Serialization.XmlSerializationReader CreateReader () {");
			this.WriteLine("return new " + generationResult.ReaderClassName + " ();");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("protected override System.Xml.Serialization.XmlSerializationWriter CreateWriter () {");
			this.WriteLine("return new " + generationResult.WriterClassName + " ();");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override bool CanDeserialize (System.Xml.XmlReader xmlReader) {");
			this.WriteLine("return true;");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			foreach (object obj in generatedMaps)
			{
				GenerationResult generationResult2 = (GenerationResult)obj;
				generationResult2.SerializerClassName = this.GetUniqueClassName(generationResult2.Mapping.ElementName + "Serializer");
				this.WriteLine(string.Concat(new string[]
				{
					text,
					" sealed class ",
					generationResult2.SerializerClassName,
					" : ",
					baseSerializerClassName
				}));
				this.WriteLineInd("{");
				this.WriteLineInd("protected override void Serialize (object obj, System.Xml.Serialization.XmlSerializationWriter writer) {");
				this.WriteLine(string.Concat(new string[]
				{
					"((",
					generationResult2.WriterClassName,
					")writer).",
					generationResult2.WriteMethodName,
					"(obj);"
				}));
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.WriteLineInd("protected override object Deserialize (System.Xml.Serialization.XmlSerializationReader reader) {");
				this.WriteLine(string.Concat(new string[]
				{
					"return ((",
					generationResult2.ReaderClassName,
					")reader).",
					generationResult2.ReadMethodName,
					"();"
				}));
				this.WriteLineUni("}");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
			}
			this.WriteLine("#if !TARGET_JVM");
			this.WriteLine(text + " class " + generationResult.ImplementationClassName + " : System.Xml.Serialization.XmlSerializerImplementation");
			this.WriteLineInd("{");
			this.WriteLine("System.Collections.Hashtable readMethods = null;");
			this.WriteLine("System.Collections.Hashtable writeMethods = null;");
			this.WriteLine("System.Collections.Hashtable typedSerializers = null;");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override System.Xml.Serialization.XmlSerializationReader Reader {");
			this.WriteLineInd("get {");
			this.WriteLine("return new " + generationResult.ReaderClassName + "();");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override System.Xml.Serialization.XmlSerializationWriter Writer {");
			this.WriteLineInd("get {");
			this.WriteLine("return new " + generationResult.WriterClassName + "();");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override System.Collections.Hashtable ReadMethods {");
			this.WriteLineInd("get {");
			this.WriteLineInd("lock (this) {");
			this.WriteLineInd("if (readMethods == null) {");
			this.WriteLine("readMethods = new System.Collections.Hashtable ();");
			foreach (object obj2 in generatedMaps)
			{
				GenerationResult generationResult3 = (GenerationResult)obj2;
				this.WriteLine(string.Concat(new string[]
				{
					"readMethods.Add (@\"",
					generationResult3.Mapping.GetKey(),
					"\", @\"",
					generationResult3.ReadMethodName,
					"\");"
				}));
			}
			this.WriteLineUni("}");
			this.WriteLine("return readMethods;");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override System.Collections.Hashtable WriteMethods {");
			this.WriteLineInd("get {");
			this.WriteLineInd("lock (this) {");
			this.WriteLineInd("if (writeMethods == null) {");
			this.WriteLine("writeMethods = new System.Collections.Hashtable ();");
			foreach (object obj3 in generatedMaps)
			{
				GenerationResult generationResult4 = (GenerationResult)obj3;
				this.WriteLine(string.Concat(new string[]
				{
					"writeMethods.Add (@\"",
					generationResult4.Mapping.GetKey(),
					"\", @\"",
					generationResult4.WriteMethodName,
					"\");"
				}));
			}
			this.WriteLineUni("}");
			this.WriteLine("return writeMethods;");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override System.Collections.Hashtable TypedSerializers {");
			this.WriteLineInd("get {");
			this.WriteLineInd("lock (this) {");
			this.WriteLineInd("if (typedSerializers == null) {");
			this.WriteLine("typedSerializers = new System.Collections.Hashtable ();");
			foreach (object obj4 in generatedMaps)
			{
				GenerationResult generationResult5 = (GenerationResult)obj4;
				this.WriteLine(string.Concat(new string[]
				{
					"typedSerializers.Add (@\"",
					generationResult5.Mapping.GetKey(),
					"\", new ",
					generationResult5.SerializerClassName,
					"());"
				}));
			}
			this.WriteLineUni("}");
			this.WriteLine("return typedSerializers;");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLine("public override XmlSerializer GetSerializer (Type type)");
			this.WriteLineInd("{");
			this.WriteLine("switch (type.FullName) {");
			foreach (object obj5 in generatedMaps)
			{
				GenerationResult generationResult6 = (GenerationResult)obj5;
				if (generationResult6.Mapping is XmlTypeMapping)
				{
					this.WriteLineInd("case \"" + ((XmlTypeMapping)generationResult6.Mapping).TypeData.CSharpFullName + "\":");
					this.WriteLine("return (XmlSerializer) TypedSerializers [\"" + generationResult6.Mapping.GetKey() + "\"];");
					this.WriteLineUni(string.Empty);
				}
			}
			this.WriteLine("}");
			this.WriteLine("return base.GetSerializer (type);");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLineInd("public override bool CanSerialize (System.Type type) {");
			foreach (object obj6 in generatedMaps)
			{
				GenerationResult generationResult7 = (GenerationResult)obj6;
				if (generationResult7.Mapping is XmlTypeMapping)
				{
					this.WriteLine("if (type == typeof(" + (generationResult7.Mapping as XmlTypeMapping).TypeData.CSharpFullName + ")) return true;");
				}
			}
			this.WriteLine("return false;");
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLine("#endif");
		}

		public void GenerateWriter(string writerClassName, ArrayList maps)
		{
			this._mapsToGenerate = new ArrayList();
			this.InitHooks();
			if (this._config == null || !this._config.GenerateAsInternal)
			{
				this.WriteLine("public class " + writerClassName + " : XmlSerializationWriter");
			}
			else
			{
				this.WriteLine("internal class " + writerClassName + " : XmlSerializationWriter");
			}
			this.WriteLineInd("{");
			this.WriteLine("const string xmlNamespace = \"http://www.w3.org/2000/xmlns/\";");
			this.WriteLine("static readonly System.Reflection.MethodInfo toBinHexStringMethod = typeof (XmlConvert).GetMethod (\"ToBinHexString\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type [] {typeof (byte [])}, null);");
			this.WriteLine("static string ToBinHexString (byte [] input)");
			this.WriteLineInd("{");
			this.WriteLine("return input == null ? null : (string) toBinHexStringMethod.Invoke (null, new object [] {input});");
			this.WriteLineUni("}");
			for (int i = 0; i < maps.Count; i++)
			{
				GenerationResult generationResult = (GenerationResult)maps[i];
				this._typeMap = generationResult.Mapping;
				this._format = this._typeMap.Format;
				this._result = generationResult;
				this.GenerateWriteRoot();
			}
			for (int j = 0; j < this._mapsToGenerate.Count; j++)
			{
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)this._mapsToGenerate[j];
				this.GenerateWriteObject(xmlTypeMapping);
				if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Enum)
				{
					this.GenerateGetXmlEnumValue(xmlTypeMapping);
				}
			}
			this.GenerateWriteInitCallbacks();
			this.UpdateGeneratedTypes(this._mapsToGenerate);
			this.WriteLineUni("}");
		}

		private void GenerateWriteRoot()
		{
			this.WriteLine("public void " + this._result.WriteMethodName + " (object o)");
			this.WriteLineInd("{");
			this.WriteLine("WriteStartDocument ();");
			if (this._typeMap is XmlTypeMapping)
			{
				this.WriteLine(this.GetRootTypeName() + " ob = (" + this.GetRootTypeName() + ") o;");
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)this._typeMap;
				if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class || xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Array)
				{
					this.WriteLine("TopLevelElement ();");
				}
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteLine(string.Concat(new string[]
					{
						this.GetWriteObjectName(xmlTypeMapping),
						" (ob, ",
						this.GetLiteral(xmlTypeMapping.ElementName),
						", ",
						this.GetLiteral(xmlTypeMapping.Namespace),
						", true, false, true);"
					}));
				}
				else
				{
					this.RegisterReferencingMap(xmlTypeMapping);
					this.WriteLine(string.Concat(new string[]
					{
						"WritePotentiallyReferencingElement (",
						this.GetLiteral(xmlTypeMapping.ElementName),
						", ",
						this.GetLiteral(xmlTypeMapping.Namespace),
						", ob, ",
						this.GetTypeOf(xmlTypeMapping.TypeData),
						", true, false);"
					}));
				}
			}
			else
			{
				if (!(this._typeMap is XmlMembersMapping))
				{
					throw new InvalidOperationException("Unknown type map");
				}
				this.WriteLine("object[] pars = (object[]) o;");
				this.GenerateWriteMessage((XmlMembersMapping)this._typeMap);
			}
			if (this._format == SerializationFormat.Encoded)
			{
				this.WriteLine("WriteReferencedElements ();");
			}
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		private void GenerateWriteMessage(XmlMembersMapping membersMap)
		{
			if (membersMap.HasWrapperElement)
			{
				this.WriteLine("TopLevelElement ();");
				this.WriteLine(string.Concat(new string[]
				{
					"WriteStartElement (",
					this.GetLiteral(membersMap.ElementName),
					", ",
					this.GetLiteral(membersMap.Namespace),
					", (",
					this.GetLiteral(this._format == SerializationFormat.Encoded),
					"));"
				}));
			}
			this.GenerateWriteObjectElement(membersMap, "pars", true);
			if (membersMap.HasWrapperElement)
			{
				this.WriteLine("WriteEndElement();");
			}
		}

		private void GenerateGetXmlEnumValue(XmlTypeMapping map)
		{
			EnumMap enumMap = (EnumMap)map.ObjectMap;
			string str = null;
			string text = null;
			if (enumMap.IsFlags)
			{
				str = this.GetUniqueName("gxe", map, "_xmlNames" + map.XmlType);
				this.Write("static readonly string[] " + str + " = { ");
				for (int i = 0; i < enumMap.XmlNames.Length; i++)
				{
					if (i > 0)
					{
						this._writer.Write(',');
					}
					this._writer.Write('"');
					this._writer.Write(enumMap.XmlNames[i]);
					this._writer.Write('"');
				}
				this._writer.WriteLine(" };");
				text = this.GetUniqueName("gve", map, "_values" + map.XmlType);
				this.Write("static readonly long[] " + text + " = { ");
				for (int j = 0; j < enumMap.Values.Length; j++)
				{
					if (j > 0)
					{
						this._writer.Write(',');
					}
					this._writer.Write(enumMap.Values[j].ToString(CultureInfo.InvariantCulture));
					this._writer.Write("L");
				}
				this._writer.WriteLine(" };");
				this.WriteLine(string.Empty);
			}
			this.WriteLine(string.Concat(new string[]
			{
				"string ",
				this.GetGetEnumValueName(map),
				" (",
				map.TypeData.CSharpFullName,
				" val)"
			}));
			this.WriteLineInd("{");
			this.WriteLineInd("switch (val) {");
			for (int k = 0; k < enumMap.EnumNames.Length; k++)
			{
				this.WriteLine(string.Concat(new string[]
				{
					"case ",
					map.TypeData.CSharpFullName,
					".@",
					enumMap.EnumNames[k],
					": return ",
					this.GetLiteral(enumMap.XmlNames[k]),
					";"
				}));
			}
			if (enumMap.IsFlags)
			{
				this.WriteLineInd("default:");
				this.WriteLine("if (val.ToString () == \"0\") return string.Empty;");
				this.Write("return FromEnum ((long) val, " + str + ", " + text);
				this._writer.Write(", typeof (");
				this._writer.Write(map.TypeData.CSharpFullName);
				this._writer.Write(").FullName");
				this._writer.Write(')');
				this.WriteUni(";");
			}
			else
			{
				this.WriteLine("default: throw CreateInvalidEnumValueException ((long) val, typeof (" + map.TypeData.CSharpFullName + ").FullName);");
			}
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		private void GenerateWriteObject(XmlTypeMapping typeMap)
		{
			this.WriteLine(string.Concat(new string[]
			{
				"void ",
				this.GetWriteObjectName(typeMap),
				" (",
				typeMap.TypeData.CSharpFullName,
				" ob, string element, string namesp, bool isNullable, bool needType, bool writeWrappingElem)"
			}));
			this.WriteLineInd("{");
			this.PushHookContext();
			this.SetHookVar("$TYPE", typeMap.TypeData.CSharpName);
			this.SetHookVar("$FULLTYPE", typeMap.TypeData.CSharpFullName);
			this.SetHookVar("$OBJECT", "ob");
			this.SetHookVar("$NULLABLE", "isNullable");
			if (this.GenerateWriteHook(HookType.type, typeMap.TypeData.Type))
			{
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.PopHookContext();
				return;
			}
			if (!typeMap.TypeData.IsValueType)
			{
				this.WriteLine("if (((object)ob) == null)");
				this.WriteLineInd("{");
				this.WriteLineInd("if (isNullable)");
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteLine("WriteNullTagLiteral(element, namesp);");
				}
				else
				{
					this.WriteLine("WriteNullTagEncoded (element, namesp);");
				}
				this.WriteLineUni("return;");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
			}
			if (typeMap.TypeData.SchemaType == SchemaTypes.XmlNode)
			{
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteLine("WriteElementLiteral (ob, \"\", \"\", true, false);");
				}
				else
				{
					this.WriteLine("WriteElementEncoded (ob, \"\", \"\", true, false);");
				}
				this.GenerateEndHook();
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.PopHookContext();
				return;
			}
			if (typeMap.TypeData.SchemaType == SchemaTypes.XmlSerializable)
			{
				this.WriteLine("WriteSerializable (ob, element, namesp, isNullable);");
				this.GenerateEndHook();
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.PopHookContext();
				return;
			}
			ArrayList derivedTypes = typeMap.DerivedTypes;
			this.WriteLine("System.Type type = ob.GetType ();");
			this.WriteLine("if (type == typeof(" + typeMap.TypeData.CSharpFullName + "))");
			this.WriteLine("{ }");
			for (int i = 0; i < derivedTypes.Count; i++)
			{
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)derivedTypes[i];
				this.WriteLineInd("else if (type == typeof(" + xmlTypeMapping.TypeData.CSharpFullName + ")) { ");
				this.WriteLine(this.GetWriteObjectName(xmlTypeMapping) + "((" + xmlTypeMapping.TypeData.CSharpFullName + ")ob, element, namesp, isNullable, true, writeWrappingElem);");
				this.WriteLine("return;");
				this.WriteLineUni("}");
			}
			if (typeMap.TypeData.Type == typeof(object))
			{
				this.WriteLineInd("else {");
				this.WriteLineInd("if (ob.GetType().IsArray && typeof(XmlNode).IsAssignableFrom(ob.GetType().GetElementType())) {");
				this.WriteLine(string.Concat(new string[]
				{
					"Writer.WriteStartElement (",
					this.GetLiteral(typeMap.ElementName),
					", ",
					this.GetLiteral(typeMap.Namespace),
					");"
				}));
				this.WriteLineInd("foreach (XmlNode node in (System.Collections.IEnumerable) ob)");
				this.WriteLineUni("node.WriteTo (Writer);");
				this.WriteLineUni("Writer.WriteEndElement ();");
				this.WriteLine("}");
				this.WriteLineInd("else");
				this.WriteLineUni("WriteTypedPrimitive (element, namesp, ob, true);");
				this.WriteLine("return;");
				this.WriteLineUni("}");
			}
			else
			{
				this.WriteLineInd("else {");
				this.WriteLine("throw CreateUnknownTypeException (ob);");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.WriteLineInd("if (writeWrappingElem) {");
				if (this._format == SerializationFormat.Encoded)
				{
					this.WriteLine("needType = true;");
				}
				this.WriteLine("WriteStartElement (element, namesp, ob);");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.WriteLine(string.Concat(new string[]
				{
					"if (needType) WriteXsiType(",
					this.GetLiteral(typeMap.XmlType),
					", ",
					this.GetLiteral(typeMap.XmlTypeNamespace),
					");"
				}));
				this.WriteLine(string.Empty);
				switch (typeMap.TypeData.SchemaType)
				{
				case SchemaTypes.Primitive:
					this.GenerateWritePrimitiveElement(typeMap, "ob");
					break;
				case SchemaTypes.Enum:
					this.GenerateWriteEnumElement(typeMap, "ob");
					break;
				case SchemaTypes.Array:
					this.GenerateWriteListElement(typeMap, "ob");
					break;
				case SchemaTypes.Class:
					this.GenerateWriteObjectElement(typeMap, "ob", false);
					break;
				}
				this.WriteLine("if (writeWrappingElem) WriteEndElement (ob);");
			}
			this.GenerateEndHook();
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.PopHookContext();
		}

		private void GenerateWriteObjectElement(XmlMapping xmlMap, string ob, bool isValueList)
		{
			XmlTypeMapping xmlTypeMapping = xmlMap as XmlTypeMapping;
			Type type = (xmlTypeMapping == null) ? typeof(object[]) : xmlTypeMapping.TypeData.Type;
			ClassMap classMap = (ClassMap)xmlMap.ObjectMap;
			if (!this.GenerateWriteHook(HookType.attributes, type))
			{
				if (classMap.NamespaceDeclarations != null)
				{
					this.WriteLine(string.Concat(new string[]
					{
						"WriteNamespaceDeclarations ((XmlSerializerNamespaces) ",
						ob,
						".@",
						classMap.NamespaceDeclarations.Name,
						");"
					}));
					this.WriteLine(string.Empty);
				}
				XmlTypeMapMember defaultAnyAttributeMember = classMap.DefaultAnyAttributeMember;
				if (defaultAnyAttributeMember != null && !this.GenerateWriteMemberHook(type, defaultAnyAttributeMember))
				{
					string text = this.GenerateMemberHasValueCondition(defaultAnyAttributeMember, ob, isValueList);
					if (text != null)
					{
						this.WriteLineInd("if (" + text + ") {");
					}
					string obTempVar = this.GetObTempVar();
					this.WriteLine(string.Concat(new string[]
					{
						"ICollection ",
						obTempVar,
						" = ",
						this.GenerateGetMemberValue(defaultAnyAttributeMember, ob, isValueList),
						";"
					}));
					this.WriteLineInd("if (" + obTempVar + " != null) {");
					string obTempVar2 = this.GetObTempVar();
					this.WriteLineInd(string.Concat(new string[]
					{
						"foreach (XmlAttribute ",
						obTempVar2,
						" in ",
						obTempVar,
						")"
					}));
					this.WriteLineInd("if (" + obTempVar2 + ".NamespaceURI != xmlNamespace)");
					this.WriteLine(string.Concat(new string[]
					{
						"WriteXmlAttribute (",
						obTempVar2,
						", ",
						ob,
						");"
					}));
					this.Unindent();
					this.Unindent();
					this.WriteLineUni("}");
					if (text != null)
					{
						this.WriteLineUni("}");
					}
					this.WriteLine(string.Empty);
					this.GenerateEndHook();
				}
				ICollection attributeMembers = classMap.AttributeMembers;
				if (attributeMembers != null)
				{
					foreach (object obj in attributeMembers)
					{
						XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)obj;
						if (!this.GenerateWriteMemberHook(type, xmlTypeMapMemberAttribute))
						{
							string value = this.GenerateGetMemberValue(xmlTypeMapMemberAttribute, ob, isValueList);
							string text2 = this.GenerateMemberHasValueCondition(xmlTypeMapMemberAttribute, ob, isValueList);
							if (text2 != null)
							{
								this.WriteLineInd("if (" + text2 + ") {");
							}
							string text3 = this.GenerateGetStringValue(xmlTypeMapMemberAttribute.MappedType, xmlTypeMapMemberAttribute.TypeData, value, false);
							this.WriteLine(string.Concat(new string[]
							{
								"WriteAttribute (",
								this.GetLiteral(xmlTypeMapMemberAttribute.AttributeName),
								", ",
								this.GetLiteral(xmlTypeMapMemberAttribute.Namespace),
								", ",
								text3,
								");"
							}));
							if (text2 != null)
							{
								this.WriteLineUni("}");
							}
							this.GenerateEndHook();
						}
					}
					this.WriteLine(string.Empty);
				}
				this.GenerateEndHook();
			}
			if (!this.GenerateWriteHook(HookType.elements, type))
			{
				ICollection elementMembers = classMap.ElementMembers;
				if (elementMembers != null)
				{
					foreach (object obj2 in elementMembers)
					{
						XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)obj2;
						if (!this.GenerateWriteMemberHook(type, xmlTypeMapMemberElement))
						{
							string text4 = this.GenerateMemberHasValueCondition(xmlTypeMapMemberElement, ob, isValueList);
							if (text4 != null)
							{
								this.WriteLineInd("if (" + text4 + ") {");
							}
							string text5 = this.GenerateGetMemberValue(xmlTypeMapMemberElement, ob, isValueList);
							Type type2 = xmlTypeMapMemberElement.GetType();
							if (type2 == typeof(XmlTypeMapMemberList))
							{
								this.GenerateWriteMemberElement((XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0], text5);
							}
							else if (type2 == typeof(XmlTypeMapMemberFlatList))
							{
								this.WriteLineInd("if (" + text5 + " != null) {");
								this.GenerateWriteListContent(ob, xmlTypeMapMemberElement.TypeData, ((XmlTypeMapMemberFlatList)xmlTypeMapMemberElement).ListMap, text5, false);
								this.WriteLineUni("}");
							}
							else if (type2 == typeof(XmlTypeMapMemberAnyElement))
							{
								this.WriteLineInd("if (" + text5 + " != null) {");
								this.GenerateWriteAnyElementContent((XmlTypeMapMemberAnyElement)xmlTypeMapMemberElement, text5);
								this.WriteLineUni("}");
							}
							else if (type2 == typeof(XmlTypeMapMemberAnyElement))
							{
								this.WriteLineInd("if (" + text5 + " != null) {");
								this.GenerateWriteAnyElementContent((XmlTypeMapMemberAnyElement)xmlTypeMapMemberElement, text5);
								this.WriteLineUni("}");
							}
							else if (type2 != typeof(XmlTypeMapMemberAnyAttribute))
							{
								if (type2 != typeof(XmlTypeMapMemberElement))
								{
									throw new InvalidOperationException("Unknown member type");
								}
								if (xmlTypeMapMemberElement.ElementInfo.Count == 1)
								{
									this.GenerateWriteMemberElement((XmlTypeMapElementInfo)xmlTypeMapMemberElement.ElementInfo[0], text5);
								}
								else if (xmlTypeMapMemberElement.ChoiceMember != null)
								{
									string text6 = ob + ".@" + xmlTypeMapMemberElement.ChoiceMember;
									foreach (object obj3 in xmlTypeMapMemberElement.ElementInfo)
									{
										XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj3;
										this.WriteLineInd(string.Concat(new string[]
										{
											"if (",
											text6,
											" == ",
											this.GetLiteral(xmlTypeMapElementInfo.ChoiceValue),
											") {"
										}));
										this.GenerateWriteMemberElement(xmlTypeMapElementInfo, this.GetCast(xmlTypeMapElementInfo.TypeData, xmlTypeMapMemberElement.TypeData, text5));
										this.WriteLineUni("}");
									}
								}
								else
								{
									bool flag = true;
									foreach (object obj4 in xmlTypeMapMemberElement.ElementInfo)
									{
										XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)obj4;
										this.WriteLineInd(string.Concat(new string[]
										{
											(!flag) ? "else " : string.Empty,
											"if (",
											text5,
											" is ",
											xmlTypeMapElementInfo2.TypeData.CSharpFullName,
											") {"
										}));
										this.GenerateWriteMemberElement(xmlTypeMapElementInfo2, this.GetCast(xmlTypeMapElementInfo2.TypeData, xmlTypeMapMemberElement.TypeData, text5));
										this.WriteLineUni("}");
										flag = false;
									}
								}
							}
							if (text4 != null)
							{
								this.WriteLineUni("}");
							}
							this.GenerateEndHook();
						}
					}
				}
				this.GenerateEndHook();
			}
		}

		private void GenerateWriteMemberElement(XmlTypeMapElementInfo elem, string memberValue)
		{
			switch (elem.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
			case SchemaTypes.Enum:
				if (this._format == SerializationFormat.Literal)
				{
					this.GenerateWritePrimitiveValueLiteral(memberValue, elem.ElementName, elem.Namespace, elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
				}
				else
				{
					this.GenerateWritePrimitiveValueEncoded(memberValue, elem.ElementName, elem.Namespace, new XmlQualifiedName(elem.TypeData.XmlType, elem.DataTypeNamespace), elem.MappedType, elem.TypeData, elem.WrappedElement, elem.IsNullable);
				}
				break;
			case SchemaTypes.Array:
				this.WriteLineInd("if (" + memberValue + " != null) {");
				if (elem.MappedType.MultiReferenceType)
				{
					this.WriteMetCall("WriteReferencingElement", new string[]
					{
						this.GetLiteral(elem.ElementName),
						this.GetLiteral(elem.Namespace),
						memberValue,
						this.GetLiteral(elem.IsNullable)
					});
					this.RegisterReferencingMap(elem.MappedType);
				}
				else
				{
					this.WriteMetCall("WriteStartElement", new string[]
					{
						this.GetLiteral(elem.ElementName),
						this.GetLiteral(elem.Namespace),
						memberValue
					});
					this.GenerateWriteListContent(null, elem.TypeData, (ListMap)elem.MappedType.ObjectMap, memberValue, false);
					this.WriteMetCall("WriteEndElement", new string[]
					{
						memberValue
					});
				}
				this.WriteLineUni("}");
				if (elem.IsNullable)
				{
					this.WriteLineInd("else");
					if (this._format == SerializationFormat.Literal)
					{
						this.WriteMetCall("WriteNullTagLiteral", new string[]
						{
							this.GetLiteral(elem.ElementName),
							this.GetLiteral(elem.Namespace)
						});
					}
					else
					{
						this.WriteMetCall("WriteNullTagEncoded", new string[]
						{
							this.GetLiteral(elem.ElementName),
							this.GetLiteral(elem.Namespace)
						});
					}
					this.Unindent();
				}
				break;
			case SchemaTypes.Class:
				if (elem.MappedType.MultiReferenceType)
				{
					this.RegisterReferencingMap(elem.MappedType);
					if (elem.MappedType.TypeData.Type == typeof(object))
					{
						this.WriteMetCall("WritePotentiallyReferencingElement", new string[]
						{
							this.GetLiteral(elem.ElementName),
							this.GetLiteral(elem.Namespace),
							memberValue,
							"null",
							"false",
							this.GetLiteral(elem.IsNullable)
						});
					}
					else
					{
						this.WriteMetCall("WriteReferencingElement", new string[]
						{
							this.GetLiteral(elem.ElementName),
							this.GetLiteral(elem.Namespace),
							memberValue,
							this.GetLiteral(elem.IsNullable)
						});
					}
				}
				else
				{
					this.WriteMetCall(this.GetWriteObjectName(elem.MappedType), new string[]
					{
						memberValue,
						this.GetLiteral(elem.ElementName),
						this.GetLiteral(elem.Namespace),
						this.GetLiteral(elem.IsNullable),
						"false",
						"true"
					});
				}
				break;
			case SchemaTypes.XmlSerializable:
				this.WriteMetCall("WriteSerializable", new string[]
				{
					"(" + SerializationCodeGenerator.ToCSharpFullName(elem.MappedType.TypeData.Type) + ") " + memberValue,
					this.GetLiteral(elem.ElementName),
					this.GetLiteral(elem.Namespace),
					this.GetLiteral(elem.IsNullable)
				});
				break;
			case SchemaTypes.XmlNode:
			{
				string ob = (!elem.WrappedElement) ? string.Empty : elem.ElementName;
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteMetCall("WriteElementLiteral", new string[]
					{
						memberValue,
						this.GetLiteral(ob),
						this.GetLiteral(elem.Namespace),
						this.GetLiteral(elem.IsNullable),
						"false"
					});
				}
				else
				{
					this.WriteMetCall("WriteElementEncoded", new string[]
					{
						memberValue,
						this.GetLiteral(ob),
						this.GetLiteral(elem.Namespace),
						this.GetLiteral(elem.IsNullable),
						"false"
					});
				}
				break;
			}
			default:
				throw new NotSupportedException("Invalid value type");
			}
		}

		private void GenerateWriteListElement(XmlTypeMapping typeMap, string ob)
		{
			if (this._format == SerializationFormat.Encoded)
			{
				string itemCount = this.GenerateGetListCount(typeMap.TypeData, ob);
				string text;
				string text2;
				this.GenerateGetArrayType((ListMap)typeMap.ObjectMap, itemCount, out text, out text2);
				string text3;
				if (text2 != string.Empty)
				{
					text3 = string.Concat(new string[]
					{
						"FromXmlQualifiedName (new XmlQualifiedName(",
						text,
						",",
						text2,
						"))"
					});
				}
				else
				{
					text3 = this.GetLiteral(text);
				}
				this.WriteMetCall("WriteAttribute", new string[]
				{
					this.GetLiteral("arrayType"),
					this.GetLiteral("http://schemas.xmlsoap.org/soap/encoding/"),
					text3
				});
			}
			this.GenerateWriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, ob, false);
		}

		private void GenerateWriteAnyElementContent(XmlTypeMapMemberAnyElement member, string memberValue)
		{
			bool flag = member.TypeData.Type == typeof(XmlElement);
			string text;
			if (flag)
			{
				text = memberValue;
			}
			else
			{
				text = this.GetObTempVar();
				this.WriteLineInd(string.Concat(new string[]
				{
					"foreach (XmlNode ",
					text,
					" in ",
					memberValue,
					") {"
				}));
			}
			string obTempVar = this.GetObTempVar();
			this.WriteLine(string.Concat(new string[]
			{
				"XmlNode ",
				obTempVar,
				" = ",
				text,
				";"
			}));
			this.WriteLine("if (" + obTempVar + " is XmlElement) {");
			if (!member.IsDefaultAny)
			{
				for (int i = 0; i < member.ElementInfo.Count; i++)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)member.ElementInfo[i];
					string text2 = string.Concat(new string[]
					{
						"(",
						obTempVar,
						".LocalName == ",
						this.GetLiteral(xmlTypeMapElementInfo.ElementName),
						" && ",
						obTempVar,
						".NamespaceURI == ",
						this.GetLiteral(xmlTypeMapElementInfo.Namespace),
						")"
					});
					if (i == member.ElementInfo.Count - 1)
					{
						text2 += ") {";
					}
					if (i == 0)
					{
						this.WriteLineInd("if (" + text2);
					}
					else
					{
						this.WriteLine("|| " + text2);
					}
				}
			}
			this.WriteLine("}");
			this.WriteLine("else " + obTempVar + ".WriteTo (Writer);");
			if (this._format == SerializationFormat.Literal)
			{
				this.WriteLine("WriteElementLiteral (" + obTempVar + ", \"\", \"\", false, true);");
			}
			else
			{
				this.WriteLine("WriteElementEncoded (" + obTempVar + ", \"\", \"\", false, true);");
			}
			if (!member.IsDefaultAny)
			{
				this.WriteLineUni("}");
				this.WriteLineInd("else");
				this.WriteLine(string.Concat(new string[]
				{
					"throw CreateUnknownAnyElementException (",
					obTempVar,
					".Name, ",
					obTempVar,
					".NamespaceURI);"
				}));
				this.Unindent();
			}
			if (!flag)
			{
				this.WriteLineUni("}");
			}
		}

		private void GenerateWritePrimitiveElement(XmlTypeMapping typeMap, string ob)
		{
			string str = this.GenerateGetStringValue(typeMap, typeMap.TypeData, ob, false);
			this.WriteLine("Writer.WriteString (" + str + ");");
		}

		private void GenerateWriteEnumElement(XmlTypeMapping typeMap, string ob)
		{
			string str = this.GenerateGetEnumXmlValue(typeMap, ob);
			this.WriteLine("Writer.WriteString (" + str + ");");
		}

		private string GenerateGetStringValue(XmlTypeMapping typeMap, TypeData type, string value, bool isNullable)
		{
			if (type.SchemaType == SchemaTypes.Array)
			{
				string strTempVar = this.GetStrTempVar();
				this.WriteLine("string " + strTempVar + " = null;");
				this.WriteLineInd("if (" + value + " != null) {");
				string str = this.GenerateWriteListContent(null, typeMap.TypeData, (ListMap)typeMap.ObjectMap, value, true);
				this.WriteLine(strTempVar + " = " + str + ".ToString ().Trim ();");
				this.WriteLineUni("}");
				return strTempVar;
			}
			if (type.SchemaType == SchemaTypes.Enum)
			{
				if (isNullable)
				{
					return string.Concat(new string[]
					{
						"(",
						value,
						").HasValue ? ",
						this.GenerateGetEnumXmlValue(typeMap, "(" + value + ").Value"),
						" : null"
					});
				}
				return this.GenerateGetEnumXmlValue(typeMap, value);
			}
			else
			{
				if (type.Type == typeof(XmlQualifiedName))
				{
					return "FromXmlQualifiedName (" + value + ")";
				}
				if (value == null)
				{
					return null;
				}
				return XmlCustomFormatter.GenerateToXmlString(type, value);
			}
		}

		private string GenerateGetEnumXmlValue(XmlTypeMapping typeMap, string ob)
		{
			return this.GetGetEnumValueName(typeMap) + " (" + ob + ")";
		}

		private string GenerateGetListCount(TypeData listType, string ob)
		{
			if (listType.Type.IsArray)
			{
				return "ob.Length";
			}
			return "ob.Count";
		}

		private void GenerateGetArrayType(ListMap map, string itemCount, out string localName, out string ns)
		{
			string str;
			if (itemCount != string.Empty)
			{
				str = string.Empty;
			}
			else
			{
				str = "[]";
			}
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)map.ItemInfo[0];
			if (xmlTypeMapElementInfo.TypeData.SchemaType == SchemaTypes.Array)
			{
				string str2;
				this.GenerateGetArrayType((ListMap)xmlTypeMapElementInfo.MappedType.ObjectMap, string.Empty, out str2, out ns);
				localName = str2 + str;
			}
			else if (xmlTypeMapElementInfo.MappedType != null)
			{
				localName = xmlTypeMapElementInfo.MappedType.XmlType + str;
				ns = xmlTypeMapElementInfo.MappedType.Namespace;
			}
			else
			{
				localName = xmlTypeMapElementInfo.TypeData.XmlType + str;
				ns = xmlTypeMapElementInfo.DataTypeNamespace;
			}
			if (itemCount != string.Empty)
			{
				localName = string.Concat(new string[]
				{
					"\"",
					localName,
					"[\" + ",
					itemCount,
					" + \"]\""
				});
				ns = this.GetLiteral(ns);
			}
		}

		private string GenerateWriteListContent(string container, TypeData listType, ListMap map, string ob, bool writeToString)
		{
			string text = null;
			if (writeToString)
			{
				text = this.GetStrTempVar();
				this.WriteLine("System.Text.StringBuilder " + text + " = new System.Text.StringBuilder();");
			}
			if (listType.Type.IsArray)
			{
				string numTempVar = this.GetNumTempVar();
				this.WriteLineInd(string.Concat(new string[]
				{
					"for (int ",
					numTempVar,
					" = 0; ",
					numTempVar,
					" < ",
					ob,
					".Length; ",
					numTempVar,
					"++) {"
				}));
				this.GenerateListLoop(container, map, ob + "[" + numTempVar + "]", numTempVar, listType.ListItemTypeData, text);
				this.WriteLineUni("}");
			}
			else if (typeof(ICollection).IsAssignableFrom(listType.Type))
			{
				string numTempVar2 = this.GetNumTempVar();
				this.WriteLineInd(string.Concat(new string[]
				{
					"for (int ",
					numTempVar2,
					" = 0; ",
					numTempVar2,
					" < ",
					ob,
					".Count; ",
					numTempVar2,
					"++) {"
				}));
				this.GenerateListLoop(container, map, ob + "[" + numTempVar2 + "]", numTempVar2, listType.ListItemTypeData, text);
				this.WriteLineUni("}");
			}
			else
			{
				if (!typeof(IEnumerable).IsAssignableFrom(listType.Type))
				{
					throw new Exception("Unsupported collection type");
				}
				string obTempVar = this.GetObTempVar();
				this.WriteLineInd(string.Concat(new string[]
				{
					"foreach (",
					listType.ListItemTypeData.CSharpFullName,
					" ",
					obTempVar,
					" in ",
					ob,
					") {"
				}));
				this.GenerateListLoop(container, map, obTempVar, null, listType.ListItemTypeData, text);
				this.WriteLineUni("}");
			}
			return text;
		}

		private void GenerateListLoop(string container, ListMap map, string item, string index, TypeData itemTypeData, string targetString)
		{
			bool flag = map.ItemInfo.Count > 1;
			if (map.ChoiceMember != null && container != null && index != null)
			{
				this.WriteLineInd(string.Concat(new string[]
				{
					"if ((",
					container,
					".@",
					map.ChoiceMember,
					" == null) || (",
					index,
					" >= ",
					container,
					".@",
					map.ChoiceMember,
					".Length))"
				}));
				this.WriteLine(string.Concat(new string[]
				{
					"throw CreateInvalidChoiceIdentifierValueException (",
					container,
					".GetType().ToString(), \"",
					map.ChoiceMember,
					"\");"
				}));
				this.Unindent();
			}
			if (flag)
			{
				this.WriteLine("if (((object)" + item + ") == null) { }");
			}
			foreach (object obj in map.ItemInfo)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				if (map.ChoiceMember != null && flag)
				{
					this.WriteLineInd(string.Concat(new string[]
					{
						"else if (",
						container,
						".@",
						map.ChoiceMember,
						"[",
						index,
						"] == ",
						this.GetLiteral(xmlTypeMapElementInfo.ChoiceValue),
						") {"
					}));
				}
				else if (flag)
				{
					this.WriteLineInd(string.Concat(new string[]
					{
						"else if (",
						item,
						".GetType() == typeof(",
						xmlTypeMapElementInfo.TypeData.CSharpFullName,
						")) {"
					}));
				}
				if (targetString == null)
				{
					this.GenerateWriteMemberElement(xmlTypeMapElementInfo, this.GetCast(xmlTypeMapElementInfo.TypeData, itemTypeData, item));
				}
				else
				{
					string str = this.GenerateGetStringValue(xmlTypeMapElementInfo.MappedType, xmlTypeMapElementInfo.TypeData, this.GetCast(xmlTypeMapElementInfo.TypeData, itemTypeData, item), false);
					this.WriteLine(targetString + ".Append (" + str + ").Append (\" \");");
				}
				if (flag)
				{
					this.WriteLineUni("}");
				}
			}
			if (flag)
			{
				this.WriteLine("else throw CreateUnknownTypeException (" + item + ");");
			}
		}

		private void GenerateWritePrimitiveValueLiteral(string memberValue, string name, string ns, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
		{
			if (!wrapped)
			{
				string text = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
				this.WriteMetCall("WriteValue", new string[]
				{
					text
				});
			}
			else if (isNullable)
			{
				if (typeData.Type == typeof(XmlQualifiedName))
				{
					this.WriteMetCall("WriteNullableQualifiedNameLiteral", new string[]
					{
						this.GetLiteral(name),
						this.GetLiteral(ns),
						memberValue
					});
				}
				else
				{
					string text2 = this.GenerateGetStringValue(mappedType, typeData, memberValue, true);
					this.WriteMetCall("WriteNullableStringLiteral", new string[]
					{
						this.GetLiteral(name),
						this.GetLiteral(ns),
						text2
					});
				}
			}
			else if (typeData.Type == typeof(XmlQualifiedName))
			{
				this.WriteMetCall("WriteElementQualifiedName", new string[]
				{
					this.GetLiteral(name),
					this.GetLiteral(ns),
					memberValue
				});
			}
			else
			{
				string text3 = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
				this.WriteMetCall("WriteElementString", new string[]
				{
					this.GetLiteral(name),
					this.GetLiteral(ns),
					text3
				});
			}
		}

		private void GenerateWritePrimitiveValueEncoded(string memberValue, string name, string ns, XmlQualifiedName xsiType, XmlTypeMapping mappedType, TypeData typeData, bool wrapped, bool isNullable)
		{
			if (!wrapped)
			{
				string text = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
				this.WriteMetCall("WriteValue", new string[]
				{
					text
				});
			}
			else if (isNullable)
			{
				if (typeData.Type == typeof(XmlQualifiedName))
				{
					this.WriteMetCall("WriteNullableQualifiedNameEncoded", new string[]
					{
						this.GetLiteral(name),
						this.GetLiteral(ns),
						memberValue,
						this.GetLiteral(xsiType)
					});
				}
				else
				{
					string text2 = this.GenerateGetStringValue(mappedType, typeData, memberValue, true);
					this.WriteMetCall("WriteNullableStringEncoded", new string[]
					{
						this.GetLiteral(name),
						this.GetLiteral(ns),
						text2,
						this.GetLiteral(xsiType)
					});
				}
			}
			else if (typeData.Type == typeof(XmlQualifiedName))
			{
				this.WriteMetCall("WriteElementQualifiedName", new string[]
				{
					this.GetLiteral(name),
					this.GetLiteral(ns),
					memberValue,
					this.GetLiteral(xsiType)
				});
			}
			else
			{
				string text3 = this.GenerateGetStringValue(mappedType, typeData, memberValue, false);
				this.WriteMetCall("WriteElementString", new string[]
				{
					this.GetLiteral(name),
					this.GetLiteral(ns),
					text3,
					this.GetLiteral(xsiType)
				});
			}
		}

		private string GenerateGetMemberValue(XmlTypeMapMember member, string ob, bool isValueList)
		{
			if (isValueList)
			{
				return this.GetCast(member.TypeData, TypeTranslator.GetTypeData(typeof(object)), string.Concat(new object[]
				{
					ob,
					"[",
					member.GlobalIndex,
					"]"
				}));
			}
			return ob + ".@" + member.Name;
		}

		private string GenerateMemberHasValueCondition(XmlTypeMapMember member, string ob, bool isValueList)
		{
			if (isValueList)
			{
				return ob + ".Length > " + member.GlobalIndex;
			}
			if (member.DefaultValue != DBNull.Value)
			{
				string str = ob + ".@" + member.Name;
				if (member.DefaultValue == null)
				{
					return str + " != null";
				}
				if (member.TypeData.SchemaType == SchemaTypes.Enum)
				{
					return str + " != " + this.GetCast(member.TypeData, this.GetLiteral(member.DefaultValue));
				}
				return str + " != " + this.GetLiteral(member.DefaultValue);
			}
			else
			{
				if (member.IsOptionalValueType)
				{
					return ob + ".@" + member.Name + "Specified";
				}
				return null;
			}
		}

		private void GenerateWriteInitCallbacks()
		{
			this.WriteLine("protected override void InitCallbacks ()");
			this.WriteLineInd("{");
			if (this._format == SerializationFormat.Encoded)
			{
				foreach (object obj in this._mapsToGenerate)
				{
					XmlMapping xmlMapping = (XmlMapping)obj;
					XmlTypeMapping xmlTypeMapping = xmlMapping as XmlTypeMapping;
					if (xmlTypeMapping != null)
					{
						this.WriteMetCall("AddWriteCallback", new string[]
						{
							this.GetTypeOf(xmlTypeMapping.TypeData),
							this.GetLiteral(xmlTypeMapping.XmlType),
							this.GetLiteral(xmlTypeMapping.Namespace),
							"new XmlSerializationWriteCallback (" + this.GetWriteObjectCallbackName(xmlTypeMapping) + ")"
						});
					}
				}
			}
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			if (this._format == SerializationFormat.Encoded)
			{
				foreach (object obj2 in this._mapsToGenerate)
				{
					XmlTypeMapping xmlTypeMapping2 = (XmlTypeMapping)obj2;
					XmlTypeMapping xmlTypeMapping3 = xmlTypeMapping2;
					if (xmlTypeMapping3 != null)
					{
						if (xmlTypeMapping3.TypeData.SchemaType == SchemaTypes.Enum)
						{
							this.WriteWriteEnumCallback(xmlTypeMapping3);
						}
						else
						{
							this.WriteWriteObjectCallback(xmlTypeMapping3);
						}
					}
				}
			}
		}

		private void WriteWriteEnumCallback(XmlTypeMapping map)
		{
			this.WriteLine("void " + this.GetWriteObjectCallbackName(map) + " (object ob)");
			this.WriteLineInd("{");
			this.WriteMetCall(this.GetWriteObjectName(map), new string[]
			{
				this.GetCast(map.TypeData, "ob"),
				this.GetLiteral(map.ElementName),
				this.GetLiteral(map.Namespace),
				"false",
				"true",
				"false"
			});
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		private void WriteWriteObjectCallback(XmlTypeMapping map)
		{
			this.WriteLine("void " + this.GetWriteObjectCallbackName(map) + " (object ob)");
			this.WriteLineInd("{");
			this.WriteMetCall(this.GetWriteObjectName(map), new string[]
			{
				this.GetCast(map.TypeData, "ob"),
				this.GetLiteral(map.ElementName),
				this.GetLiteral(map.Namespace),
				"false",
				"false",
				"false"
			});
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		public void GenerateReader(string readerClassName, ArrayList maps)
		{
			if (this._config == null || !this._config.GenerateAsInternal)
			{
				this.WriteLine("public class " + readerClassName + " : XmlSerializationReader");
			}
			else
			{
				this.WriteLine("internal class " + readerClassName + " : XmlSerializationReader");
			}
			this.WriteLineInd("{");
			this.WriteLine("static readonly System.Reflection.MethodInfo fromBinHexStringMethod = typeof (XmlConvert).GetMethod (\"FromBinHexString\", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic, null, new Type [] {typeof (string)}, null);");
			this.WriteLine("static byte [] FromBinHexString (string input)");
			this.WriteLineInd("{");
			this.WriteLine("return input == null ? null : (byte []) fromBinHexStringMethod.Invoke (null, new object [] {input});");
			this.WriteLineUni("}");
			this._mapsToGenerate = new ArrayList();
			this._fixupCallbacks = new ArrayList();
			this.InitHooks();
			for (int i = 0; i < maps.Count; i++)
			{
				GenerationResult generationResult = (GenerationResult)maps[i];
				this._typeMap = generationResult.Mapping;
				this._format = this._typeMap.Format;
				this._result = generationResult;
				this.GenerateReadRoot();
			}
			for (int j = 0; j < this._mapsToGenerate.Count; j++)
			{
				XmlTypeMapping xmlTypeMapping = this._mapsToGenerate[j] as XmlTypeMapping;
				if (xmlTypeMapping != null)
				{
					this.GenerateReadObject(xmlTypeMapping);
					if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Enum)
					{
						this.GenerateGetEnumValueMethod(xmlTypeMapping);
					}
				}
			}
			this.GenerateReadInitCallbacks();
			if (this._format == SerializationFormat.Encoded)
			{
				this.GenerateFixupCallbacks();
				this.GenerateFillerCallbacks();
			}
			this.WriteLineUni("}");
			this.UpdateGeneratedTypes(this._mapsToGenerate);
		}

		private void GenerateReadRoot()
		{
			this.WriteLine("public object " + this._result.ReadMethodName + " ()");
			this.WriteLineInd("{");
			this.WriteLine("Reader.MoveToContent();");
			if (this._typeMap is XmlTypeMapping)
			{
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)this._typeMap;
				if (this._format == SerializationFormat.Literal)
				{
					if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.XmlNode)
					{
						if (xmlTypeMapping.TypeData.Type == typeof(XmlDocument))
						{
							this.WriteLine("return ReadXmlDocument (false);");
						}
						else
						{
							this.WriteLine("return ReadXmlNode (false);");
						}
					}
					else
					{
						this.WriteLineInd(string.Concat(new string[]
						{
							"if (Reader.LocalName != ",
							this.GetLiteral(xmlTypeMapping.ElementName),
							" || Reader.NamespaceURI != ",
							this.GetLiteral(xmlTypeMapping.Namespace),
							")"
						}));
						this.WriteLine("throw CreateUnknownNodeException();");
						this.Unindent();
						this.WriteLine("return " + this.GetReadObjectCall(xmlTypeMapping, this.GetLiteral(xmlTypeMapping.IsNullable), "true") + ";");
					}
				}
				else
				{
					this.WriteLine("object ob = null;");
					this.WriteLine("Reader.MoveToContent();");
					this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
					this.WriteLineInd("{");
					this.WriteLineInd(string.Concat(new string[]
					{
						"if (Reader.LocalName == ",
						this.GetLiteral(xmlTypeMapping.ElementName),
						" && Reader.NamespaceURI == ",
						this.GetLiteral(xmlTypeMapping.Namespace),
						")"
					}));
					this.WriteLine("ob = ReadReferencedElement();");
					this.Unindent();
					this.WriteLineInd("else ");
					this.WriteLine("throw CreateUnknownNodeException();");
					this.Unindent();
					this.WriteLineUni("}");
					this.WriteLineInd("else ");
					this.WriteLine("UnknownNode(null);");
					this.Unindent();
					this.WriteLine(string.Empty);
					this.WriteLine("ReadReferencedElements();");
					this.WriteLine("return ob;");
					this.RegisterReferencingMap(xmlTypeMapping);
				}
			}
			else
			{
				this.WriteLine("return " + this.GenerateReadMessage((XmlMembersMapping)this._typeMap) + ";");
			}
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		private string GenerateReadMessage(XmlMembersMapping typeMap)
		{
			this.WriteLine("object[] parameters = new object[" + typeMap.Count + "];");
			this.WriteLine(string.Empty);
			if (typeMap.HasWrapperElement)
			{
				if (this._format == SerializationFormat.Encoded)
				{
					this.WriteLine("while (Reader.NodeType == System.Xml.XmlNodeType.Element)");
					this.WriteLineInd("{");
					this.WriteLine("string root = Reader.GetAttribute (\"root\", " + this.GetLiteral("http://schemas.xmlsoap.org/soap/encoding/") + ");");
					this.WriteLine("if (root == null || System.Xml.XmlConvert.ToBoolean(root)) break;");
					this.WriteLine("ReadReferencedElement ();");
					this.WriteLine("Reader.MoveToContent ();");
					this.WriteLineUni("}");
					this.WriteLine(string.Empty);
					this.WriteLine("if (Reader.NodeType != System.Xml.XmlNodeType.EndElement)");
					this.WriteLineInd("{");
					this.WriteLineInd("if (Reader.IsEmptyElement) {");
					this.WriteLine("Reader.Skip();");
					this.WriteLine("Reader.MoveToContent();");
					this.WriteLineUni("}");
					this.WriteLineInd("else {");
					this.WriteLine("Reader.ReadStartElement();");
					this.GenerateReadMembers(typeMap, (ClassMap)typeMap.ObjectMap, "parameters", true, false);
					this.WriteLine("ReadEndElement();");
					this.WriteLineUni("}");
					this.WriteLine(string.Empty);
					this.WriteLine("Reader.MoveToContent();");
					this.WriteLineUni("}");
				}
				else
				{
					ClassMap classMap = (ClassMap)typeMap.ObjectMap;
					ArrayList allMembers = classMap.AllMembers;
					for (int i = 0; i < allMembers.Count; i++)
					{
						XmlTypeMapMember xmlTypeMapMember = (XmlTypeMapMember)allMembers[i];
						if (!xmlTypeMapMember.IsReturnValue && xmlTypeMapMember.TypeData.IsValueType)
						{
							this.GenerateSetMemberValueFromAttr(xmlTypeMapMember, "parameters", string.Format("({0}) Activator.CreateInstance(typeof({0}), true)", xmlTypeMapMember.TypeData.FullTypeName), true);
						}
					}
					this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement && Reader.ReadState == ReadState.Interactive)");
					this.WriteLineInd("{");
					this.WriteLine(string.Concat(new string[]
					{
						"if (Reader.IsStartElement(",
						this.GetLiteral(typeMap.ElementName),
						", ",
						this.GetLiteral(typeMap.Namespace),
						"))"
					}));
					this.WriteLineInd("{");
					bool flag = false;
					this.GenerateReadAttributeMembers(typeMap, (ClassMap)typeMap.ObjectMap, "parameters", true, ref flag);
					this.WriteLine("if (Reader.IsEmptyElement)");
					this.WriteLineInd("{");
					this.WriteLine("Reader.Skip(); Reader.MoveToContent(); continue;");
					this.WriteLineUni("}");
					this.WriteLine("Reader.ReadStartElement();");
					this.GenerateReadMembers(typeMap, (ClassMap)typeMap.ObjectMap, "parameters", true, false);
					this.WriteLine("ReadEndElement();");
					this.WriteLine("break;");
					this.WriteLineUni("}");
					this.WriteLineInd("else ");
					this.WriteLine("UnknownNode(null);");
					this.Unindent();
					this.WriteLine(string.Empty);
					this.WriteLine("Reader.MoveToContent();");
					this.WriteLineUni("}");
				}
			}
			else
			{
				this.GenerateReadMembers(typeMap, (ClassMap)typeMap.ObjectMap, "parameters", true, this._format == SerializationFormat.Encoded);
			}
			if (this._format == SerializationFormat.Encoded)
			{
				this.WriteLine("ReadReferencedElements();");
			}
			return "parameters";
		}

		private void GenerateReadObject(XmlTypeMapping typeMap)
		{
			string isNullable;
			if (this._format == SerializationFormat.Literal)
			{
				this.WriteLine(string.Concat(new string[]
				{
					"public ",
					typeMap.TypeData.CSharpFullName,
					" ",
					this.GetReadObjectName(typeMap),
					" (bool isNullable, bool checkType)"
				}));
				isNullable = "isNullable";
			}
			else
			{
				this.WriteLine("public object " + this.GetReadObjectName(typeMap) + " ()");
				isNullable = "true";
			}
			this.WriteLineInd("{");
			this.PushHookContext();
			this.SetHookVar("$TYPE", typeMap.TypeData.CSharpName);
			this.SetHookVar("$FULLTYPE", typeMap.TypeData.CSharpFullName);
			this.SetHookVar("$NULLABLE", "isNullable");
			switch (typeMap.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
				this.GenerateReadPrimitiveElement(typeMap, isNullable);
				break;
			case SchemaTypes.Enum:
				this.GenerateReadEnumElement(typeMap, isNullable);
				break;
			case SchemaTypes.Array:
			{
				string text = this.GenerateReadListElement(typeMap, null, isNullable, true);
				if (text != null)
				{
					this.WriteLine("return " + text + ";");
				}
				break;
			}
			case SchemaTypes.Class:
				this.GenerateReadClassInstance(typeMap, isNullable, "checkType");
				break;
			case SchemaTypes.XmlSerializable:
				this.GenerateReadXmlSerializableElement(typeMap, isNullable);
				break;
			case SchemaTypes.XmlNode:
				this.GenerateReadXmlNodeElement(typeMap, isNullable);
				break;
			default:
				throw new Exception("Unsupported map type");
			}
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.PopHookContext();
		}

		private void GenerateReadClassInstance(XmlTypeMapping typeMap, string isNullable, string checkType)
		{
			this.SetHookVar("$OBJECT", "ob");
			if (!typeMap.TypeData.IsValueType)
			{
				this.WriteLine(typeMap.TypeData.CSharpFullName + " ob = null;");
				if (this.GenerateReadHook(HookType.type, typeMap.TypeData.Type))
				{
					this.WriteLine("return ob;");
					return;
				}
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteLine("if (" + isNullable + " && ReadNull()) return null;");
					this.WriteLine(string.Empty);
					this.WriteLine("if (checkType) ");
					this.WriteLineInd("{");
				}
				else
				{
					this.WriteLine("if (ReadNull()) return null;");
					this.WriteLine(string.Empty);
				}
			}
			else
			{
				this.WriteLine(typeMap.TypeData.CSharpFullName + string.Format(" ob = ({0}) Activator.CreateInstance(typeof({0}), true);", typeMap.TypeData.CSharpFullName));
				if (this.GenerateReadHook(HookType.type, typeMap.TypeData.Type))
				{
					this.WriteLine("return ob;");
					return;
				}
			}
			this.WriteLine("System.Xml.XmlQualifiedName t = GetXsiType();");
			this.WriteLine("if (t == null)");
			if (typeMap.TypeData.Type != typeof(object))
			{
				this.WriteLine("{ }");
			}
			else
			{
				this.WriteLine("\treturn " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (new System.Xml.XmlQualifiedName(\"anyType\", System.Xml.Schema.XmlSchema.Namespace))") + ";");
			}
			foreach (object obj in typeMap.DerivedTypes)
			{
				XmlTypeMapping xmlTypeMapping = (XmlTypeMapping)obj;
				this.WriteLineInd(string.Concat(new string[]
				{
					"else if (t.Name == ",
					this.GetLiteral(xmlTypeMapping.XmlType),
					" && t.Namespace == ",
					this.GetLiteral(xmlTypeMapping.XmlTypeNamespace),
					")"
				}));
				this.WriteLine("return " + this.GetReadObjectCall(xmlTypeMapping, isNullable, checkType) + ";");
				this.Unindent();
			}
			this.WriteLine(string.Concat(new string[]
			{
				"else if (t.Name != ",
				this.GetLiteral(typeMap.XmlType),
				" || t.Namespace != ",
				this.GetLiteral(typeMap.XmlTypeNamespace),
				")"
			}));
			if (typeMap.TypeData.Type == typeof(object))
			{
				this.WriteLine("\treturn " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (t)") + ";");
			}
			else
			{
				this.WriteLine("\tthrow CreateUnknownTypeException(t);");
			}
			if (!typeMap.TypeData.IsValueType)
			{
				if (this._format == SerializationFormat.Literal)
				{
					this.WriteLineUni("}");
				}
				if (typeMap.TypeData.Type.IsAbstract)
				{
					this.GenerateEndHook();
					this.WriteLine("return ob;");
					return;
				}
				this.WriteLine(string.Empty);
				this.WriteLine(string.Format("ob = ({0}) Activator.CreateInstance(typeof({0}), true);", typeMap.TypeData.CSharpFullName));
			}
			this.WriteLine(string.Empty);
			this.WriteLine("Reader.MoveToElement();");
			this.WriteLine(string.Empty);
			this.GenerateReadMembers(typeMap, (ClassMap)typeMap.ObjectMap, "ob", false, false);
			this.WriteLine(string.Empty);
			this.GenerateEndHook();
			this.WriteLine("return ob;");
		}

		private void GenerateReadMembers(XmlMapping xmlMap, ClassMap map, string ob, bool isValueList, bool readByOrder)
		{
			XmlTypeMapping xmlTypeMapping = xmlMap as XmlTypeMapping;
			Type type = (xmlTypeMapping == null) ? typeof(object[]) : xmlTypeMapping.TypeData.Type;
			bool flag = false;
			this.GenerateReadAttributeMembers(xmlMap, map, ob, isValueList, ref flag);
			if (!isValueList)
			{
				this.WriteLine("Reader.MoveToElement();");
				this.WriteLineInd("if (Reader.IsEmptyElement) {");
				this.WriteLine("Reader.Skip ();");
				this.GenerateSetListMembersDefaults(xmlTypeMapping, map, ob, isValueList);
				this.WriteLine("return " + ob + ";");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				this.WriteLine("Reader.ReadStartElement();");
			}
			this.WriteLine("Reader.MoveToContent();");
			this.WriteLine(string.Empty);
			if (!this.GenerateReadHook(HookType.elements, type))
			{
				string[] array = null;
				if (map.ElementMembers != null && !readByOrder)
				{
					string text = string.Empty;
					array = new string[map.ElementMembers.Count];
					int num = 0;
					foreach (object obj in map.ElementMembers)
					{
						XmlTypeMapMember xmlTypeMapMember = (XmlTypeMapMember)obj;
						if (!(xmlTypeMapMember is XmlTypeMapMemberElement) || !((XmlTypeMapMemberElement)xmlTypeMapMember).IsXmlTextCollector)
						{
							array[num] = this.GetBoolTempVar();
							if (text.Length > 0)
							{
								text += ", ";
							}
							text = text + array[num] + "=false";
						}
						num++;
					}
					if (text.Length > 0)
					{
						text = "bool " + text;
						this.WriteLine(text + ";");
					}
					this.WriteLine(string.Empty);
				}
				string[] array2 = null;
				string[] array3 = null;
				string[] array4 = null;
				if (map.FlatLists != null)
				{
					array2 = new string[map.FlatLists.Count];
					array3 = new string[map.FlatLists.Count];
					string str = "int ";
					for (int i = 0; i < map.FlatLists.Count; i++)
					{
						XmlTypeMapMemberElement xmlTypeMapMemberElement = (XmlTypeMapMemberElement)map.FlatLists[i];
						array2[i] = this.GetNumTempVar();
						if (i > 0)
						{
							str += ", ";
						}
						str = str + array2[i] + "=0";
						if (!this.MemberHasReadReplaceHook(type, xmlTypeMapMemberElement))
						{
							array3[i] = this.GetObTempVar();
							this.WriteLine(xmlTypeMapMemberElement.TypeData.CSharpFullName + " " + array3[i] + ";");
							if (this.IsReadOnly(xmlTypeMapping, xmlTypeMapMemberElement, xmlTypeMapMemberElement.TypeData, isValueList))
							{
								string str2 = this.GenerateGetMemberValue(xmlTypeMapMemberElement, ob, isValueList);
								this.WriteLine(array3[i] + " = " + str2 + ";");
							}
							else if (xmlTypeMapMemberElement.TypeData.Type.IsArray)
							{
								string str2 = this.GenerateInitializeList(xmlTypeMapMemberElement.TypeData);
								this.WriteLine(array3[i] + " = " + str2 + ";");
							}
							else
							{
								this.WriteLine(array3[i] + " = " + this.GenerateGetMemberValue(xmlTypeMapMemberElement, ob, isValueList) + ";");
								this.WriteLineInd("if (((object)" + array3[i] + ") == null) {");
								this.WriteLine(array3[i] + " = " + this.GenerateInitializeList(xmlTypeMapMemberElement.TypeData) + ";");
								this.GenerateSetMemberValue(xmlTypeMapMemberElement, ob, array3[i], isValueList);
								this.WriteLineUni("}");
							}
						}
						if (xmlTypeMapMemberElement.ChoiceMember != null)
						{
							if (array4 == null)
							{
								array4 = new string[map.FlatLists.Count];
							}
							array4[i] = this.GetObTempVar();
							string text2 = this.GenerateInitializeList(xmlTypeMapMemberElement.ChoiceTypeData);
							this.WriteLine(string.Concat(new string[]
							{
								xmlTypeMapMemberElement.ChoiceTypeData.CSharpFullName,
								" ",
								array4[i],
								" = ",
								text2,
								";"
							}));
						}
					}
					this.WriteLine(str + ";");
					this.WriteLine(string.Empty);
				}
				if (this._format == SerializationFormat.Encoded && map.ElementMembers != null)
				{
					this._fixupCallbacks.Add(xmlMap);
					this.WriteLine(string.Concat(new object[]
					{
						"Fixup fixup = new Fixup(",
						ob,
						", new XmlSerializationFixupCallback(",
						this.GetFixupCallbackName(xmlMap),
						"), ",
						map.ElementMembers.Count,
						");"
					}));
					this.WriteLine("AddFixup (fixup);");
					this.WriteLine(string.Empty);
				}
				ArrayList arrayList = null;
				int num2;
				if (readByOrder)
				{
					if (map.ElementMembers != null)
					{
						num2 = map.ElementMembers.Count;
					}
					else
					{
						num2 = 0;
					}
				}
				else
				{
					arrayList = new ArrayList();
					arrayList.AddRange(map.AllElementInfos);
					num2 = arrayList.Count;
					this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement) ");
					this.WriteLineInd("{");
					this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
					this.WriteLineInd("{");
				}
				flag = true;
				int j = 0;
				while (j < num2)
				{
					XmlTypeMapElementInfo xmlTypeMapElementInfo = (!readByOrder) ? ((XmlTypeMapElementInfo)arrayList[j]) : map.GetElement(j);
					if (readByOrder)
					{
						goto IL_66D;
					}
					if (!xmlTypeMapElementInfo.IsTextElement && !xmlTypeMapElementInfo.IsUnnamedAnyElement)
					{
						string text3 = (!flag) ? "else " : string.Empty;
						text3 += "if (";
						if (!xmlTypeMapElementInfo.Member.IsReturnValue || this._format != SerializationFormat.Encoded)
						{
							text3 = text3 + "Reader.LocalName == " + this.GetLiteral(xmlTypeMapElementInfo.ElementName);
							if (!map.IgnoreMemberNamespace)
							{
								text3 = text3 + " && Reader.NamespaceURI == " + this.GetLiteral(xmlTypeMapElementInfo.Namespace);
							}
							text3 += " && ";
						}
						text3 = text3 + "!" + array[xmlTypeMapElementInfo.Member.Index] + ") {";
						this.WriteLineInd(text3);
						goto IL_66D;
					}
					IL_FB5:
					j++;
					continue;
					IL_66D:
					if (xmlTypeMapElementInfo.Member.GetType() == typeof(XmlTypeMapMemberList))
					{
						if (this._format == SerializationFormat.Encoded && xmlTypeMapElementInfo.MultiReferenceType)
						{
							string obTempVar = this.GetObTempVar();
							this.WriteLine(string.Concat(new object[]
							{
								"object ",
								obTempVar,
								" = ReadReferencingElement (out fixup.Ids[",
								xmlTypeMapElementInfo.Member.Index,
								"]);"
							}));
							this.RegisterReferencingMap(xmlTypeMapElementInfo.MappedType);
							this.WriteLineInd("if (fixup.Ids[" + xmlTypeMapElementInfo.Member.Index + "] == null) {");
							if (this.IsReadOnly(xmlTypeMapping, xmlTypeMapElementInfo.Member, xmlTypeMapElementInfo.TypeData, isValueList))
							{
								this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral(xmlTypeMapElementInfo.TypeData.CSharpFullName) + ");");
							}
							else
							{
								this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, this.GetCast(xmlTypeMapElementInfo.Member.TypeData, obTempVar), isValueList);
							}
							this.WriteLineUni("}");
							if (!xmlTypeMapElementInfo.MappedType.TypeData.Type.IsArray)
							{
								this.WriteLineInd("else {");
								if (this.IsReadOnly(xmlTypeMapping, xmlTypeMapElementInfo.Member, xmlTypeMapElementInfo.TypeData, isValueList))
								{
									this.WriteLine(obTempVar + " = " + this.GenerateGetMemberValue(xmlTypeMapElementInfo.Member, ob, isValueList) + ";");
								}
								else
								{
									this.WriteLine(obTempVar + " = " + this.GenerateCreateList(xmlTypeMapElementInfo.MappedType.TypeData.Type) + ";");
									this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, this.GetCast(xmlTypeMapElementInfo.Member.TypeData, obTempVar), isValueList);
								}
								this.WriteLine(string.Concat(new object[]
								{
									"AddFixup (new CollectionFixup (",
									obTempVar,
									", new XmlSerializationCollectionFixupCallback (",
									this.GetFillListName(xmlTypeMapElementInfo.Member.TypeData),
									"), fixup.Ids[",
									xmlTypeMapElementInfo.Member.Index,
									"]));"
								}));
								this.WriteLine("fixup.Ids[" + xmlTypeMapElementInfo.Member.Index + "] = null;");
								this.WriteLineUni("}");
							}
						}
						else if (!this.GenerateReadMemberHook(type, xmlTypeMapElementInfo.Member))
						{
							if (this.IsReadOnly(xmlTypeMapping, xmlTypeMapElementInfo.Member, xmlTypeMapElementInfo.TypeData, isValueList))
							{
								this.GenerateReadListElement(xmlTypeMapElementInfo.MappedType, this.GenerateGetMemberValue(xmlTypeMapElementInfo.Member, ob, isValueList), this.GetLiteral(xmlTypeMapElementInfo.IsNullable), false);
							}
							else if (xmlTypeMapElementInfo.MappedType.TypeData.Type.IsArray)
							{
								if (xmlTypeMapElementInfo.IsNullable)
								{
									this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, this.GenerateReadListElement(xmlTypeMapElementInfo.MappedType, null, this.GetLiteral(xmlTypeMapElementInfo.IsNullable), true), isValueList);
								}
								else
								{
									string obTempVar2 = this.GetObTempVar();
									this.WriteLine(string.Concat(new string[]
									{
										xmlTypeMapElementInfo.MappedType.TypeData.CSharpFullName,
										" ",
										obTempVar2,
										" = ",
										this.GenerateReadListElement(xmlTypeMapElementInfo.MappedType, null, this.GetLiteral(xmlTypeMapElementInfo.IsNullable), true),
										";"
									}));
									this.WriteLineInd("if (((object)" + obTempVar2 + ") != null) {");
									this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, obTempVar2, isValueList);
									this.WriteLineUni("}");
								}
							}
							else
							{
								string obTempVar3 = this.GetObTempVar();
								this.WriteLine(string.Concat(new string[]
								{
									xmlTypeMapElementInfo.MappedType.TypeData.CSharpFullName,
									" ",
									obTempVar3,
									" = ",
									this.GenerateGetMemberValue(xmlTypeMapElementInfo.Member, ob, isValueList),
									";"
								}));
								this.WriteLineInd("if (((object)" + obTempVar3 + ") == null) {");
								this.WriteLine(obTempVar3 + " = " + this.GenerateCreateList(xmlTypeMapElementInfo.MappedType.TypeData.Type) + ";");
								this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, obTempVar3, isValueList);
								this.WriteLineUni("}");
								this.GenerateReadListElement(xmlTypeMapElementInfo.MappedType, obTempVar3, this.GetLiteral(xmlTypeMapElementInfo.IsNullable), true);
							}
							this.GenerateEndHook();
						}
						if (!readByOrder)
						{
							this.WriteLine(array[xmlTypeMapElementInfo.Member.Index] + " = true;");
						}
					}
					else if (xmlTypeMapElementInfo.Member.GetType() == typeof(XmlTypeMapMemberFlatList))
					{
						XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList = (XmlTypeMapMemberFlatList)xmlTypeMapElementInfo.Member;
						if (!this.GenerateReadArrayMemberHook(type, xmlTypeMapElementInfo.Member, array2[xmlTypeMapMemberFlatList.FlatArrayIndex]))
						{
							this.GenerateAddListValue(xmlTypeMapMemberFlatList.TypeData, array3[xmlTypeMapMemberFlatList.FlatArrayIndex], array2[xmlTypeMapMemberFlatList.FlatArrayIndex], this.GenerateReadObjectElement(xmlTypeMapElementInfo), !this.IsReadOnly(xmlTypeMapping, xmlTypeMapElementInfo.Member, xmlTypeMapElementInfo.TypeData, isValueList));
							if (xmlTypeMapMemberFlatList.ChoiceMember != null)
							{
								this.GenerateAddListValue(xmlTypeMapMemberFlatList.ChoiceTypeData, array4[xmlTypeMapMemberFlatList.FlatArrayIndex], array2[xmlTypeMapMemberFlatList.FlatArrayIndex], this.GetLiteral(xmlTypeMapElementInfo.ChoiceValue), true);
							}
							this.GenerateEndHook();
						}
						this.WriteLine(array2[xmlTypeMapMemberFlatList.FlatArrayIndex] + "++;");
					}
					else if (xmlTypeMapElementInfo.Member.GetType() == typeof(XmlTypeMapMemberAnyElement))
					{
						XmlTypeMapMemberAnyElement xmlTypeMapMemberAnyElement = (XmlTypeMapMemberAnyElement)xmlTypeMapElementInfo.Member;
						if (xmlTypeMapMemberAnyElement.TypeData.IsListType)
						{
							if (!this.GenerateReadArrayMemberHook(type, xmlTypeMapElementInfo.Member, array2[xmlTypeMapMemberAnyElement.FlatArrayIndex]))
							{
								this.GenerateAddListValue(xmlTypeMapMemberAnyElement.TypeData, array3[xmlTypeMapMemberAnyElement.FlatArrayIndex], array2[xmlTypeMapMemberAnyElement.FlatArrayIndex], this.GetReadXmlNode(xmlTypeMapMemberAnyElement.TypeData.ListItemTypeData, false), true);
								this.GenerateEndHook();
							}
							this.WriteLine(array2[xmlTypeMapMemberAnyElement.FlatArrayIndex] + "++;");
						}
						else if (!this.GenerateReadMemberHook(type, xmlTypeMapElementInfo.Member))
						{
							this.GenerateSetMemberValue(xmlTypeMapMemberAnyElement, ob, this.GetReadXmlNode(xmlTypeMapMemberAnyElement.TypeData, false), isValueList);
							this.GenerateEndHook();
						}
					}
					else
					{
						if (xmlTypeMapElementInfo.Member.GetType() != typeof(XmlTypeMapMemberElement))
						{
							throw new InvalidOperationException("Unknown member type");
						}
						if (!readByOrder)
						{
							this.WriteLine(array[xmlTypeMapElementInfo.Member.Index] + " = true;");
						}
						if (this._format == SerializationFormat.Encoded)
						{
							string obTempVar4 = this.GetObTempVar();
							this.RegisterReferencingMap(xmlTypeMapElementInfo.MappedType);
							if (xmlTypeMapElementInfo.Member.TypeData.SchemaType != SchemaTypes.Primitive)
							{
								this.WriteLine(string.Concat(new object[]
								{
									"object ",
									obTempVar4,
									" = ReadReferencingElement (out fixup.Ids[",
									xmlTypeMapElementInfo.Member.Index,
									"]);"
								}));
							}
							else
							{
								this.WriteLine(string.Concat(new object[]
								{
									"object ",
									obTempVar4,
									" = ReadReferencingElement (",
									this.GetLiteral(xmlTypeMapElementInfo.Member.TypeData.XmlType),
									", ",
									this.GetLiteral("http://www.w3.org/2001/XMLSchema"),
									", out fixup.Ids[",
									xmlTypeMapElementInfo.Member.Index,
									"]);"
								}));
							}
							if (xmlTypeMapElementInfo.MultiReferenceType)
							{
								this.WriteLineInd("if (fixup.Ids[" + xmlTypeMapElementInfo.Member.Index + "] == null) {");
							}
							else
							{
								this.WriteLineInd("if (" + obTempVar4 + " != null) {");
							}
							this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, this.GetCast(xmlTypeMapElementInfo.Member.TypeData, obTempVar4), isValueList);
							this.WriteLineUni("}");
						}
						else if (!this.GenerateReadMemberHook(type, xmlTypeMapElementInfo.Member))
						{
							if (xmlTypeMapElementInfo.ChoiceValue != null)
							{
								XmlTypeMapMemberElement xmlTypeMapMemberElement2 = (XmlTypeMapMemberElement)xmlTypeMapElementInfo.Member;
								this.WriteLine(string.Concat(new string[]
								{
									ob,
									".@",
									xmlTypeMapMemberElement2.ChoiceMember,
									" = ",
									this.GetLiteral(xmlTypeMapElementInfo.ChoiceValue),
									";"
								}));
							}
							this.GenerateSetMemberValue(xmlTypeMapElementInfo.Member, ob, this.GenerateReadObjectElement(xmlTypeMapElementInfo), isValueList);
							this.GenerateEndHook();
						}
					}
					if (!readByOrder)
					{
						this.WriteLineUni("}");
					}
					else
					{
						this.WriteLine("Reader.MoveToContent();");
					}
					flag = false;
					goto IL_FB5;
				}
				if (!readByOrder)
				{
					if (!flag)
					{
						this.WriteLineInd("else {");
					}
					if (map.DefaultAnyElementMember != null)
					{
						XmlTypeMapMemberAnyElement defaultAnyElementMember = map.DefaultAnyElementMember;
						if (defaultAnyElementMember.TypeData.IsListType)
						{
							if (!this.GenerateReadArrayMemberHook(type, defaultAnyElementMember, array2[defaultAnyElementMember.FlatArrayIndex]))
							{
								this.GenerateAddListValue(defaultAnyElementMember.TypeData, array3[defaultAnyElementMember.FlatArrayIndex], array2[defaultAnyElementMember.FlatArrayIndex], this.GetReadXmlNode(defaultAnyElementMember.TypeData.ListItemTypeData, false), true);
								this.GenerateEndHook();
							}
							this.WriteLine(array2[defaultAnyElementMember.FlatArrayIndex] + "++;");
						}
						else if (!this.GenerateReadMemberHook(type, defaultAnyElementMember))
						{
							this.GenerateSetMemberValue(defaultAnyElementMember, ob, this.GetReadXmlNode(defaultAnyElementMember.TypeData, false), isValueList);
							this.GenerateEndHook();
						}
					}
					else if (!this.GenerateReadHook(HookType.unknownElement, type))
					{
						this.WriteLine("UnknownNode (" + ob + ");");
						this.GenerateEndHook();
					}
					if (!flag)
					{
						this.WriteLineUni("}");
					}
					this.WriteLineUni("}");
					if (map.XmlTextCollector != null)
					{
						this.WriteLine("else if (Reader.NodeType == System.Xml.XmlNodeType.Text || Reader.NodeType == System.Xml.XmlNodeType.CDATA)");
						this.WriteLineInd("{");
						if (map.XmlTextCollector is XmlTypeMapMemberExpandable)
						{
							XmlTypeMapMemberExpandable xmlTypeMapMemberExpandable = (XmlTypeMapMemberExpandable)map.XmlTextCollector;
							XmlTypeMapMemberFlatList xmlTypeMapMemberFlatList2 = xmlTypeMapMemberExpandable as XmlTypeMapMemberFlatList;
							TypeData typeData = (xmlTypeMapMemberFlatList2 != null) ? xmlTypeMapMemberFlatList2.ListMap.FindTextElement().TypeData : xmlTypeMapMemberExpandable.TypeData.ListItemTypeData;
							if (!this.GenerateReadArrayMemberHook(type, map.XmlTextCollector, array2[xmlTypeMapMemberExpandable.FlatArrayIndex]))
							{
								string value = (typeData.Type != typeof(string)) ? this.GetReadXmlNode(typeData, false) : "Reader.ReadString()";
								this.GenerateAddListValue(xmlTypeMapMemberExpandable.TypeData, array3[xmlTypeMapMemberExpandable.FlatArrayIndex], array2[xmlTypeMapMemberExpandable.FlatArrayIndex], value, true);
								this.GenerateEndHook();
							}
							this.WriteLine(array2[xmlTypeMapMemberExpandable.FlatArrayIndex] + "++;");
						}
						else if (!this.GenerateReadMemberHook(type, map.XmlTextCollector))
						{
							XmlTypeMapMemberElement xmlTypeMapMemberElement3 = (XmlTypeMapMemberElement)map.XmlTextCollector;
							XmlTypeMapElementInfo xmlTypeMapElementInfo2 = (XmlTypeMapElementInfo)xmlTypeMapMemberElement3.ElementInfo[0];
							if (xmlTypeMapElementInfo2.TypeData.Type == typeof(string))
							{
								this.GenerateSetMemberValue(xmlTypeMapMemberElement3, ob, "ReadString (" + this.GenerateGetMemberValue(xmlTypeMapMemberElement3, ob, isValueList) + ")", isValueList);
							}
							else
							{
								this.WriteLineInd("{");
								string strTempVar = this.GetStrTempVar();
								this.WriteLine("string " + strTempVar + " = Reader.ReadString();");
								this.GenerateSetMemberValue(xmlTypeMapMemberElement3, ob, this.GenerateGetValueFromXmlString(strTempVar, xmlTypeMapElementInfo2.TypeData, xmlTypeMapElementInfo2.MappedType, xmlTypeMapElementInfo2.IsNullable), isValueList);
								this.WriteLineUni("}");
							}
							this.GenerateEndHook();
						}
						this.WriteLineUni("}");
					}
					this.WriteLine("else");
					this.WriteLine("\tUnknownNode(" + ob + ");");
					this.WriteLine(string.Empty);
					this.WriteLine("Reader.MoveToContent();");
					this.WriteLineUni("}");
				}
				else
				{
					this.WriteLine("Reader.MoveToContent();");
				}
				if (array3 != null)
				{
					this.WriteLine(string.Empty);
					foreach (object obj2 in map.FlatLists)
					{
						XmlTypeMapMemberExpandable xmlTypeMapMemberExpandable2 = (XmlTypeMapMemberExpandable)obj2;
						if (!this.MemberHasReadReplaceHook(type, xmlTypeMapMemberExpandable2))
						{
							string text4 = array3[xmlTypeMapMemberExpandable2.FlatArrayIndex];
							if (xmlTypeMapMemberExpandable2.TypeData.Type.IsArray)
							{
								this.WriteLine(string.Concat(new string[]
								{
									text4,
									" = (",
									xmlTypeMapMemberExpandable2.TypeData.CSharpFullName,
									") ShrinkArray (",
									text4,
									", ",
									array2[xmlTypeMapMemberExpandable2.FlatArrayIndex],
									", ",
									this.GetTypeOf(xmlTypeMapMemberExpandable2.TypeData.Type.GetElementType()),
									", true);"
								}));
							}
							if (!this.IsReadOnly(xmlTypeMapping, xmlTypeMapMemberExpandable2, xmlTypeMapMemberExpandable2.TypeData, isValueList) && xmlTypeMapMemberExpandable2.TypeData.Type.IsArray)
							{
								this.GenerateSetMemberValue(xmlTypeMapMemberExpandable2, ob, text4, isValueList);
							}
						}
					}
				}
				if (array4 != null)
				{
					this.WriteLine(string.Empty);
					foreach (object obj3 in map.FlatLists)
					{
						XmlTypeMapMemberExpandable xmlTypeMapMemberExpandable3 = (XmlTypeMapMemberExpandable)obj3;
						if (!this.MemberHasReadReplaceHook(type, xmlTypeMapMemberExpandable3))
						{
							if (xmlTypeMapMemberExpandable3.ChoiceMember != null)
							{
								string text5 = array4[xmlTypeMapMemberExpandable3.FlatArrayIndex];
								this.WriteLine(string.Concat(new string[]
								{
									text5,
									" = (",
									xmlTypeMapMemberExpandable3.ChoiceTypeData.CSharpFullName,
									") ShrinkArray (",
									text5,
									", ",
									array2[xmlTypeMapMemberExpandable3.FlatArrayIndex],
									", ",
									this.GetTypeOf(xmlTypeMapMemberExpandable3.ChoiceTypeData.Type.GetElementType()),
									", true);"
								}));
								this.WriteLine(string.Concat(new string[]
								{
									ob,
									".@",
									xmlTypeMapMemberExpandable3.ChoiceMember,
									" = ",
									text5,
									";"
								}));
							}
						}
					}
				}
				this.GenerateSetListMembersDefaults(xmlTypeMapping, map, ob, isValueList);
				this.GenerateEndHook();
			}
			if (!isValueList)
			{
				this.WriteLine(string.Empty);
				this.WriteLine("ReadEndElement();");
			}
		}

		private void GenerateReadAttributeMembers(XmlMapping xmlMap, ClassMap map, string ob, bool isValueList, ref bool first)
		{
			XmlTypeMapping xmlTypeMapping = xmlMap as XmlTypeMapping;
			Type type = (xmlTypeMapping == null) ? typeof(object[]) : xmlTypeMapping.TypeData.Type;
			if (this.GenerateReadHook(HookType.attributes, type))
			{
				return;
			}
			XmlTypeMapMember defaultAnyAttributeMember = map.DefaultAnyAttributeMember;
			if (defaultAnyAttributeMember != null)
			{
				this.WriteLine("int anyAttributeIndex = 0;");
				this.WriteLine(defaultAnyAttributeMember.TypeData.CSharpFullName + " anyAttributeArray = null;");
			}
			this.WriteLine("while (Reader.MoveToNextAttribute())");
			this.WriteLineInd("{");
			first = true;
			if (map.AttributeMembers != null)
			{
				foreach (object obj in map.AttributeMembers)
				{
					XmlTypeMapMemberAttribute xmlTypeMapMemberAttribute = (XmlTypeMapMemberAttribute)obj;
					this.WriteLineInd(string.Concat(new string[]
					{
						(!first) ? "else " : string.Empty,
						"if (Reader.LocalName == ",
						this.GetLiteral(xmlTypeMapMemberAttribute.AttributeName),
						" && Reader.NamespaceURI == ",
						this.GetLiteral(xmlTypeMapMemberAttribute.Namespace),
						") {"
					}));
					if (!this.GenerateReadMemberHook(type, xmlTypeMapMemberAttribute))
					{
						this.GenerateSetMemberValue(xmlTypeMapMemberAttribute, ob, this.GenerateGetValueFromXmlString("Reader.Value", xmlTypeMapMemberAttribute.TypeData, xmlTypeMapMemberAttribute.MappedType, false), isValueList);
						this.GenerateEndHook();
					}
					this.WriteLineUni("}");
					first = false;
				}
			}
			this.WriteLineInd(((!first) ? "else " : string.Empty) + "if (IsXmlnsAttribute (Reader.Name)) {");
			if (map.NamespaceDeclarations != null && !this.GenerateReadMemberHook(type, map.NamespaceDeclarations))
			{
				string text = ob + ".@" + map.NamespaceDeclarations.Name;
				this.WriteLine(string.Concat(new string[]
				{
					"if (",
					text,
					" == null) ",
					text,
					" = new XmlSerializerNamespaces ();"
				}));
				this.WriteLineInd("if (Reader.Prefix == \"xmlns\")");
				this.WriteLine(text + ".Add (Reader.LocalName, Reader.Value);");
				this.Unindent();
				this.WriteLineInd("else");
				this.WriteLine(text + ".Add (\"\", Reader.Value);");
				this.Unindent();
				this.GenerateEndHook();
			}
			this.WriteLineUni("}");
			this.WriteLineInd("else {");
			if (defaultAnyAttributeMember != null)
			{
				if (!this.GenerateReadArrayMemberHook(type, defaultAnyAttributeMember, "anyAttributeIndex"))
				{
					this.WriteLine("System.Xml.XmlAttribute attr = (System.Xml.XmlAttribute) Document.ReadNode(Reader);");
					if (typeof(XmlSchemaAnnotated).IsAssignableFrom(type))
					{
						this.WriteLine("ParseWsdlArrayType (attr);");
					}
					this.GenerateAddListValue(defaultAnyAttributeMember.TypeData, "anyAttributeArray", "anyAttributeIndex", this.GetCast(defaultAnyAttributeMember.TypeData.ListItemTypeData, "attr"), true);
					this.GenerateEndHook();
				}
				this.WriteLine("anyAttributeIndex++;");
			}
			else if (!this.GenerateReadHook(HookType.unknownAttribute, type))
			{
				this.WriteLine("UnknownNode (" + ob + ");");
				this.GenerateEndHook();
			}
			this.WriteLineUni("}");
			this.WriteLineUni("}");
			if (defaultAnyAttributeMember != null && !this.MemberHasReadReplaceHook(type, defaultAnyAttributeMember))
			{
				this.WriteLine(string.Empty);
				this.WriteLine(string.Concat(new string[]
				{
					"anyAttributeArray = (",
					defaultAnyAttributeMember.TypeData.CSharpFullName,
					") ShrinkArray (anyAttributeArray, anyAttributeIndex, ",
					this.GetTypeOf(defaultAnyAttributeMember.TypeData.Type.GetElementType()),
					", true);"
				}));
				this.GenerateSetMemberValue(defaultAnyAttributeMember, ob, "anyAttributeArray", isValueList);
			}
			this.WriteLine(string.Empty);
			this.WriteLine("Reader.MoveToElement ();");
			this.GenerateEndHook();
		}

		private void GenerateSetListMembersDefaults(XmlTypeMapping typeMap, ClassMap map, string ob, bool isValueList)
		{
			if (map.ListMembers != null)
			{
				ArrayList listMembers = map.ListMembers;
				for (int i = 0; i < listMembers.Count; i++)
				{
					XmlTypeMapMember xmlTypeMapMember = (XmlTypeMapMember)listMembers[i];
					if (!this.IsReadOnly(typeMap, xmlTypeMapMember, xmlTypeMapMember.TypeData, isValueList))
					{
						this.WriteLineInd("if (" + this.GenerateGetMemberValue(xmlTypeMapMember, ob, isValueList) + " == null) {");
						this.GenerateSetMemberValue(xmlTypeMapMember, ob, this.GenerateInitializeList(xmlTypeMapMember.TypeData), isValueList);
						this.WriteLineUni("}");
					}
				}
			}
		}

		private bool IsReadOnly(XmlTypeMapping map, XmlTypeMapMember member, TypeData memType, bool isValueList)
		{
			if (isValueList)
			{
				return !memType.HasPublicConstructor;
			}
			return member.IsReadOnly(map.TypeData.Type) || !memType.HasPublicConstructor;
		}

		private void GenerateSetMemberValue(XmlTypeMapMember member, string ob, string value, bool isValueList)
		{
			if (isValueList)
			{
				this.WriteLine(string.Concat(new object[]
				{
					ob,
					"[",
					member.GlobalIndex,
					"] = ",
					value,
					";"
				}));
			}
			else
			{
				this.WriteLine(string.Concat(new string[]
				{
					ob,
					".@",
					member.Name,
					" = ",
					value,
					";"
				}));
				if (member.IsOptionalValueType)
				{
					this.WriteLine(ob + "." + member.Name + "Specified = true;");
				}
			}
		}

		private void GenerateSetMemberValueFromAttr(XmlTypeMapMember member, string ob, string value, bool isValueList)
		{
			if (member.TypeData.Type.IsEnum)
			{
				value = this.GetCast(member.TypeData.Type, value);
			}
			this.GenerateSetMemberValue(member, ob, value, isValueList);
		}

		private string GenerateReadObjectElement(XmlTypeMapElementInfo elem)
		{
			switch (elem.TypeData.SchemaType)
			{
			case SchemaTypes.Primitive:
			case SchemaTypes.Enum:
				return this.GenerateReadPrimitiveValue(elem);
			case SchemaTypes.Array:
				return this.GenerateReadListElement(elem.MappedType, null, this.GetLiteral(elem.IsNullable), true);
			case SchemaTypes.Class:
				return this.GetReadObjectCall(elem.MappedType, this.GetLiteral(elem.IsNullable), "true");
			case SchemaTypes.XmlSerializable:
				return this.GetCast(elem.TypeData, string.Format("({0}) ReadSerializable (({0}) Activator.CreateInstance(typeof({0}), true))", elem.TypeData.CSharpFullName));
			case SchemaTypes.XmlNode:
				return this.GetReadXmlNode(elem.TypeData, true);
			default:
				throw new NotSupportedException("Invalid value type");
			}
		}

		private string GenerateReadPrimitiveValue(XmlTypeMapElementInfo elem)
		{
			if (elem.TypeData.Type == typeof(XmlQualifiedName))
			{
				if (elem.IsNullable)
				{
					return "ReadNullableQualifiedName ()";
				}
				return "ReadElementQualifiedName ()";
			}
			else
			{
				if (elem.IsNullable)
				{
					string strTempVar = this.GetStrTempVar();
					this.WriteLine("string " + strTempVar + " = ReadNullableString ();");
					return this.GenerateGetValueFromXmlString(strTempVar, elem.TypeData, elem.MappedType, true);
				}
				string strTempVar2 = this.GetStrTempVar();
				this.WriteLine("string " + strTempVar2 + " = Reader.ReadElementString ();");
				return this.GenerateGetValueFromXmlString(strTempVar2, elem.TypeData, elem.MappedType, false);
			}
		}

		private string GenerateGetValueFromXmlString(string value, TypeData typeData, XmlTypeMapping typeMap, bool isNullable)
		{
			if (typeData.SchemaType == SchemaTypes.Array)
			{
				return this.GenerateReadListString(typeMap, value);
			}
			if (typeData.SchemaType == SchemaTypes.Enum)
			{
				return this.GenerateGetEnumValue(typeMap, value, isNullable);
			}
			if (typeData.Type == typeof(XmlQualifiedName))
			{
				return "ToXmlQualifiedName (" + value + ")";
			}
			return XmlCustomFormatter.GenerateFromXmlString(typeData, value);
		}

		private string GenerateReadListElement(XmlTypeMapping typeMap, string list, string isNullable, bool canCreateInstance)
		{
			Type type = typeMap.TypeData.Type;
			ListMap listMap = (ListMap)typeMap.ObjectMap;
			bool flag = typeMap.TypeData.Type.IsArray;
			if (canCreateInstance && typeMap.TypeData.HasPublicConstructor)
			{
				if (list == null)
				{
					list = this.GetObTempVar();
					this.WriteLine(typeMap.TypeData.CSharpFullName + " " + list + " = null;");
					if (flag)
					{
						this.WriteLineInd("if (!ReadNull()) {");
					}
					this.WriteLine(list + " = " + this.GenerateCreateList(type) + ";");
				}
				else if (flag)
				{
					this.WriteLineInd("if (!ReadNull()) {");
				}
			}
			else
			{
				if (list == null)
				{
					this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral(typeMap.TypeData.CSharpFullName) + ");");
					return list;
				}
				this.WriteLineInd("if (((object)" + list + ") == null)");
				this.WriteLine("throw CreateReadOnlyCollectionException (" + this.GetLiteral(typeMap.TypeData.CSharpFullName) + ");");
				this.Unindent();
				flag = false;
			}
			this.WriteLineInd("if (Reader.IsEmptyElement) {");
			this.WriteLine("Reader.Skip();");
			if (type.IsArray)
			{
				this.WriteLine(string.Concat(new string[]
				{
					list,
					" = (",
					typeMap.TypeData.CSharpFullName,
					") ShrinkArray (",
					list,
					", 0, ",
					this.GetTypeOf(type.GetElementType()),
					", false);"
				}));
			}
			this.Unindent();
			this.WriteLineInd("} else {");
			string numTempVar = this.GetNumTempVar();
			this.WriteLine("int " + numTempVar + " = 0;");
			this.WriteLine("Reader.ReadStartElement();");
			this.WriteLine("Reader.MoveToContent();");
			this.WriteLine(string.Empty);
			this.WriteLine("while (Reader.NodeType != System.Xml.XmlNodeType.EndElement) ");
			this.WriteLineInd("{");
			this.WriteLine("if (Reader.NodeType == System.Xml.XmlNodeType.Element) ");
			this.WriteLineInd("{");
			bool flag2 = true;
			foreach (object obj in listMap.ItemInfo)
			{
				XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)obj;
				this.WriteLineInd(string.Concat(new string[]
				{
					(!flag2) ? "else " : string.Empty,
					"if (Reader.LocalName == ",
					this.GetLiteral(xmlTypeMapElementInfo.ElementName),
					" && Reader.NamespaceURI == ",
					this.GetLiteral(xmlTypeMapElementInfo.Namespace),
					") {"
				}));
				this.GenerateAddListValue(typeMap.TypeData, list, numTempVar, this.GenerateReadObjectElement(xmlTypeMapElementInfo), false);
				this.WriteLine(numTempVar + "++;");
				this.WriteLineUni("}");
				flag2 = false;
			}
			if (!flag2)
			{
				this.WriteLine("else UnknownNode (null);");
			}
			else
			{
				this.WriteLine("UnknownNode (null);");
			}
			this.WriteLineUni("}");
			this.WriteLine("else UnknownNode (null);");
			this.WriteLine(string.Empty);
			this.WriteLine("Reader.MoveToContent();");
			this.WriteLineUni("}");
			this.WriteLine("ReadEndElement();");
			if (type.IsArray)
			{
				this.WriteLine(string.Concat(new string[]
				{
					list,
					" = (",
					typeMap.TypeData.CSharpFullName,
					") ShrinkArray (",
					list,
					", ",
					numTempVar,
					", ",
					this.GetTypeOf(type.GetElementType()),
					", false);"
				}));
			}
			this.WriteLineUni("}");
			if (flag)
			{
				this.WriteLineUni("}");
			}
			return list;
		}

		private string GenerateReadListString(XmlTypeMapping typeMap, string values)
		{
			Type type = typeMap.TypeData.Type;
			ListMap listMap = (ListMap)typeMap.ObjectMap;
			string str = SerializationCodeGenerator.ToCSharpFullName(type.GetElementType());
			string obTempVar = this.GetObTempVar();
			this.WriteLine(str + "[] " + obTempVar + ";");
			string strTempVar = this.GetStrTempVar();
			this.WriteLine(string.Concat(new string[]
			{
				"string ",
				strTempVar,
				" = ",
				values,
				".Trim();"
			}));
			this.WriteLineInd("if (" + strTempVar + " != string.Empty) {");
			string obTempVar2 = this.GetObTempVar();
			this.WriteLine(string.Concat(new string[]
			{
				"string[] ",
				obTempVar2,
				" = ",
				strTempVar,
				".Split (' ');"
			}));
			this.WriteLine(obTempVar + " = new " + this.GetArrayDeclaration(type, obTempVar2 + ".Length") + ";");
			XmlTypeMapElementInfo xmlTypeMapElementInfo = (XmlTypeMapElementInfo)listMap.ItemInfo[0];
			string numTempVar = this.GetNumTempVar();
			this.WriteLineInd(string.Concat(new string[]
			{
				"for (int ",
				numTempVar,
				" = 0; ",
				numTempVar,
				" < ",
				obTempVar2,
				".Length; ",
				numTempVar,
				"++)"
			}));
			this.WriteLine(string.Concat(new string[]
			{
				obTempVar,
				"[",
				numTempVar,
				"] = ",
				this.GenerateGetValueFromXmlString(obTempVar2 + "[" + numTempVar + "]", xmlTypeMapElementInfo.TypeData, xmlTypeMapElementInfo.MappedType, xmlTypeMapElementInfo.IsNullable),
				";"
			}));
			this.Unindent();
			this.WriteLineUni("}");
			this.WriteLine("else");
			this.WriteLine(string.Concat(new string[]
			{
				"\t",
				obTempVar,
				" = new ",
				this.GetArrayDeclaration(type, "0"),
				";"
			}));
			return obTempVar;
		}

		private string GetArrayDeclaration(Type type, string length)
		{
			Type elementType = type.GetElementType();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('[').Append(length).Append(']');
			while (elementType.IsArray)
			{
				stringBuilder.Append("[]");
				elementType = elementType.GetElementType();
			}
			stringBuilder.Insert(0, SerializationCodeGenerator.ToCSharpFullName(elementType));
			return stringBuilder.ToString();
		}

		private void GenerateAddListValue(TypeData listType, string list, string index, string value, bool canCreateInstance)
		{
			Type type = listType.Type;
			if (type.IsArray)
			{
				this.WriteLine(string.Concat(new string[]
				{
					list,
					" = (",
					SerializationCodeGenerator.ToCSharpFullName(type),
					") EnsureArrayIndex (",
					list,
					", ",
					index,
					", ",
					this.GetTypeOf(type.GetElementType()),
					");"
				}));
				this.WriteLine(string.Concat(new string[]
				{
					list,
					"[",
					index,
					"] = ",
					value,
					";"
				}));
			}
			else
			{
				this.WriteLine("if (((object)" + list + ") == null)");
				if (canCreateInstance)
				{
					this.WriteLine("\t" + list + string.Format(" = ({0}) Activator.CreateInstance(typeof({0}), true);", listType.CSharpFullName));
				}
				else
				{
					this.WriteLine("\tthrow CreateReadOnlyCollectionException (" + this.GetLiteral(listType.CSharpFullName) + ");");
				}
				this.WriteLine(list + ".Add (" + value + ");");
			}
		}

		private string GenerateCreateList(Type listType)
		{
			if (listType.IsArray)
			{
				return string.Concat(new string[]
				{
					"(",
					SerializationCodeGenerator.ToCSharpFullName(listType),
					") EnsureArrayIndex (null, 0, ",
					this.GetTypeOf(listType.GetElementType()),
					")"
				});
			}
			return "new " + SerializationCodeGenerator.ToCSharpFullName(listType) + "()";
		}

		private string GenerateInitializeList(TypeData listType)
		{
			if (listType.Type.IsArray)
			{
				return "null";
			}
			return "new " + listType.CSharpFullName + "()";
		}

		private void GenerateFillerCallbacks()
		{
			foreach (object obj in this._listsToFill)
			{
				TypeData typeData = (TypeData)obj;
				string fillListName = this.GetFillListName(typeData);
				this.WriteLine("void " + fillListName + " (object list, object source)");
				this.WriteLineInd("{");
				this.WriteLine("if (((object)list) == null) throw CreateReadOnlyCollectionException (" + this.GetLiteral(typeData.CSharpFullName) + ");");
				this.WriteLine(string.Empty);
				this.WriteLine(typeData.CSharpFullName + " dest = (" + typeData.CSharpFullName + ") list;");
				this.WriteLine("foreach (object ob in (IEnumerable)source)");
				this.WriteLine("\tdest.Add (" + this.GetCast(typeData.ListItemTypeData, "ob") + ");");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
			}
		}

		private void GenerateReadXmlNodeElement(XmlTypeMapping typeMap, string isNullable)
		{
			this.WriteLine("return " + this.GetReadXmlNode(typeMap.TypeData, false) + ";");
		}

		private void GenerateReadPrimitiveElement(XmlTypeMapping typeMap, string isNullable)
		{
			this.WriteLine("XmlQualifiedName t = GetXsiType();");
			this.WriteLine(string.Concat(new string[]
			{
				"if (t == null) t = new XmlQualifiedName (",
				this.GetLiteral(typeMap.XmlType),
				", ",
				this.GetLiteral(typeMap.Namespace),
				");"
			}));
			this.WriteLine("return " + this.GetCast(typeMap.TypeData, "ReadTypedPrimitive (t)") + ";");
		}

		private void GenerateReadEnumElement(XmlTypeMapping typeMap, string isNullable)
		{
			this.WriteLine("Reader.ReadStartElement ();");
			this.WriteLine(typeMap.TypeData.CSharpFullName + " res = " + this.GenerateGetEnumValue(typeMap, "Reader.ReadString()", false) + ";");
			this.WriteLineInd("if (Reader.NodeType != XmlNodeType.None)");
			this.WriteLineUni("Reader.ReadEndElement ();");
			this.WriteLine("return res;");
		}

		private string GenerateGetEnumValue(XmlTypeMapping typeMap, string val, bool isNullable)
		{
			if (isNullable)
			{
				return string.Concat(new string[]
				{
					"(",
					val,
					") != null ? ",
					this.GetGetEnumValueName(typeMap),
					" (",
					val,
					") : (",
					typeMap.TypeData.CSharpFullName,
					"?) null"
				});
			}
			return this.GetGetEnumValueName(typeMap) + " (" + val + ")";
		}

		private void GenerateGetEnumValueMethod(XmlTypeMapping typeMap)
		{
			string text = this.GetGetEnumValueName(typeMap);
			EnumMap enumMap = (EnumMap)typeMap.ObjectMap;
			if (enumMap.IsFlags)
			{
				string text2 = text + "_Switch";
				this.WriteLine(typeMap.TypeData.CSharpFullName + " " + text + " (string xmlName)");
				this.WriteLineInd("{");
				this.WriteLine("xmlName = xmlName.Trim();");
				this.WriteLine("if (xmlName.Length == 0) return (" + typeMap.TypeData.CSharpFullName + ")0;");
				this.WriteLine(typeMap.TypeData.CSharpFullName + " sb = (" + typeMap.TypeData.CSharpFullName + ")0;");
				this.WriteLine("string[] enumNames = xmlName.Split (null);");
				this.WriteLine("foreach (string name in enumNames)");
				this.WriteLineInd("{");
				this.WriteLine("if (name == string.Empty) continue;");
				this.WriteLine("sb |= " + text2 + " (name); ");
				this.WriteLineUni("}");
				this.WriteLine("return sb;");
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
				text = text2;
			}
			this.WriteLine(typeMap.TypeData.CSharpFullName + " " + text + " (string xmlName)");
			this.WriteLineInd("{");
			this.GenerateGetSingleEnumValue(typeMap, "xmlName");
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
		}

		private void GenerateGetSingleEnumValue(XmlTypeMapping typeMap, string val)
		{
			EnumMap enumMap = (EnumMap)typeMap.ObjectMap;
			this.WriteLine("switch (" + val + ")");
			this.WriteLineInd("{");
			foreach (EnumMap.EnumMapMember enumMapMember in enumMap.Members)
			{
				this.WriteLine(string.Concat(new string[]
				{
					"case ",
					this.GetLiteral(enumMapMember.XmlName),
					": return ",
					typeMap.TypeData.CSharpFullName,
					".@",
					enumMapMember.EnumName,
					";"
				}));
			}
			this.WriteLineInd("default:");
			this.WriteLine(string.Concat(new string[]
			{
				"throw CreateUnknownConstantException (",
				val,
				", typeof(",
				typeMap.TypeData.CSharpFullName,
				"));"
			}));
			this.Unindent();
			this.WriteLineUni("}");
		}

		private void GenerateReadXmlSerializableElement(XmlTypeMapping typeMap, string isNullable)
		{
			this.WriteLine("Reader.MoveToContent ();");
			this.WriteLine("if (Reader.NodeType == XmlNodeType.Element)");
			this.WriteLineInd("{");
			this.WriteLine(string.Concat(new string[]
			{
				"if (Reader.LocalName == ",
				this.GetLiteral(typeMap.ElementName),
				" && Reader.NamespaceURI == ",
				this.GetLiteral(typeMap.Namespace),
				")"
			}));
			this.WriteLine(string.Format("\treturn ({0}) ReadSerializable (({0}) Activator.CreateInstance(typeof({0}), true));", typeMap.TypeData.CSharpFullName));
			this.WriteLine("else");
			this.WriteLine("\tthrow CreateUnknownNodeException ();");
			this.WriteLineUni("}");
			this.WriteLine("else UnknownNode (null);");
			this.WriteLine(string.Empty);
			this.WriteLine("return null;");
		}

		private void GenerateReadInitCallbacks()
		{
			this.WriteLine("protected override void InitCallbacks ()");
			this.WriteLineInd("{");
			if (this._format == SerializationFormat.Encoded)
			{
				foreach (object obj in this._mapsToGenerate)
				{
					XmlMapping xmlMapping = (XmlMapping)obj;
					XmlTypeMapping xmlTypeMapping = xmlMapping as XmlTypeMapping;
					if (xmlTypeMapping != null)
					{
						if (xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Class || xmlTypeMapping.TypeData.SchemaType == SchemaTypes.Enum)
						{
							this.WriteMetCall("AddReadCallback", new string[]
							{
								this.GetLiteral(xmlTypeMapping.XmlType),
								this.GetLiteral(xmlTypeMapping.Namespace),
								this.GetTypeOf(xmlTypeMapping.TypeData.Type),
								"new XmlSerializationReadCallback (" + this.GetReadObjectName(xmlTypeMapping) + ")"
							});
						}
					}
				}
			}
			this.WriteLineUni("}");
			this.WriteLine(string.Empty);
			this.WriteLine("protected override void InitIDs ()");
			this.WriteLine("{");
			this.WriteLine("}");
			this.WriteLine(string.Empty);
		}

		private void GenerateFixupCallbacks()
		{
			foreach (object obj in this._fixupCallbacks)
			{
				XmlMapping xmlMapping = (XmlMapping)obj;
				bool flag = xmlMapping is XmlMembersMapping;
				string text = flag ? "object[]" : ((XmlTypeMapping)xmlMapping).TypeData.CSharpFullName;
				this.WriteLine("void " + this.GetFixupCallbackName(xmlMapping) + " (object obfixup)");
				this.WriteLineInd("{");
				this.WriteLine("Fixup fixup = (Fixup)obfixup;");
				this.WriteLine(text + " source = (" + text + ") fixup.Source;");
				this.WriteLine("string[] ids = fixup.Ids;");
				this.WriteLine(string.Empty);
				ClassMap classMap = (ClassMap)xmlMapping.ObjectMap;
				ICollection elementMembers = classMap.ElementMembers;
				if (elementMembers != null)
				{
					foreach (object obj2 in elementMembers)
					{
						XmlTypeMapMember xmlTypeMapMember = (XmlTypeMapMember)obj2;
						this.WriteLineInd("if (ids[" + xmlTypeMapMember.Index + "] != null)");
						string text2 = "GetTarget(ids[" + xmlTypeMapMember.Index + "])";
						if (!flag)
						{
							text2 = this.GetCast(xmlTypeMapMember.TypeData, text2);
						}
						this.GenerateSetMemberValue(xmlTypeMapMember, "source", text2, flag);
						this.Unindent();
					}
				}
				this.WriteLineUni("}");
				this.WriteLine(string.Empty);
			}
		}

		private string GetReadXmlNode(TypeData type, bool wrapped)
		{
			if (type.Type == typeof(XmlDocument))
			{
				return this.GetCast(type, TypeTranslator.GetTypeData(typeof(XmlDocument)), "ReadXmlDocument (" + this.GetLiteral(wrapped) + ")");
			}
			return this.GetCast(type, TypeTranslator.GetTypeData(typeof(XmlNode)), "ReadXmlNode (" + this.GetLiteral(wrapped) + ")");
		}

		private void InitHooks()
		{
			this._hookContexts = new Stack();
			this._hookOpenHooks = new Stack();
			this._hookVariables = new Hashtable();
		}

		private void PushHookContext()
		{
			this._hookContexts.Push(this._hookVariables);
			this._hookVariables = (Hashtable)this._hookVariables.Clone();
		}

		private void PopHookContext()
		{
			this._hookVariables = (Hashtable)this._hookContexts.Pop();
		}

		private void SetHookVar(string var, string value)
		{
			this._hookVariables[var] = value;
		}

		private bool GenerateReadHook(HookType hookType, Type type)
		{
			return this.GenerateHook(hookType, XmlMappingAccess.Read, type, null);
		}

		private bool GenerateWriteHook(HookType hookType, Type type)
		{
			return this.GenerateHook(hookType, XmlMappingAccess.Write, type, null);
		}

		private bool GenerateWriteMemberHook(Type type, XmlTypeMapMember member)
		{
			this.SetHookVar("$MEMBER", member.Name);
			return this.GenerateHook(HookType.member, XmlMappingAccess.Write, type, member.Name);
		}

		private bool GenerateReadMemberHook(Type type, XmlTypeMapMember member)
		{
			this.SetHookVar("$MEMBER", member.Name);
			return this.GenerateHook(HookType.member, XmlMappingAccess.Read, type, member.Name);
		}

		private bool GenerateReadArrayMemberHook(Type type, XmlTypeMapMember member, string index)
		{
			this.SetHookVar("$INDEX", index);
			return this.GenerateReadMemberHook(type, member);
		}

		private bool MemberHasReadReplaceHook(Type type, XmlTypeMapMember member)
		{
			return this._config != null && this._config.GetHooks(HookType.member, XmlMappingAccess.Read, HookAction.Replace, type, member.Name).Count > 0;
		}

		private bool GenerateHook(HookType hookType, XmlMappingAccess dir, Type type, string member)
		{
			this.GenerateHooks(hookType, dir, type, null, HookAction.InsertBefore);
			if (this.GenerateHooks(hookType, dir, type, null, HookAction.Replace))
			{
				this.GenerateHooks(hookType, dir, type, null, HookAction.InsertAfter);
				return true;
			}
			SerializationCodeGenerator.HookInfo hookInfo = new SerializationCodeGenerator.HookInfo();
			hookInfo.HookType = hookType;
			hookInfo.Type = type;
			hookInfo.Member = member;
			hookInfo.Direction = dir;
			this._hookOpenHooks.Push(hookInfo);
			return false;
		}

		private void GenerateEndHook()
		{
			SerializationCodeGenerator.HookInfo hookInfo = (SerializationCodeGenerator.HookInfo)this._hookOpenHooks.Pop();
			this.GenerateHooks(hookInfo.HookType, hookInfo.Direction, hookInfo.Type, hookInfo.Member, HookAction.InsertAfter);
		}

		private bool GenerateHooks(HookType hookType, XmlMappingAccess dir, Type type, string member, HookAction action)
		{
			if (this._config == null)
			{
				return false;
			}
			ArrayList hooks = this._config.GetHooks(hookType, dir, action, type, null);
			if (hooks.Count == 0)
			{
				return false;
			}
			foreach (object obj in hooks)
			{
				Hook hook = (Hook)obj;
				string text = hook.GetCode(action);
				foreach (object obj2 in this._hookVariables)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
					text = text.Replace((string)dictionaryEntry.Key, (string)dictionaryEntry.Value);
				}
				this.WriteMultilineCode(text);
			}
			return true;
		}

		private string GetRootTypeName()
		{
			if (this._typeMap is XmlTypeMapping)
			{
				return ((XmlTypeMapping)this._typeMap).TypeData.CSharpFullName;
			}
			return "object[]";
		}

		private string GetNumTempVar()
		{
			return "n" + this._tempVarId++;
		}

		private string GetObTempVar()
		{
			return "o" + this._tempVarId++;
		}

		private string GetStrTempVar()
		{
			return "s" + this._tempVarId++;
		}

		private string GetBoolTempVar()
		{
			return "b" + this._tempVarId++;
		}

		private string GetUniqueName(string uniqueGroup, object ob, string name)
		{
			name = name.Replace("[]", "_array");
			Hashtable hashtable = (Hashtable)this._uniqueNames[uniqueGroup];
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				this._uniqueNames[uniqueGroup] = hashtable;
			}
			string text = (string)hashtable[ob];
			if (text != null)
			{
				return text;
			}
			foreach (object obj in hashtable.Values)
			{
				string a = (string)obj;
				if (a == name)
				{
					return this.GetUniqueName(uniqueGroup, ob, name + this._methodId++);
				}
			}
			hashtable[ob] = name;
			return name;
		}

		private void RegisterReferencingMap(XmlTypeMapping typeMap)
		{
			if (typeMap != null && !this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
		}

		private string GetWriteObjectName(XmlTypeMapping typeMap)
		{
			if (!this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
			return this.GetUniqueName("rw", typeMap, "WriteObject_" + typeMap.XmlType);
		}

		private string GetReadObjectName(XmlTypeMapping typeMap)
		{
			if (!this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
			return this.GetUniqueName("rr", typeMap, "ReadObject_" + typeMap.XmlType);
		}

		private string GetGetEnumValueName(XmlTypeMapping typeMap)
		{
			if (!this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
			return this.GetUniqueName("ge", typeMap, "GetEnumValue_" + typeMap.XmlType);
		}

		private string GetWriteObjectCallbackName(XmlTypeMapping typeMap)
		{
			if (!this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
			return this.GetUniqueName("wc", typeMap, "WriteCallback_" + typeMap.XmlType);
		}

		private string GetFixupCallbackName(XmlMapping typeMap)
		{
			if (!this._mapsToGenerate.Contains(typeMap))
			{
				this._mapsToGenerate.Add(typeMap);
			}
			if (typeMap is XmlTypeMapping)
			{
				return this.GetUniqueName("fc", typeMap, "FixupCallback_" + ((XmlTypeMapping)typeMap).XmlType);
			}
			return this.GetUniqueName("fc", typeMap, "FixupCallback__Message");
		}

		private string GetUniqueClassName(string s)
		{
			return this.classNames.AddUnique(s, null);
		}

		private string GetReadObjectCall(XmlTypeMapping typeMap, string isNullable, string checkType)
		{
			if (this._format == SerializationFormat.Literal)
			{
				return string.Concat(new string[]
				{
					this.GetReadObjectName(typeMap),
					" (",
					isNullable,
					", ",
					checkType,
					")"
				});
			}
			return this.GetCast(typeMap.TypeData, this.GetReadObjectName(typeMap) + " ()");
		}

		private string GetFillListName(TypeData td)
		{
			if (!this._listsToFill.Contains(td))
			{
				this._listsToFill.Add(td);
			}
			return this.GetUniqueName("fl", td, "Fill_" + CodeIdentifier.MakeValid(td.CSharpName));
		}

		private string GetCast(TypeData td, TypeData tdval, string val)
		{
			if (td.CSharpFullName == tdval.CSharpFullName)
			{
				return val;
			}
			return this.GetCast(td, val);
		}

		private string GetCast(TypeData td, string val)
		{
			return string.Concat(new string[]
			{
				"((",
				td.CSharpFullName,
				") ",
				val,
				")"
			});
		}

		private string GetCast(Type td, string val)
		{
			return string.Concat(new string[]
			{
				"((",
				SerializationCodeGenerator.ToCSharpFullName(td),
				") ",
				val,
				")"
			});
		}

		private string GetTypeOf(TypeData td)
		{
			return "typeof(" + td.CSharpFullName + ")";
		}

		private string GetTypeOf(Type td)
		{
			return "typeof(" + SerializationCodeGenerator.ToCSharpFullName(td) + ")";
		}

		private string GetLiteral(object ob)
		{
			if (ob == null)
			{
				return "null";
			}
			if (ob is string)
			{
				return "\"" + ob.ToString().Replace("\"", "\"\"") + "\"";
			}
			if (ob is DateTime)
			{
				return "new DateTime (" + ((DateTime)ob).Ticks + ")";
			}
			if (ob is DateTimeOffset)
			{
				return "new DateTimeOffset (" + ((DateTimeOffset)ob).Ticks + ")";
			}
			if (ob is TimeSpan)
			{
				return "new TimeSpan (" + ((TimeSpan)ob).Ticks + ")";
			}
			if (ob is bool)
			{
				return (!(bool)ob) ? "false" : "true";
			}
			if (ob is XmlQualifiedName)
			{
				XmlQualifiedName xmlQualifiedName = (XmlQualifiedName)ob;
				return string.Concat(new string[]
				{
					"new XmlQualifiedName (",
					this.GetLiteral(xmlQualifiedName.Name),
					",",
					this.GetLiteral(xmlQualifiedName.Namespace),
					")"
				});
			}
			if (ob is Enum)
			{
				string value = SerializationCodeGenerator.ToCSharpFullName(ob.GetType());
				StringBuilder stringBuilder = new StringBuilder();
				string text = Enum.Format(ob.GetType(), ob, "g");
				string[] array = text.Split(new char[]
				{
					','
				});
				foreach (string text2 in array)
				{
					string text3 = text2.Trim();
					if (text3.Length != 0)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(" | ");
						}
						stringBuilder.Append(value);
						stringBuilder.Append('.');
						stringBuilder.Append(text3);
					}
				}
				return stringBuilder.ToString();
			}
			return (!(ob is IFormattable)) ? ob.ToString() : ((IFormattable)ob).ToString(null, CultureInfo.InvariantCulture);
		}

		private void WriteLineInd(string code)
		{
			this.WriteLine(code);
			this._indent++;
		}

		private void WriteLineUni(string code)
		{
			if (this._indent > 0)
			{
				this._indent--;
			}
			this.WriteLine(code);
		}

		private void Write(string code)
		{
			if (code.Length > 0)
			{
				this._writer.Write(new string('\t', this._indent));
			}
			this._writer.Write(code);
		}

		private void WriteUni(string code)
		{
			if (this._indent > 0)
			{
				this._indent--;
			}
			this._writer.Write(code);
			this._writer.WriteLine(string.Empty);
		}

		private void WriteLine(string code)
		{
			if (code.Length > 0)
			{
				this._writer.Write(new string('\t', this._indent));
			}
			this._writer.WriteLine(code);
		}

		private void WriteMultilineCode(string code)
		{
			string str = new string('\t', this._indent);
			code = code.Replace("\r", string.Empty);
			code = code.Replace("\t", string.Empty);
			while (code.StartsWith("\n"))
			{
				code = code.Substring(1);
			}
			while (code.EndsWith("\n"))
			{
				code = code.Substring(0, code.Length - 1);
			}
			code = code.Replace("\n", "\n" + str);
			this.WriteLine(code);
		}

		private string Params(params string[] pars)
		{
			string text = string.Empty;
			foreach (string str in pars)
			{
				if (text != string.Empty)
				{
					text += ", ";
				}
				text += str;
			}
			return text;
		}

		private void WriteMetCall(string method, params string[] pars)
		{
			this.WriteLine(method + " (" + this.Params(pars) + ");");
		}

		private void Unindent()
		{
			this._indent--;
		}

		private class HookInfo
		{
			public HookType HookType;

			public Type Type;

			public string Member;

			public XmlMappingAccess Direction;
		}
	}
}
