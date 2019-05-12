using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.IO
{
	/// <summary>The exception that is thrown when an attempt to access a file that does not exist on disk fails.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[Serializable]
	public class FileNotFoundException : IOException
	{
		private const int Result = -2146232799;

		private string fileName;

		private string fusionLog;

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with its message string set to a system-supplied message and its HRESULT set to COR_E_FILENOTFOUND.</summary>
		public FileNotFoundException() : base(Locale.GetText("Unable to find the specified file."))
		{
			base.HResult = -2146232799;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with its message string set to <paramref name="message" /> and its HRESULT set to COR_E_FILENOTFOUND.</summary>
		/// <param name="message">A description of the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
		public FileNotFoundException(string message) : base(message)
		{
			base.HResult = -2146232799;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">A description of the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public FileNotFoundException(string message, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232799;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with its message string set to <paramref name="message" />, specifying the file name that cannot be found, and its HRESULT set to COR_E_FILENOTFOUND.</summary>
		/// <param name="message">A description of the error. The content of <paramref name="message" /> is intended to be understood by humans. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
		/// <param name="fileName">The full name of the file with the invalid image. </param>
		public FileNotFoundException(string message, string fileName) : base(message)
		{
			base.HResult = -2146232799;
			this.fileName = fileName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="fileName">The full name of the file with the invalid image. </param>
		/// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public FileNotFoundException(string message, string fileName, Exception innerException) : base(message, innerException)
		{
			base.HResult = -2146232799;
			this.fileName = fileName;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.IO.FileNotFoundException" /> class with the specified serialization and context information.</summary>
		/// <param name="info">An object that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">An object that contains contextual information about the source or destination. </param>
		protected FileNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.fileName = info.GetString("FileNotFound_FileName");
			this.fusionLog = info.GetString("FileNotFound_FusionLog");
		}

		/// <summary>Gets the name of the file that cannot be found.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the name of the file, or null if no file name was passed to the constructor for this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public string FileName
		{
			get
			{
				return this.fileName;
			}
		}

		/// <summary>Gets the log file that describes why loading of an assembly failed.</summary>
		/// <returns>A String containing errors reported by the assembly cache.</returns>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public string FusionLog
		{
			get
			{
				return this.fusionLog;
			}
		}

		/// <summary>Gets the error message that explains the reason for the exception.</summary>
		/// <returns>A string containing the error message.</returns>
		/// <filterpriority>2</filterpriority>
		public override string Message
		{
			get
			{
				if (this.message == null && this.fileName != null)
				{
					return string.Format(CultureInfo.CurrentCulture, "Could not load file or assembly '{0}' or one of its dependencies. The system cannot find the file specified.", new object[]
					{
						this.fileName
					});
				}
				return this.message;
			}
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the file name and additional exception information.</summary>
		/// <param name="info">The object that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The object that contains contextual information about the source or destination. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("FileNotFound_FileName", this.fileName);
			info.AddValue("FileNotFound_FusionLog", this.fusionLog);
		}

		/// <summary>Returns the fully qualified name of this exception and possibly the error message, the name of the inner exception, and the stack trace.</summary>
		/// <returns>A string containing the fully qualified name of this exception and possibly the error message, the name of the inner exception, and the stack trace.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(this.GetType().FullName);
			stringBuilder.AppendFormat(": {0}", this.Message);
			if (this.fileName != null && this.fileName.Length > 0)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.AppendFormat("File name: '{0}'", this.fileName);
			}
			if (this.InnerException != null)
			{
				stringBuilder.AppendFormat(" ---> {0}", this.InnerException);
			}
			if (this.StackTrace != null)
			{
				stringBuilder.Append(Environment.NewLine);
				stringBuilder.Append(this.StackTrace);
			}
			return stringBuilder.ToString();
		}
	}
}
