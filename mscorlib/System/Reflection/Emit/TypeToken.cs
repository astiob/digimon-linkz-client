using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	/// <summary>Represents the Token returned by the metadata to represent a type.</summary>
	[ComVisible(true)]
	[Serializable]
	public struct TypeToken
	{
		internal int tokValue;

		/// <summary>The default TypeToken with <see cref="P:System.Reflection.Emit.TypeToken.Token" /> value 0.</summary>
		public static readonly TypeToken Empty = default(TypeToken);

		internal TypeToken(int val)
		{
			this.tokValue = val;
		}

		/// <summary>Checks if the given object is an instance of TypeToken and is equal to this instance.</summary>
		/// <returns>true if <paramref name="obj" /> is an instance of TypeToken and is equal to this object; otherwise, false.</returns>
		/// <param name="obj">The object to compare with this TypeToken. </param>
		public override bool Equals(object obj)
		{
			bool flag = obj is TypeToken;
			if (flag)
			{
				TypeToken typeToken = (TypeToken)obj;
				flag = (this.tokValue == typeToken.tokValue);
			}
			return flag;
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Reflection.Emit.TypeToken" />.</summary>
		/// <returns>true if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Reflection.Emit.TypeToken" /> to compare to the current instance.</param>
		public bool Equals(TypeToken obj)
		{
			return this.tokValue == obj.tokValue;
		}

		/// <summary>Generates the hash code for this type.</summary>
		/// <returns>Returns the hash code for this type.</returns>
		public override int GetHashCode()
		{
			return this.tokValue;
		}

		/// <summary>Retrieves the metadata token for this class.</summary>
		/// <returns>Read-only. Retrieves the metadata token of this type.</returns>
		public int Token
		{
			get
			{
				return this.tokValue;
			}
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.TypeToken" /> structures are equal.</summary>
		/// <returns>true if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.TypeToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.TypeToken" /> to compare to <paramref name="a" />.</param>
		public static bool operator ==(TypeToken a, TypeToken b)
		{
			return object.Equals(a, b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Reflection.Emit.TypeToken" /> structures are not equal.</summary>
		/// <returns>true if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Reflection.Emit.TypeToken" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Reflection.Emit.TypeToken" /> to compare to <paramref name="a" />.</param>
		public static bool operator !=(TypeToken a, TypeToken b)
		{
			return !object.Equals(a, b);
		}
	}
}
