using System;

namespace System.Xml.Serialization
{
	/// <summary>Represents certain attributes of a XSD &lt;part&gt; element in a WSDL document for generating classes from the document. </summary>
	public class SoapSchemaMember
	{
		private string memberName;

		private XmlQualifiedName memberType = XmlQualifiedName.Empty;

		/// <summary>Gets or sets a value that corresponds to the name attribute of the WSDL part element. </summary>
		/// <returns>The element name.</returns>
		public string MemberName
		{
			get
			{
				if (this.memberName == null)
				{
					return string.Empty;
				}
				return this.memberName;
			}
			set
			{
				this.memberName = value;
			}
		}

		/// <summary>Gets or sets a value that corresponds to the type attribute of the WSDL part element.</summary>
		/// <returns>An <see cref="T:System.Xml.XmlQualifiedName" /> that corresponds to the XML type.</returns>
		public XmlQualifiedName MemberType
		{
			get
			{
				return this.memberType;
			}
			set
			{
				this.memberType = value;
			}
		}
	}
}
