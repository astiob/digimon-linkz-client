using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Selects a member from a list of candidates, and performs type conversion from actual argument type to formal argument type. </summary>
	[ClassInterface(ClassInterfaceType.AutoDual)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Binder
	{
		private static Binder default_binder = new Binder.Default();

		/// <summary>Selects a field from the given set of fields, based on the specified criteria.</summary>
		/// <returns>The matching field. </returns>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values. </param>
		/// <param name="match">The set of fields that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of fields that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
		/// <param name="value">The field value used to locate a matching field. </param>
		/// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types, in binder implementations that coerce types. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.Note   For example, if a binder implementation allows coercion of string values to numeric types, this parameter is necessary to convert a String that represents 1000 to a Double value, because 1000 is represented differently by different cultures. The default binder does not do such string coercions.</param>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> contains multiple fields that are equally good matches for <paramref name="value" />. For example, <paramref name="value" /> contains a MyClass object that implements the IMyClass interface, and <paramref name="match" /> contains a field of type MyClass and a field of type IMyClass. </exception>
		/// <exception cref="T:System.MissingFieldException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> contains no fields that can accept <paramref name="value" />.</exception>
		/// <exception cref="T:System.NullReferenceException">For the default binder, <paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="match" /> is null or an empty array.-or-<paramref name="bindingAttr" /> includes <see cref="F:System.Reflection.BindingFlags.SetField" />, and <paramref name="value" /> is null.</exception>
		public abstract FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture);

		/// <summary>Selects a method to invoke from the given set of methods, based on the supplied arguments.</summary>
		/// <returns>The matching method.</returns>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values. </param>
		/// <param name="match">The set of methods that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
		/// <param name="args">The arguments that are passed in. The binder can change the order of the arguments in this array; for example, the default binder changes the order of arguments if the <paramref name="names" /> parameter is used to specify an order other than positional order. If a binder implementation coerces argument types, the types and values of the arguments can be changed as well. </param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified. The default binder implementation does not use this parameter.</param>
		/// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types, in binder implementations that coerce types. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used. Note   For example, if a binder implementation allows coercion of string values to numeric types, this parameter is necessary to convert a String that represents 1000 to a Double value, because 1000 is represented differently by different cultures. The default binder does not do such string coercions.</param>
		/// <param name="names">The parameter names, if parameter names are to be considered when matching, or null if arguments are to be treated as purely positional. For example, parameter names must be used if arguments are not supplied in positional order. </param>
		/// <param name="state">After the method returns, <paramref name="state" /> contains a binder-provided object that keeps track of argument reordering. The binder creates this object, and the binder is the sole consumer of this object. If <paramref name="state" /> is not null when BindToMethod returns, you must pass <paramref name="state" /> to the <see cref="M:System.Reflection.Binder.ReorderArgumentArray(System.Object[]@,System.Object)" /> method if you want to restore <paramref name="args" /> to its original order, for example, so that you can retrieve the values of ref parameters (ByRef parameters in Visual Basic). </param>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple methods that are equally good matches for <paramref name="args" />. For example, <paramref name="args" /> contains a MyClass object that implements the IMyClass interface, and <paramref name="match" /> contains a method that takes MyClass and a method that takes IMyClass. </exception>
		/// <exception cref="T:System.MissingMethodException">For the default binder, <paramref name="match" /> contains no methods that can accept the arguments supplied in <paramref name="args" />.</exception>
		/// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is null or an empty array.</exception>
		public abstract MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state);

		/// <summary>Changes the type of the given Object to the given Type.</summary>
		/// <returns>An object that contains the given value as the new type. </returns>
		/// <param name="value">The object to change into a new Type. </param>
		/// <param name="type">The new Type that <paramref name="value" /> will become. </param>
		/// <param name="culture">An instance of <see cref="T:System.Globalization.CultureInfo" /> that is used to control the coercion of data types. If <paramref name="culture" /> is null, the <see cref="T:System.Globalization.CultureInfo" /> for the current thread is used.Note   For example, this parameter is necessary to convert a String that represents 1000 to a Double value, because 1000 is represented differently by different cultures. </param>
		public abstract object ChangeType(object value, Type type, CultureInfo culture);

		/// <summary>Upon returning from <see cref="M:System.Reflection.Binder.BindToMethod(System.Reflection.BindingFlags,System.Reflection.MethodBase[],System.Object[]@,System.Reflection.ParameterModifier[],System.Globalization.CultureInfo,System.String[],System.Object@)" />, restores the <paramref name="args" /> argument to what it was when it came from BindToMethod.</summary>
		/// <param name="args">The actual arguments that are passed in. Both the types and values of the arguments can be changed. </param>
		/// <param name="state">A binder-provided object that keeps track of argument reordering. </param>
		public abstract void ReorderArgumentArray(ref object[] args, object state);

		/// <summary>Selects a method from the given set of methods, based on the argument type.</summary>
		/// <returns>The matching method, if found; otherwise, null.</returns>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values. </param>
		/// <param name="match">The set of methods that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of methods that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
		/// <param name="types">The parameter types used to locate a matching method. </param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified. </param>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple methods that are equally good matches for the parameter types described by <paramref name="types" />. For example, the array in <paramref name="types" /> contains a <see cref="T:System.Type" /> object for MyClass and the array in <paramref name="match" /> contains a method that takes a base class of MyClass and a method that takes an interface that MyClass implements. </exception>
		/// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is null or an empty array.-or-An element of <paramref name="types" /> derives from <see cref="T:System.Type" />, but is not of type RuntimeType.</exception>
		public abstract MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers);

		/// <summary>Selects a property from the given set of properties, based on the specified criteria.</summary>
		/// <returns>The matching property.</returns>
		/// <param name="bindingAttr">A bitwise combination of <see cref="T:System.Reflection.BindingFlags" /> values. </param>
		/// <param name="match">The set of properties that are candidates for matching. For example, when a <see cref="T:System.Reflection.Binder" /> object is used by <see cref="Overload:System.Type.InvokeMember" />, this parameter specifies the set of properties that reflection has determined to be possible matches, typically because they have the correct member name. The default implementation provided by <see cref="P:System.Type.DefaultBinder" /> changes the order of this array.</param>
		/// <param name="returnType">The return value the matching property must have. </param>
		/// <param name="indexes">The index types of the property being searched for. Used for index properties such as the indexer for a class. </param>
		/// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified. </param>
		/// <exception cref="T:System.Reflection.AmbiguousMatchException">For the default binder, <paramref name="match" /> contains multiple properties that are equally good matches for <paramref name="returnType" /> and <paramref name="indexes" />. </exception>
		/// <exception cref="T:System.ArgumentException">For the default binder, <paramref name="match" /> is null or an empty array. </exception>
		public abstract PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers);

		internal static Binder DefaultBinder
		{
			get
			{
				return Binder.default_binder;
			}
		}

		internal static bool ConvertArgs(Binder binder, object[] args, ParameterInfo[] pinfo, CultureInfo culture)
		{
			if (args == null)
			{
				if (pinfo.Length == 0)
				{
					return true;
				}
				throw new TargetParameterCountException();
			}
			else
			{
				if (pinfo.Length != args.Length)
				{
					throw new TargetParameterCountException();
				}
				for (int i = 0; i < args.Length; i++)
				{
					object obj = binder.ChangeType(args[i], pinfo[i].ParameterType, culture);
					if (obj == null && args[i] != null)
					{
						return false;
					}
					args[i] = obj;
				}
				return true;
			}
		}

		internal static int GetDerivedLevel(Type type)
		{
			Type type2 = type;
			int num = 1;
			while (type2.BaseType != null)
			{
				num++;
				type2 = type2.BaseType;
			}
			return num;
		}

		internal static MethodBase FindMostDerivedMatch(MethodBase[] match)
		{
			int num = 0;
			int num2 = -1;
			int num3 = match.Length;
			for (int i = 0; i < num3; i++)
			{
				MethodBase methodBase = match[i];
				int derivedLevel = Binder.GetDerivedLevel(methodBase.DeclaringType);
				if (derivedLevel == num)
				{
					throw new AmbiguousMatchException();
				}
				if (num2 >= 0)
				{
					ParameterInfo[] parameters = methodBase.GetParameters();
					ParameterInfo[] parameters2 = match[num2].GetParameters();
					bool flag = true;
					if (parameters.Length != parameters2.Length)
					{
						flag = false;
					}
					else
					{
						for (int j = 0; j < parameters.Length; j++)
						{
							if (parameters[j].ParameterType != parameters2[j].ParameterType)
							{
								flag = false;
								break;
							}
						}
					}
					if (!flag)
					{
						throw new AmbiguousMatchException();
					}
				}
				if (derivedLevel > num)
				{
					num = derivedLevel;
					num2 = i;
				}
			}
			return match[num2];
		}

		internal sealed class Default : Binder
		{
			public override FieldInfo BindToField(BindingFlags bindingAttr, FieldInfo[] match, object value, CultureInfo culture)
			{
				if (match == null)
				{
					throw new ArgumentNullException("match");
				}
				foreach (FieldInfo fieldInfo in match)
				{
					if (Binder.Default.check_type(value.GetType(), fieldInfo.FieldType))
					{
						return fieldInfo;
					}
				}
				return null;
			}

			public override MethodBase BindToMethod(BindingFlags bindingAttr, MethodBase[] match, ref object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] names, out object state)
			{
				Type[] array;
				if (args == null)
				{
					array = Type.EmptyTypes;
				}
				else
				{
					array = new Type[args.Length];
					for (int i = 0; i < args.Length; i++)
					{
						if (args[i] != null)
						{
							array[i] = args[i].GetType();
						}
					}
				}
				MethodBase methodBase = this.SelectMethod(bindingAttr, match, array, modifiers, true);
				state = null;
				if (names != null)
				{
					this.ReorderParameters(names, ref args, methodBase);
				}
				return methodBase;
			}

			private void ReorderParameters(string[] names, ref object[] args, MethodBase selected)
			{
				object[] array = new object[args.Length];
				Array.Copy(args, array, args.Length);
				ParameterInfo[] parameters = selected.GetParameters();
				for (int i = 0; i < names.Length; i++)
				{
					for (int j = 0; j < parameters.Length; j++)
					{
						if (names[i] == parameters[j].Name)
						{
							array[j] = args[i];
							break;
						}
					}
				}
				Array.Copy(array, args, args.Length);
			}

			private static bool IsArrayAssignable(Type object_type, Type target_type)
			{
				if (object_type.IsArray && target_type.IsArray)
				{
					return Binder.Default.IsArrayAssignable(object_type.GetElementType(), target_type.GetElementType());
				}
				return target_type.IsAssignableFrom(object_type);
			}

			public override object ChangeType(object value, Type type, CultureInfo culture)
			{
				if (value == null)
				{
					return null;
				}
				Type type2 = value.GetType();
				if (type.IsByRef)
				{
					type = type.GetElementType();
				}
				if (type2 == type || type.IsInstanceOfType(value))
				{
					return value;
				}
				if (type2.IsArray && type.IsArray && Binder.Default.IsArrayAssignable(type2.GetElementType(), type.GetElementType()))
				{
					return value;
				}
				if (!Binder.Default.check_type(type2, type))
				{
					return null;
				}
				if (type.IsEnum)
				{
					return Enum.ToObject(type, value);
				}
				if (type2 == typeof(char))
				{
					if (type == typeof(double))
					{
						return (double)((char)value);
					}
					if (type == typeof(float))
					{
						return (float)((char)value);
					}
				}
				if (type2 == typeof(IntPtr) && type.IsPointer)
				{
					return value;
				}
				return Convert.ChangeType(value, type);
			}

			[MonoTODO("This method does not do anything in Mono")]
			public override void ReorderArgumentArray(ref object[] args, object state)
			{
			}

			private static bool check_type(Type from, Type to)
			{
				if (from == to)
				{
					return true;
				}
				if (from == null)
				{
					return true;
				}
				if (to.IsByRef != from.IsByRef)
				{
					return false;
				}
				if (to.IsInterface)
				{
					return to.IsAssignableFrom(from);
				}
				if (to.IsEnum)
				{
					to = Enum.GetUnderlyingType(to);
					if (from == to)
					{
						return true;
					}
				}
				if (to.IsGenericType && to.GetGenericTypeDefinition() == typeof(Nullable<>) && to.GetGenericArguments()[0] == from)
				{
					return true;
				}
				TypeCode typeCode = Type.GetTypeCode(from);
				TypeCode typeCode2 = Type.GetTypeCode(to);
				switch (typeCode)
				{
				case TypeCode.Char:
					switch (typeCode2)
					{
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					default:
						return to == typeof(object);
					}
					break;
				case TypeCode.SByte:
					switch (typeCode2)
					{
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					}
					return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
				case TypeCode.Byte:
					switch (typeCode2)
					{
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					}
					return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
				case TypeCode.Int16:
					switch (typeCode2)
					{
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					}
					return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
				case TypeCode.UInt16:
					switch (typeCode2)
					{
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					default:
						return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
					}
					break;
				case TypeCode.Int32:
					switch (typeCode2)
					{
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					}
					return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
				case TypeCode.UInt32:
					switch (typeCode2)
					{
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return true;
					default:
						return to == typeof(object) || (from.IsEnum && to == typeof(Enum));
					}
					break;
				case TypeCode.Int64:
				case TypeCode.UInt64:
				{
					TypeCode typeCode3 = typeCode2;
					return typeCode3 == TypeCode.Single || typeCode3 == TypeCode.Double || to == typeof(object) || (from.IsEnum && to == typeof(Enum));
				}
				case TypeCode.Single:
					return typeCode2 == TypeCode.Double || to == typeof(object);
				default:
					return (to == typeof(object) && from.IsValueType) || (to.IsPointer && from == typeof(IntPtr)) || to.IsAssignableFrom(from);
				}
			}

			private static bool check_arguments(Type[] types, ParameterInfo[] args, bool allowByRefMatch)
			{
				for (int i = 0; i < types.Length; i++)
				{
					bool flag = Binder.Default.check_type(types[i], args[i].ParameterType);
					if (!flag && allowByRefMatch)
					{
						Type parameterType = args[i].ParameterType;
						if (parameterType.IsByRef)
						{
							flag = Binder.Default.check_type(types[i], parameterType.GetElementType());
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			}

			public override MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers)
			{
				return this.SelectMethod(bindingAttr, match, types, modifiers, false);
			}

			private MethodBase SelectMethod(BindingFlags bindingAttr, MethodBase[] match, Type[] types, ParameterModifier[] modifiers, bool allowByRefMatch)
			{
				if (match == null)
				{
					throw new ArgumentNullException("match");
				}
				foreach (MethodBase methodBase in match)
				{
					ParameterInfo[] parameters = methodBase.GetParameters();
					if (parameters.Length == types.Length)
					{
						int j;
						for (j = 0; j < types.Length; j++)
						{
							if (types[j] != parameters[j].ParameterType)
							{
								break;
							}
						}
						if (j == types.Length)
						{
							return methodBase;
						}
					}
				}
				foreach (MethodBase methodBase in match)
				{
					ParameterInfo[] parameters2 = methodBase.GetParameters();
					if (parameters2.Length <= types.Length)
					{
						if (parameters2.Length != 0)
						{
							if (Attribute.IsDefined(parameters2[parameters2.Length - 1], typeof(ParamArrayAttribute)))
							{
								Type elementType = parameters2[parameters2.Length - 1].ParameterType.GetElementType();
								int j;
								for (j = 0; j < types.Length; j++)
								{
									if (j < parameters2.Length - 1 && types[j] != parameters2[j].ParameterType)
									{
										break;
									}
									if (j >= parameters2.Length - 1 && types[j] != elementType)
									{
										break;
									}
								}
								if (j == types.Length)
								{
									return methodBase;
								}
							}
						}
					}
				}
				if ((bindingAttr & BindingFlags.ExactBinding) != BindingFlags.Default)
				{
					return null;
				}
				MethodBase methodBase2 = null;
				foreach (MethodBase methodBase in match)
				{
					ParameterInfo[] parameters3 = methodBase.GetParameters();
					if (parameters3.Length == types.Length)
					{
						if (Binder.Default.check_arguments(types, parameters3, allowByRefMatch))
						{
							if (methodBase2 != null)
							{
								methodBase2 = this.GetBetterMethod(methodBase2, methodBase, types);
							}
							else
							{
								methodBase2 = methodBase;
							}
						}
					}
				}
				return methodBase2;
			}

			private MethodBase GetBetterMethod(MethodBase m1, MethodBase m2, Type[] types)
			{
				if (m1.IsGenericMethodDefinition && !m2.IsGenericMethodDefinition)
				{
					return m2;
				}
				if (m2.IsGenericMethodDefinition && !m1.IsGenericMethodDefinition)
				{
					return m1;
				}
				ParameterInfo[] parameters = m1.GetParameters();
				ParameterInfo[] parameters2 = m2.GetParameters();
				int num = 0;
				for (int i = 0; i < parameters.Length; i++)
				{
					int num2 = this.CompareCloserType(parameters[i].ParameterType, parameters2[i].ParameterType);
					if (num2 != 0 && num != 0 && num != num2)
					{
						throw new AmbiguousMatchException();
					}
					if (num2 != 0)
					{
						num = num2;
					}
				}
				if (num != 0)
				{
					return (num <= 0) ? m1 : m2;
				}
				Type declaringType = m1.DeclaringType;
				Type declaringType2 = m2.DeclaringType;
				if (declaringType != declaringType2)
				{
					if (declaringType.IsSubclassOf(declaringType2))
					{
						return m1;
					}
					if (declaringType2.IsSubclassOf(declaringType))
					{
						return m2;
					}
				}
				bool flag = (m1.CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0;
				bool flag2 = (m2.CallingConvention & CallingConventions.VarArgs) != (CallingConventions)0;
				if (flag && !flag2)
				{
					return m2;
				}
				if (flag2 && !flag)
				{
					return m1;
				}
				throw new AmbiguousMatchException();
			}

			private int CompareCloserType(Type t1, Type t2)
			{
				if (t1 == t2)
				{
					return 0;
				}
				if (t1.IsGenericParameter && !t2.IsGenericParameter)
				{
					return 1;
				}
				if (!t1.IsGenericParameter && t2.IsGenericParameter)
				{
					return -1;
				}
				if (t1.HasElementType && t2.HasElementType)
				{
					return this.CompareCloserType(t1.GetElementType(), t2.GetElementType());
				}
				if (t1.IsSubclassOf(t2))
				{
					return -1;
				}
				if (t2.IsSubclassOf(t1))
				{
					return 1;
				}
				if (t1.IsInterface && Array.IndexOf<Type>(t2.GetInterfaces(), t1) >= 0)
				{
					return 1;
				}
				if (t2.IsInterface && Array.IndexOf<Type>(t1.GetInterfaces(), t2) >= 0)
				{
					return -1;
				}
				return 0;
			}

			public override PropertyInfo SelectProperty(BindingFlags bindingAttr, PropertyInfo[] match, Type returnType, Type[] indexes, ParameterModifier[] modifiers)
			{
				if (match == null || match.Length == 0)
				{
					throw new ArgumentException("No properties provided", "match");
				}
				bool flag = returnType != null;
				int num = (indexes == null) ? -1 : indexes.Length;
				PropertyInfo propertyInfo = null;
				int num2 = 2147483646;
				int num3 = int.MaxValue;
				int num4 = 0;
				for (int i = match.Length - 1; i >= 0; i--)
				{
					PropertyInfo propertyInfo2 = match[i];
					ParameterInfo[] indexParameters = propertyInfo2.GetIndexParameters();
					if (num < 0 || num == indexParameters.Length)
					{
						if (!flag || propertyInfo2.PropertyType == returnType)
						{
							int num5 = 2147483646;
							if (num > 0)
							{
								num5 = Binder.Default.check_arguments_with_score(indexes, indexParameters);
								if (num5 == -1)
								{
									goto IL_10E;
								}
							}
							int derivedLevel = Binder.GetDerivedLevel(propertyInfo2.DeclaringType);
							if (propertyInfo != null)
							{
								if (num2 < num5)
								{
									goto IL_10E;
								}
								if (num2 == num5)
								{
									if (num4 == derivedLevel)
									{
										num3 = num5;
										goto IL_10E;
									}
									if (num4 > derivedLevel)
									{
										goto IL_10E;
									}
								}
							}
							propertyInfo = propertyInfo2;
							num2 = num5;
							num4 = derivedLevel;
						}
					}
					IL_10E:;
				}
				if (num3 <= num2)
				{
					throw new AmbiguousMatchException();
				}
				return propertyInfo;
			}

			private static int check_arguments_with_score(Type[] types, ParameterInfo[] args)
			{
				int num = -1;
				for (int i = 0; i < types.Length; i++)
				{
					int num2 = Binder.Default.check_type_with_score(types[i], args[i].ParameterType);
					if (num2 == -1)
					{
						return -1;
					}
					if (num < num2)
					{
						num = num2;
					}
				}
				return num;
			}

			private static int check_type_with_score(Type from, Type to)
			{
				if (from == null)
				{
					return (!to.IsValueType) ? 0 : -1;
				}
				if (from == to)
				{
					return 0;
				}
				if (to == typeof(object))
				{
					return 4;
				}
				TypeCode typeCode = Type.GetTypeCode(from);
				TypeCode typeCode2 = Type.GetTypeCode(to);
				switch (typeCode)
				{
				case TypeCode.Char:
					switch (typeCode2)
					{
					case TypeCode.UInt16:
						return 0;
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					default:
						return -1;
					}
					break;
				case TypeCode.SByte:
					switch (typeCode2)
					{
					case TypeCode.Int16:
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					}
					return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
				case TypeCode.Byte:
					switch (typeCode2)
					{
					case TypeCode.Char:
					case TypeCode.Int16:
					case TypeCode.UInt16:
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					}
					return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
				case TypeCode.Int16:
					switch (typeCode2)
					{
					case TypeCode.Int32:
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					}
					return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
				case TypeCode.UInt16:
					switch (typeCode2)
					{
					case TypeCode.Int32:
					case TypeCode.UInt32:
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					default:
						return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
					}
					break;
				case TypeCode.Int32:
					switch (typeCode2)
					{
					case TypeCode.Int64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					}
					return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
				case TypeCode.UInt32:
					switch (typeCode2)
					{
					case TypeCode.Int64:
					case TypeCode.UInt64:
					case TypeCode.Single:
					case TypeCode.Double:
						return 2;
					default:
						return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
					}
					break;
				case TypeCode.Int64:
				case TypeCode.UInt64:
				{
					TypeCode typeCode3 = typeCode2;
					if (typeCode3 != TypeCode.Single && typeCode3 != TypeCode.Double)
					{
						return (!from.IsEnum || to != typeof(Enum)) ? -1 : 1;
					}
					return 2;
				}
				case TypeCode.Single:
					return (typeCode2 != TypeCode.Double) ? -1 : 2;
				default:
					return (!to.IsAssignableFrom(from)) ? -1 : 3;
				}
			}
		}
	}
}
