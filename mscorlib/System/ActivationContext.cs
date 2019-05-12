using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Identifies the activation context for the current application. This class cannot be inherited. </summary>
	/// <filterpriority>1</filterpriority>
	[ComVisible(false)]
	[Serializable]
	public sealed class ActivationContext : IDisposable, ISerializable
	{
		private ActivationContext.ContextForm _form;

		private ApplicationIdentity _appid;

		private bool _disposed;

		private ActivationContext(ApplicationIdentity identity)
		{
			this._appid = identity;
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" />  with the data needed to serialize the target object.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" />  to populate with data.</param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure for this serialization.</param>
		[MonoTODO("Missing serialization support")]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		~ActivationContext()
		{
			this.Dispose(false);
		}

		/// <summary>Gets the form, or store context, for the current application. </summary>
		/// <returns>One of the <see cref="T:System.ActivationContext.ContextForm" /> values. </returns>
		/// <filterpriority>1</filterpriority>
		public ActivationContext.ContextForm Form
		{
			get
			{
				return this._form;
			}
		}

		/// <summary>Gets the application identity for the current application.</summary>
		/// <returns>An <see cref="T:System.ApplicationIdentity" /> object that identifies the current application.</returns>
		/// <filterpriority>1</filterpriority>
		public ApplicationIdentity Identity
		{
			get
			{
				return this._appid;
			}
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ActivationContext" /> class using the specified application identity.</summary>
		/// <returns>An <see cref="T:System.ActivationContext" /> object.</returns>
		/// <param name="identity">An <see cref="T:System.ApplicationIdentity" /> object that identifies an application.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="identity" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">No deployment or application identity is specified in <paramref name="identity" />.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[MonoTODO("Missing validation")]
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			return new ActivationContext(identity);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.ActivationContext" /> class using the specified application identity and array of manifest paths.</summary>
		/// <returns>An <see cref="T:System.ActivationContext" /> object.</returns>
		/// <param name="identity">An <see cref="T:System.ApplicationIdentity" /> object that identifies an application.</param>
		/// <param name="manifestPaths">A string array of manifest paths for the application.</param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="identity" /> is null. -or-<paramref name="manifestPaths" /> is null. </exception>
		/// <exception cref="T:System.ArgumentException">No deployment or application identity is specified in <paramref name="identity" />.-or-<paramref name="identity" /> does not match the identity in the manifests.-or-<paramref name="identity" /> does not have the same number of components as the manifest paths.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		[MonoTODO("Missing validation")]
		public static ActivationContext CreatePartialActivationContext(ApplicationIdentity identity, string[] manifestPaths)
		{
			if (identity == null)
			{
				throw new ArgumentNullException("identity");
			}
			if (manifestPaths == null)
			{
				throw new ArgumentNullException("manifestPaths");
			}
			return new ActivationContext(identity);
		}

		/// <summary>Releases all resources used by the <see cref="T:System.ActivationContext" />. </summary>
		/// <filterpriority>1</filterpriority>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (this._disposed)
			{
				if (disposing)
				{
				}
				this._disposed = true;
			}
		}

		/// <summary>Indicates the context for a manifest-activated application.</summary>
		public enum ContextForm
		{
			/// <summary>The application is not in the ClickOnce store.</summary>
			Loose,
			/// <summary>The application is contained in the ClickOnce store.</summary>
			StoreBounded
		}
	}
}
