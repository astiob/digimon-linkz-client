using System;
using UnityEngine.Scripting;

namespace UnityEngine
{
	[UsedByNativeCode]
	public struct PropertyName
	{
		internal int id;

		public PropertyName(string name)
		{
			this = new PropertyName(PropertyNameUtils.PropertyNameFromString(name));
		}

		public PropertyName(PropertyName other)
		{
			this.id = other.id;
		}

		public PropertyName(int id)
		{
			this.id = id;
		}

		public static bool IsNullOrEmpty(PropertyName prop)
		{
			return prop.id == 0;
		}

		public static bool operator ==(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id == rhs.id;
		}

		public static bool operator !=(PropertyName lhs, PropertyName rhs)
		{
			return lhs.id != rhs.id;
		}

		public override int GetHashCode()
		{
			return this.id;
		}

		public override bool Equals(object other)
		{
			return other is PropertyName && this == (PropertyName)other;
		}

		public static implicit operator PropertyName(string name)
		{
			return new PropertyName(name);
		}

		public static implicit operator PropertyName(int id)
		{
			return new PropertyName(id);
		}

		public override string ToString()
		{
			return string.Format("Unknown:{0}", this.id);
		}
	}
}
