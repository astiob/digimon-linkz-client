using System;
using System.Reflection;

namespace System.Xml.Serialization
{
	internal class XmlTypeMapMember
	{
		private const int OPTIONAL = 1;

		private const int RETURN_VALUE = 2;

		private const int IGNORE = 4;

		private string _name;

		private int _index;

		private int _globalIndex;

		private TypeData _typeData;

		private MemberInfo _member;

		private MemberInfo _specifiedMember;

		private object _defaultValue = DBNull.Value;

		private string documentation;

		private int _flags;

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}

		public object DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
			set
			{
				this._defaultValue = value;
			}
		}

		public string Documentation
		{
			get
			{
				return this.documentation;
			}
			set
			{
				this.documentation = value;
			}
		}

		public bool IsReadOnly(Type type)
		{
			if (this._member == null)
			{
				this.InitMember(type);
			}
			return this._member is PropertyInfo && !((PropertyInfo)this._member).CanWrite;
		}

		public static object GetValue(object ob, string name)
		{
			MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
			if (member[0] is PropertyInfo)
			{
				return ((PropertyInfo)member[0]).GetValue(ob, null);
			}
			return ((FieldInfo)member[0]).GetValue(ob);
		}

		public object GetValue(object ob)
		{
			if (this._member == null)
			{
				this.InitMember(ob.GetType());
			}
			if (this._member is PropertyInfo)
			{
				return ((PropertyInfo)this._member).GetValue(ob, null);
			}
			return ((FieldInfo)this._member).GetValue(ob);
		}

		public void SetValue(object ob, object value)
		{
			if (this._member == null)
			{
				this.InitMember(ob.GetType());
			}
			if (this._member is PropertyInfo)
			{
				((PropertyInfo)this._member).SetValue(ob, value, null);
			}
			else
			{
				((FieldInfo)this._member).SetValue(ob, value);
			}
		}

		public static void SetValue(object ob, string name, object value)
		{
			MemberInfo[] member = ob.GetType().GetMember(name, BindingFlags.Instance | BindingFlags.Public);
			if (member[0] is PropertyInfo)
			{
				((PropertyInfo)member[0]).SetValue(ob, value, null);
			}
			else
			{
				((FieldInfo)member[0]).SetValue(ob, value);
			}
		}

		private void InitMember(Type type)
		{
			MemberInfo[] member = type.GetMember(this._name, BindingFlags.Instance | BindingFlags.Public);
			this._member = member[0];
			member = type.GetMember(this._name + "Specified", BindingFlags.Instance | BindingFlags.Public);
			if (member.Length > 0)
			{
				this._specifiedMember = member[0];
			}
			if (this._specifiedMember is PropertyInfo && !((PropertyInfo)this._specifiedMember).CanWrite)
			{
				this._specifiedMember = null;
			}
		}

		public TypeData TypeData
		{
			get
			{
				return this._typeData;
			}
			set
			{
				this._typeData = value;
			}
		}

		public int Index
		{
			get
			{
				return this._index;
			}
			set
			{
				this._index = value;
			}
		}

		public int GlobalIndex
		{
			get
			{
				return this._globalIndex;
			}
			set
			{
				this._globalIndex = value;
			}
		}

		public bool IsOptionalValueType
		{
			get
			{
				return (this._flags & 1) != 0;
			}
			set
			{
				this._flags = ((!value) ? (this._flags & -2) : (this._flags | 1));
			}
		}

		public bool IsReturnValue
		{
			get
			{
				return (this._flags & 2) != 0;
			}
			set
			{
				this._flags = ((!value) ? (this._flags & -3) : (this._flags | 2));
			}
		}

		public bool Ignore
		{
			get
			{
				return (this._flags & 4) != 0;
			}
			set
			{
				this._flags = ((!value) ? (this._flags & -5) : (this._flags | 4));
			}
		}

		public void CheckOptionalValueType(Type type)
		{
			if (this._member == null)
			{
				this.InitMember(type);
			}
			this.IsOptionalValueType = (this._specifiedMember != null);
		}

		public bool GetValueSpecified(object ob)
		{
			if (this._specifiedMember is PropertyInfo)
			{
				return (bool)((PropertyInfo)this._specifiedMember).GetValue(ob, null);
			}
			return (bool)((FieldInfo)this._specifiedMember).GetValue(ob);
		}

		public void SetValueSpecified(object ob, bool value)
		{
			if (this._specifiedMember is PropertyInfo)
			{
				((PropertyInfo)this._specifiedMember).SetValue(ob, value, null);
			}
			else
			{
				((FieldInfo)this._specifiedMember).SetValue(ob, value);
			}
		}

		public virtual bool RequiresNullable
		{
			get
			{
				return false;
			}
		}
	}
}
