using System;

namespace System.Xml.Schema
{
	/// <summary>Provides schema compilation options for the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> class This class cannot be inherited.</summary>
	public sealed class XmlSchemaCompilationSettings
	{
		private bool enable_upa_check = true;

		/// <summary>Gets or sets a value indicating whether the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> should check for Unique Particle Attribution (UPA) violations.</summary>
		/// <returns>true if the <see cref="T:System.Xml.Schema.XmlSchemaSet" /> should check for Unique Particle Attribution (UPA) violations; otherwise, false. The default is true.</returns>
		public bool EnableUpaCheck
		{
			get
			{
				return this.enable_upa_check;
			}
			set
			{
				this.enable_upa_check = value;
			}
		}
	}
}
