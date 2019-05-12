using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.Runtime.Remoting.Channels
{
	/// <summary>Stores sink provider data for sink providers.</summary>
	[ComVisible(true)]
	public class SinkProviderData
	{
		private string sinkName;

		private ArrayList children;

		private Hashtable properties;

		/// <summary>Initializes a new instance of the <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> class.</summary>
		/// <param name="name">The name of the sink provider that the data in the current <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> object is associated with. </param>
		public SinkProviderData(string name)
		{
			this.sinkName = name;
			this.children = new ArrayList();
			this.properties = new Hashtable();
		}

		/// <summary>Gets a list of the child <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> nodes.</summary>
		/// <returns>A <see cref="T:System.Collections.IList" /> of the child <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> nodes.</returns>
		public IList Children
		{
			get
			{
				return this.children;
			}
		}

		/// <summary>Gets the name of the sink provider that the data in the current <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> object is associated with.</summary>
		/// <returns>A <see cref="T:System.String" /> with the name of the XML node that the data in the current <see cref="T:System.Runtime.Remoting.Channels.SinkProviderData" /> object is associated with.</returns>
		public string Name
		{
			get
			{
				return this.sinkName;
			}
		}

		/// <summary>Gets a dictionary through which properties on the sink provider can be accessed.</summary>
		/// <returns>A dictionary through which properties on the sink provider can be accessed.</returns>
		public IDictionary Properties
		{
			get
			{
				return this.properties;
			}
		}
	}
}
