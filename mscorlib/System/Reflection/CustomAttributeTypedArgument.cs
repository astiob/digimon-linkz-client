using System;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Represents an argument of a custom attribute in the reflection-only context, or an element of an array argument.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct CustomAttributeTypedArgument
	{
		private Type argumentType;

		private object value;

		internal CustomAttributeTypedArgument(Type argumentType, object value)
		{
			this.argumentType = argumentType;
			this.value = value;
			if (value is Array)
			{
				Array array = (Array)value;
				Type elementType = array.GetType().GetElementType();
				CustomAttributeTypedArgument[] array2 = new CustomAttributeTypedArgument[array.GetLength(0)];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = new CustomAttributeTypedArgument(elementType, array.GetValue(i));
				}
				this.value = new ReadOnlyCollection<CustomAttributeTypedArgument>(array2);
			}
		}

		/// <summary>Gets the type of the argument or of the array argument element.</summary>
		/// <returns>A <see cref="T:System.Type" /> object representing the type of the argument or of the array element.</returns>
		public Type ArgumentType
		{
			get
			{
				return this.argumentType;
			}
		}

		/// <summary>Gets the value of the argument for a simple argument or for an element of an array argument; gets a collection of values for an array argument.</summary>
		/// <returns>An object that represents the value of the argument or element, or a generic <see cref="T:System.Collections.ObjectModel.ReadOnlyCollection`1" /> of <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> objects that represent the values of an array-type argument.</returns>
		public object Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>Returns a string consisting of the argument name, the equal sign, and a string representation of the argument value.</summary>
		/// <returns>A string consisting of the argument name, the equal sign, and a string representation of the argument value.</returns>
		public override string ToString()
		{
			string text = (this.value == null) ? string.Empty : this.value.ToString();
			if (this.argumentType == typeof(string))
			{
				return "\"" + text + "\"";
			}
			if (this.argumentType == typeof(Type))
			{
				return "typeof (" + text + ")";
			}
			if (this.argumentType.IsEnum)
			{
				return "(" + this.argumentType.Name + ")" + text;
			}
			return text;
		}

		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		/// <param name="obj">Another object to compare to. </param>
		public override bool Equals(object obj)
		{
			if (!(obj is CustomAttributeTypedArgument))
			{
				return false;
			}
			CustomAttributeTypedArgument customAttributeTypedArgument = (CustomAttributeTypedArgument)obj;
			return (customAttributeTypedArgument.argumentType != this.argumentType || this.value == null) ? (customAttributeTypedArgument.value == null) : this.value.Equals(customAttributeTypedArgument.value);
		}

		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return (this.argumentType.GetHashCode() << 16) + ((this.value == null) ? 0 : this.value.GetHashCode());
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are equivalent.</summary>
		/// <returns>true if the two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are equal; otherwise, false.</returns>
		/// <param name="left">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the left of the equality operator.</param>
		/// <param name="right">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the right of the equality operator.</param>
		public static bool operator ==(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return left.Equals(right);
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are different.</summary>
		/// <returns>true if the two <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structures are different; otherwise, false.</returns>
		/// <param name="left">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the left of the inequality operator.</param>
		/// <param name="right">The <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure to the right of the inequality operator.</param>
		public static bool operator !=(CustomAttributeTypedArgument left, CustomAttributeTypedArgument right)
		{
			return !left.Equals(right);
		}
	}
}
