using System;

namespace System.Net.Mime
{
	/// <summary>Supplies the strings used to specify the disposition type for an e-mail attachment.</summary>
	public static class DispositionTypeNames
	{
		/// <summary>Specifies that the attachment is to be displayed as a file attached to the e-mail message.</summary>
		public const string Attachment = "attachment";

		/// <summary>Specifies that the attachment is to be displayed as part of the e-mail message body.</summary>
		public const string Inline = "inline";
	}
}
