using System;
using System.Runtime.Serialization;

namespace System.ComponentModel
{
	/// <summary>Represents the exception thrown when a component cannot be granted a license.</summary>
	[Serializable]
	public class LicenseException : SystemException
	{
		private Type type;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LicenseException" /> class for the type of component that was denied a license. </summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of component that was not granted a license. </param>
		public LicenseException(Type type) : this(type, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LicenseException" /> class for the type and the instance of the component that was denied a license.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of component that was not granted a license. </param>
		/// <param name="instance">The instance of the component that was not granted a license. </param>
		public LicenseException(Type type, object instance)
		{
			this.type = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LicenseException" /> class for the type and the instance of the component that was denied a license, along with a message to display.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of component that was not granted a license. </param>
		/// <param name="instance">The instance of the component that was not granted a license. </param>
		/// <param name="message">The exception message to display. </param>
		public LicenseException(Type type, object instance, string message) : this(type, instance, message, null)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LicenseException" /> class for the type and the instance of the component that was denied a license, along with a message to display and the original exception thrown.</summary>
		/// <param name="type">A <see cref="T:System.Type" /> that represents the type of component that was not granted a license. </param>
		/// <param name="instance">The instance of the component that was not granted a license. </param>
		/// <param name="message">The exception message to display. </param>
		/// <param name="innerException">An <see cref="T:System.Exception" /> that represents the original exception. </param>
		public LicenseException(Type type, object instance, string message, Exception innerException) : base(message, innerException)
		{
			this.type = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.LicenseException" /> class with the given <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to be used for deserialization.</param>
		/// <param name="context">The destination to be used for deserialization.</param>
		protected LicenseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			this.type = (Type)info.GetValue("LicensedType", typeof(Type));
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to be used for deserialization.</param>
		/// <param name="context">The destination to be used for deserialization.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null.</exception>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			info.AddValue("LicensedType", this.type);
			base.GetObjectData(info, context);
		}

		/// <summary>Gets the type of the component that was not granted a license.</summary>
		/// <returns>A <see cref="T:System.Type" /> that represents the type of component that was not granted a license.</returns>
		public Type LicensedType
		{
			get
			{
				return this.type;
			}
		}
	}
}
