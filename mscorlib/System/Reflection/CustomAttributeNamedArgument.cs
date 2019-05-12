using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Represents a named argument of a custom attribute in the reflection-only context.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct CustomAttributeNamedArgument
	{
		private CustomAttributeTypedArgument typedArgument;

		private MemberInfo memberInfo;

		internal CustomAttributeNamedArgument(MemberInfo memberInfo, object typedArgument)
		{
			this.memberInfo = memberInfo;
			this.typedArgument = (CustomAttributeTypedArgument)typedArgument;
		}

		/// <summary>Gets the attribute member that would be used to set the named argument.</summary>
		/// <returns>A <see cref="T:System.Reflection.MemberInfo" /> representing the attribute member that would be used to set the named argument.</returns>
		public MemberInfo MemberInfo
		{
			get
			{
				return this.memberInfo;
			}
		}

		/// <summary>Gets a <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure that can be used to obtain the type and value of the current named argument.</summary>
		/// <returns>A <see cref="T:System.Reflection.CustomAttributeTypedArgument" /> structure that can be used to obtain the type and value of the current named argument.</returns>
		public CustomAttributeTypedArgument TypedValue
		{
			get
			{
				return this.typedArgument;
			}
		}

		/// <summary>Returns a string consisting of the argument name, the equal sign, and a string representation of the argument value.</summary>
		/// <returns>A string consisting of the argument name, the equal sign, and a string representation of the argument value.</returns>
		public override string ToString()
		{
			return this.memberInfo.Name + " = " + this.typedArgument.ToString();
		}

		/// <returns>true if <paramref name="obj" /> and this instance are the same type and represent the same value; otherwise, false.</returns>
		/// <param name="obj">Another object to compare to. </param>
		public override bool Equals(object obj)
		{
			if (!(obj is CustomAttributeNamedArgument))
			{
				return false;
			}
			CustomAttributeNamedArgument customAttributeNamedArgument = (CustomAttributeNamedArgument)obj;
			return customAttributeNamedArgument.memberInfo == this.memberInfo && this.typedArgument.Equals(customAttributeNamedArgument.typedArgument);
		}

		/// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
		public override int GetHashCode()
		{
			return (this.memberInfo.GetHashCode() << 16) + this.typedArgument.GetHashCode();
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equivalent.</summary>
		/// <returns>true if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are equal; otherwise, false.</returns>
		/// <param name="left">The <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structure to the left of the equality operator.</param>
		/// <param name="right">The <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structure to the right of the equality operator.</param>
		public static bool operator ==(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return left.Equals(right);
		}

		/// <summary>Tests whether two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different.</summary>
		/// <returns>true if the two <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structures are different; otherwise, false.</returns>
		/// <param name="left">The <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structure to the left of the inequality operator.</param>
		/// <param name="right">The <see cref="T:System.Reflection.CustomAttributeNamedArgument" /> structure to the right of the inequality operator.</param>
		public static bool operator !=(CustomAttributeNamedArgument left, CustomAttributeNamedArgument right)
		{
			return !left.Equals(right);
		}
	}
}
