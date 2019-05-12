using System;

namespace System.Xml
{
	/// <summary>Specifies how to handle line breaks.</summary>
	public enum NewLineHandling
	{
		/// <summary>New line characters are replaced to match the character specified in the <see cref="P:System.Xml.XmlWriterSettings.NewLineChars" />  property.</summary>
		Replace,
		/// <summary>New line characters are entitized. This setting preserves all characters when the output is read by a normalizing <see cref="T:System.Xml.XmlReader" />.</summary>
		Entitize,
		/// <summary>The new line characters are unchanged. The output is the same as the input.</summary>
		None
	}
}
