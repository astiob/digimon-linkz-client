using System;

namespace System.Xml
{
	/// <summary>Represents a notation declaration, such as &lt;!NOTATION... &gt;.</summary>
	public class XmlNotation : XmlNode
	{
		private string localName;

		private string publicId;

		private string systemId;

		private string prefix;

		internal XmlNotation(string localName, string prefix, string publicId, string systemId, XmlDocument doc) : base(doc)
		{
			this.localName = doc.NameTable.Add(localName);
			this.prefix = doc.NameTable.Add(prefix);
			this.publicId = publicId;
			this.systemId = systemId;
		}

		/// <summary>Gets the markup representing the children of this node.</summary>
		/// <returns>For XmlNotation nodes, String.Empty is returned.</returns>
		/// <exception cref="T:System.InvalidOperationException">Attempting to set the property. </exception>
		public override string InnerXml
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException("This operation is not allowed.");
			}
		}

		/// <summary>Gets a value indicating whether the node is read-only.</summary>
		/// <returns>true if the node is read-only; otherwise false.Because XmlNotation nodes are read-only, this property always returns true.</returns>
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		/// <summary>Gets the name of the current node without the namespace prefix.</summary>
		/// <returns>For XmlNotation nodes, this property returns the name of the notation.</returns>
		public override string LocalName
		{
			get
			{
				return this.localName;
			}
		}

		/// <summary>Gets the name of the current node.</summary>
		/// <returns>The name of the notation.</returns>
		public override string Name
		{
			get
			{
				return (!(this.prefix != string.Empty)) ? this.localName : (this.prefix + ":" + this.localName);
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>The node type. For XmlNotation nodes, the value is XmlNodeType.Notation.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Notation;
			}
		}

		/// <summary>Gets the markup representing this node and all its children.</summary>
		/// <returns>For XmlNotation nodes, String.Empty is returned.</returns>
		public override string OuterXml
		{
			get
			{
				return string.Empty;
			}
		}

		/// <summary>Gets the value of the public identifier on the notation declaration.</summary>
		/// <returns>The public identifier on the notation. If there is no public identifier, null is returned.</returns>
		public string PublicId
		{
			get
			{
				if (this.publicId != null)
				{
					return this.publicId;
				}
				return null;
			}
		}

		/// <summary>Gets the value of the system identifier on the notation declaration.</summary>
		/// <returns>The system identifier on the notation. If there is no system identifier, null is returned.</returns>
		public string SystemId
		{
			get
			{
				if (this.systemId != null)
				{
					return this.systemId;
				}
				return null;
			}
		}

		/// <summary>Creates a duplicate of this node. Notation nodes cannot be cloned. Calling this method on an <see cref="T:System.Xml.XmlNotation" /> object throws an exception.</summary>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself.</param>
		/// <exception cref="T:System.InvalidOperationException">Notation nodes cannot be cloned. Calling this method on an <see cref="T:System.Xml.XmlNotation" /> object throws an exception.</exception>
		public override XmlNode CloneNode(bool deep)
		{
			throw new InvalidOperationException("This operation is not allowed.");
		}

		/// <summary>Saves the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />. This method has no effect on XmlNotation nodes.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />. This method has no effect on XmlNotation nodes.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
		}
	}
}
