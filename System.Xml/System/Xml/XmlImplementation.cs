using System;
using System.Collections.Generic;
using System.Globalization;

namespace System.Xml
{
	/// <summary>Defines the context for a set of <see cref="T:System.Xml.XmlDocument" /> objects.</summary>
	public class XmlImplementation
	{
		internal XmlNameTable InternalNameTable;

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlImplementation" /> class.</summary>
		public XmlImplementation() : this(new NameTable())
		{
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Xml.XmlImplementation" /> class with the <see cref="T:System.Xml.XmlNameTable" /> specified.</summary>
		/// <param name="nt">An <see cref="T:System.Xml.XmlNameTable" /> object.</param>
		public XmlImplementation(XmlNameTable nameTable)
		{
			this.InternalNameTable = nameTable;
		}

		/// <summary>Creates a new <see cref="T:System.Xml.XmlDocument" />.</summary>
		/// <returns>The new XmlDocument object.</returns>
		public virtual XmlDocument CreateDocument()
		{
			return new XmlDocument(this);
		}

		/// <summary>Tests if the Document Object Model (DOM) implementation implements a specific feature.</summary>
		/// <returns>true if the feature is implemented in the specified version; otherwise, false.The following table shows the combinations that cause HasFeature to return true.strFeature strVersion XML 1.0 XML 2.0 </returns>
		/// <param name="strFeature">The package name of the feature to test. This name is not case-sensitive. </param>
		/// <param name="strVersion">This is the version number of the package name to test. If the version is not specified (null), supporting any version of the feature causes the method to return true. </param>
		public bool HasFeature(string strFeature, string strVersion)
		{
			if (string.Compare(strFeature, "xml", true, CultureInfo.InvariantCulture) == 0)
			{
				if (strVersion != null)
				{
					if (XmlImplementation.<>f__switch$map4C == null)
					{
						XmlImplementation.<>f__switch$map4C = new Dictionary<string, int>(2)
						{
							{
								"1.0",
								0
							},
							{
								"2.0",
								0
							}
						};
					}
					int num;
					if (!XmlImplementation.<>f__switch$map4C.TryGetValue(strVersion, out num))
					{
						return false;
					}
					if (num != 0)
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}
	}
}
