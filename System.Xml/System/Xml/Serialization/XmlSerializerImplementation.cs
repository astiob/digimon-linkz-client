using System;
using System.Collections;

namespace System.Xml.Serialization
{
	/// <summary>Defines the reader, writer, and methods for pre-generated, typed serializers.</summary>
	public abstract class XmlSerializerImplementation
	{
		/// <summary>Gets the XML reader object that is used by the serializer.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlSerializationReader" /> that is used to read an XML document or data stream.</returns>
		public virtual XmlSerializationReader Reader
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets the collection of methods that is used to read a data stream.</summary>
		/// <returns>A <see cref="T:System.Collections.Hashtable" /> that contains the methods.</returns>
		public virtual Hashtable ReadMethods
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets the collection of typed serializers that is found in the assembly.</summary>
		/// <returns>A <see cref="T:System.Collections.Hashtable" /> that contains the typed serializers.</returns>
		public virtual Hashtable TypedSerializers
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Get the collection of methods that is used to write to a data stream.</summary>
		/// <returns>A <see cref="T:System.Collections.Hashtable" /> that contains the methods.</returns>
		public virtual Hashtable WriteMethods
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets the XML writer object for the serializer.</summary>
		/// <returns>An <see cref="T:System.Xml.Serialization.XmlSerializationWriter" /> that is used to write to an XML data stream or document.</returns>
		public virtual XmlSerializationWriter Writer
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		/// <summary>Gets a value that determines whether a type can be serialized.</summary>
		/// <returns>true if the type can be serialized; otherwise, false.</returns>
		/// <param name="type">The <see cref="T:System.Type" /> to be serialized.</param>
		public virtual bool CanSerialize(Type type)
		{
			throw new NotSupportedException();
		}

		/// <summary>Returns a serializer for the specified type.</summary>
		/// <returns>An instance of a type derived from the <see cref="T:System.Xml.Serialization.XmlSerializer" /> class. </returns>
		/// <param name="type">The <see cref="T:System.Type" /> to be serialized.</param>
		public virtual XmlSerializer GetSerializer(Type type)
		{
			throw new NotSupportedException();
		}
	}
}
