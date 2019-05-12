using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.IO.IsolatedStorage
{
	/// <summary>Represents the abstract base class from which all isolated storage implementations must derive.</summary>
	[ComVisible(true)]
	public abstract class IsolatedStorage : MarshalByRefObject
	{
		internal IsolatedStorageScope storage_scope;

		internal object _assemblyIdentity;

		internal object _domainIdentity;

		internal object _applicationIdentity;

		/// <summary>Gets an application identity that scopes isolated storage.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Application" /> identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object. These permissions are granted by the runtime based on security policy. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object is not isolated by the application <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		[ComVisible(false)]
		[MonoTODO("requires manifest support")]
		public object ApplicationIdentity
		{
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Application) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._applicationIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				throw new NotImplementedException(Locale.GetText("CAS related"));
			}
		}

		/// <summary>Gets an assembly identity used to scope isolated storage.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the <see cref="T:System.Reflection.Assembly" /> identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object. </exception>
		/// <exception cref="T:System.InvalidOperationException">The assembly is not defined.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		public object AssemblyIdentity
		{
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Assembly) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._assemblyIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				return this._assemblyIdentity;
			}
		}

		/// <summary>Gets a value representing the current size of isolated storage.</summary>
		/// <returns>The number of storage units currently used within the isolated storage scope.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current size of the isolated store is undefined. </exception>
		[CLSCompliant(false)]
		public virtual ulong CurrentSize
		{
			get
			{
				throw new InvalidOperationException(Locale.GetText("IsolatedStorage does not have a preset CurrentSize."));
			}
		}

		/// <summary>Gets a domain identity that scopes isolated storage.</summary>
		/// <returns>An <see cref="T:System.Object" /> that represents the <see cref="F:System.IO.IsolatedStorage.IsolatedStorageScope.Domain" /> identity.</returns>
		/// <exception cref="T:System.Security.SecurityException">The code lacks the required <see cref="T:System.Security.Permissions.SecurityPermission" /> to access this object. These permissions are granted by the runtime based on security policy. </exception>
		/// <exception cref="T:System.InvalidOperationException">The <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object is not isolated by the domain <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />. </exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlPolicy" />
		/// </PermissionSet>
		public object DomainIdentity
		{
			get
			{
				if ((this.storage_scope & IsolatedStorageScope.Domain) == IsolatedStorageScope.None)
				{
					throw new InvalidOperationException(Locale.GetText("Invalid Isolation Scope."));
				}
				if (this._domainIdentity == null)
				{
					throw new InvalidOperationException(Locale.GetText("Identity unavailable."));
				}
				return this._domainIdentity;
			}
		}

		/// <summary>Gets a value representing the maximum amount of space available for isolated storage. When overridden in a derived class, this value can take different units of measure.</summary>
		/// <returns>The maximum amount of isolated storage space in bytes. Derived classes can return different units of value.</returns>
		/// <exception cref="T:System.InvalidOperationException">The quota has not been defined. </exception>
		[CLSCompliant(false)]
		public virtual ulong MaximumSize
		{
			get
			{
				throw new InvalidOperationException(Locale.GetText("IsolatedStorage does not have a preset MaximumSize."));
			}
		}

		/// <summary>Gets an <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> enumeration value specifying the scope used to isolate the store.</summary>
		/// <returns>A bitwise combination of <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" />  values specifying the scope used to isolate the store.</returns>
		public IsolatedStorageScope Scope
		{
			get
			{
				return this.storage_scope;
			}
		}

		/// <summary>Gets a backslash character that can be used in a directory string. When overridden in a derived class, another character might be returned.</summary>
		/// <returns>The default implementation returns the '\' (backslash) character.</returns>
		protected virtual char SeparatorExternal
		{
			get
			{
				return Path.DirectorySeparatorChar;
			}
		}

		/// <summary>Gets a period character that can be used in a directory string. When overridden in a derived class, another character might be returned.</summary>
		/// <returns>The default implementation returns the '.' (period) character.</returns>
		protected virtual char SeparatorInternal
		{
			get
			{
				return '.';
			}
		}

		/// <summary>When implemented by a derived class, returns a permission that represents access to isolated storage from within a permission set.</summary>
		/// <returns>An <see cref="T:System.Security.Permissions.IsolatedStoragePermission" /> object.</returns>
		/// <param name="ps">The <see cref="T:System.Security.PermissionSet" /> object that contains the set of permissions granted to code attempting to use isolated storage. </param>
		protected abstract IsolatedStoragePermission GetPermission(PermissionSet ps);

		/// <summary>Initializes a new <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object.</summary>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="domainEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the domain of the calling application. null lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence. </param>
		/// <param name="assemblyEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> present in the assembly of the calling application. null lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The assembly specified has insufficient permissions to create isolated stores. </exception>
		protected void InitStore(IsolatedStorageScope scope, Type domainEvidenceType, Type assemblyEvidenceType)
		{
			switch (scope)
			{
			case IsolatedStorageScope.User | IsolatedStorageScope.Assembly:
			case IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly:
				throw new NotImplementedException(scope.ToString());
			}
			throw new ArgumentException(scope.ToString());
		}

		/// <summary>Initializes a new <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object.</summary>
		/// <param name="scope">A bitwise combination of the <see cref="T:System.IO.IsolatedStorage.IsolatedStorageScope" /> values. </param>
		/// <param name="appEvidenceType">The type of <see cref="T:System.Security.Policy.Evidence" /> that you can choose from the list of <see cref="T:System.Security.Policy.Evidence" /> for the calling application. null lets the <see cref="T:System.IO.IsolatedStorage.IsolatedStorage" /> object choose the evidence. </param>
		/// <exception cref="T:System.IO.IsolatedStorage.IsolatedStorageException">The assembly specified has insufficient permissions to create isolated stores. </exception>
		[MonoTODO("requires manifest support")]
		protected void InitStore(IsolatedStorageScope scope, Type appEvidenceType)
		{
			if (AppDomain.CurrentDomain.ApplicationIdentity == null)
			{
				throw new IsolatedStorageException(Locale.GetText("No ApplicationIdentity available for AppDomain."));
			}
			if (appEvidenceType == null)
			{
			}
			this.storage_scope = scope;
		}

		/// <summary>When overridden in a derived class, removes the individual isolated store and all contained data.</summary>
		public abstract void Remove();
	}
}
