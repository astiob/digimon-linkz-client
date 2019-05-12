using System;
using System.Security.Principal;
using System.Threading;

namespace System.Security
{
	/// <summary>Encapsulates and propagates all security-related data for execution contexts transferred across threads. This class cannot be inherited.</summary>
	public sealed class SecurityContext
	{
		private bool _capture;

		private IntPtr _winid;

		private CompressedStack _stack;

		private bool _suppressFlowWindowsIdentity;

		private bool _suppressFlow;

		internal SecurityContext()
		{
		}

		internal SecurityContext(SecurityContext sc)
		{
			this._capture = true;
			this._winid = sc._winid;
			if (sc._stack != null)
			{
				this._stack = sc._stack.CreateCopy();
			}
		}

		/// <summary>Creates a copy of the current security context.</summary>
		/// <returns>A <see cref="T:System.Security.SecurityContext" /> object representing the security context for the current thread.</returns>
		/// <exception cref="T:System.InvalidOperationException">The current security context has been previously used, was marshaled across application domains, or was not acquired through the <see cref="M:System.Security.SecurityContext.Capture" /> method.</exception>
		public SecurityContext CreateCopy()
		{
			if (!this._capture)
			{
				throw new InvalidOperationException();
			}
			return new SecurityContext(this);
		}

		/// <summary>Captures the security context for the current thread.</summary>
		/// <returns>A <see cref="T:System.Security.SecurityContext" /> object representing the security context for the current thread.</returns>
		public static SecurityContext Capture()
		{
			SecurityContext securityContext = Thread.CurrentThread.ExecutionContext.SecurityContext;
			if (securityContext.FlowSuppressed)
			{
				return null;
			}
			return new SecurityContext
			{
				_capture = true,
				_winid = WindowsIdentity.GetCurrentToken(),
				_stack = CompressedStack.Capture()
			};
		}

		internal bool FlowSuppressed
		{
			get
			{
				return this._suppressFlow;
			}
			set
			{
				this._suppressFlow = value;
			}
		}

		internal bool WindowsIdentityFlowSuppressed
		{
			get
			{
				return this._suppressFlowWindowsIdentity;
			}
			set
			{
				this._suppressFlowWindowsIdentity = value;
			}
		}

		internal CompressedStack CompressedStack
		{
			get
			{
				return this._stack;
			}
			set
			{
				this._stack = value;
			}
		}

		internal IntPtr IdentityToken
		{
			get
			{
				return this._winid;
			}
			set
			{
				this._winid = value;
			}
		}

		/// <summary>Determines whether the flow of the security context has been suppressed.</summary>
		/// <returns>true if the flow has been suppressed; otherwise, false. </returns>
		public static bool IsFlowSuppressed()
		{
			return Thread.CurrentThread.ExecutionContext.SecurityContext.FlowSuppressed;
		}

		/// <summary>Determines whether the flow of the Windows identity portion of the current security context has been suppressed.</summary>
		/// <returns>true if the flow has been suppressed; otherwise, false. </returns>
		public static bool IsWindowsIdentityFlowSuppressed()
		{
			return Thread.CurrentThread.ExecutionContext.SecurityContext.WindowsIdentityFlowSuppressed;
		}

		/// <summary>Restores the flow of the security context across asynchronous threads.</summary>
		/// <exception cref="T:System.InvalidOperationException">The security context is null or an empty string.</exception>
		public static void RestoreFlow()
		{
			SecurityContext securityContext = Thread.CurrentThread.ExecutionContext.SecurityContext;
			if (!securityContext.FlowSuppressed && !securityContext.WindowsIdentityFlowSuppressed)
			{
				throw new InvalidOperationException();
			}
			securityContext.FlowSuppressed = false;
			securityContext.WindowsIdentityFlowSuppressed = false;
		}

		/// <summary>Runs the specified method in the specified security context on the current thread.</summary>
		/// <param name="securityContext">The <see cref="T:System.Security.SecurityContext" /> to set.</param>
		/// <param name="callback">The <see cref="T:System.Threading.ContextCallback" /> delegate that represents the method to run in the specified security context.</param>
		/// <param name="state">The object to pass to the callback method.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="securityContext" /> is null.-or-<paramref name="securityContext" /> was not acquired through a capture operation -or-<paramref name="securityContext" /> has already been used as the argument to a <see cref="M:System.Security.SecurityContext.Run(System.Security.SecurityContext,System.Threading.ContextCallback,System.Object)" /> method call.</exception>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static void Run(SecurityContext securityContext, ContextCallback callback, object state)
		{
			if (securityContext == null)
			{
				throw new InvalidOperationException(Locale.GetText("Null SecurityContext"));
			}
			SecurityContext securityContext2 = Thread.CurrentThread.ExecutionContext.SecurityContext;
			IPrincipal currentPrincipal = Thread.CurrentPrincipal;
			try
			{
				if (securityContext2.IdentityToken != IntPtr.Zero)
				{
					Thread.CurrentPrincipal = new WindowsPrincipal(new WindowsIdentity(securityContext2.IdentityToken));
				}
				if (securityContext.CompressedStack != null)
				{
					CompressedStack.Run(securityContext.CompressedStack, callback, state);
				}
				else
				{
					callback(state);
				}
			}
			finally
			{
				if (currentPrincipal != null && securityContext2.IdentityToken != IntPtr.Zero)
				{
					Thread.CurrentPrincipal = currentPrincipal;
				}
			}
		}

		/// <summary>Suppresses the flow of the security context across asynchronous threads.</summary>
		/// <returns>An <see cref="T:System.Threading.AsyncFlowControl" /> structure for restoring the flow.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static AsyncFlowControl SuppressFlow()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.ExecutionContext.SecurityContext.FlowSuppressed = true;
			currentThread.ExecutionContext.SecurityContext.WindowsIdentityFlowSuppressed = true;
			return new AsyncFlowControl(currentThread, AsyncFlowControlType.Security);
		}

		/// <summary>Suppresses the flow of the Windows identity portion of the current security context across asynchronous threads.</summary>
		/// <returns>An <see cref="T:System.Threading.AsyncFlowControl" /> structure for restoring the flow.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static AsyncFlowControl SuppressFlowWindowsIdentity()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.ExecutionContext.SecurityContext.WindowsIdentityFlowSuppressed = true;
			return new AsyncFlowControl(currentThread, AsyncFlowControlType.Security);
		}
	}
}
