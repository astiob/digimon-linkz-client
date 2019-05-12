using System;

namespace System.Xml.Serialization
{
	internal sealed class TypeMember
	{
		private Type type;

		private string member;

		internal TypeMember(Type type, string member)
		{
			this.type = type;
			this.member = member;
		}

		public override int GetHashCode()
		{
			return this.type.GetHashCode() + this.member.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj is TypeMember && TypeMember.Equals(this, (TypeMember)obj);
		}

		public static bool Equals(TypeMember tm1, TypeMember tm2)
		{
			return object.ReferenceEquals(tm1, tm2) || (!object.ReferenceEquals(tm1, null) && !object.ReferenceEquals(tm2, null) && (tm1.type == tm2.type && tm1.member == tm2.member));
		}

		public override string ToString()
		{
			return this.type.ToString() + " " + this.member;
		}

		public static bool operator ==(TypeMember tm1, TypeMember tm2)
		{
			return TypeMember.Equals(tm1, tm2);
		}

		public static bool operator !=(TypeMember tm1, TypeMember tm2)
		{
			return !TypeMember.Equals(tm1, tm2);
		}
	}
}
