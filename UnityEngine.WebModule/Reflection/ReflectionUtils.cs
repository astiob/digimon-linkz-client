﻿using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SimpleJson.Reflection
{
	[GeneratedCode("reflection-utils", "1.0.0")]
	internal class ReflectionUtils
	{
		private static readonly object[] EmptyObjects = new object[0];

		public static Attribute GetAttribute(MemberInfo info, Type type)
		{
			Attribute result;
			if (info == null || type == null || !Attribute.IsDefined(info, type))
			{
				result = null;
			}
			else
			{
				result = Attribute.GetCustomAttribute(info, type);
			}
			return result;
		}

		public static Attribute GetAttribute(Type objectType, Type attributeType)
		{
			Attribute result;
			if (objectType == null || attributeType == null || !Attribute.IsDefined(objectType, attributeType))
			{
				result = null;
			}
			else
			{
				result = Attribute.GetCustomAttribute(objectType, attributeType);
			}
			return result;
		}

		public static Type[] GetGenericTypeArguments(Type type)
		{
			return type.GetGenericArguments();
		}

		public static bool IsTypeGenericeCollectionInterface(Type type)
		{
			bool result;
			if (!type.IsGenericType)
			{
				result = false;
			}
			else
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				result = (genericTypeDefinition == typeof(IList<>) || genericTypeDefinition == typeof(ICollection<>) || genericTypeDefinition == typeof(IEnumerable<>));
			}
			return result;
		}

		public static bool IsAssignableFrom(Type type1, Type type2)
		{
			return type1.IsAssignableFrom(type2);
		}

		public static bool IsTypeDictionary(Type type)
		{
			bool result;
			if (typeof(IDictionary).IsAssignableFrom(type))
			{
				result = true;
			}
			else if (!type.IsGenericType)
			{
				result = false;
			}
			else
			{
				Type genericTypeDefinition = type.GetGenericTypeDefinition();
				result = (genericTypeDefinition == typeof(IDictionary<, >));
			}
			return result;
		}

		public static bool IsNullableType(Type type)
		{
			return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
		}

		public static object ToNullableType(object obj, Type nullableType)
		{
			return (obj != null) ? Convert.ChangeType(obj, Nullable.GetUnderlyingType(nullableType), CultureInfo.InvariantCulture) : null;
		}

		public static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		public static IEnumerable<ConstructorInfo> GetConstructors(Type type)
		{
			return type.GetConstructors();
		}

		public static ConstructorInfo GetConstructorInfo(Type type, params Type[] argsType)
		{
			IEnumerable<ConstructorInfo> constructors = ReflectionUtils.GetConstructors(type);
			foreach (ConstructorInfo constructorInfo in constructors)
			{
				ParameterInfo[] parameters = constructorInfo.GetParameters();
				if (argsType.Length == parameters.Length)
				{
					int num = 0;
					bool flag = true;
					foreach (ParameterInfo parameterInfo in constructorInfo.GetParameters())
					{
						if (parameterInfo.ParameterType != argsType[num])
						{
							flag = false;
							break;
						}
					}
					if (flag)
					{
						return constructorInfo;
					}
				}
			}
			return null;
		}

		public static IEnumerable<PropertyInfo> GetProperties(Type type)
		{
			return type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static IEnumerable<FieldInfo> GetFields(Type type)
		{
			return type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		public static MethodInfo GetGetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetGetMethod(true);
		}

		public static MethodInfo GetSetterMethodInfo(PropertyInfo propertyInfo)
		{
			return propertyInfo.GetSetMethod(true);
		}

		public static ReflectionUtils.ConstructorDelegate GetContructor(ConstructorInfo constructorInfo)
		{
			return ReflectionUtils.GetConstructorByReflection(constructorInfo);
		}

		public static ReflectionUtils.ConstructorDelegate GetContructor(Type type, params Type[] argsType)
		{
			return ReflectionUtils.GetConstructorByReflection(type, argsType);
		}

		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(ConstructorInfo constructorInfo)
		{
			return (object[] args) => constructorInfo.Invoke(args);
		}

		public static ReflectionUtils.ConstructorDelegate GetConstructorByReflection(Type type, params Type[] argsType)
		{
			ConstructorInfo constructorInfo = ReflectionUtils.GetConstructorInfo(type, argsType);
			return (constructorInfo != null) ? ReflectionUtils.GetConstructorByReflection(constructorInfo) : null;
		}

		public static ReflectionUtils.GetDelegate GetGetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(propertyInfo);
		}

		public static ReflectionUtils.GetDelegate GetGetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetGetMethodByReflection(fieldInfo);
		}

		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetGetterMethodInfo(propertyInfo);
			return (object source) => methodInfo.Invoke(source, ReflectionUtils.EmptyObjects);
		}

		public static ReflectionUtils.GetDelegate GetGetMethodByReflection(FieldInfo fieldInfo)
		{
			return (object source) => fieldInfo.GetValue(source);
		}

		public static ReflectionUtils.SetDelegate GetSetMethod(PropertyInfo propertyInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(propertyInfo);
		}

		public static ReflectionUtils.SetDelegate GetSetMethod(FieldInfo fieldInfo)
		{
			return ReflectionUtils.GetSetMethodByReflection(fieldInfo);
		}

		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(PropertyInfo propertyInfo)
		{
			MethodInfo methodInfo = ReflectionUtils.GetSetterMethodInfo(propertyInfo);
			return delegate(object source, object value)
			{
				methodInfo.Invoke(source, new object[]
				{
					value
				});
			};
		}

		public static ReflectionUtils.SetDelegate GetSetMethodByReflection(FieldInfo fieldInfo)
		{
			return delegate(object source, object value)
			{
				fieldInfo.SetValue(source, value);
			};
		}

		public delegate object GetDelegate(object source);

		public delegate void SetDelegate(object source, object value);

		public delegate object ConstructorDelegate(params object[] args);

		public delegate TValue ThreadSafeDictionaryValueFactory<TKey, TValue>(TKey key);

		public sealed class ThreadSafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
		{
			private readonly object _lock = new object();

			private readonly ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> _valueFactory;

			private Dictionary<TKey, TValue> _dictionary;

			public ThreadSafeDictionary(ReflectionUtils.ThreadSafeDictionaryValueFactory<TKey, TValue> valueFactory)
			{
				this._valueFactory = valueFactory;
			}

			private TValue Get(TKey key)
			{
				TValue result;
				TValue tvalue;
				if (this._dictionary == null)
				{
					result = this.AddValue(key);
				}
				else if (!this._dictionary.TryGetValue(key, out tvalue))
				{
					result = this.AddValue(key);
				}
				else
				{
					result = tvalue;
				}
				return result;
			}

			private TValue AddValue(TKey key)
			{
				TValue tvalue = this._valueFactory(key);
				object @lock = this._lock;
				lock (@lock)
				{
					if (this._dictionary == null)
					{
						this._dictionary = new Dictionary<TKey, TValue>();
						this._dictionary[key] = tvalue;
					}
					else
					{
						TValue result;
						if (this._dictionary.TryGetValue(key, out result))
						{
							return result;
						}
						Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>(this._dictionary);
						dictionary[key] = tvalue;
						this._dictionary = dictionary;
					}
				}
				return tvalue;
			}

			public void Add(TKey key, TValue value)
			{
				throw new NotImplementedException();
			}

			public bool ContainsKey(TKey key)
			{
				return this._dictionary.ContainsKey(key);
			}

			public ICollection<TKey> Keys
			{
				get
				{
					return this._dictionary.Keys;
				}
			}

			public bool Remove(TKey key)
			{
				throw new NotImplementedException();
			}

			public bool TryGetValue(TKey key, out TValue value)
			{
				value = this[key];
				return true;
			}

			public ICollection<TValue> Values
			{
				get
				{
					return this._dictionary.Values;
				}
			}

			public TValue this[TKey key]
			{
				get
				{
					return this.Get(key);
				}
				set
				{
					throw new NotImplementedException();
				}
			}

			public void Add(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			public void Clear()
			{
				throw new NotImplementedException();
			}

			public bool Contains(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
			{
				throw new NotImplementedException();
			}

			public int Count
			{
				get
				{
					return this._dictionary.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			public bool Remove(KeyValuePair<TKey, TValue> item)
			{
				throw new NotImplementedException();
			}

			public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this._dictionary.GetEnumerator();
			}
		}
	}
}
