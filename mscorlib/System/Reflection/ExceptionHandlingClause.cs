using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Represents a clause in a structured exception-handling block.</summary>
	[ComVisible(true)]
	public sealed class ExceptionHandlingClause
	{
		internal Type catch_type;

		internal int filter_offset;

		internal ExceptionHandlingClauseOptions flags;

		internal int try_offset;

		internal int try_length;

		internal int handler_offset;

		internal int handler_length;

		internal ExceptionHandlingClause()
		{
		}

		/// <summary>Gets the type of exception handled by this clause.</summary>
		/// <returns>A <see cref="T:System.Type" /> object that represents that type of exception handled by this clause, or null if the <see cref="P:System.Reflection.ExceptionHandlingClause.Flags" /> property is <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Filter" /> or <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Finally" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">Invalid use of property for the object's current state.</exception>
		public Type CatchType
		{
			get
			{
				return this.catch_type;
			}
		}

		/// <summary>Gets the offset within the method body, in bytes, of the user-supplied filter code.</summary>
		/// <returns>The offset within the method body, in bytes, of the user-supplied filter code. The value of this property has no meaning if the <see cref="P:System.Reflection.ExceptionHandlingClause.Flags" /> property has any value other than <see cref="F:System.Reflection.ExceptionHandlingClauseOptions.Filter" />.</returns>
		/// <exception cref="T:System.InvalidOperationException">Cannot get the offset because the exception handling clause is not a filter.</exception>
		public int FilterOffset
		{
			get
			{
				return this.filter_offset;
			}
		}

		/// <summary>Gets a value indicating whether this exception-handling clause is a finally clause, a type-filtered clause, or a user-filtered clause.</summary>
		/// <returns>An <see cref="T:System.Reflection.ExceptionHandlingClauseOptions" /> value that indicates what kind of action this clause performs.</returns>
		public ExceptionHandlingClauseOptions Flags
		{
			get
			{
				return this.flags;
			}
		}

		/// <summary>Gets the length, in bytes, of the body of this exception-handling clause.</summary>
		/// <returns>An integer that represents the length, in bytes, of the MSIL that forms the body of this exception-handling clause.</returns>
		public int HandlerLength
		{
			get
			{
				return this.handler_length;
			}
		}

		/// <summary>Gets the offset within the method body, in bytes, of this exception-handling clause.</summary>
		/// <returns>An integer that represents the offset within the method body, in bytes, of this exception-handling clause.</returns>
		public int HandlerOffset
		{
			get
			{
				return this.handler_offset;
			}
		}

		/// <summary>The total length, in bytes, of the try block that includes this exception-handling clause.</summary>
		/// <returns>The total length, in bytes, of the try block that includes this exception-handling clause.</returns>
		public int TryLength
		{
			get
			{
				return this.try_length;
			}
		}

		/// <summary>The offset within the method, in bytes, of the try block that includes this exception-handling clause.</summary>
		/// <returns>An integer that represents the offset within the method, in bytes, of the try block that includes this exception-handling clause.</returns>
		public int TryOffset
		{
			get
			{
				return this.try_offset;
			}
		}

		/// <summary>A string representation of the exception-handling clause.</summary>
		/// <returns>A string that lists appropriate property values for the filter clause type.</returns>
		public override string ToString()
		{
			string text = string.Format("Flags={0}, TryOffset={1}, TryLength={2}, HandlerOffset={3}, HandlerLength={4}", new object[]
			{
				this.flags,
				this.try_offset,
				this.try_length,
				this.handler_offset,
				this.handler_length
			});
			if (this.catch_type != null)
			{
				text = string.Format("{0}, CatchType={1}", text, this.catch_type);
			}
			if (this.flags == ExceptionHandlingClauseOptions.Filter)
			{
				text = string.Format(CultureInfo.InvariantCulture, "{0}, FilterOffset={1}", new object[]
				{
					text,
					this.filter_offset
				});
			}
			return text;
		}
	}
}
