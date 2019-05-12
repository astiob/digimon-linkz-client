using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when there is an attempt to dynamically access a field that does not exist.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class MissingFieldException : MissingMemberException
	{
		private const int Result = -2146233071;

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingFieldException" /> class.</summary>
		public MissingFieldException() : base(Locale.GetText("Cannot find requested field."))
		{
			base.HResult = -2146233071;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingFieldException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. </param>
		public MissingFieldException(string message) : base(message)
		{
			base.HResult = -2146233071;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingFieldException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected MissingFieldException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingFieldException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public MissingFieldException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233071;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingFieldException" /> class with the specified class name and field name.</summary>
		/// <param name="className">The name of the class in which access to a nonexistent field was attempted. </param>
		/// <param name="fieldName">The name of the field that cannot be accessed. </param>
		public MissingFieldException(string className, string fieldName) : base(className, fieldName)
		{
			base.HResult = -2146233071;
		}

		/// <summary>Gets the text string showing the signature of the missing field, the class name, and the field name. This property is read-only.</summary>
		/// <returns>The error message string.</returns>
		/// <filterpriority>2</filterpriority>
		public override string Message
		{
			get
			{
				if (this.ClassName == null)
				{
					return base.Message;
				}
				string text = Locale.GetText("Field '{0}.{1}' not found.");
				return string.Format(text, this.ClassName, this.MemberName);
			}
		}
	}
}
