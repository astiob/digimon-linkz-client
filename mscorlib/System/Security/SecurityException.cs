using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;

namespace System.Security
{
	/// <summary>The exception that is thrown when a security error is detected.</summary>
	[ComVisible(true)]
	[Serializable]
	public class SecurityException : SystemException
	{
		private string permissionState;

		private Type permissionType;

		private string _granted;

		private string _refused;

		private object _demanded;

		private IPermission _firstperm;

		private MethodInfo _method;

		private Evidence _evidence;

		private SecurityAction _action;

		private object _denyset;

		private object _permitset;

		private AssemblyName _assembly;

		private string _url;

		private SecurityZone _zone;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with default properties.</summary>
		public SecurityException() : base(Locale.GetText("A security error has been detected."))
		{
			base.HResult = -2146233078;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with a specified error message.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		public SecurityException(string message) : base(message)
		{
			base.HResult = -2146233078;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with serialized data.</summary>
		/// <param name="info">The object that holds the serialized object data. </param>
		/// <param name="context">The contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info " />is null.</exception>
		protected SecurityException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			base.HResult = -2146233078;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				if (enumerator.Name == "PermissionState")
				{
					this.permissionState = (string)enumerator.Value;
					break;
				}
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="inner">The exception that is the cause of the current exception. If the <paramref name="inner" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
		public SecurityException(string message, Exception inner) : base(message, inner)
		{
			base.HResult = -2146233078;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with a specified error message and the permission type that caused the exception to be thrown.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="type">The type of the permission that caused the exception to be thrown. </param>
		public SecurityException(string message, Type type) : base(message)
		{
			base.HResult = -2146233078;
			this.permissionType = type;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class with a specified error message, the permission type that caused the exception to be thrown, and the permission state.</summary>
		/// <param name="message">The error message that explains the reason for the exception. </param>
		/// <param name="type">The type of the permission that caused the exception to be thrown. </param>
		/// <param name="state">The state of the permission that caused the exception to be thrown. </param>
		public SecurityException(string message, Type type, string state) : base(message)
		{
			base.HResult = -2146233078;
			this.permissionType = type;
			this.permissionState = state;
		}

		internal SecurityException(string message, PermissionSet granted, PermissionSet refused) : base(message)
		{
			base.HResult = -2146233078;
			this._granted = granted.ToString();
			this._refused = refused.ToString();
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class for an exception caused by a Deny on the stack.  </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="deny">The denied permission or permission set.</param>
		/// <param name="permitOnly">The permit-only permission or permission set.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that identifies the method that encountered the exception.</param>
		/// <param name="demanded">The demanded permission, permission set, or permission set collection.</param>
		/// <param name="permThatFailed">An <see cref="T:System.Security.IPermission" /> that identifies the permission that failed.</param>
		public SecurityException(string message, object deny, object permitOnly, MethodInfo method, object demanded, IPermission permThatFailed) : base(message)
		{
			base.HResult = -2146233078;
			this._denyset = deny;
			this._permitset = permitOnly;
			this._method = method;
			this._demanded = demanded;
			this._firstperm = permThatFailed;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityException" /> class for an exception caused by an insufficient grant set. </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="assemblyName">An <see cref="T:System.Reflection.AssemblyName" /> that specifies the name of the assembly that caused the exception.</param>
		/// <param name="grant">A <see cref="T:System.Security.PermissionSet" /> that represents the permissions granted the assembly.</param>
		/// <param name="refused">A <see cref="T:System.Security.PermissionSet" /> that represents the refused permission or permission set.</param>
		/// <param name="method">A <see cref="T:System.Reflection.MethodInfo" /> that represents the method that encountered the exception.</param>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</param>
		/// <param name="demanded">The demanded permission, permission set, or permission set collection.</param>
		/// <param name="permThatFailed">An <see cref="T:System.Security.IPermission" /> that represents the permission that failed.</param>
		/// <param name="evidence">The <see cref="T:System.Security.Policy.Evidence" /> for the assembly that caused the exception.</param>
		public SecurityException(string message, AssemblyName assemblyName, PermissionSet grant, PermissionSet refused, MethodInfo method, SecurityAction action, object demanded, IPermission permThatFailed, Evidence evidence) : base(message)
		{
			base.HResult = -2146233078;
			this._assembly = assemblyName;
			this._granted = ((grant != null) ? grant.ToString() : string.Empty);
			this._refused = ((refused != null) ? refused.ToString() : string.Empty);
			this._method = method;
			this._action = action;
			this._demanded = demanded;
			this._firstperm = permThatFailed;
			if (this._firstperm != null)
			{
				this.permissionType = this._firstperm.GetType();
			}
			this._evidence = evidence;
		}

		/// <summary>Gets or sets the security action that caused the exception.</summary>
		/// <returns>One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values.</returns>
		[ComVisible(false)]
		public SecurityAction Action
		{
			get
			{
				return this._action;
			}
			set
			{
				this._action = value;
			}
		}

		/// <summary>Gets or sets the denied security permission, permission set, or permission set collection that caused a demand to fail.</summary>
		/// <returns>A permission, permission set, or permission set collection object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object DenySetInstance
		{
			get
			{
				return this._denyset;
			}
			set
			{
				this._denyset = value;
			}
		}

		/// <summary>Gets or sets information about the failed assembly.</summary>
		/// <returns>An <see cref="T:System.Reflection.AssemblyName" /> that identifies the failed assembly.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		public AssemblyName FailedAssemblyInfo
		{
			get
			{
				return this._assembly;
			}
			set
			{
				this._assembly = value;
			}
		}

		/// <summary>Gets or sets the information about the method associated with the exception.</summary>
		/// <returns>A <see cref="T:System.Reflection.MethodInfo" /> object describing the method.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		public MethodInfo Method
		{
			get
			{
				return this._method;
			}
			set
			{
				this._method = value;
			}
		}

		/// <summary>Gets or sets the permission, permission set, or permission set collection that is part of the permit-only stack frame that caused a security check to fail.</summary>
		/// <returns>A permission, permission set, or permission set collection object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object PermitOnlySetInstance
		{
			get
			{
				return this._permitset;
			}
			set
			{
				this._permitset = value;
			}
		}

		/// <summary>Gets or sets the URL of the assembly that caused the exception.</summary>
		/// <returns>A URL that identifies the location of the assembly.</returns>
		public string Url
		{
			get
			{
				return this._url;
			}
			set
			{
				this._url = value;
			}
		}

		/// <summary>Gets or sets the zone of the assembly that caused the exception.</summary>
		/// <returns>One of the <see cref="T:System.Security.SecurityZone" /> values that identifies the zone of the assembly that caused the exception.</returns>
		public SecurityZone Zone
		{
			get
			{
				return this._zone;
			}
			set
			{
				this._zone = value;
			}
		}

		/// <summary>Gets or sets the demanded security permission, permission set, or permission set collection that failed.</summary>
		/// <returns>A permission, permission set, or permission set collection object.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		public object Demanded
		{
			get
			{
				return this._demanded;
			}
			set
			{
				this._demanded = value;
			}
		}

		/// <summary>Gets or sets the first permission in a permission set or permission set collection that failed the demand.</summary>
		/// <returns>An <see cref="T:System.Security.IPermission" /> object representing the first permission that failed.</returns>
		public IPermission FirstPermissionThatFailed
		{
			get
			{
				return this._firstperm;
			}
			set
			{
				this._firstperm = value;
			}
		}

		/// <summary>Gets or sets the state of the permission that threw the exception.</summary>
		/// <returns>The state of the permission at the time the exception was thrown.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public string PermissionState
		{
			get
			{
				return this.permissionState;
			}
			set
			{
				this.permissionState = value;
			}
		}

		/// <summary>Gets or sets the type of the permission that failed.</summary>
		/// <returns>The type of the permission that failed.</returns>
		public Type PermissionType
		{
			get
			{
				return this.permissionType;
			}
			set
			{
				this.permissionType = value;
			}
		}

		/// <summary>Gets or sets the granted permission set of the assembly that caused the <see cref="T:System.Security.SecurityException" />.</summary>
		/// <returns>The XML representation of the granted set of the assembly.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public string GrantedSet
		{
			get
			{
				return this._granted;
			}
			set
			{
				this._granted = value;
			}
		}

		/// <summary>Gets or sets the refused permission set of the assembly that caused the <see cref="T:System.Security.SecurityException" />.</summary>
		/// <returns>The XML representation of the refused permission set of the assembly.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public string RefusedSet
		{
			get
			{
				return this._refused;
			}
			set
			{
				this._refused = value;
			}
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the <see cref="T:System.Security.SecurityException" />.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
		/// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is null. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			try
			{
				info.AddValue("PermissionState", this.PermissionState);
			}
			catch (SecurityException)
			{
			}
		}

		/// <summary>Returns a representation of the current <see cref="T:System.Security.SecurityException" />.</summary>
		/// <returns>A string representation of the current <see cref="T:System.Security.SecurityException" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
		///   <IPermission class="System.Security.Permissions.ReflectionPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="MemberAccess" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence, ControlPolicy" />
		/// </PermissionSet>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString());
			try
			{
				if (this.permissionType != null)
				{
					stringBuilder.AppendFormat("{0}Type: {1}", Environment.NewLine, this.PermissionType);
				}
				if (this._method != null)
				{
					string text = this._method.ToString();
					int startIndex = text.IndexOf(" ") + 1;
					stringBuilder.AppendFormat("{0}Method: {1} {2}.{3}", new object[]
					{
						Environment.NewLine,
						this._method.ReturnType.Name,
						this._method.ReflectedType,
						text.Substring(startIndex)
					});
				}
				if (this.permissionState != null)
				{
					stringBuilder.AppendFormat("{0}State: {1}", Environment.NewLine, this.PermissionState);
				}
				if (this._granted != null && this._granted.Length > 0)
				{
					stringBuilder.AppendFormat("{0}Granted: {1}", Environment.NewLine, this.GrantedSet);
				}
				if (this._refused != null && this._refused.Length > 0)
				{
					stringBuilder.AppendFormat("{0}Refused: {1}", Environment.NewLine, this.RefusedSet);
				}
				if (this._demanded != null)
				{
					stringBuilder.AppendFormat("{0}Demanded: {1}", Environment.NewLine, this.Demanded);
				}
				if (this._firstperm != null)
				{
					stringBuilder.AppendFormat("{0}Failed Permission: {1}", Environment.NewLine, this.FirstPermissionThatFailed);
				}
				if (this._evidence != null)
				{
					stringBuilder.AppendFormat("{0}Evidences:", Environment.NewLine);
					foreach (object obj in this._evidence)
					{
						if (!(obj is Hash))
						{
							stringBuilder.AppendFormat("{0}\t{1}", Environment.NewLine, obj);
						}
					}
				}
			}
			catch (SecurityException)
			{
			}
			return stringBuilder.ToString();
		}
	}
}
