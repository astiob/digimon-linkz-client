using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;

namespace System.Runtime.Serialization.Formatters.Binary
{
	/// <summary>Serializes and deserializes an object, or an entire graph of connected objects, in binary format.</summary>
	[ComVisible(true)]
	public sealed class BinaryFormatter : IRemotingFormatter, IFormatter
	{
		private FormatterAssemblyStyle assembly_format;

		private SerializationBinder binder;

		private StreamingContext context;

		private ISurrogateSelector surrogate_selector;

		private FormatterTypeStyle type_format = FormatterTypeStyle.TypesAlways;

		private TypeFilterLevel filter_level = TypeFilterLevel.Full;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> class with default values.</summary>
		public BinaryFormatter()
		{
			this.surrogate_selector = BinaryFormatter.DefaultSurrogateSelector;
			this.context = new StreamingContext(StreamingContextStates.All);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> class with a given surrogate selector and streaming context.</summary>
		/// <param name="selector">The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> to use. Can be null. </param>
		/// <param name="context">The source and destination for the serialized data. </param>
		public BinaryFormatter(ISurrogateSelector selector, StreamingContext context)
		{
			this.surrogate_selector = selector;
			this.context = context;
		}

		public static ISurrogateSelector DefaultSurrogateSelector { get; set; }

		/// <summary>Gets or sets the behavior of the deserializer with regards to finding and loading assemblies.</summary>
		/// <returns>One of the <see cref="T:System.Runtime.Serialization.Formatters.FormatterAssemblyStyle" /> values that specifies the deserializer behavior.</returns>
		public FormatterAssemblyStyle AssemblyFormat
		{
			get
			{
				return this.assembly_format;
			}
			set
			{
				this.assembly_format = value;
			}
		}

		/// <summary>Gets or sets an object of type <see cref="T:System.Runtime.Serialization.SerializationBinder" /> that controls the binding of a serialized object to a type.</summary>
		/// <returns>The serialization binder to use with this formatter.</returns>
		public SerializationBinder Binder
		{
			get
			{
				return this.binder;
			}
			set
			{
				this.binder = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext" /> for this formatter.</summary>
		/// <returns>The streaming context to use with this formatter.</returns>
		public StreamingContext Context
		{
			get
			{
				return this.context;
			}
			set
			{
				this.context = value;
			}
		}

		/// <summary>Gets or sets a <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> that controls type substitution during serialization and deserialization.</summary>
		/// <returns>The surrogate selector to use with this formatter.</returns>
		public ISurrogateSelector SurrogateSelector
		{
			get
			{
				return this.surrogate_selector;
			}
			set
			{
				this.surrogate_selector = value;
			}
		}

		/// <summary>Gets or sets the format in which type descriptions are laid out in the serialized stream.</summary>
		/// <returns>The style of type layouts to use.</returns>
		public FormatterTypeStyle TypeFormat
		{
			get
			{
				return this.type_format;
			}
			set
			{
				this.type_format = value;
			}
		}

		/// <summary>Gets or sets the <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel" /> of automatic deserialization the <see cref="T:System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" /> performs.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.Formatters.TypeFilterLevel" /> that represents the current automatic deserialization level.</returns>
		public TypeFilterLevel FilterLevel
		{
			get
			{
				return this.filter_level;
			}
			set
			{
				this.filter_level = value;
			}
		}

		/// <summary>Deserializes the specified stream into an object graph.</summary>
		/// <returns>The top (root) of the object graph.</returns>
		/// <param name="serializationStream">The stream from which to deserialize the object graph. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public object Deserialize(Stream serializationStream)
		{
			return this.NoCheckDeserialize(serializationStream, null);
		}

		/// <summary>Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> handles any headers in that stream.</summary>
		/// <returns>The deserialized object or the top object (root) of the object graph.</returns>
		/// <param name="serializationStream">The stream from which to deserialize the object graph. </param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public object Deserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.NoCheckDeserialize(serializationStream, handler);
		}

		private object NoCheckDeserialize(Stream serializationStream, HeaderHandler handler)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream");
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException("serializationStream supports seeking, but its length is 0");
			}
			BinaryReader binaryReader = new BinaryReader(serializationStream);
			bool flag;
			this.ReadBinaryHeader(binaryReader, out flag);
			BinaryElement binaryElement = (BinaryElement)binaryReader.Read();
			if (binaryElement == BinaryElement.MethodCall)
			{
				return MessageFormatter.ReadMethodCall(binaryElement, binaryReader, flag, handler, this);
			}
			if (binaryElement == BinaryElement.MethodResponse)
			{
				return MessageFormatter.ReadMethodResponse(binaryElement, binaryReader, flag, handler, null, this);
			}
			ObjectReader objectReader = new ObjectReader(this);
			object result;
			Header[] headers;
			objectReader.ReadObjectGraph(binaryElement, binaryReader, flag, out result, out headers);
			if (handler != null)
			{
				handler(headers);
			}
			return result;
		}

		/// <summary>Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream" />.</summary>
		/// <returns>The deserialized response to the remote method call.</returns>
		/// <param name="serializationStream">The stream from which to deserialize the object graph. </param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be null. </param>
		/// <param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> that contains details about where the call came from. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public object DeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.NoCheckDeserializeMethodResponse(serializationStream, handler, methodCallMessage);
		}

		private object NoCheckDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream");
			}
			if (serializationStream.CanSeek && serializationStream.Length == 0L)
			{
				throw new SerializationException("serializationStream supports seeking, but its length is 0");
			}
			BinaryReader reader = new BinaryReader(serializationStream);
			bool hasHeaders;
			this.ReadBinaryHeader(reader, out hasHeaders);
			return MessageFormatter.ReadMethodResponse(reader, hasHeaders, handler, methodCallMessage, this);
		}

		/// <summary>Serializes the object, or graph of objects with the specified top (root), to the given stream.</summary>
		/// <param name="serializationStream">The stream to which the graph is to be serialized. </param>
		/// <param name="graph">The object at the root of the graph to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. -or-The <paramref name="graph" /> is null.</exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph" /> parameter is not marked as serializable. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public void Serialize(Stream serializationStream, object graph)
		{
			this.Serialize(serializationStream, graph, null);
		}

		/// <summary>Serializes the object, or graph of objects with the specified top (root), to the given stream attaching the provided headers.</summary>
		/// <param name="serializationStream">The stream to which the object is to be serialized. </param>
		/// <param name="graph">The object at the root of the graph to serialize. </param>
		/// <param name="headers">Remoting headers to include in the serialization. Can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An error has occurred during serialization, such as if an object in the <paramref name="graph" /> parameter is not marked as serializable. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		/// </PermissionSet>
		public void Serialize(Stream serializationStream, object graph, Header[] headers)
		{
			if (serializationStream == null)
			{
				throw new ArgumentNullException("serializationStream");
			}
			BinaryWriter binaryWriter = new BinaryWriter(serializationStream);
			this.WriteBinaryHeader(binaryWriter, headers != null);
			if (graph is IMethodCallMessage)
			{
				MessageFormatter.WriteMethodCall(binaryWriter, graph, headers, this.surrogate_selector, this.context, this.assembly_format, this.type_format);
			}
			else if (graph is IMethodReturnMessage)
			{
				MessageFormatter.WriteMethodResponse(binaryWriter, graph, headers, this.surrogate_selector, this.context, this.assembly_format, this.type_format);
			}
			else
			{
				ObjectWriter objectWriter = new ObjectWriter(this.surrogate_selector, this.context, this.assembly_format, this.type_format);
				objectWriter.WriteObjectGraph(binaryWriter, graph, headers);
			}
			binaryWriter.Flush();
		}

		/// <summary>Deserializes the specified stream into an object graph. The provided <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> handles any headers in that stream.</summary>
		/// <returns>The deserialized object or the top object (root) of the object graph.</returns>
		/// <param name="serializationStream">The stream from which to deserialize the object graph. </param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be null. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, SerializationFormatter" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object UnsafeDeserialize(Stream serializationStream, HeaderHandler handler)
		{
			return this.NoCheckDeserialize(serializationStream, handler);
		}

		/// <summary>Deserializes a response to a remote method call from the provided <see cref="T:System.IO.Stream" />.</summary>
		/// <returns>The deserialized response to the remote method call.</returns>
		/// <param name="serializationStream">The stream from which to deserialize the object graph. </param>
		/// <param name="handler">The <see cref="T:System.Runtime.Remoting.Messaging.HeaderHandler" /> that handles any headers in the <paramref name="serializationStream" />. Can be null. </param>
		/// <param name="methodCallMessage">The <see cref="T:System.Runtime.Remoting.Messaging.IMethodCallMessage" /> that contains details about where the call came from. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="serializationStream" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The <paramref name="serializationStream" /> supports seeking, but its length is 0. </exception>
		/// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, SerializationFormatter" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object UnsafeDeserializeMethodResponse(Stream serializationStream, HeaderHandler handler, IMethodCallMessage methodCallMessage)
		{
			return this.NoCheckDeserializeMethodResponse(serializationStream, handler, methodCallMessage);
		}

		private void WriteBinaryHeader(BinaryWriter writer, bool hasHeaders)
		{
			writer.Write(0);
			writer.Write(1);
			if (hasHeaders)
			{
				writer.Write(2);
			}
			else
			{
				writer.Write(-1);
			}
			writer.Write(1);
			writer.Write(0);
		}

		private void ReadBinaryHeader(BinaryReader reader, out bool hasHeaders)
		{
			reader.ReadByte();
			reader.ReadInt32();
			int num = reader.ReadInt32();
			hasHeaders = (num == 2);
			reader.ReadInt32();
			reader.ReadInt32();
		}
	}
}
