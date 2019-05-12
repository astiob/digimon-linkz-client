using System;

namespace System.Xml
{
	/// <summary>Specifies how the <see cref="T:System.Xml.XmlTextReader" /> or <see cref="T:System.Xml.XmlValidatingReader" /> handle entities.</summary>
	public enum EntityHandling
	{
		/// <summary>Expands all entities and returns the expanded nodes.</summary>
		ExpandEntities = 1,
		/// <summary>Expands character entities and returns general entities as <see cref="F:System.Xml.XmlNodeType.EntityReference" /> nodes. </summary>
		ExpandCharEntities
	}
}
