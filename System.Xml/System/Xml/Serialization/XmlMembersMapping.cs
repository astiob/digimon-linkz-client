using System;

namespace System.Xml.Serialization
{
	/// <summary>Provides mappings between .NET Framework Web service methods and Web Services Description Language (WSDL) messages that are defined for SOAP Web services. </summary>
	public class XmlMembersMapping : XmlMapping
	{
		private bool _hasWrapperElement;

		private XmlMemberMapping[] _mapping;

		internal XmlMembersMapping()
		{
		}

		internal XmlMembersMapping(XmlMemberMapping[] mapping) : this(string.Empty, null, false, false, mapping)
		{
		}

		internal XmlMembersMapping(string elementName, string ns, XmlMemberMapping[] mapping) : this(elementName, ns, true, false, mapping)
		{
		}

		internal XmlMembersMapping(string elementName, string ns, bool hasWrapperElement, bool writeAccessors, XmlMemberMapping[] mapping) : base(elementName, ns)
		{
			this._hasWrapperElement = hasWrapperElement;
			this._mapping = mapping;
			ClassMap classMap = new ClassMap();
			classMap.IgnoreMemberNamespace = writeAccessors;
			foreach (XmlMemberMapping xmlMemberMapping in mapping)
			{
				classMap.AddMember(xmlMemberMapping.TypeMapMember);
			}
			base.ObjectMap = classMap;
		}

		/// <summary>Gets the number of .NET Framework code entities that belong to a Web service method to which a SOAP message is being mapped. </summary>
		/// <returns>The number of mappings in the collection.</returns>
		public int Count
		{
			get
			{
				return this._mapping.Length;
			}
		}

		/// <summary>Gets an item that contains internal type mapping information for a .NET Framework code entity that belongs to a Web service method being mapped to a SOAP message.</summary>
		/// <returns>The requested <see cref="T:System.Xml.Serialization.XmlMemberMapping" />.</returns>
		/// <param name="index">The index of the mapping to return.</param>
		public XmlMemberMapping this[int index]
		{
			get
			{
				return this._mapping[index];
			}
		}

		/// <summary>Gets the name of the .NET Framework type being mapped to the data type of an XML Schema element that represents a SOAP message.</summary>
		/// <returns>The name of the .NET Framework type.</returns>
		public string TypeName
		{
			[MonoTODO]
			get
			{
				throw new NotImplementedException();
			}
		}

		/// <summary>Gets the namespace of the .NET Framework type being mapped to the data type of an XML Schema element that represents a SOAP message.</summary>
		/// <returns>The .NET Framework namespace of the mapping.</returns>
		public string TypeNamespace
		{
			[MonoTODO]
			get
			{
				throw new NotImplementedException();
			}
		}

		internal bool HasWrapperElement
		{
			get
			{
				return this._hasWrapperElement;
			}
		}
	}
}
