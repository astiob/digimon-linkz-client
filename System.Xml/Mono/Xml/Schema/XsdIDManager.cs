using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;

namespace Mono.Xml.Schema
{
	internal class XsdIDManager
	{
		private Hashtable idList = new Hashtable();

		private ArrayList missingIDReferences;

		private string thisElementId;

		private ArrayList MissingIDReferences
		{
			get
			{
				if (this.missingIDReferences == null)
				{
					this.missingIDReferences = new ArrayList();
				}
				return this.missingIDReferences;
			}
		}

		public void OnStartElement()
		{
			this.thisElementId = null;
		}

		public string AssessEachAttributeIdentityConstraint(XmlSchemaDatatype dt, object parsedValue, string elementName)
		{
			string text = parsedValue as string;
			switch (dt.TokenizedType)
			{
			case XmlTokenizedType.ID:
				if (this.thisElementId != null)
				{
					return "ID type attribute was already assigned in the containing element.";
				}
				this.thisElementId = text;
				if (this.idList.ContainsKey(text))
				{
					return "Duplicate ID value was found.";
				}
				this.idList.Add(text, elementName);
				if (this.MissingIDReferences.Contains(text))
				{
					this.MissingIDReferences.Remove(text);
				}
				break;
			case XmlTokenizedType.IDREF:
				if (!this.idList.Contains(text))
				{
					this.MissingIDReferences.Add(text);
				}
				break;
			case XmlTokenizedType.IDREFS:
				foreach (string text2 in (string[])parsedValue)
				{
					if (!this.idList.Contains(text2))
					{
						this.MissingIDReferences.Add(text2);
					}
				}
				break;
			}
			return null;
		}

		public bool HasMissingIDReferences()
		{
			return this.missingIDReferences != null && this.missingIDReferences.Count > 0;
		}

		public string GetMissingIDString()
		{
			return string.Join(" ", this.MissingIDReferences.ToArray(typeof(string)) as string[]);
		}
	}
}
