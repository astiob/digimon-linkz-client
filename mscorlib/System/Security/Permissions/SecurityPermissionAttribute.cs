using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	/// <summary>Allows security actions for <see cref="T:System.Security.Permissions.SecurityPermission" /> to be applied to code using declarative security. This class cannot be inherited.</summary>
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[Obsolete("CAS support is not available with Silverlight applications.")]
	[ComVisible(true)]
	[Serializable]
	public sealed class SecurityPermissionAttribute : CodeAccessSecurityAttribute
	{
		private SecurityPermissionFlag m_Flags;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Permissions.SecurityPermissionAttribute" /> class with the specified <see cref="T:System.Security.Permissions.SecurityAction" />.</summary>
		/// <param name="action">One of the <see cref="T:System.Security.Permissions.SecurityAction" /> values. </param>
		public SecurityPermissionAttribute(SecurityAction action) : base(action)
		{
			this.m_Flags = SecurityPermissionFlag.NoFlags;
		}

		/// <summary>Gets or sets a value indicating whether permission to assert that all this code's callers have the requisite permission for the operation is declared.</summary>
		/// <returns>true if permission to assert is declared; otherwise, false.</returns>
		public bool Assertion
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Assertion) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Assertion;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.Assertion;
				}
			}
		}

		/// <summary>Gets or sets a value that indicates whether code has permission to perform binding redirection in the application configuration file.</summary>
		/// <returns>true if code can perform binding redirects; otherwise, false.</returns>
		public bool BindingRedirects
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.BindingRedirects) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.BindingRedirects;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.BindingRedirects;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate <see cref="T:System.AppDomain" /> is declared.</summary>
		/// <returns>true if permission to manipulate <see cref="T:System.AppDomain" /> is declared; otherwise, false.</returns>
		public bool ControlAppDomain
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlAppDomain) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlAppDomain;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlAppDomain;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to alter or manipulate domain security policy is declared.</summary>
		/// <returns>true if permission to alter or manipulate security policy in an application domain is declared; otherwise, false.</returns>
		public bool ControlDomainPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlDomainPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlDomainPolicy;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlDomainPolicy;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to alter or manipulate evidence is declared.</summary>
		/// <returns>true if the ability to alter or manipulate evidence is declared; otherwise, false.</returns>
		public bool ControlEvidence
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlEvidence) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlEvidence;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlEvidence;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to view and manipulate security policy is declared.</summary>
		/// <returns>true if permission to manipulate security policy is declared; otherwise, false.</returns>
		public bool ControlPolicy
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPolicy) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPolicy;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlPolicy;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate the current principal is declared.</summary>
		/// <returns>true if permission to manipulate the current principal is declared; otherwise, false.</returns>
		public bool ControlPrincipal
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlPrincipal) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlPrincipal;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlPrincipal;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to manipulate threads is declared.</summary>
		/// <returns>true if permission to manipulate threads is declared; otherwise, false.</returns>
		public bool ControlThread
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.ControlThread) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.ControlThread;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.ControlThread;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to execute code is declared.</summary>
		/// <returns>true if permission to execute code is declared; otherwise, false.</returns>
		public bool Execution
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Execution) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Execution;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.Execution;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether code can plug into the common language runtime infrastructure, such as adding Remoting Context Sinks, Envoy Sinks and Dynamic Sinks.</summary>
		/// <returns>true if code can plug into the common language runtime infrastructure; otherwise, false.</returns>
		[ComVisible(true)]
		public bool Infrastructure
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.Infrastructure) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.Infrastructure;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.Infrastructure;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether code can configure remoting types and channels.</summary>
		/// <returns>true if code can configure remoting types and channels; otherwise, false.</returns>
		public bool RemotingConfiguration
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.RemotingConfiguration) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.RemotingConfiguration;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.RemotingConfiguration;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether code can use a serialization formatter to serialize or deserialize an object.</summary>
		/// <returns>true if code can use a serialization formatter to serialize or deserialize an object; otherwise, false.</returns>
		public bool SerializationFormatter
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SerializationFormatter) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SerializationFormatter;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.SerializationFormatter;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to bypass code verification is declared.</summary>
		/// <returns>true if permission to bypass code verification is declared; otherwise, false.</returns>
		public bool SkipVerification
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.SkipVerification) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.SkipVerification;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.SkipVerification;
				}
			}
		}

		/// <summary>Gets or sets a value indicating whether permission to call unmanaged code is declared.</summary>
		/// <returns>true if permission to call unmanaged code is declared; otherwise, false.</returns>
		public bool UnmanagedCode
		{
			get
			{
				return (this.m_Flags & SecurityPermissionFlag.UnmanagedCode) != SecurityPermissionFlag.NoFlags;
			}
			set
			{
				if (value)
				{
					this.m_Flags |= SecurityPermissionFlag.UnmanagedCode;
				}
				else
				{
					this.m_Flags &= ~SecurityPermissionFlag.UnmanagedCode;
				}
			}
		}

		/// <summary>Creates and returns a new <see cref="T:System.Security.Permissions.SecurityPermission" />.</summary>
		/// <returns>A <see cref="T:System.Security.Permissions.SecurityPermission" /> that corresponds to this attribute.</returns>
		public override IPermission CreatePermission()
		{
			return null;
		}

		/// <summary>Gets or sets all permission flags comprising the <see cref="T:System.Security.Permissions.SecurityPermission" /> permissions.</summary>
		/// <returns>One or more of the <see cref="T:System.Security.Permissions.SecurityPermissionFlag" /> values combined using a bitwise OR.</returns>
		/// <exception cref="T:System.ArgumentException">An attempt is made to set this property to an invalid value. See <see cref="T:System.Security.Permissions.SecurityPermissionFlag" /> for the valid values. </exception>
		public SecurityPermissionFlag Flags
		{
			get
			{
				return this.m_Flags;
			}
			set
			{
				this.m_Flags = value;
			}
		}
	}
}
