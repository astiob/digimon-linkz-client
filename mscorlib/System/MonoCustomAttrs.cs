using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System
{
	internal class MonoCustomAttrs
	{
		private static Assembly corlib;

		private static readonly Type AttributeUsageType = typeof(AttributeUsageAttribute);

		private static readonly AttributeUsageAttribute DefaultAttributeUsage = new AttributeUsageAttribute(AttributeTargets.All);

		private static bool IsUserCattrProvider(object obj)
		{
			Type type = obj as Type;
			if (type is MonoType || type is TypeBuilder)
			{
				return false;
			}
			if (obj is Type)
			{
				return true;
			}
			if (MonoCustomAttrs.corlib == null)
			{
				MonoCustomAttrs.corlib = typeof(int).Assembly;
			}
			return obj.GetType().Assembly != MonoCustomAttrs.corlib;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern object[] GetCustomAttributesInternal(ICustomAttributeProvider obj, Type attributeType, bool pseudoAttrs);

		internal static object[] GetPseudoCustomAttributes(ICustomAttributeProvider obj, Type attributeType)
		{
			object[] array = null;
			if (obj is MonoMethod)
			{
				array = ((MonoMethod)obj).GetPseudoCustomAttributes();
			}
			else if (obj is FieldInfo)
			{
				array = ((FieldInfo)obj).GetPseudoCustomAttributes();
			}
			else if (obj is ParameterInfo)
			{
				array = ((ParameterInfo)obj).GetPseudoCustomAttributes();
			}
			else if (obj is Type)
			{
				array = ((Type)obj).GetPseudoCustomAttributes();
			}
			if (attributeType != null && array != null)
			{
				int i = 0;
				while (i < array.Length)
				{
					if (attributeType.IsAssignableFrom(array[i].GetType()))
					{
						if (array.Length == 1)
						{
							return array;
						}
						return new object[]
						{
							array[i]
						};
					}
					else
					{
						i++;
					}
				}
				return new object[0];
			}
			return array;
		}

		internal static object[] GetCustomAttributesBase(ICustomAttributeProvider obj, Type attributeType)
		{
			object[] array;
			if (MonoCustomAttrs.IsUserCattrProvider(obj))
			{
				array = obj.GetCustomAttributes(attributeType, true);
			}
			else
			{
				array = MonoCustomAttrs.GetCustomAttributesInternal(obj, attributeType, false);
			}
			object[] pseudoCustomAttributes = MonoCustomAttrs.GetPseudoCustomAttributes(obj, attributeType);
			if (pseudoCustomAttributes != null)
			{
				object[] array2 = new object[array.Length + pseudoCustomAttributes.Length];
				Array.Copy(array, array2, array.Length);
				Array.Copy(pseudoCustomAttributes, 0, array2, array.Length, pseudoCustomAttributes.Length);
				return array2;
			}
			return array;
		}

		internal static Attribute GetCustomAttribute(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			object[] customAttributes = MonoCustomAttrs.GetCustomAttributes(obj, attributeType, inherit);
			if (customAttributes.Length == 0)
			{
				return null;
			}
			if (customAttributes.Length > 1)
			{
				string text = "'{0}' has more than one attribute of type '{1}";
				text = string.Format(text, obj, attributeType);
				throw new AmbiguousMatchException(text);
			}
			return (Attribute)customAttributes[0];
		}

		internal static object[] GetCustomAttributes(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (attributeType == typeof(MonoCustomAttrs))
			{
				attributeType = null;
			}
			object[] customAttributesBase = MonoCustomAttrs.GetCustomAttributesBase(obj, attributeType);
			if (!inherit && customAttributesBase.Length == 1)
			{
				object[] array;
				if (attributeType != null)
				{
					if (attributeType.IsAssignableFrom(customAttributesBase[0].GetType()))
					{
						array = (object[])Array.CreateInstance(attributeType, 1);
						array[0] = customAttributesBase[0];
					}
					else
					{
						array = (object[])Array.CreateInstance(attributeType, 0);
					}
				}
				else
				{
					array = (object[])Array.CreateInstance(customAttributesBase[0].GetType(), 1);
					array[0] = customAttributesBase[0];
				}
				return array;
			}
			if (attributeType != null && attributeType.IsSealed && inherit)
			{
				AttributeUsageAttribute attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(attributeType);
				if (!attributeUsageAttribute.Inherited)
				{
					inherit = false;
				}
			}
			int capacity = (customAttributesBase.Length >= 16) ? 16 : customAttributesBase.Length;
			Hashtable hashtable = new Hashtable(capacity);
			ArrayList arrayList = new ArrayList(capacity);
			ICustomAttributeProvider customAttributeProvider = obj;
			int num = 0;
			do
			{
				foreach (object obj2 in customAttributesBase)
				{
					Type type = obj2.GetType();
					if (attributeType == null || attributeType.IsAssignableFrom(type))
					{
						MonoCustomAttrs.AttributeInfo attributeInfo = (MonoCustomAttrs.AttributeInfo)hashtable[type];
						AttributeUsageAttribute attributeUsageAttribute2;
						if (attributeInfo != null)
						{
							attributeUsageAttribute2 = attributeInfo.Usage;
						}
						else
						{
							attributeUsageAttribute2 = MonoCustomAttrs.RetrieveAttributeUsage(type);
						}
						if ((num == 0 || attributeUsageAttribute2.Inherited) && (attributeUsageAttribute2.AllowMultiple || attributeInfo == null || (attributeInfo != null && attributeInfo.InheritanceLevel == num)))
						{
							arrayList.Add(obj2);
						}
						if (attributeInfo == null)
						{
							hashtable.Add(type, new MonoCustomAttrs.AttributeInfo(attributeUsageAttribute2, num));
						}
					}
				}
				if ((customAttributeProvider = MonoCustomAttrs.GetBase(customAttributeProvider)) != null)
				{
					num++;
					customAttributesBase = MonoCustomAttrs.GetCustomAttributesBase(customAttributeProvider, attributeType);
				}
			}
			while (inherit && customAttributeProvider != null);
			object[] array3;
			if (attributeType == null || attributeType.IsValueType)
			{
				array3 = (object[])Array.CreateInstance(typeof(Attribute), arrayList.Count);
			}
			else
			{
				array3 = (Array.CreateInstance(attributeType, arrayList.Count) as object[]);
			}
			arrayList.CopyTo(array3, 0);
			return array3;
		}

		internal static object[] GetCustomAttributes(ICustomAttributeProvider obj, bool inherit)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			if (!inherit)
			{
				return (object[])MonoCustomAttrs.GetCustomAttributesBase(obj, null).Clone();
			}
			return MonoCustomAttrs.GetCustomAttributes(obj, typeof(MonoCustomAttrs), inherit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern CustomAttributeData[] GetCustomAttributesDataInternal(ICustomAttributeProvider obj);

		internal static IList<CustomAttributeData> GetCustomAttributesData(ICustomAttributeProvider obj)
		{
			if (obj == null)
			{
				throw new ArgumentNullException("obj");
			}
			CustomAttributeData[] customAttributesDataInternal = MonoCustomAttrs.GetCustomAttributesDataInternal(obj);
			return Array.AsReadOnly<CustomAttributeData>(customAttributesDataInternal);
		}

		internal static bool IsDefined(ICustomAttributeProvider obj, Type attributeType, bool inherit)
		{
			if (attributeType == null)
			{
				throw new ArgumentNullException("attributeType");
			}
			if (MonoCustomAttrs.IsUserCattrProvider(obj))
			{
				return obj.IsDefined(attributeType, inherit);
			}
			if (MonoCustomAttrs.IsDefinedInternal(obj, attributeType))
			{
				return true;
			}
			object[] pseudoCustomAttributes = MonoCustomAttrs.GetPseudoCustomAttributes(obj, attributeType);
			if (pseudoCustomAttributes != null)
			{
				for (int i = 0; i < pseudoCustomAttributes.Length; i++)
				{
					if (attributeType.IsAssignableFrom(pseudoCustomAttributes[i].GetType()))
					{
						return true;
					}
				}
			}
			ICustomAttributeProvider @base;
			return inherit && (@base = MonoCustomAttrs.GetBase(obj)) != null && MonoCustomAttrs.IsDefined(@base, attributeType, inherit);
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern bool IsDefinedInternal(ICustomAttributeProvider obj, Type AttributeType);

		private static PropertyInfo GetBasePropertyDefinition(PropertyInfo property)
		{
			MethodInfo methodInfo = property.GetGetMethod(true);
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				methodInfo = property.GetSetMethod(true);
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				return null;
			}
			MethodInfo baseDefinition = methodInfo.GetBaseDefinition();
			if (baseDefinition == null || baseDefinition == methodInfo)
			{
				return null;
			}
			ParameterInfo[] indexParameters = property.GetIndexParameters();
			if (indexParameters != null && indexParameters.Length > 0)
			{
				Type[] array = new Type[indexParameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = indexParameters[i].ParameterType;
				}
				return baseDefinition.DeclaringType.GetProperty(property.Name, property.PropertyType, array);
			}
			return baseDefinition.DeclaringType.GetProperty(property.Name, property.PropertyType);
		}

		private static ICustomAttributeProvider GetBase(ICustomAttributeProvider obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (obj is Type)
			{
				return ((Type)obj).BaseType;
			}
			MethodInfo methodInfo = null;
			if (obj is MonoProperty)
			{
				return MonoCustomAttrs.GetBasePropertyDefinition((MonoProperty)obj);
			}
			if (obj is MonoMethod)
			{
				methodInfo = (MethodInfo)obj;
			}
			if (methodInfo == null || !methodInfo.IsVirtual)
			{
				return null;
			}
			MethodInfo baseDefinition = methodInfo.GetBaseDefinition();
			if (baseDefinition == methodInfo)
			{
				return null;
			}
			return baseDefinition;
		}

		private static AttributeUsageAttribute RetrieveAttributeUsage(Type attributeType)
		{
			if (attributeType == typeof(AttributeUsageAttribute))
			{
				return new AttributeUsageAttribute(AttributeTargets.Class);
			}
			AttributeUsageAttribute attributeUsageAttribute = null;
			object[] customAttributes = MonoCustomAttrs.GetCustomAttributes(attributeType, MonoCustomAttrs.AttributeUsageType, false);
			if (customAttributes.Length == 0)
			{
				if (attributeType.BaseType != null)
				{
					attributeUsageAttribute = MonoCustomAttrs.RetrieveAttributeUsage(attributeType.BaseType);
				}
				if (attributeUsageAttribute != null)
				{
					return attributeUsageAttribute;
				}
				return MonoCustomAttrs.DefaultAttributeUsage;
			}
			else
			{
				if (customAttributes.Length > 1)
				{
					throw new FormatException("Duplicate AttributeUsageAttribute cannot be specified on an attribute type.");
				}
				return (AttributeUsageAttribute)customAttributes[0];
			}
		}

		private class AttributeInfo
		{
			private AttributeUsageAttribute _usage;

			private int _inheritanceLevel;

			public AttributeInfo(AttributeUsageAttribute usage, int inheritanceLevel)
			{
				this._usage = usage;
				this._inheritanceLevel = inheritanceLevel;
			}

			public AttributeUsageAttribute Usage
			{
				get
				{
					return this._usage;
				}
			}

			public int InheritanceLevel
			{
				get
				{
					return this._inheritanceLevel;
				}
			}
		}
	}
}
