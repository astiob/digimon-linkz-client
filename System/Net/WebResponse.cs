using System;
using System.IO;
using System.Runtime.Serialization;

namespace System.Net
{
	/// <summary>Provides a response from a Uniform Resource Identifier (URI). This is an abstract class.</summary>
	[Serializable]
	public abstract class WebResponse : MarshalByRefObject, IDisposable, ISerializable
	{
		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebResponse" /> class.</summary>
		protected WebResponse()
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Net.WebResponse" /> class from the specified instances of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> and <see cref="T:System.Runtime.Serialization.StreamingContext" /> classes.</summary>
		/// <param name="serializationInfo">An instance of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class that contains the information required to serialize the new <see cref="T:System.Net.WebRequest" /> instance. </param>
		/// <param name="streamingContext">An instance of the <see cref="T:System.Runtime.Serialization.StreamingContext" /> class that indicates the source of the serialized stream that is associated with the new <see cref="T:System.Net.WebRequest" /> instance. </param>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the constructor, when the constructor is not overridden in a descendant class. </exception>
		protected WebResponse(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotSupportedException();
		}

		/// <summary>When overridden in a derived class, releases all resources used by the <see cref="T:System.Net.WebResponse" />. </summary>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception>
		void IDisposable.Dispose()
		{
			this.Close();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> instance with the data that is needed to serialize <see cref="T:System.Net.WebResponse" />.  </summary>
		/// <param name="serializationInfo">A <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that will hold the serialized data for the <see cref="T:System.Net.WebResponse" />. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains the destination of the serialized stream that is associated with the new <see cref="T:System.Net.WebResponse" />. </param>
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw new NotSupportedException();
		}

		/// <summary>When overridden in a descendant class, gets or sets the content length of data being received.</summary>
		/// <returns>The number of bytes returned from the Internet resource.</returns>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual long ContentLength
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>When overridden in a derived class, gets or sets the content type of the data being received.</summary>
		/// <returns>A string that contains the content type of the response.</returns>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual string ContentType
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>When overridden in a derived class, gets a collection of header name-value pairs associated with this request.</summary>
		/// <returns>An instance of the <see cref="T:System.Net.WebHeaderCollection" /> class that contains header values associated with this response.</returns>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual WebHeaderCollection Headers
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		private static Exception GetMustImplement()
		{
			return new NotImplementedException();
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether this response was obtained from the cache.</summary>
		/// <returns>true if the response was taken from the cache; otherwise, false.</returns>
		[MonoTODO]
		public virtual bool IsFromCache
		{
			get
			{
				throw WebResponse.GetMustImplement();
			}
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value that indicates whether mutual authentication occurred.</summary>
		/// <returns>true if both client and server were authenticated; otherwise, false.</returns>
		[MonoTODO]
		public virtual bool IsMutuallyAuthenticated
		{
			get
			{
				throw WebResponse.GetMustImplement();
			}
		}

		/// <summary>When overridden in a derived class, gets the URI of the Internet resource that actually responded to the request.</summary>
		/// <returns>An instance of the <see cref="T:System.Uri" /> class that contains the URI of the Internet resource that actually responded to the request.</returns>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to get or set the property, when the property is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual System.Uri ResponseUri
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>When overridden by a descendant class, closes the response stream.</summary>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual void Close()
		{
			throw new NotSupportedException();
		}

		/// <summary>When overridden in a descendant class, returns the data stream from the Internet resource.</summary>
		/// <returns>An instance of the <see cref="T:System.IO.Stream" /> class for reading data from the Internet resource.</returns>
		/// <exception cref="T:System.NotSupportedException">Any attempt is made to access the method, when the method is not overridden in a descendant class. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public virtual Stream GetResponseStream()
		{
			throw new NotSupportedException();
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data that is needed to serialize the target object.</summary>
		/// <param name="serializationInfo">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> to populate with data. </param>
		/// <param name="streamingContext">A <see cref="T:System.Runtime.Serialization.StreamingContext" /> that specifies the destination for this serialization. </param>
		[MonoTODO]
		protected virtual void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			throw WebResponse.GetMustImplement();
		}
	}
}
