using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Provides a base implementation for channels that want to expose a dictionary interface to their properties.</summary>
	[ComVisible(true)]
	public abstract class BaseChannelWithProperties : BaseChannelObjectWithProperties
	{
		/// <summary>Indicates the top channel sink in the channel sink stack.</summary>
		protected IChannelSinkBase SinksWithProperties;

		/// <summary>Gets a <see cref="T:System.Collections.IDictionary" /> of the channel properties associated with the current channel object.</summary>
		/// <returns>A <see cref="T:System.Collections.IDictionary" /> of the channel properties associated with the current channel object.</returns>
		/// <exception cref="T:System.Security.SecurityException">The immediate caller does not have infrastructure permission. </exception>
		public override IDictionary Properties
		{
			get
			{
				if (this.SinksWithProperties == null || this.SinksWithProperties.Properties == null)
				{
					return base.Properties;
				}
				IDictionary[] dics = new IDictionary[]
				{
					base.Properties,
					this.SinksWithProperties.Properties
				};
				return new AggregateDictionary(dics);
			}
		}
	}
}
