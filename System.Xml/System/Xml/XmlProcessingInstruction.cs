using System;
using System.Xml.XPath;

namespace System.Xml
{
	/// <summary>Represents a processing instruction, which XML defines to keep processor-specific information in the text of the document.</summary>
	public class XmlProcessingInstruction : XmlLinkedNode
	{
		private string target;

		private string data;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlProcessingInstruction" /> class.</summary>
		/// <param name="target">The target of the processing instruction; see the <see cref="P:System.Xml.XmlProcessingInstruction.Target" /> property.</param>
		/// <param name="data">The content of the instruction; see the <see cref="P:System.Xml.XmlProcessingInstruction.Data" /> property.</param>
		/// <param name="doc">The parent XML document.</param>
		protected internal XmlProcessingInstruction(string target, string data, XmlDocument doc) : base(doc)
		{
			XmlConvert.VerifyName(target);
			if (data == null)
			{
				data = string.Empty;
			}
			this.target = target;
			this.data = data;
		}

		/// <summary>Gets or sets the content of the processing instruction, excluding the target.</summary>
		/// <returns>The content of the processing instruction, excluding the target.</returns>
		public string Data
		{
			get
			{
				return this.data;
			}
			set
			{
				this.data = value;
			}
		}

		/// <summary>Gets or sets the concatenated values of the node and all its children.</summary>
		/// <returns>The concatenated values of the node and all its children.</returns>
		public override string InnerText
		{
			get
			{
				return this.Data;
			}
			set
			{
				this.data = value;
			}
		}

		/// <summary>Gets the local name of the node.</summary>
		/// <returns>For processing instruction nodes, this property returns the target of the processing instruction.</returns>
		public override string LocalName
		{
			get
			{
				return this.target;
			}
		}

		/// <summary>Gets the qualified name of the node.</summary>
		/// <returns>For processing instruction nodes, this property returns the target of the processing instruction.</returns>
		public override string Name
		{
			get
			{
				return this.target;
			}
		}

		/// <summary>Gets the type of the current node.</summary>
		/// <returns>For XmlProcessingInstruction nodes, this value is XmlNodeType.ProcessingInstruction.</returns>
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.ProcessingInstruction;
			}
		}

		internal override XPathNodeType XPathNodeType
		{
			get
			{
				return XPathNodeType.ProcessingInstruction;
			}
		}

		/// <summary>Gets the target of the processing instruction.</summary>
		/// <returns>The target of the processing instruction.</returns>
		public string Target
		{
			get
			{
				return this.target;
			}
		}

		/// <summary>Gets or sets the value of the node.</summary>
		/// <returns>The entire content of the processing instruction, excluding the target.</returns>
		/// <exception cref="T:System.ArgumentException">Node is read-only. </exception>
		public override string Value
		{
			get
			{
				return this.data;
			}
			set
			{
				if (this.IsReadOnly)
				{
					throw new ArgumentException("This node is read-only.");
				}
				this.data = value;
			}
		}

		/// <summary>Creates a duplicate of this node.</summary>
		/// <returns>The duplicate node.</returns>
		/// <param name="deep">true to recursively clone the subtree under the specified node; false to clone only the node itself. </param>
		public override XmlNode CloneNode(bool deep)
		{
			return new XmlProcessingInstruction(this.target, this.data, this.OwnerDocument);
		}

		/// <summary>Saves all the children of the node to the specified <see cref="T:System.Xml.XmlWriter" />. Because ProcessingInstruction nodes do not have children, this method has no effect.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteContentTo(XmlWriter w)
		{
		}

		/// <summary>Saves the node to the specified <see cref="T:System.Xml.XmlWriter" />.</summary>
		/// <param name="w">The XmlWriter to which you want to save. </param>
		public override void WriteTo(XmlWriter w)
		{
			w.WriteProcessingInstruction(this.target, this.data);
		}
	}
}
