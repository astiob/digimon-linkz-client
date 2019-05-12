using System;

namespace System.Security
{
	/// <summary>Specifies that code or an assembly performs security-critical operations.</summary>
	[MonoTODO("Only supported by the runtime when CoreCLR is enabled")]
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, AllowMultiple = false, Inherited = false)]
	public sealed class SecurityCriticalAttribute : Attribute
	{
		private SecurityCriticalScope _scope;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityCriticalAttribute" /> class with default scope. </summary>
		public SecurityCriticalAttribute()
		{
			this._scope = SecurityCriticalScope.Explicit;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.SecurityCriticalAttribute" /> class with the specified scope. </summary>
		/// <param name="scope">One of the <see cref="T:System.Security.SecurityCriticalScope" /> values that specifies the scope of the attribute. </param>
		public SecurityCriticalAttribute(SecurityCriticalScope scope)
		{
			if (scope != SecurityCriticalScope.Everything)
			{
				this._scope = SecurityCriticalScope.Explicit;
			}
			else
			{
				this._scope = SecurityCriticalScope.Everything;
			}
		}

		/// <summary>Gets the scope for the attribute.</summary>
		/// <returns>One of the <see cref="T:System.Security.SecurityCriticalScope" /> values that specifies the scope of the attribute. The default is <see cref="F:System.Security.SecurityCriticalScope.Explicit" />.</returns>
		public SecurityCriticalScope Scope
		{
			get
			{
				return this._scope;
			}
		}
	}
}
