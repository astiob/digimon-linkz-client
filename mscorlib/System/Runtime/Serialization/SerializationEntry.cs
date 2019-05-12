using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	/// <summary>Holds the value, <see cref="T:System.Type" />, and name of a serialized object. </summary>
	[ComVisible(true)]
	public struct SerializationEntry
	{
		private string name;

		private Type objectType;

		private object value;

		internal SerializationEntry(string name, Type type, object value)
		{
			this.name = name;
			this.objectType = type;
			this.value = value;
		}

		/// <summary>Gets the name of the object.</summary>
		/// <returns>The name of the object.</returns>
		public string Name
		{
			get
			{
				return this.name;
			}
		}

		/// <summary>Gets the <see cref="T:System.Type" /> of the object.</summary>
		/// <returns>The <see cref="T:System.Type" /> of the object.</returns>
		public Type ObjectType
		{
			get
			{
				return this.objectType;
			}
		}

		/// <summary>Gets the value contained in the object.</summary>
		/// <returns>The value contained in the object.</returns>
		public object Value
		{
			get
			{
				return this.value;
			}
		}
	}
}
