using System;
using System.Runtime.InteropServices;

namespace System.Security.Policy
{
	/// <summary>Represents the context for the trust manager to consider when making the decision to run an application, and when setting up the security on a new <see cref="T:System.AppDomain" /> in which to run an application.</summary>
	[ComVisible(true)]
	public class TrustManagerContext
	{
		private bool _ignorePersistedDecision;

		private bool _noPrompt;

		private bool _keepAlive;

		private bool _persist;

		private ApplicationIdentity _previousId;

		private TrustManagerUIContext _ui;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.TrustManagerContext" /> class. </summary>
		public TrustManagerContext() : this(TrustManagerUIContext.Run)
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Policy.TrustManagerContext" /> class using the specified <see cref="T:System.Security.Policy.TrustManagerUIContext" /> object. </summary>
		/// <param name="uiContext">One of the enumeration values that specifies the type of trust manager user interface to use. </param>
		public TrustManagerContext(TrustManagerUIContext uiContext)
		{
			this._ignorePersistedDecision = false;
			this._noPrompt = false;
			this._keepAlive = false;
			this._persist = false;
			this._ui = uiContext;
		}

		/// <summary>Gets or sets a value indicating whether the application security manager should ignore any persisted decisions and call the trust manager.</summary>
		/// <returns>true to call the trust manager; otherwise, false. </returns>
		public virtual bool IgnorePersistedDecision
		{
			get
			{
				return this._ignorePersistedDecision;
			}
			set
			{
				this._ignorePersistedDecision = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the trust manager should cache state for this application, to facilitate future requests to determine application trust.</summary>
		/// <returns>true to cache state data; otherwise, false. The default is false.</returns>
		public virtual bool KeepAlive
		{
			get
			{
				return this._keepAlive;
			}
			set
			{
				this._keepAlive = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the trust manager should prompt the user for trust decisions.</summary>
		/// <returns>true to not prompt the user; false to prompt the user. The default is false.</returns>
		public virtual bool NoPrompt
		{
			get
			{
				return this._noPrompt;
			}
			set
			{
				this._noPrompt = value;
			}
		}

		/// <summary>Gets or sets a value indicating whether the user's response to the consent dialog should be persisted. </summary>
		/// <returns>true to cache state data; otherwise, false. The default is true.</returns>
		public virtual bool Persist
		{
			get
			{
				return this._persist;
			}
			set
			{
				this._persist = value;
			}
		}

		/// <summary>Gets or sets the identity of the previous application identity.</summary>
		/// <returns>An <see cref="T:System.ApplicationIdentity" /> object representing the previous <see cref="T:System.ApplicationIdentity" />.</returns>
		public virtual ApplicationIdentity PreviousApplicationIdentity
		{
			get
			{
				return this._previousId;
			}
			set
			{
				this._previousId = value;
			}
		}

		/// <summary>Gets or sets the type of user interface the trust manager should display.</summary>
		/// <returns>One of the enumeration values. The default is <see cref="F:System.Security.Policy.TrustManagerUIContext.Run" />. </returns>
		public virtual TrustManagerUIContext UIContext
		{
			get
			{
				return this._ui;
			}
			set
			{
				this._ui = value;
			}
		}
	}
}
