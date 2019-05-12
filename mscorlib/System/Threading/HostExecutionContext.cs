using System;

namespace System.Threading
{
	/// <summary>Encapsulates and propagates the host execution context across threads. </summary>
	/// <filterpriority>2</filterpriority>
	[MonoTODO("Useless until the runtime supports it")]
	public class HostExecutionContext
	{
		private object _state;

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class. </summary>
		public HostExecutionContext()
		{
			this._state = null;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Threading.HostExecutionContext" /> class using the specified state. </summary>
		/// <param name="state">An object representing the host execution context state.</param>
		public HostExecutionContext(object state)
		{
			this._state = state;
		}

		/// <summary>Creates a copy of the current host execution context.</summary>
		/// <returns>A <see cref="T:System.Threading.HostExecutionContext" /> object representing the host context for the current thread.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual HostExecutionContext CreateCopy()
		{
			return new HostExecutionContext(this._state);
		}

		/// <summary>Gets or sets the state of the host execution context.</summary>
		/// <returns>An object representing the host execution context state.</returns>
		protected internal object State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
			}
		}
	}
}
