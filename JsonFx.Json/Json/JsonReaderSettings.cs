using System;

namespace JsonFx.Json
{
	public class JsonReaderSettings
	{
		public TypeCoercionUtility Coercion = new TypeCoercionUtility();

		private bool allowUnquotedObjectKeys;

		private string typeHintName;

		public bool AllowNullValueTypes
		{
			get
			{
				return this.Coercion.AllowNullValueTypes;
			}
			set
			{
				this.Coercion.AllowNullValueTypes = value;
			}
		}

		public bool AllowUnquotedObjectKeys
		{
			get
			{
				return this.allowUnquotedObjectKeys;
			}
			set
			{
				this.allowUnquotedObjectKeys = value;
			}
		}

		public string TypeHintName
		{
			get
			{
				return this.typeHintName;
			}
			set
			{
				this.typeHintName = value;
			}
		}

		public bool IsTypeHintName(string name)
		{
			return !string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(this.typeHintName) && StringComparer.Ordinal.Equals(this.typeHintName, name);
		}
	}
}
