using System;

namespace System.Xml.Serialization
{
	/// <summary>Provides data for the known, but unreferenced, object found in an encoded SOAP XML stream during deserialization.</summary>
	public class UnreferencedObjectEventArgs : EventArgs
	{
		private object unreferencedObject;

		private string unreferencedId;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.UnreferencedObjectEventArgs" /> class.</summary>
		/// <param name="o">The unreferenced object. </param>
		/// <param name="id">A unique string used to identify the unreferenced object. </param>
		public UnreferencedObjectEventArgs(object o, string id)
		{
			this.unreferencedObject = o;
			this.unreferencedId = id;
		}

		/// <summary>Gets the ID of the object.</summary>
		/// <returns>The ID of the object.</returns>
		public string UnreferencedId
		{
			get
			{
				return this.unreferencedId;
			}
		}

		/// <summary>Gets the deserialized, but unreferenced, object.</summary>
		/// <returns>The deserialized, but unreferenced, object.</returns>
		public object UnreferencedObject
		{
			get
			{
				return this.unreferencedObject;
			}
		}
	}
}
