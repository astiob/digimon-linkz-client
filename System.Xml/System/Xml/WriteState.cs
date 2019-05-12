using System;

namespace System.Xml
{
	/// <summary>Specifies the state of the <see cref="T:System.Xml.XmlWriter" />.</summary>
	public enum WriteState
	{
		/// <summary>Indicates that a Write method has not yet been called.</summary>
		Start,
		/// <summary>Indicates that the prolog is being written.</summary>
		Prolog,
		/// <summary>Indicates that an element start tag is being written.</summary>
		Element,
		/// <summary>Indicates that an attribute value is being written.</summary>
		Attribute,
		/// <summary>Indicates that element content is being written.</summary>
		Content,
		/// <summary>Indicates that the <see cref="M:System.Xml.XmlWriter.Close" /> method has been called.</summary>
		Closed,
		/// <summary>An exception has been thrown, which has left the <see cref="T:System.Xml.XmlWriter" /> in an invalid state. You can call the <see cref="M:System.Xml.XmlWriter.Close" /> method to put the <see cref="T:System.Xml.XmlWriter" /> in the <see cref="F:System.Xml.WriteState.Closed" /> state. Any other <see cref="T:System.Xml.XmlWriter" /> method calls results in an <see cref="T:System.InvalidOperationException" />.</summary>
		Error
	}
}
