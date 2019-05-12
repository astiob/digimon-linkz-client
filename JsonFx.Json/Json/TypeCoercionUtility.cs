using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace JsonFx.Json
{
	public class TypeCoercionUtility
	{
		private const string ErrorNullValueType = "{0} does not accept null as a value";

		private const string ErrorDefaultCtor = "Only objects with default constructors can be deserialized. ({0})";

		private const string ErrorCannotInstantiate = "Interfaces, Abstract classes, and unsupported ValueTypes cannot be deserialized. ({0})";

		private Dictionary<Type, Dictionary<string, MemberInfo>> memberMapCache;

		private bool allowNullValueTypes = true;

		private Dictionary<Type, Dictionary<string, MemberInfo>> MemberMapCache
		{
			get
			{
				if (this.memberMapCache == null)
				{
					this.memberMapCache = new Dictionary<Type, Dictionary<string, MemberInfo>>();
				}
				return this.memberMapCache;
			}
		}

		public bool AllowNullValueTypes
		{
			get
			{
				return this.allowNullValueTypes;
			}
			set
			{
				this.allowNullValueTypes = value;
			}
		}

		public object ProcessTypeHint(IDictionary result, string typeInfo, out Type objectType, out Dictionary<string, MemberInfo> memberMap)
		{
			if (string.IsNullOrEmpty(typeInfo))
			{
				objectType = null;
				memberMap = null;
				return result;
			}
			Type type = Type.GetType(typeInfo, false);
			if (type == null)
			{
				objectType = null;
				memberMap = null;
				return result;
			}
			objectType = type;
			return this.CoerceType(type, result, out memberMap);
		}

		public virtual object InstantiateObject(Type objectType, out Dictionary<string, MemberInfo> memberMap)
		{
			if (objectType.IsInterface || objectType.IsAbstract || objectType.IsValueType)
			{
				throw new JsonTypeCoercionException(string.Format("Interfaces, Abstract classes, and unsupported ValueTypes cannot be deserialized. ({0})", objectType.FullName));
			}
			ConstructorInfo constructor = objectType.GetConstructor(Type.EmptyTypes);
			if (constructor == null)
			{
				throw new JsonTypeCoercionException(string.Format("Only objects with default constructors can be deserialized. ({0})", objectType.FullName));
			}
			object result;
			try
			{
				result = constructor.Invoke(null);
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					throw new JsonTypeCoercionException(ex.InnerException.Message, ex.InnerException);
				}
				throw new JsonTypeCoercionException("Error instantiating " + objectType.FullName, ex);
			}
			if (typeof(IDictionary).IsAssignableFrom(objectType))
			{
				memberMap = null;
			}
			else
			{
				memberMap = this.CreateMemberMap(objectType);
			}
			return result;
		}

		protected Dictionary<string, MemberInfo> CreateMemberMap(Type objectType)
		{
			if (this.MemberMapCache.ContainsKey(objectType))
			{
				return this.MemberMapCache[objectType];
			}
			Dictionary<string, MemberInfo> dictionary = new Dictionary<string, MemberInfo>();
			PropertyInfo[] properties = objectType.GetProperties();
			foreach (PropertyInfo propertyInfo in properties)
			{
				if (propertyInfo.CanRead && propertyInfo.CanWrite)
				{
					if (!JsonIgnoreAttribute.IsJsonIgnore(propertyInfo))
					{
						string jsonName = JsonNameAttribute.GetJsonName(propertyInfo);
						if (string.IsNullOrEmpty(jsonName))
						{
							dictionary[propertyInfo.Name] = propertyInfo;
						}
						else
						{
							dictionary[jsonName] = propertyInfo;
						}
					}
				}
			}
			FieldInfo[] fields = objectType.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				if (fieldInfo.IsPublic)
				{
					if (!JsonIgnoreAttribute.IsJsonIgnore(fieldInfo))
					{
						string jsonName2 = JsonNameAttribute.GetJsonName(fieldInfo);
						if (string.IsNullOrEmpty(jsonName2))
						{
							dictionary[fieldInfo.Name] = fieldInfo;
						}
						else
						{
							dictionary[jsonName2] = fieldInfo;
						}
					}
				}
			}
			this.MemberMapCache[objectType] = dictionary;
			return dictionary;
		}

		public static Type GetMemberInfo(Dictionary<string, MemberInfo> memberMap, string memberName, out MemberInfo memberInfo)
		{
			if (memberMap != null && memberMap.ContainsKey(memberName))
			{
				memberInfo = memberMap[memberName];
				if (memberInfo is PropertyInfo)
				{
					return ((PropertyInfo)memberInfo).PropertyType;
				}
				if (memberInfo is FieldInfo)
				{
					return ((FieldInfo)memberInfo).FieldType;
				}
			}
			memberInfo = null;
			return null;
		}

		public virtual void SetMemberValue(object result, Type memberType, MemberInfo memberInfo, object value)
		{
			if (memberInfo is PropertyInfo)
			{
				((PropertyInfo)memberInfo).SetValue(result, this.CoerceType(memberType, value), null);
			}
			else if (memberInfo is FieldInfo)
			{
				((FieldInfo)memberInfo).SetValue(result, this.CoerceType(memberType, value));
			}
		}

		public virtual object CoerceType(Type targetType, object value)
		{
			bool flag = TypeCoercionUtility.IsNullable(targetType);
			if (value == null)
			{
				if (!this.allowNullValueTypes && targetType.IsValueType && !flag)
				{
					throw new JsonTypeCoercionException(string.Format("{0} does not accept null as a value", targetType.FullName));
				}
				return value;
			}
			else
			{
				if (flag)
				{
					Type[] genericArguments = targetType.GetGenericArguments();
					if (genericArguments.Length == 1)
					{
						targetType = genericArguments[0];
					}
				}
				Type type = value.GetType();
				if (targetType.IsAssignableFrom(type))
				{
					return value;
				}
				if (targetType.IsEnum)
				{
					if (value is string)
					{
						if (!Enum.IsDefined(targetType, value))
						{
							foreach (FieldInfo fieldInfo in targetType.GetFields())
							{
								string jsonName = JsonNameAttribute.GetJsonName(fieldInfo);
								if (((string)value).Equals(jsonName))
								{
									value = fieldInfo.Name;
									break;
								}
							}
						}
						return Enum.Parse(targetType, (string)value);
					}
					value = this.CoerceType(Enum.GetUnderlyingType(targetType), value);
					return Enum.ToObject(targetType, value);
				}
				else
				{
					if (value is IDictionary)
					{
						Dictionary<string, MemberInfo> dictionary;
						return this.CoerceType(targetType, (IDictionary)value, out dictionary);
					}
					if (typeof(IEnumerable).IsAssignableFrom(targetType) && typeof(IEnumerable).IsAssignableFrom(type))
					{
						return this.CoerceList(targetType, type, (IEnumerable)value);
					}
					if (value is string)
					{
						if (targetType == typeof(DateTime))
						{
							DateTime dateTime;
							if (DateTime.TryParse((string)value, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowLeadingWhite | DateTimeStyles.AllowTrailingWhite | DateTimeStyles.AllowInnerWhite | DateTimeStyles.NoCurrentDateDefault | DateTimeStyles.RoundtripKind, out dateTime))
							{
								return dateTime;
							}
						}
						else
						{
							if (targetType == typeof(Guid))
							{
								return new Guid((string)value);
							}
							if (targetType == typeof(char))
							{
								if (((string)value).Length == 1)
								{
									return ((string)value)[0];
								}
							}
							else if (targetType == typeof(Uri))
							{
								Uri result;
								if (Uri.TryCreate((string)value, UriKind.RelativeOrAbsolute, out result))
								{
									return result;
								}
							}
							else if (targetType == typeof(Version))
							{
								return new Version((string)value);
							}
						}
					}
					else if (targetType == typeof(TimeSpan))
					{
						return new TimeSpan((long)this.CoerceType(typeof(long), value));
					}
					TypeConverter converter = TypeDescriptor.GetConverter(targetType);
					if (converter.CanConvertFrom(type))
					{
						return converter.ConvertFrom(value);
					}
					converter = TypeDescriptor.GetConverter(type);
					if (converter.CanConvertTo(targetType))
					{
						return converter.ConvertTo(value, targetType);
					}
					object result2;
					try
					{
						result2 = Convert.ChangeType(value, targetType);
					}
					catch (Exception innerException)
					{
						throw new JsonTypeCoercionException(string.Format("Error converting {0} to {1}", value.GetType().FullName, targetType.FullName), innerException);
					}
					return result2;
				}
			}
		}

		protected virtual object CoerceType(Type targetType, IDictionary value, out Dictionary<string, MemberInfo> memberMap)
		{
			object result = this.InstantiateObject(targetType, out memberMap);
			if (memberMap != null)
			{
				foreach (object obj in value.Keys)
				{
					MemberInfo memberInfo2;
					Type memberInfo = TypeCoercionUtility.GetMemberInfo(memberMap, obj as string, out memberInfo2);
					this.SetMemberValue(result, memberInfo, memberInfo2, value[obj]);
				}
			}
			return result;
		}

		protected virtual object CoerceList(Type targetType, Type arrayType, IEnumerable value)
		{
			if (targetType.IsArray)
			{
				return this.CoerceArray(targetType.GetElementType(), value);
			}
			ConstructorInfo[] constructors = targetType.GetConstructors();
			ConstructorInfo constructorInfo = null;
			foreach (ConstructorInfo constructorInfo2 in constructors)
			{
				ParameterInfo[] parameters = constructorInfo2.GetParameters();
				if (parameters.Length == 0)
				{
					constructorInfo = constructorInfo2;
				}
				else if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(arrayType))
				{
					try
					{
						return constructorInfo2.Invoke(new object[]
						{
							value
						});
					}
					catch
					{
					}
				}
			}
			if (constructorInfo == null)
			{
				throw new JsonTypeCoercionException(string.Format("Only objects with default constructors can be deserialized. ({0})", targetType.FullName));
			}
			object obj;
			try
			{
				obj = constructorInfo.Invoke(null);
			}
			catch (TargetInvocationException ex)
			{
				if (ex.InnerException != null)
				{
					throw new JsonTypeCoercionException(ex.InnerException.Message, ex.InnerException);
				}
				throw new JsonTypeCoercionException("Error instantiating " + targetType.FullName, ex);
			}
			MethodInfo method = targetType.GetMethod("AddRange");
			ParameterInfo[] array2 = (method != null) ? method.GetParameters() : null;
			Type type = (array2 != null && array2.Length == 1) ? array2[0].ParameterType : null;
			if (type != null && type.IsAssignableFrom(arrayType))
			{
				try
				{
					method.Invoke(obj, new object[]
					{
						value
					});
				}
				catch (TargetInvocationException ex2)
				{
					if (ex2.InnerException != null)
					{
						throw new JsonTypeCoercionException(ex2.InnerException.Message, ex2.InnerException);
					}
					throw new JsonTypeCoercionException("Error calling AddRange on " + targetType.FullName, ex2);
				}
				return obj;
			}
			method = targetType.GetMethod("Add");
			array2 = ((method != null) ? method.GetParameters() : null);
			type = ((array2 != null && array2.Length == 1) ? array2[0].ParameterType : null);
			if (type != null)
			{
				foreach (object value2 in value)
				{
					try
					{
						method.Invoke(obj, new object[]
						{
							this.CoerceType(type, value2)
						});
					}
					catch (TargetInvocationException ex3)
					{
						if (ex3.InnerException != null)
						{
							throw new JsonTypeCoercionException(ex3.InnerException.Message, ex3.InnerException);
						}
						throw new JsonTypeCoercionException("Error calling Add on " + targetType.FullName, ex3);
					}
				}
				return obj;
			}
			object result;
			try
			{
				result = Convert.ChangeType(value, targetType);
			}
			catch (Exception innerException)
			{
				throw new JsonTypeCoercionException(string.Format("Error converting {0} to {1}", value.GetType().FullName, targetType.FullName), innerException);
			}
			return result;
		}

		protected virtual Array CoerceArray(Type elementType, IEnumerable value)
		{
			ArrayList arrayList = new ArrayList();
			foreach (object value2 in value)
			{
				arrayList.Add(this.CoerceType(elementType, value2));
			}
			return arrayList.ToArray(elementType);
		}

		private static bool IsNullable(Type type)
		{
			return type.IsGenericType && typeof(Nullable<>) == type.GetGenericTypeDefinition();
		}
	}
}
