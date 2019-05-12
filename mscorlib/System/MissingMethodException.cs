using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when there is an attempt to dynamically access a method that does not exist.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class MissingMethodException : MissingMemberException
	{
		private const int Result = -2146233069;

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMethodException" /> class.</summary>
		public MissingMethodException() : base(Locale.GetText("Cannot find the requested method."))
		{
			base.HResult = -2146233069;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMethodException" /> class with a specified error message.</summary>
		/// <param name="message">A <see cref="T:System.String" /> that describes the error. </param>
		public MissingMethodException(string message) : base(message)
		{
			base.HResult = -2146233069;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMethodException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		protected MissingMethodException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMethodException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not a null reference (Nothing in Visual Basic), the current exception is raised in a catch block that handles the inner exception. </param>
		public MissingMethodException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233069;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.MissingMethodException" /> class with the specified class name and method name.</summary>
		/// <param name="className">The name of the class in which access to a nonexistent method was attempted. </param>
		/// <param name="methodName">The name of the method that cannot be accessed. </param>
		public MissingMethodException(string className, string methodName) : base(className, methodName)
		{
			base.HResult = -2146233069;
		}

		/// <summary>Gets the text string showing the class name, the method name, and the signature of the missing method. This property is read-only.</summary>
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
				string text = Locale.GetText("Method not found: '{0}.{1}'.");
				return string.Format(text, this.ClassName, this.MemberName);
			}
		}
	}
}
