using System;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
	/// <summary>Manages the execution context for the current thread. This class cannot be inherited.</summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public sealed class ExecutionContext : ISerializable
	{
		private SecurityContext _sc;

		private bool _suppressFlow;

		private bool _capture;

		internal ExecutionContext()
		{
		}

		internal ExecutionContext(ExecutionContext ec)
		{
			if (ec._sc != null)
			{
				this._sc = new SecurityContext(ec._sc);
			}
			this._suppressFlow = ec._suppressFlow;
			this._capture = true;
		}

		[MonoTODO]
		internal ExecutionContext(SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException();
		}

		/// <summary>Captures the execution context from the current thread.</summary>
		/// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the execution context for the current thread.</returns>
		/// <filterpriority>1</filterpriority>
		public static ExecutionContext Capture()
		{
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			if (executionContext.FlowSuppressed)
			{
				return null;
			}
			ExecutionContext executionContext2 = new ExecutionContext(executionContext);
			if (SecurityManager.SecurityEnabled)
			{
				executionContext2.SecurityContext = SecurityContext.Capture();
			}
			return executionContext2;
		}

		/// <summary>Creates a copy of the current execution context.</summary>
		/// <returns>An <see cref="T:System.Threading.ExecutionContext" /> object representing the current execution context.</returns>
		/// <exception cref="T:System.InvalidOperationException">This context cannot be copied because it is used. Only newly captured contexts can be copied.</exception>
		/// <filterpriority>2</filterpriority>
		public ExecutionContext CreateCopy()
		{
			if (!this._capture)
			{
				throw new InvalidOperationException();
			}
			return new ExecutionContext(this);
		}

		/// <summary>Sets the specified <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of the current execution context.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure representing the destination context of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <filterpriority>2</filterpriority>
		[MonoTODO]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			throw new NotImplementedException();
		}

		internal SecurityContext SecurityContext
		{
			get
			{
				if (this._sc == null)
				{
					this._sc = new SecurityContext();
				}
				return this._sc;
			}
			set
			{
				this._sc = value;
			}
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

		/// <summary>Indicates whether the flow of the execution context is currently suppressed.</summary>
		/// <returns>true if the flow is suppressed; otherwise, false. </returns>
		/// <filterpriority>1</filterpriority>
		public static bool IsFlowSuppressed()
		{
			return Thread.CurrentThread.ExecutionContext.FlowSuppressed;
		}

		/// <summary>Restores the flow of the execution context across asynchronous threads.</summary>
		/// <exception cref="T:System.InvalidOperationException">The context flow cannot be restored because it is not being suppressed.</exception>
		/// <filterpriority>1</filterpriority>
		public static void RestoreFlow()
		{
			ExecutionContext executionContext = Thread.CurrentThread.ExecutionContext;
			if (!executionContext.FlowSuppressed)
			{
				throw new InvalidOperationException();
			}
			executionContext.FlowSuppressed = false;
		}

		/// <summary>Runs a method in a specified execution context on the current thread.</summary>
		/// <param name="executionContext">The <see cref="T:System.Threading.ExecutionContext" /> to set.</param>
		/// <param name="callback">A <see cref="T:System.Threading.ContextCallback" /> delegate that represents the method to be run in the provided execution context.</param>
		/// <param name="state">The object to pass to the callback method.</param>
		/// <exception cref="T:System.InvalidOperationException">
		///   <paramref name="executionContext" /> is null.-or-<paramref name="executionContext" /> was not acquired through a capture operation. -or-<paramref name="executionContext" /> has already been used as the argument to a <see cref="M:System.Threading.ExecutionContext.Run(System.Threading.ExecutionContext,System.Threading.ContextCallback,System.Object)" /> call.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		[MonoTODO("only the SecurityContext is considered")]
		public static void Run(ExecutionContext executionContext, ContextCallback callback, object state)
		{
			if (executionContext == null)
			{
				throw new InvalidOperationException(Locale.GetText("Null ExecutionContext"));
			}
			SecurityContext.Run(executionContext.SecurityContext, callback, state);
		}

		/// <summary>Suppresses the flow of the execution context across asynchronous threads.</summary>
		/// <returns>An <see cref="T:System.Threading.AsyncFlowControl" /> structure for restoring the flow.</returns>
		/// <exception cref="T:System.InvalidOperationException">The context flow is already suppressed.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure" />
		/// </PermissionSet>
		public static AsyncFlowControl SuppressFlow()
		{
			Thread currentThread = Thread.CurrentThread;
			currentThread.ExecutionContext.FlowSuppressed = true;
			return new AsyncFlowControl(currentThread, AsyncFlowControlType.Execution);
		}
	}
}
