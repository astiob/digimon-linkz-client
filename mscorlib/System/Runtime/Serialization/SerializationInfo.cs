using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Stores all the data needed to serialize or deserialize an object. This class cannot be inherited.</summary>
	[ComVisible(true)]
	public sealed class SerializationInfo
	{
		private Hashtable serialized = new Hashtable();

		private ArrayList values = new ArrayList();

		private string assemblyName;

		private string fullTypeName;

		private IFormatterConverter converter;

		private SerializationInfo(Type type)
		{
			this.assemblyName = type.Assembly.FullName;
			this.fullTypeName = type.FullName;
			this.converter = new FormatterConverter();
		}

		private SerializationInfo(Type type, SerializationEntry[] data)
		{
			int num = data.Length;
			this.assemblyName = type.Assembly.FullName;
			this.fullTypeName = type.FullName;
			this.converter = new FormatterConverter();
			for (int i = 0; i < num; i++)
			{
				this.serialized.Add(data[i].Name, data[i]);
				this.values.Add(data[i]);
			}
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> class.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to serialize. </param>
		/// <param name="converter">The <see cref="T:System.Runtime.Serialization.IFormatterConverter" /> used during deserialization. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="type" /> or <paramref name="converter" /> is null. </exception>
		[CLSCompliant(false)]
		public SerializationInfo(Type type, IFormatterConverter converter)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type", "Null argument");
			}
			if (converter == null)
			{
				throw new ArgumentNullException("converter", "Null argument");
			}
			this.converter = converter;
			this.assemblyName = type.Assembly.FullName;
			this.fullTypeName = type.FullName;
		}

		/// <summary>Gets or sets the assembly name of the type to serialize during serialization only.</summary>
		/// <returns>The full name of the assembly of the type to serialize.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value the property is set to is null. </exception>
		public string AssemblyName
		{
			get
			{
				return this.assemblyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Argument is null.");
				}
				this.assemblyName = value;
			}
		}

		/// <summary>Gets or sets the full name of the <see cref="T:System.Type" /> to serialize.</summary>
		/// <returns>The full name of the type to serialize.</returns>
		/// <exception cref="T:System.ArgumentNullException">The value this property is set to is null. </exception>
		public string FullTypeName
		{
			get
			{
				return this.fullTypeName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Argument is null.");
				}
				this.fullTypeName = value;
			}
		}

		/// <summary>Gets the number of members that have been added to the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The number of members that have been added to the current <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</returns>
		public int MemberCount
		{
			get
			{
				return this.serialized.Count;
			}
		}

		/// <summary>Adds a value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store, where <paramref name="value" /> is associated with <paramref name="name" /> and is serialized as being of <see cref="T:System.Type" /><paramref name="type" />.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The value to be serialized. Any children of this object will automatically be serialized. </param>
		/// <param name="type">The <see cref="T:System.Type" /> to associate with the current object. This parameter must always be the type of the object itself or of one of its base classes. </param>
		/// <exception cref="T:System.ArgumentNullException">If <paramref name="name" /> or <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, object value, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name is null");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type is null");
			}
			if (this.serialized.ContainsKey(name))
			{
				throw new SerializationException("Value has been serialized already.");
			}
			SerializationEntry serializationEntry = new SerializationEntry(name, type, value);
			this.serialized.Add(name, serializationEntry);
			this.values.Add(serializationEntry);
		}

		/// <summary>Retrieves a value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The object of the specified <see cref="T:System.Type" /> associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <param name="type">The <see cref="T:System.Type" /> of the value to retrieve. If the stored value cannot be converted to this type, the system will throw a <see cref="T:System.InvalidCastException" />. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> or <paramref name="type" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to <paramref name="type" />. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public object GetValue(string name, Type type)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name is null.");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (!this.serialized.ContainsKey(name))
			{
				throw new SerializationException("No element named " + name + " could be found.");
			}
			SerializationEntry serializationEntry = (SerializationEntry)this.serialized[name];
			if (serializationEntry.Value != null && !type.IsInstanceOfType(serializationEntry.Value))
			{
				return this.converter.Convert(serializationEntry.Value, type);
			}
			return serializationEntry.Value;
		}

		/// <summary>Sets the <see cref="T:System.Type" /> of the object to serialize.</summary>
		/// <param name="type">The <see cref="T:System.Type" /> of the object to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="type" /> parameter is null. </exception>
		public void SetType(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type is null.");
			}
			this.fullTypeName = type.FullName;
			this.assemblyName = type.Assembly.FullName;
		}

		/// <summary>Returns a <see cref="T:System.Runtime.Serialization.SerializationInfoEnumerator" /> used to iterate through the name-value pairs in the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>A <see cref="T:System.Runtime.Serialization.SerializationInfoEnumerator" /> for parsing the name-value pairs contained in the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</returns>
		public SerializationInfoEnumerator GetEnumerator()
		{
			return new SerializationInfoEnumerator(this.values);
		}

		/// <summary>Adds a 16-bit signed integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The Int16 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, short value)
		{
			this.AddValue(name, value, typeof(short));
		}

		/// <summary>Adds a 16-bit unsigned integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The UInt16 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		[CLSCompliant(false)]
		public void AddValue(string name, ushort value)
		{
			this.AddValue(name, value, typeof(ushort));
		}

		/// <summary>Adds a 32-bit signed integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The Int32 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, int value)
		{
			this.AddValue(name, value, typeof(int));
		}

		/// <summary>Adds an 8-bit unsigned integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The byte value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, byte value)
		{
			this.AddValue(name, value, typeof(byte));
		}

		/// <summary>Adds a Boolean value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The Boolean value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, bool value)
		{
			this.AddValue(name, value, typeof(bool));
		}

		/// <summary>Adds a Unicode character value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The character value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, char value)
		{
			this.AddValue(name, value, typeof(char));
		}

		/// <summary>Adds an 8-bit signed integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The Sbyte value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		[CLSCompliant(false)]
		public void AddValue(string name, sbyte value)
		{
			this.AddValue(name, value, typeof(sbyte));
		}

		/// <summary>Adds a double-precision floating-point value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The double value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, double value)
		{
			this.AddValue(name, value, typeof(double));
		}

		/// <summary>Adds a decimal value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The decimal value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">If The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">If a value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, decimal value)
		{
			this.AddValue(name, value, typeof(decimal));
		}

		/// <summary>Adds a <see cref="T:System.DateTime" /> value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The <see cref="T:System.DateTime" /> value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, DateTime value)
		{
			this.AddValue(name, value, typeof(DateTime));
		}

		/// <summary>Adds a single-precision floating-point value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The single value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, float value)
		{
			this.AddValue(name, value, typeof(float));
		}

		/// <summary>Adds a 32-bit unsigned integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The UInt32 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		[CLSCompliant(false)]
		public void AddValue(string name, uint value)
		{
			this.AddValue(name, value, typeof(uint));
		}

		/// <summary>Adds a 64-bit signed integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The Int64 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, long value)
		{
			this.AddValue(name, value, typeof(long));
		}

		/// <summary>Adds a 64-bit unsigned integer value into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The UInt64 value to serialize. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="name" /> parameter is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		[CLSCompliant(false)]
		public void AddValue(string name, ulong value)
		{
			this.AddValue(name, value, typeof(ulong));
		}

		/// <summary>Adds the specified object into the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store, where it is associated with a specified name.</summary>
		/// <param name="name">The name to associate with the value, so it can be deserialized later. </param>
		/// <param name="value">The value to be serialized. Any children of this object will automatically be serialized. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">A value has already been associated with <paramref name="name" />. </exception>
		public void AddValue(string name, object value)
		{
			if (value == null)
			{
				this.AddValue(name, value, typeof(object));
			}
			else
			{
				this.AddValue(name, value, value.GetType());
			}
		}

		/// <summary>Retrieves a Boolean value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The Boolean value associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a Boolean value. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public bool GetBoolean(string name)
		{
			object value = this.GetValue(name, typeof(bool));
			return this.converter.ToBoolean(value);
		}

		/// <summary>Retrieves an 8-bit unsigned integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 8-bit unsigned integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to an 8-bit unsigned integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public byte GetByte(string name)
		{
			object value = this.GetValue(name, typeof(byte));
			return this.converter.ToByte(value);
		}

		/// <summary>Retrieves a Unicode character value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The Unicode character associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a Unicode character. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public char GetChar(string name)
		{
			object value = this.GetValue(name, typeof(char));
			return this.converter.ToChar(value);
		}

		/// <summary>Retrieves a <see cref="T:System.DateTime" /> value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The <see cref="T:System.DateTime" /> value associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.  </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a <see cref="T:System.DateTime" /> value. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public DateTime GetDateTime(string name)
		{
			object value = this.GetValue(name, typeof(DateTime));
			return this.converter.ToDateTime(value);
		}

		/// <summary>Retrieves a decimal value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>A decimal value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.  </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a decimal. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public decimal GetDecimal(string name)
		{
			object value = this.GetValue(name, typeof(decimal));
			return this.converter.ToDecimal(value);
		}

		/// <summary>Retrieves a double-precision floating-point value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The double-precision floating-point value associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a double-precision floating-point value. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public double GetDouble(string name)
		{
			object value = this.GetValue(name, typeof(double));
			return this.converter.ToDouble(value);
		}

		/// <summary>Retrieves a 16-bit signed integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 16-bit signed integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 16-bit signed integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public short GetInt16(string name)
		{
			object value = this.GetValue(name, typeof(short));
			return this.converter.ToInt16(value);
		}

		/// <summary>Retrieves a 32-bit signed integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 32-bit signed integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name of the value to retrieve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 32-bit signed integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public int GetInt32(string name)
		{
			object value = this.GetValue(name, typeof(int));
			return this.converter.ToInt32(value);
		}

		/// <summary>Retrieves a 64-bit signed integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 64-bit signed integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 64-bit signed integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public long GetInt64(string name)
		{
			object value = this.GetValue(name, typeof(long));
			return this.converter.ToInt64(value);
		}

		/// <summary>Retrieves an 8-bit signed integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 8-bit signed integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to an 8-bit signed integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		[CLSCompliant(false)]
		public sbyte GetSByte(string name)
		{
			object value = this.GetValue(name, typeof(sbyte));
			return this.converter.ToSByte(value);
		}

		/// <summary>Retrieves a single-precision floating-point value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The single-precision floating-point value associated with <paramref name="name" />.</returns>
		/// <param name="name">The name of the value to retrieve. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a single-precision floating-point value. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public float GetSingle(string name)
		{
			object value = this.GetValue(name, typeof(float));
			return this.converter.ToSingle(value);
		}

		/// <summary>Retrieves a <see cref="T:System.String" /> value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The <see cref="T:System.String" /> associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a <see cref="T:System.String" />. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		public string GetString(string name)
		{
			object value = this.GetValue(name, typeof(string));
			if (value == null)
			{
				return null;
			}
			return this.converter.ToString(value);
		}

		/// <summary>Retrieves a 16-bit unsigned integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 16-bit unsigned integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 16-bit unsigned integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		[CLSCompliant(false)]
		public ushort GetUInt16(string name)
		{
			object value = this.GetValue(name, typeof(ushort));
			return this.converter.ToUInt16(value);
		}

		/// <summary>Retrieves a 32-bit unsigned integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 32-bit unsigned integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 32-bit unsigned integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		[CLSCompliant(false)]
		public uint GetUInt32(string name)
		{
			object value = this.GetValue(name, typeof(uint));
			return this.converter.ToUInt32(value);
		}

		/// <summary>Retrieves a 64-bit unsigned integer value from the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> store.</summary>
		/// <returns>The 64-bit unsigned integer associated with <paramref name="name" />.</returns>
		/// <param name="name">The name associated with the value to retrieve.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="name" /> is null. </exception>
		/// <exception cref="T:System.InvalidCastException">The value associated with <paramref name="name" /> cannot be converted to a 64-bit unsigned integer. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">An element with the specified name is not found in the current instance. </exception>
		[CLSCompliant(false)]
		public ulong GetUInt64(string name)
		{
			object value = this.GetValue(name, typeof(ulong));
			return this.converter.ToUInt64(value);
		}

		private SerializationEntry[] get_entries()
		{
			SerializationEntry[] array = new SerializationEntry[this.MemberCount];
			int num = 0;
			foreach (SerializationEntry serializationEntry in this)
			{
				array[num++] = serializationEntry;
			}
			return array;
		}
	}
}
