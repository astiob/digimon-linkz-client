using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata;

namespace System.Runtime.Serialization.Formatters
{
	/// <summary>Carries error and status information within a SOAP message. This class cannot be inherited.</summary>
	[SoapType]
	[ComVisible(true)]
	[Serializable]
	public sealed class SoapFault : ISerializable
	{
		private string code;

		private string actor;

		private string faultString;

		private object detail;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" /> class with default values.</summary>
		public SoapFault()
		{
		}

		private SoapFault(SerializationInfo info, StreamingContext context)
		{
			this.code = info.GetString("faultcode");
			this.faultString = info.GetString("faultstring");
			this.detail = info.GetValue("detail", typeof(object));
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" /> class, setting the properties to specified values.</summary>
		/// <param name="faultCode">The fault code for the new instance of <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />. The fault code identifies the type of the fault that occurred. </param>
		/// <param name="faultString">The fault string for the new instance of <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />. The fault string provides a human readable explanation of the fault. </param>
		/// <param name="faultActor">The URI of the object that generated the fault. </param>
		/// <param name="serverFault">The description of a common language runtime exception. This information is also present in the <see cref="P:System.Runtime.Serialization.Formatters.SoapFault.Detail" /> property. </param>
		public SoapFault(string faultCode, string faultString, string faultActor, ServerFault serverFault)
		{
			this.code = faultCode;
			this.actor = faultActor;
			this.faultString = faultString;
			this.detail = serverFault;
		}

		/// <summary>Gets or sets additional information required for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</summary>
		/// <returns>Additional information required for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</returns>
		public object Detail
		{
			get
			{
				return this.detail;
			}
			set
			{
				this.detail = value;
			}
		}

		/// <summary>Gets or sets the fault actor for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</summary>
		/// <returns>The fault actor for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</returns>
		public string FaultActor
		{
			get
			{
				return this.actor;
			}
			set
			{
				this.actor = value;
			}
		}

		/// <summary>Gets or sets the fault code for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</summary>
		/// <returns>The fault code for this <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</returns>
		public string FaultCode
		{
			get
			{
				return this.code;
			}
			set
			{
				this.code = value;
			}
		}

		/// <summary>Gets or sets the fault message for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</summary>
		/// <returns>The fault message for the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" />.</returns>
		public string FaultString
		{
			get
			{
				return this.faultString;
			}
			set
			{
				this.faultString = value;
			}
		}

		/// <summary>Populates the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data to serialize the <see cref="T:System.Runtime.Serialization.Formatters.SoapFault" /> object.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext" />) for the current serialization. </param>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("faultcode", this.code, typeof(string));
			info.AddValue("faultstring", this.faultString, typeof(string));
			info.AddValue("detail", this.detail, typeof(object));
		}
	}
}
