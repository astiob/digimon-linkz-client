using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace System.Security
{
	internal struct SecurityFrame
	{
		private AppDomain _domain;

		private MethodInfo _method;

		private PermissionSet _assert;

		private PermissionSet _deny;

		private PermissionSet _permitonly;

		internal SecurityFrame(RuntimeSecurityFrame frame)
		{
			this._domain = null;
			this._method = null;
			this._assert = null;
			this._deny = null;
			this._permitonly = null;
			this.InitFromRuntimeFrame(frame);
		}

		internal SecurityFrame(int skip)
		{
			this._domain = null;
			this._method = null;
			this._assert = null;
			this._deny = null;
			this._permitonly = null;
			this.InitFromRuntimeFrame(SecurityFrame._GetSecurityFrame(skip + 2));
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RuntimeSecurityFrame _GetSecurityFrame(int skip);

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Array _GetSecurityStack(int skip);

		internal void InitFromRuntimeFrame(RuntimeSecurityFrame frame)
		{
			this._domain = frame.domain;
			this._method = frame.method;
			if (frame.assert.size > 0)
			{
				this._assert = SecurityManager.Decode(frame.assert.blob, frame.assert.size);
			}
			if (frame.deny.size > 0)
			{
				this._deny = SecurityManager.Decode(frame.deny.blob, frame.deny.size);
			}
			if (frame.permitonly.size > 0)
			{
				this._permitonly = SecurityManager.Decode(frame.permitonly.blob, frame.permitonly.size);
			}
		}

		public Assembly Assembly
		{
			get
			{
				return this._method.ReflectedType.Assembly;
			}
		}

		public AppDomain Domain
		{
			get
			{
				return this._domain;
			}
		}

		public MethodInfo Method
		{
			get
			{
				return this._method;
			}
		}

		public PermissionSet Assert
		{
			get
			{
				return this._assert;
			}
		}

		public PermissionSet Deny
		{
			get
			{
				return this._deny;
			}
		}

		public PermissionSet PermitOnly
		{
			get
			{
				return this._permitonly;
			}
		}

		public bool HasStackModifiers
		{
			get
			{
				return this._assert != null || this._deny != null || this._permitonly != null;
			}
		}

		public bool Equals(SecurityFrame sf)
		{
			return object.ReferenceEquals(this._domain, sf.Domain) && !(this.Assembly.ToString() != sf.Assembly.ToString()) && !(this.Method.ToString() != sf.Method.ToString()) && (this._assert == null || this._assert.Equals(sf.Assert)) && (this._deny == null || this._deny.Equals(sf.Deny)) && (this._permitonly == null || this._permitonly.Equals(sf.PermitOnly));
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Frame: {0}{1}", this._method, Environment.NewLine);
			stringBuilder.AppendFormat("\tAppDomain: {0}{1}", this.Domain, Environment.NewLine);
			stringBuilder.AppendFormat("\tAssembly: {0}{1}", this.Assembly, Environment.NewLine);
			if (this._assert != null)
			{
				stringBuilder.AppendFormat("\tAssert: {0}{1}", this._assert, Environment.NewLine);
			}
			if (this._deny != null)
			{
				stringBuilder.AppendFormat("\tDeny: {0}{1}", this._deny, Environment.NewLine);
			}
			if (this._permitonly != null)
			{
				stringBuilder.AppendFormat("\tPermitOnly: {0}{1}", this._permitonly, Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		public static ArrayList GetStack(int skipFrames)
		{
			Array array = SecurityFrame._GetSecurityStack(skipFrames + 2);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < array.Length; i++)
			{
				object value = array.GetValue(i);
				if (value == null)
				{
					break;
				}
				arrayList.Add(new SecurityFrame((RuntimeSecurityFrame)value));
			}
			return arrayList;
		}
	}
}
