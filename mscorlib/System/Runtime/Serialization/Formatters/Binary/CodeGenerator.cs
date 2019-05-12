using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace System.Runtime.Serialization.Formatters.Binary
{
	internal class CodeGenerator
	{
		private static object monitor = new object();

		private static ModuleBuilder _module;

		static CodeGenerator()
		{
			AppDomain domain = Thread.GetDomain();
			AssemblyBuilder assemblyBuilder = domain.DefineInternalDynamicAssembly(new AssemblyName
			{
				Name = "__MetadataTypes"
			}, AssemblyBuilderAccess.Run);
			CodeGenerator._module = assemblyBuilder.DefineDynamicModule("__MetadataTypesModule", false);
		}

		public static Type GenerateMetadataType(Type type, StreamingContext context)
		{
			object obj = CodeGenerator.monitor;
			Type result;
			lock (obj)
			{
				result = CodeGenerator.GenerateMetadataTypeInternal(type, context);
			}
			return result;
		}

		public static Type GenerateMetadataTypeInternal(Type type, StreamingContext context)
		{
			string text = type.Name + "__TypeMetadata";
			string str = string.Empty;
			int num = 0;
			while (CodeGenerator._module.GetType(text + str) != null)
			{
				int num2;
				num = (num2 = num + 1);
				str = num2.ToString();
			}
			text += str;
			MemberInfo[] serializableMembers = FormatterServices.GetSerializableMembers(type, context);
			TypeBuilder typeBuilder = CodeGenerator._module.DefineType(text, TypeAttributes.Public, typeof(ClrTypeMetadata));
			Type[] parameterTypes = Type.EmptyTypes;
			ConstructorBuilder constructorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, parameterTypes);
			ConstructorInfo constructor = typeof(ClrTypeMetadata).GetConstructor(new Type[]
			{
				typeof(Type)
			});
			ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldtoken, type);
			ilgenerator.EmitCall(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"), null);
			ilgenerator.Emit(OpCodes.Call, constructor);
			ilgenerator.Emit(OpCodes.Ret);
			parameterTypes = new Type[]
			{
				typeof(ObjectWriter),
				typeof(BinaryWriter)
			};
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("WriteAssemblies", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(void), parameterTypes);
			ilgenerator = methodBuilder.GetILGenerator();
			foreach (FieldInfo fieldInfo in serializableMembers)
			{
				Type type2 = fieldInfo.FieldType;
				while (type2.IsArray)
				{
					type2 = type2.GetElementType();
				}
				if (type2.Assembly != ObjectWriter.CorlibAssembly)
				{
					ilgenerator.Emit(OpCodes.Ldarg_1);
					ilgenerator.Emit(OpCodes.Ldarg_2);
					CodeGenerator.EmitLoadTypeAssembly(ilgenerator, type2, fieldInfo.Name);
					ilgenerator.EmitCall(OpCodes.Callvirt, typeof(ObjectWriter).GetMethod("WriteAssembly"), null);
					ilgenerator.Emit(OpCodes.Pop);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, typeof(TypeMetadata).GetMethod("WriteAssemblies"));
			parameterTypes = new Type[]
			{
				typeof(ObjectWriter),
				typeof(BinaryWriter),
				typeof(bool)
			};
			methodBuilder = typeBuilder.DefineMethod("WriteTypeData", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(void), parameterTypes);
			ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_2);
			ilgenerator.Emit(OpCodes.Ldc_I4, serializableMembers.Length);
			CodeGenerator.EmitWrite(ilgenerator, typeof(int));
			foreach (FieldInfo fieldInfo2 in serializableMembers)
			{
				ilgenerator.Emit(OpCodes.Ldarg_2);
				ilgenerator.Emit(OpCodes.Ldstr, fieldInfo2.Name);
				CodeGenerator.EmitWrite(ilgenerator, typeof(string));
			}
			Label label = ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Ldarg_3);
			ilgenerator.Emit(OpCodes.Brfalse, label);
			foreach (FieldInfo fieldInfo3 in serializableMembers)
			{
				ilgenerator.Emit(OpCodes.Ldarg_2);
				ilgenerator.Emit(OpCodes.Ldc_I4_S, (byte)ObjectWriter.GetTypeTag(fieldInfo3.FieldType));
				CodeGenerator.EmitWrite(ilgenerator, typeof(byte));
			}
			foreach (FieldInfo fieldInfo4 in serializableMembers)
			{
				CodeGenerator.EmitWriteTypeSpec(ilgenerator, fieldInfo4.FieldType, fieldInfo4.Name);
			}
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, typeof(TypeMetadata).GetMethod("WriteTypeData"));
			parameterTypes = new Type[]
			{
				typeof(ObjectWriter),
				typeof(BinaryWriter),
				typeof(object)
			};
			methodBuilder = typeBuilder.DefineMethod("WriteObjectData", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual, typeof(void), parameterTypes);
			ilgenerator = methodBuilder.GetILGenerator();
			LocalBuilder local = ilgenerator.DeclareLocal(type);
			OpCode opcode = OpCodes.Ldloc;
			ilgenerator.Emit(OpCodes.Ldarg_3);
			if (type.IsValueType)
			{
				ilgenerator.Emit(OpCodes.Unbox, type);
				CodeGenerator.LoadFromPtr(ilgenerator, type);
				opcode = OpCodes.Ldloca_S;
			}
			else
			{
				ilgenerator.Emit(OpCodes.Castclass, type);
			}
			ilgenerator.Emit(OpCodes.Stloc, local);
			foreach (FieldInfo fieldInfo5 in serializableMembers)
			{
				Type fieldType = fieldInfo5.FieldType;
				if (BinaryCommon.IsPrimitive(fieldType))
				{
					ilgenerator.Emit(OpCodes.Ldarg_2);
					ilgenerator.Emit(opcode, local);
					if (fieldType == typeof(DateTime) || fieldType == typeof(TimeSpan) || fieldType == typeof(decimal))
					{
						ilgenerator.Emit(OpCodes.Ldflda, fieldInfo5);
					}
					else
					{
						ilgenerator.Emit(OpCodes.Ldfld, fieldInfo5);
					}
					CodeGenerator.EmitWritePrimitiveValue(ilgenerator, fieldType);
				}
				else
				{
					ilgenerator.Emit(OpCodes.Ldarg_1);
					ilgenerator.Emit(OpCodes.Ldarg_2);
					ilgenerator.Emit(OpCodes.Ldtoken, fieldType);
					ilgenerator.EmitCall(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"), null);
					ilgenerator.Emit(opcode, local);
					ilgenerator.Emit(OpCodes.Ldfld, fieldInfo5);
					if (fieldType.IsValueType)
					{
						ilgenerator.Emit(OpCodes.Box, fieldType);
					}
					ilgenerator.EmitCall(OpCodes.Call, typeof(ObjectWriter).GetMethod("WriteValue"), null);
				}
			}
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, typeof(TypeMetadata).GetMethod("WriteObjectData"));
			return typeBuilder.CreateType();
		}

		public static void LoadFromPtr(ILGenerator ig, Type t)
		{
			if (t == typeof(int))
			{
				ig.Emit(OpCodes.Ldind_I4);
			}
			else if (t == typeof(uint))
			{
				ig.Emit(OpCodes.Ldind_U4);
			}
			else if (t == typeof(short))
			{
				ig.Emit(OpCodes.Ldind_I2);
			}
			else if (t == typeof(ushort))
			{
				ig.Emit(OpCodes.Ldind_U2);
			}
			else if (t == typeof(char))
			{
				ig.Emit(OpCodes.Ldind_U2);
			}
			else if (t == typeof(byte))
			{
				ig.Emit(OpCodes.Ldind_U1);
			}
			else if (t == typeof(sbyte))
			{
				ig.Emit(OpCodes.Ldind_I1);
			}
			else if (t == typeof(ulong))
			{
				ig.Emit(OpCodes.Ldind_I8);
			}
			else if (t == typeof(long))
			{
				ig.Emit(OpCodes.Ldind_I8);
			}
			else if (t == typeof(float))
			{
				ig.Emit(OpCodes.Ldind_R4);
			}
			else if (t == typeof(double))
			{
				ig.Emit(OpCodes.Ldind_R8);
			}
			else if (t == typeof(bool))
			{
				ig.Emit(OpCodes.Ldind_I1);
			}
			else if (t == typeof(IntPtr))
			{
				ig.Emit(OpCodes.Ldind_I);
			}
			else if (t.IsEnum)
			{
				if (t == typeof(Enum))
				{
					ig.Emit(OpCodes.Ldind_Ref);
				}
				else
				{
					CodeGenerator.LoadFromPtr(ig, CodeGenerator.EnumToUnderlying(t));
				}
			}
			else if (t.IsValueType)
			{
				ig.Emit(OpCodes.Ldobj, t);
			}
			else
			{
				ig.Emit(OpCodes.Ldind_Ref);
			}
		}

		private static void EmitWriteTypeSpec(ILGenerator gen, Type type, string member)
		{
			switch (ObjectWriter.GetTypeTag(type))
			{
			case TypeTag.PrimitiveType:
				gen.Emit(OpCodes.Ldarg_2);
				gen.Emit(OpCodes.Ldc_I4_S, BinaryCommon.GetTypeCode(type));
				CodeGenerator.EmitWrite(gen, typeof(byte));
				break;
			case TypeTag.RuntimeType:
				gen.Emit(OpCodes.Ldarg_2);
				gen.Emit(OpCodes.Ldstr, type.FullName);
				CodeGenerator.EmitWrite(gen, typeof(string));
				break;
			case TypeTag.GenericType:
				gen.Emit(OpCodes.Ldarg_2);
				gen.Emit(OpCodes.Ldstr, type.FullName);
				CodeGenerator.EmitWrite(gen, typeof(string));
				gen.Emit(OpCodes.Ldarg_2);
				gen.Emit(OpCodes.Ldarg_1);
				CodeGenerator.EmitLoadTypeAssembly(gen, type, member);
				gen.EmitCall(OpCodes.Callvirt, typeof(ObjectWriter).GetMethod("GetAssemblyId"), null);
				gen.Emit(OpCodes.Conv_I4);
				CodeGenerator.EmitWrite(gen, typeof(int));
				break;
			case TypeTag.ArrayOfPrimitiveType:
				gen.Emit(OpCodes.Ldarg_2);
				gen.Emit(OpCodes.Ldc_I4_S, BinaryCommon.GetTypeCode(type.GetElementType()));
				CodeGenerator.EmitWrite(gen, typeof(byte));
				break;
			}
		}

		private static void EmitLoadTypeAssembly(ILGenerator gen, Type type, string member)
		{
			gen.Emit(OpCodes.Ldtoken, type);
			gen.EmitCall(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle"), null);
			gen.EmitCall(OpCodes.Callvirt, typeof(Type).GetProperty("Assembly").GetGetMethod(), null);
		}

		private static void EmitWrite(ILGenerator gen, Type type)
		{
			gen.EmitCall(OpCodes.Callvirt, typeof(BinaryWriter).GetMethod("Write", new Type[]
			{
				type
			}), null);
		}

		public static void EmitWritePrimitiveValue(ILGenerator gen, Type type)
		{
			switch (Type.GetTypeCode(type))
			{
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.String:
				CodeGenerator.EmitWrite(gen, type);
				return;
			case TypeCode.Decimal:
				gen.EmitCall(OpCodes.Call, typeof(CultureInfo).GetProperty("InvariantCulture").GetGetMethod(), null);
				gen.EmitCall(OpCodes.Call, typeof(decimal).GetMethod("ToString", new Type[]
				{
					typeof(IFormatProvider)
				}), null);
				CodeGenerator.EmitWrite(gen, typeof(string));
				return;
			case TypeCode.DateTime:
				gen.EmitCall(OpCodes.Call, typeof(DateTime).GetMethod("ToBinary", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), null);
				CodeGenerator.EmitWrite(gen, typeof(long));
				return;
			}
			if (type != typeof(TimeSpan))
			{
				throw new NotSupportedException("Unsupported primitive type: " + type.FullName);
			}
			gen.EmitCall(OpCodes.Call, typeof(TimeSpan).GetProperty("Ticks").GetGetMethod(), null);
			CodeGenerator.EmitWrite(gen, typeof(long));
		}

		public static Type EnumToUnderlying(Type t)
		{
			TypeCode typeCode = Type.GetTypeCode(t);
			switch (typeCode)
			{
			case TypeCode.Boolean:
				return typeof(bool);
			case TypeCode.Char:
				return typeof(char);
			case TypeCode.SByte:
				return typeof(sbyte);
			case TypeCode.Byte:
				return typeof(byte);
			case TypeCode.Int16:
				return typeof(short);
			case TypeCode.UInt16:
				return typeof(ushort);
			case TypeCode.Int32:
				return typeof(int);
			case TypeCode.UInt32:
				return typeof(uint);
			case TypeCode.Int64:
				return typeof(long);
			case TypeCode.UInt64:
				return typeof(ulong);
			default:
				throw new Exception(string.Concat(new object[]
				{
					"Unhandled typecode in enum ",
					typeCode,
					" from ",
					t.AssemblyQualifiedName
				}));
			}
		}
	}
}
