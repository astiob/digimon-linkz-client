using System;
using System.Collections;
using System.Collections.Specialized;

namespace System.Xml.Serialization
{
	/// <summary>Describes the context in which a set of schema is bound to .NET Framework code entities.</summary>
	public class ImportContext
	{
		private bool _shareTypes;

		private CodeIdentifiers _typeIdentifiers;

		private StringCollection _warnings = new StringCollection();

		internal Hashtable MappedTypes;

		internal Hashtable DataMappedTypes;

		internal Hashtable SharedAnonymousTypes;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.Serialization.ImportContext" /> class for the given code identifiers, with the given type-sharing option.</summary>
		/// <param name="identifiers">The code entities to which the context applies.</param>
		/// <param name="shareTypes">A <see cref="T:System.Boolean" /> value that determines whether custom types are shared among schema.</param>
		public ImportContext(CodeIdentifiers identifiers, bool shareTypes)
		{
			this._typeIdentifiers = identifiers;
			this._shareTypes = shareTypes;
			if (shareTypes)
			{
				this.MappedTypes = new Hashtable();
				this.DataMappedTypes = new Hashtable();
				this.SharedAnonymousTypes = new Hashtable();
			}
		}

		/// <summary>Gets a value that determines whether custom types are shared.</summary>
		/// <returns>true, if custom types are shared among schema; otherwise, false.</returns>
		public bool ShareTypes
		{
			get
			{
				return this._shareTypes;
			}
		}

		/// <summary>Gets a set of code entities to which the context applies.</summary>
		/// <returns>A <see cref="T:System.Xml.Serialization.CodeIdentifiers" /> that specifies the code entities to which the context applies.</returns>
		public CodeIdentifiers TypeIdentifiers
		{
			get
			{
				return this._typeIdentifiers;
			}
		}

		/// <summary>Gets a collection of warnings that are generated when importing the code entity descriptions.</summary>
		/// <returns>A <see cref="T:System.Collections.Specialized.StringCollection" /> that contains warnings that were generated when importing the code entity descriptions.</returns>
		public StringCollection Warnings
		{
			get
			{
				return this._warnings;
			}
		}
	}
}
