using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	/// <summary>Provides different methods for preventing derivation.</summary>
	[Flags]
	public enum XmlSchemaDerivationMethod
	{
		/// <summary>Override default derivation method to allow any derivation.</summary>
		[XmlEnum("")]
		Empty = 0,
		/// <summary>Refers to derivations by Substitution.</summary>
		[XmlEnum("substitution")]
		Substitution = 1,
		/// <summary>Refers to derivations by Extension.</summary>
		[XmlEnum("extension")]
		Extension = 2,
		/// <summary>Refers to derivations by Restriction.</summary>
		[XmlEnum("restriction")]
		Restriction = 4,
		/// <summary>Refers to derivations by List.</summary>
		[XmlEnum("list")]
		List = 8,
		/// <summary>Refers to derivations by Union.</summary>
		[XmlEnum("union")]
		Union = 16,
		/// <summary>#all. Refers to all derivation methods.</summary>
		[XmlEnum("#all")]
		All = 255,
		/// <summary>Accepts the default derivation method.</summary>
		[XmlIgnore]
		None = 256
	}
}
