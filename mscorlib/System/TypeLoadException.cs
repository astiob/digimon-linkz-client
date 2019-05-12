using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>The exception that is thrown when type-loading failures occur.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class TypeLoadException : SystemException
	{
		private const int Result = -2146233054;

		private string className;

		private string assemblyName;

		/// <summary>Initializes a new instance of the <see cref="T:System.TypeLoadException" /> class.</summary>
		public TypeLoadException() : base(Locale.GetText("A type load exception has occurred."))
		{
			base.HResult = -2146233054;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TypeLoadException" /> class with a specified error message.</summary>
		/// <param name="message">The message that describes the error. </param>
		public TypeLoadException(string message) : base(message)
		{
			base.HResult = -2146233054;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TypeLoadException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public TypeLoadException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233054;
		}

		internal TypeLoadException(string className, string assemblyName) : this()
		{
			this.className = className;
			this.assemblyName = assemblyName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.TypeLoadException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> object is null. </exception>
		protected TypeLoadException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			this.className = info.GetString("TypeLoadClassName");
			this.assemblyName = info.GetString("TypeLoadAssemblyName");
		}

		/// <summary>Gets the error message for this exception.</summary>
		/// <returns>The error message string.</returns>
		/// <filterpriority>2</filterpriority>
		public override string Message
		{
			get
			{
				if (this.className == null)
				{
					return base.Message;
				}
				if (this.assemblyName != null && this.assemblyName != string.Empty)
				{
					return string.Format("Could not load type '{0}' from assembly '{1}'.", this.className, this.assemblyName);
				}
				return string.Format("Could not load type '{0}'.", this.className);
			}
		}

		/// <summary>Gets the fully qualified name of the type that causes the exception.</summary>
		/// <returns>The fully qualified type name.</returns>
		/// <filterpriority>2</filterpriority>
		public string TypeName
		{
			get
			{
				if (this.className == null)
				{
					return string.Empty;
				}
				return this.className;
			}
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the class name, method name, resource ID, and additional exception information.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> object is null. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			base.GetObjectData(info, context);
			info.AddValue("TypeLoadClassName", this.className, typeof(string));
			info.AddValue("TypeLoadAssemblyName", this.assemblyName, typeof(string));
			info.AddValue("TypeLoadMessageArg", string.Empty, typeof(string));
			info.AddValue("TypeLoadResourceID", 0, typeof(int));
		}
	}
}
