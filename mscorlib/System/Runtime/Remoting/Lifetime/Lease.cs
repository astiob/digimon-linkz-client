using System;
using System.Collections;
using System.Threading;

namespace System.Runtime.Remoting.Lifetime
{
	internal class Lease : MarshalByRefObject, ILease
	{
		private DateTime _leaseExpireTime;

		private LeaseState _currentState;

		private TimeSpan _initialLeaseTime;

		private TimeSpan _renewOnCallTime;

		private TimeSpan _sponsorshipTimeout;

		private ArrayList _sponsors;

		private Queue _renewingSponsors;

		private Lease.RenewalDelegate _renewalDelegate;

		public Lease()
		{
			this._currentState = LeaseState.Initial;
			this._initialLeaseTime = LifetimeServices.LeaseTime;
			this._renewOnCallTime = LifetimeServices.RenewOnCallTime;
			this._sponsorshipTimeout = LifetimeServices.SponsorshipTimeout;
			this._leaseExpireTime = DateTime.Now + this._initialLeaseTime;
		}

		public TimeSpan CurrentLeaseTime
		{
			get
			{
				return this._leaseExpireTime - DateTime.Now;
			}
		}

		public LeaseState CurrentState
		{
			get
			{
				return this._currentState;
			}
		}

		public void Activate()
		{
			this._currentState = LeaseState.Active;
		}

		public TimeSpan InitialLeaseTime
		{
			get
			{
				return this._initialLeaseTime;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("InitialLeaseTime property can only be set when the lease is in initial state; state is " + this._currentState + ".");
				}
				this._initialLeaseTime = value;
				this._leaseExpireTime = DateTime.Now + this._initialLeaseTime;
				if (value == TimeSpan.Zero)
				{
					this._currentState = LeaseState.Null;
				}
			}
		}

		public TimeSpan RenewOnCallTime
		{
			get
			{
				return this._renewOnCallTime;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("RenewOnCallTime property can only be set when the lease is in initial state; state is " + this._currentState + ".");
				}
				this._renewOnCallTime = value;
			}
		}

		public TimeSpan SponsorshipTimeout
		{
			get
			{
				return this._sponsorshipTimeout;
			}
			set
			{
				if (this._currentState != LeaseState.Initial)
				{
					throw new RemotingException("SponsorshipTimeout property can only be set when the lease is in initial state; state is " + this._currentState + ".");
				}
				this._sponsorshipTimeout = value;
			}
		}

		public void Register(ISponsor obj)
		{
			this.Register(obj, TimeSpan.Zero);
		}

		public void Register(ISponsor obj, TimeSpan renewalTime)
		{
			lock (this)
			{
				if (this._sponsors == null)
				{
					this._sponsors = new ArrayList();
				}
				this._sponsors.Add(obj);
			}
			if (renewalTime != TimeSpan.Zero)
			{
				this.Renew(renewalTime);
			}
		}

		public TimeSpan Renew(TimeSpan renewalTime)
		{
			DateTime dateTime = DateTime.Now + renewalTime;
			if (dateTime > this._leaseExpireTime)
			{
				this._leaseExpireTime = dateTime;
			}
			return this.CurrentLeaseTime;
		}

		public void Unregister(ISponsor obj)
		{
			lock (this)
			{
				if (this._sponsors != null)
				{
					for (int i = 0; i < this._sponsors.Count; i++)
					{
						if (object.ReferenceEquals(this._sponsors[i], obj))
						{
							this._sponsors.RemoveAt(i);
							break;
						}
					}
				}
			}
		}

		internal void UpdateState()
		{
			if (this._currentState != LeaseState.Active)
			{
				return;
			}
			if (this.CurrentLeaseTime > TimeSpan.Zero)
			{
				return;
			}
			if (this._sponsors != null)
			{
				this._currentState = LeaseState.Renewing;
				lock (this)
				{
					this._renewingSponsors = new Queue(this._sponsors);
				}
				this.CheckNextSponsor();
			}
			else
			{
				this._currentState = LeaseState.Expired;
			}
		}

		private void CheckNextSponsor()
		{
			if (this._renewingSponsors.Count == 0)
			{
				this._currentState = LeaseState.Expired;
				this._renewingSponsors = null;
				return;
			}
			ISponsor @object = (ISponsor)this._renewingSponsors.Peek();
			this._renewalDelegate = new Lease.RenewalDelegate(@object.Renewal);
			IAsyncResult asyncResult = this._renewalDelegate.BeginInvoke(this, null, null);
			ThreadPool.RegisterWaitForSingleObject(asyncResult.AsyncWaitHandle, new WaitOrTimerCallback(this.ProcessSponsorResponse), asyncResult, this._sponsorshipTimeout, true);
		}

		private void ProcessSponsorResponse(object state, bool timedOut)
		{
			if (!timedOut)
			{
				try
				{
					IAsyncResult result = (IAsyncResult)state;
					TimeSpan timeSpan = this._renewalDelegate.EndInvoke(result);
					if (timeSpan != TimeSpan.Zero)
					{
						this.Renew(timeSpan);
						this._currentState = LeaseState.Active;
						this._renewingSponsors = null;
						return;
					}
				}
				catch
				{
				}
			}
			this.Unregister((ISponsor)this._renewingSponsors.Dequeue());
			this.CheckNextSponsor();
		}

		private delegate TimeSpan RenewalDelegate(ILease lease);
	}
}
