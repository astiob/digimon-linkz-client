using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Provides base functionality for the common language runtime serialization formatters.</summary>
	[CLSCompliant(false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class Formatter : IFormatter
	{
		/// <summary>Contains the <see cref="T:System.Runtime.Serialization.ObjectIDGenerator" /> used with the current formatter.</summary>
		protected ObjectIDGenerator m_idGenerator = new ObjectIDGenerator();

		/// <summary>Contains a <see cref="T:System.Collections.Queue" /> of the objects left to serialize.</summary>
		protected Queue m_objectQueue = new Queue();

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.SerializationBinder" /> used with the current formatter.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.SerializationBinder" /> used with the current formatter.</returns>
		public abstract SerializationBinder Binder { get; set; }

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.StreamingContext" /> used for the current serialization.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.StreamingContext" /> used for the current serialization.</returns>
		public abstract StreamingContext Context { get; set; }

		/// <summary>When overridden in a derived class, gets or sets the <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> used with the current formatter.</summary>
		/// <returns>The <see cref="T:System.Runtime.Serialization.ISurrogateSelector" /> used with the current formatter.</returns>
		public abstract ISurrogateSelector SurrogateSelector { get; set; }

		/// <summary>When overridden in a derived class, deserializes the stream attached to the formatter when it was created, creating a graph of objects identical to the graph originally serialized into that stream.</summary>
		/// <returns>The top object of the deserialized graph of objects.</returns>
		/// <param name="serializationStream">The stream to deserialize. </param>
		public abstract object Deserialize(Stream serializationStream);

		/// <summary>Returns the next object to serialize, from the formatter's internal work queue.</summary>
		/// <returns>The next object to serialize.</returns>
		/// <param name="objID">The ID assigned to the current object during serialization. </param>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">The next object retrieved from the work queue did not have an assigned ID. </exception>
		protected virtual object GetNext(out long objID)
		{
			if (this.m_objectQueue.Count == 0)
			{
				objID = 0L;
				return null;
			}
			object obj = this.m_objectQueue.Dequeue();
			bool flag;
			objID = this.m_idGenerator.HasId(obj, out flag);
			return obj;
		}

		/// <summary>Schedules an object for later serialization.</summary>
		/// <returns>The object ID assigned to the object.</returns>
		/// <param name="obj">The object to schedule for serialization. </param>
		protected virtual long Schedule(object obj)
		{
			if (obj == null)
			{
				return 0L;
			}
			bool flag;
			long id = this.m_idGenerator.GetId(obj, out flag);
			if (flag)
			{
				this.m_objectQueue.Enqueue(obj);
			}
			return id;
		}

		/// <summary>When overridden in a derived class, serializes the graph of objects with the specified root to the stream already attached to the formatter.</summary>
		/// <param name="serializationStream">The stream to which the objects are serialized. </param>
		/// <param name="graph">The object at the root of the graph to serialize. </param>
		public abstract void Serialize(Stream serializationStream, object graph);

		/// <summary>When overridden in a derived class, writes an array to the stream already attached to the formatter.</summary>
		/// <param name="obj">The array to write. </param>
		/// <param name="name">The name of the array. </param>
		/// <param name="memberType">The type of elements that the array holds. </param>
		protected abstract void WriteArray(object obj, string name, Type memberType);

		/// <summary>When overridden in a derived class, writes a Boolean value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteBoolean(bool val, string name);

		/// <summary>When overridden in a derived class, writes an 8-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteByte(byte val, string name);

		/// <summary>When overridden in a derived class, writes a Unicode character to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteChar(char val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.DateTime" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteDateTime(DateTime val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.Decimal" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteDecimal(decimal val, string name);

		/// <summary>When overridden in a derived class, writes a double-precision floating-point number to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteDouble(double val, string name);

		/// <summary>When overridden in a derived class, writes a 16-bit signed integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteInt16(short val, string name);

		/// <summary>When overridden in a derived class, writes a 32-bit signed integer to the stream.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteInt32(int val, string name);

		/// <summary>When overridden in a derived class, writes a 64-bit signed integer to the stream.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteInt64(long val, string name);

		/// <summary>Inspects the type of data received, and calls the appropriate Write method to perform the write to the stream already attached to the formatter.</summary>
		/// <param name="memberName">The name of the member to serialize. </param>
		/// <param name="data">The object to write to the stream attached to the formatter. </param>
		protected virtual void WriteMember(string memberName, object data)
		{
			if (data == null)
			{
				this.WriteObjectRef(data, memberName, typeof(object));
			}
			Type type = data.GetType();
			if (type.IsArray)
			{
				this.WriteArray(data, memberName, type);
			}
			else if (type == typeof(bool))
			{
				this.WriteBoolean((bool)data, memberName);
			}
			else if (type == typeof(byte))
			{
				this.WriteByte((byte)data, memberName);
			}
			else if (type == typeof(char))
			{
				this.WriteChar((char)data, memberName);
			}
			else if (type == typeof(DateTime))
			{
				this.WriteDateTime((DateTime)data, memberName);
			}
			else if (type == typeof(decimal))
			{
				this.WriteDecimal((decimal)data, memberName);
			}
			else if (type == typeof(double))
			{
				this.WriteDouble((double)data, memberName);
			}
			else if (type == typeof(short))
			{
				this.WriteInt16((short)data, memberName);
			}
			else if (type == typeof(int))
			{
				this.WriteInt32((int)data, memberName);
			}
			else if (type == typeof(long))
			{
				this.WriteInt64((long)data, memberName);
			}
			else if (type == typeof(sbyte))
			{
				this.WriteSByte((sbyte)data, memberName);
			}
			else if (type == typeof(float))
			{
				this.WriteSingle((float)data, memberName);
			}
			else if (type == typeof(TimeSpan))
			{
				this.WriteTimeSpan((TimeSpan)data, memberName);
			}
			else if (type == typeof(ushort))
			{
				this.WriteUInt16((ushort)data, memberName);
			}
			else if (type == typeof(uint))
			{
				this.WriteUInt32((uint)data, memberName);
			}
			else if (type == typeof(ulong))
			{
				this.WriteUInt64((ulong)data, memberName);
			}
			else if (type.IsValueType)
			{
				this.WriteValueType(data, memberName, type);
			}
			this.WriteObjectRef(data, memberName, type);
		}

		/// <summary>When overridden in a derived class, writes an object reference to the stream already attached to the formatter.</summary>
		/// <param name="obj">The object reference to write. </param>
		/// <param name="name">The name of the member. </param>
		/// <param name="memberType">The type of object the reference points to. </param>
		protected abstract void WriteObjectRef(object obj, string name, Type memberType);

		/// <summary>When overridden in a derived class, writes an 8-bit signed integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		[CLSCompliant(false)]
		protected abstract void WriteSByte(sbyte val, string name);

		/// <summary>When overridden in a derived class, writes a single-precision floating-point number to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteSingle(float val, string name);

		/// <summary>When overridden in a derived class, writes a <see cref="T:System.TimeSpan" /> value to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		protected abstract void WriteTimeSpan(TimeSpan val, string name);

		/// <summary>When overridden in a derived class, writes a 16-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		[CLSCompliant(false)]
		protected abstract void WriteUInt16(ushort val, string name);

		/// <summary>When overridden in a derived class, writes a 32-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		[CLSCompliant(false)]
		protected abstract void WriteUInt32(uint val, string name);

		/// <summary>When overridden in a derived class, writes a 64-bit unsigned integer to the stream already attached to the formatter.</summary>
		/// <param name="val">The value to write. </param>
		/// <param name="name">The name of the member. </param>
		[CLSCompliant(false)]
		protected abstract void WriteUInt64(ulong val, string name);

		/// <summary>When overridden in a derived class, writes a value of the given type to the stream already attached to the formatter.</summary>
		/// <param name="obj">The object representing the value type. </param>
		/// <param name="name">The name of the member. </param>
		/// <param name="memberType">The <see cref="T:System.Type" /> of the value type. </param>
		protected abstract void WriteValueType(object obj, string name, Type memberType);
	}
}
