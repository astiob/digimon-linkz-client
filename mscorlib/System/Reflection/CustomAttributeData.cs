using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Reflection
{
	/// <summary>Provides access to custom attribute data for assemblies, modules, types, members and parameters that are loaded into the reflection-only context.</summary>
	[ComVisible(true)]
	[Serializable]
	public sealed class CustomAttributeData
	{
		private ConstructorInfo ctorInfo;

		private IList<CustomAttributeTypedArgument> ctorArgs;

		private IList<CustomAttributeNamedArgument> namedArgs;

		internal CustomAttributeData(ConstructorInfo ctorInfo, object[] ctorArgs, object[] namedArgs)
		{
			this.ctorInfo = ctorInfo;
			this.ctorArgs = Array.AsReadOnly<CustomAttributeTypedArgument>((ctorArgs == null) ? new CustomAttributeTypedArgument[0] : CustomAttributeData.UnboxValues<CustomAttributeTypedArgument>(ctorArgs));
			this.namedArgs = Array.AsReadOnly<CustomAttributeNamedArgument>((namedArgs == null) ? new CustomAttributeNamedArgument[0] : CustomAttributeData.UnboxValues<CustomAttributeNamedArgument>(namedArgs));
		}

		/// <summary>Returns a <see cref="T:System.Reflection.ConstructorInfo" /> object representing the constructor that would have initialized the custom attribute.</summary>
		/// <returns>A <see cref="T:System.Reflection.ConstructorInfo" /> object representing the constructor that would have initialized the custom attribute represented by the current instance of the <see cref="T:System.Reflection.CustomAttributeData" /> class.</returns>
		[ComVisible(true)]
		public ConstructorInfo Constructor
		{
			get
			{
				return this.ctorInfo;
			}
		}

		/// <summary>Gets the list of positional arguments specified for the attribute instance represented by the <see cref="T:System.Reflection.CustomAttributeData" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures representing the positional arguments specified for the custom attribute instance.</returns>
		[ComVisible(true)]
		public IList<CustomAttributeTypedArgument> ConstructorArguments
		{
			get
			{
				return this.ctorArgs;
			}
		}

		/// <summary>Gets the list of named arguments specified for the attribute instance represented by the <see cref="T:System.Reflection.CustomAttributeData" /> object.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures representing the named arguments specified for the custom attribute instance.</returns>
		public IList<CustomAttributeNamedArgument> NamedArguments
		{
			get
			{
				return this.namedArgs;
			}
		}

		/// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target assembly.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target assembly.</returns>
		/// <param name="target">The assembly whose custom attribute data is to be retrieved.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.</exception>
		public static IList<CustomAttributeData> GetCustomAttributes(Assembly target)
		{
			return MonoCustomAttrs.GetCustomAttributesData(target);
		}

		/// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target member.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target member.</returns>
		/// <param name="target">A <see cref="T:System.Reflection.MemberInfo" /> object representing the member whose attribute data is to be retrieved.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.</exception>
		public static IList<CustomAttributeData> GetCustomAttributes(MemberInfo target)
		{
			return MonoCustomAttrs.GetCustomAttributesData(target);
		}

		/// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target module.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target module.</returns>
		/// <param name="target">The module whose custom attribute data is to be retrieved.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.</exception>
		public static IList<CustomAttributeData> GetCustomAttributes(Module target)
		{
			return MonoCustomAttrs.GetCustomAttributesData(target);
		}

		/// <summary>Returns a list of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target parameter.</summary>
		/// <returns>An <see cref="T:System.Collections.Generic.IList`1" /> of <see cref="T:System.Reflection.CustomAttributeData" /> objects representing data about the attributes that have been applied to the target parameter.</returns>
		/// <param name="target">A <see cref="T:System.Reflection.ParameterInfo" /> object representing the parameter whose attribute data is to be retrieved.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="target" /> is null.</exception>
		public static IList<CustomAttributeData> GetCustomAttributes(ParameterInfo target)
		{
			return MonoCustomAttrs.GetCustomAttributesData(target);
		}

		/// <summary>Returns a string representation of the custom attribute.</summary>
		/// <returns>A string value that represents the custom attribute.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[" + this.ctorInfo.DeclaringType.FullName + "(");
			for (int i = 0; i < this.ctorArgs.Count; i++)
			{
				stringBuilder.Append(this.ctorArgs[i].ToString());
				if (i + 1 < this.ctorArgs.Count)
				{
					stringBuilder.Append(", ");
				}
			}
			if (this.namedArgs.Count > 0)
			{
				stringBuilder.Append(", ");
			}
			for (int j = 0; j < this.namedArgs.Count; j++)
			{
				stringBuilder.Append(this.namedArgs[j].ToString());
				if (j + 1 < this.namedArgs.Count)
				{
					stringBuilder.Append(", ");
				}
			}
			stringBuilder.AppendFormat(")]", new object[0]);
			return stringBuilder.ToString();
		}

		private static T[] UnboxValues<T>(object[] values)
		{
			T[] array = new T[values.Length];
			for (int i = 0; i < values.Length; i++)
			{
				array[i] = (T)((object)values[i]);
			}
			return array;
		}

		/// <param name="obj"></param>
		public override bool Equals(object obj)
		{
			CustomAttributeData customAttributeData = obj as CustomAttributeData;
			if (customAttributeData == null || customAttributeData.ctorInfo != this.ctorInfo || customAttributeData.ctorArgs.Count != this.ctorArgs.Count || customAttributeData.namedArgs.Count != this.namedArgs.Count)
			{
				return false;
			}
			for (int i = 0; i < this.ctorArgs.Count; i++)
			{
				if (this.ctorArgs[i].Equals(customAttributeData.ctorArgs[i]))
				{
					return false;
				}
			}
			for (int j = 0; j < this.namedArgs.Count; j++)
			{
				bool flag = false;
				for (int k = 0; k < customAttributeData.namedArgs.Count; k++)
				{
					if (this.namedArgs[j].Equals(customAttributeData.namedArgs[k]))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = this.ctorInfo.GetHashCode() << 16;
			for (int i = 0; i < this.ctorArgs.Count; i++)
			{
				num += (num ^ 7 + this.ctorArgs[i].GetHashCode() << i * 4);
			}
			for (int j = 0; j < this.namedArgs.Count; j++)
			{
				num += this.namedArgs[j].GetHashCode() << 5;
			}
			return num;
		}
	}
}
