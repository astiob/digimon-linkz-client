using System;
using UnityEngine.Serialization;

namespace UnityEngine.Events
{
	[Serializable]
	internal class ArgumentCache : ISerializationCallbackReceiver
	{
		[SerializeField]
		[FormerlySerializedAs("objectArgument")]
		private Object m_ObjectArgument;

		[SerializeField]
		[FormerlySerializedAs("objectArgumentAssemblyTypeName")]
		private string m_ObjectArgumentAssemblyTypeName;

		[SerializeField]
		[FormerlySerializedAs("intArgument")]
		private int m_IntArgument;

		[SerializeField]
		[FormerlySerializedAs("floatArgument")]
		private float m_FloatArgument;

		[SerializeField]
		[FormerlySerializedAs("stringArgument")]
		private string m_StringArgument;

		[SerializeField]
		private bool m_BoolArgument;

		public Object unityObjectArgument
		{
			get
			{
				return this.m_ObjectArgument;
			}
			set
			{
				this.m_ObjectArgument = value;
				this.m_ObjectArgumentAssemblyTypeName = ((!(value != null)) ? string.Empty : value.GetType().AssemblyQualifiedName);
			}
		}

		public string unityObjectArgumentAssemblyTypeName
		{
			get
			{
				return this.m_ObjectArgumentAssemblyTypeName;
			}
		}

		public int intArgument
		{
			get
			{
				return this.m_IntArgument;
			}
			set
			{
				this.m_IntArgument = value;
			}
		}

		public float floatArgument
		{
			get
			{
				return this.m_FloatArgument;
			}
			set
			{
				this.m_FloatArgument = value;
			}
		}

		public string stringArgument
		{
			get
			{
				return this.m_StringArgument;
			}
			set
			{
				this.m_StringArgument = value;
			}
		}

		public bool boolArgument
		{
			get
			{
				return this.m_BoolArgument;
			}
			set
			{
				this.m_BoolArgument = value;
			}
		}

		private void TidyAssemblyTypeName()
		{
			if (string.IsNullOrEmpty(this.m_ObjectArgumentAssemblyTypeName))
			{
				return;
			}
			int num = int.MaxValue;
			int num2 = this.m_ObjectArgumentAssemblyTypeName.IndexOf(", Version=");
			if (num2 != -1)
			{
				num = Math.Min(num2, num);
			}
			num2 = this.m_ObjectArgumentAssemblyTypeName.IndexOf(", Culture=");
			if (num2 != -1)
			{
				num = Math.Min(num2, num);
			}
			num2 = this.m_ObjectArgumentAssemblyTypeName.IndexOf(", PublicKeyToken=");
			if (num2 != -1)
			{
				num = Math.Min(num2, num);
			}
			if (num == 2147483647)
			{
				return;
			}
			this.m_ObjectArgumentAssemblyTypeName = this.m_ObjectArgumentAssemblyTypeName.Substring(0, num);
		}

		public void OnBeforeSerialize()
		{
			this.TidyAssemblyTypeName();
		}

		public void OnAfterDeserialize()
		{
			this.TidyAssemblyTypeName();
		}
	}
}
