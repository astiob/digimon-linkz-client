using System;

namespace System
{
	/// <summary>Represents an object whose underlying type is a value type that can also be assigned null like a reference type.</summary>
	/// <typeparam name="T">The underlying value type of the <see cref="T:System.Nullable`1" /> generic type.</typeparam>
	/// <filterpriority>1</filterpriority>
	[Serializable]
	public struct Nullable<T> where T : struct
	{
		internal T value;

		internal bool has_value;

		/// <summary>Initializes a new instance of the <see cref="T:System.Nullable`1" /> structure to the specified value. </summary>
		/// <param name="value">A value type.</param>
		public Nullable(T value)
		{
			this.has_value = true;
			this.value = value;
		}

		/// <summary>Gets a value indicating whether the current <see cref="T:System.Nullable`1" /> object has a value.</summary>
		/// <returns>true if the current <see cref="T:System.Nullable`1" /> object has a value; false if the current <see cref="T:System.Nullable`1" /> object has no value.</returns>
		public bool HasValue
		{
			get
			{
				return this.has_value;
			}
		}

		/// <summary>Gets the value of the current <see cref="T:System.Nullable`1" /> value.</summary>
		/// <returns>The value of the current <see cref="T:System.Nullable`1" /> object if the <see cref="P:System.Nullable`1.HasValue" /> property is true. An exception is thrown if the <see cref="P:System.Nullable`1.HasValue" /> property is false.</returns>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="P:System.Nullable`1.HasValue" /> property is false.</exception>
		public T Value
		{
			get
			{
				if (!this.has_value)
				{
					throw new InvalidOperationException("Nullable object must have a value.");
				}
				return this.value;
			}
		}

		/// <summary>Indicates whether the current <see cref="T:System.Nullable`1" /> object is equal to a specified object.</summary>
		/// <returns>true if the <paramref name="other" /> parameter is equal to the current <see cref="T:System.Nullable`1" /> object; otherwise, false. This table describes how equality is defined for the compared values: Return ValueDescriptiontrueThe <see cref="P:System.Nullable`1.HasValue" /> property is false, and the <paramref name="other" /> parameter is null. That is, two null values are equal by definition.-or-The <see cref="P:System.Nullable`1.HasValue" /> property is true, and the value returned by the <see cref="P:System.Nullable`1.Value" /> property is equal to the <paramref name="other" /> parameter.falseThe <see cref="P:System.Nullable`1.HasValue" /> property for the current <see cref="T:System.Nullable`1" /> structure is true, and the <paramref name="other" /> parameter is null.-or-The <see cref="P:System.Nullable`1.HasValue" /> property for the current <see cref="T:System.Nullable`1" /> structure is false, and the <paramref name="other" /> parameter is not null.-or-The <see cref="P:System.Nullable`1.HasValue" /> property for the current <see cref="T:System.Nullable`1" /> structure is true, and the value returned by the <see cref="P:System.Nullable`1.Value" /> property is not equal to the <paramref name="other" /> parameter.</returns>
		/// <param name="other">An object.</param>
		/// <filterpriority>1</filterpriority>
		public override bool Equals(object other)
		{
			if (other == null)
			{
				return !this.has_value;
			}
			return other is T? && this.Equals((T?)other);
		}

		private bool Equals(T? other)
		{
			return other.has_value == this.has_value && (!this.has_value || other.value.Equals(this.value));
		}

		/// <summary>Retrieves the hash code of the object returned by the <see cref="P:System.Nullable`1.Value" /> property.</summary>
		/// <returns>The hash code of the object returned by the <see cref="P:System.Nullable`1.Value" /> property if the <see cref="P:System.Nullable`1.HasValue" /> property is true, or zero if the <see cref="P:System.Nullable`1.HasValue" /> property is false. </returns>
		/// <filterpriority>1</filterpriority>
		public override int GetHashCode()
		{
			if (!this.has_value)
			{
				return 0;
			}
			return this.value.GetHashCode();
		}

		/// <summary>Retrieves the value of the current <see cref="T:System.Nullable`1" /> object, or the object's default value.</summary>
		/// <returns>The value of the <see cref="P:System.Nullable`1.Value" /> property if the  <see cref="P:System.Nullable`1.HasValue" /> property is true; otherwise, the default value of the current <see cref="T:System.Nullable`1" /> object. The type of the default value is the type argument of the current <see cref="T:System.Nullable`1" /> object, and the value of the default value consists solely of binary zeroes.</returns>
		public T GetValueOrDefault()
		{
			return (!this.has_value) ? default(T) : this.value;
		}

		/// <summary>Retrieves the value of the current <see cref="T:System.Nullable`1" /> object, or the specified default value.</summary>
		/// <returns>The value of the <see cref="P:System.Nullable`1.Value" /> property if the <see cref="P:System.Nullable`1.HasValue" /> property is true; otherwise, the <paramref name="defaultValue" /> parameter.</returns>
		/// <param name="defaultValue">A value to return if the <see cref="P:System.Nullable`1.HasValue" /> property is false.</param>
		public T GetValueOrDefault(T defaultValue)
		{
			return (!this.has_value) ? defaultValue : this.value;
		}

		/// <summary>Returns the text representation of the value of the current <see cref="T:System.Nullable`1" /> object.</summary>
		/// <returns>The text representation of the value of the current <see cref="T:System.Nullable`1" /> object if the <see cref="P:System.Nullable`1.HasValue" /> property is true, or an empty string ("") if the <see cref="P:System.Nullable`1.HasValue" /> property is false.</returns>
		/// <filterpriority>1</filterpriority>
		public override string ToString()
		{
			if (this.has_value)
			{
				return this.value.ToString();
			}
			return string.Empty;
		}

		private static object Box(T? o)
		{
			if (!o.has_value)
			{
				return null;
			}
			return o.value;
		}

		private static T? Unbox(object o)
		{
			if (o == null)
			{
				return null;
			}
			return new T?((T)((object)o));
		}

		/// <summary>Creates a new <see cref="T:System.Nullable`1" /> object initialized to a specified value. </summary>
		/// <returns>A <see cref="T:System.Nullable`1" /> object whose <see cref="P:System.Nullable`1.Value" /> property is initialized with the <paramref name="value" /> parameter.</returns>
		/// <param name="value">A value type.</param>
		public static implicit operator T?(T value)
		{
			return new T?(value);
		}

		/// <summary>Returns the value of a specified <see cref="T:System.Nullable`1" /> value.</summary>
		/// <returns>The value of the <see cref="P:System.Nullable`1.Value" /> property for the <paramref name="value" /> parameter.</returns>
		/// <param name="value">A <see cref="T:System.Nullable`1" /> value.</param>
		public static explicit operator T(T? value)
		{
			return value.Value;
		}
	}
}
