using Mono.Xml2;
using System;
using System.IO;
using System.Xml;

namespace Mono.Xml
{
	internal class DTDEntityBase : DTDNode
	{
		private string name;

		private string publicId;

		private string systemId;

		private string literalValue;

		private string replacementText;

		private string uriString;

		private Uri absUri;

		private bool isInvalid;

		private bool loadFailed;

		private XmlResolver resolver;

		protected DTDEntityBase(DTDObjectModel root)
		{
			base.SetRoot(root);
		}

		internal bool IsInvalid
		{
			get
			{
				return this.isInvalid;
			}
			set
			{
				this.isInvalid = value;
			}
		}

		public bool LoadFailed
		{
			get
			{
				return this.loadFailed;
			}
			set
			{
				this.loadFailed = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public string PublicId
		{
			get
			{
				return this.publicId;
			}
			set
			{
				this.publicId = value;
			}
		}

		public string SystemId
		{
			get
			{
				return this.systemId;
			}
			set
			{
				this.systemId = value;
			}
		}

		public string LiteralEntityValue
		{
			get
			{
				return this.literalValue;
			}
			set
			{
				this.literalValue = value;
			}
		}

		public string ReplacementText
		{
			get
			{
				return this.replacementText;
			}
			set
			{
				this.replacementText = value;
			}
		}

		public XmlResolver XmlResolver
		{
			set
			{
				this.resolver = value;
			}
		}

		public string ActualUri
		{
			get
			{
				if (this.uriString == null)
				{
					if (this.resolver == null || this.SystemId == null || this.SystemId.Length == 0)
					{
						this.uriString = this.BaseURI;
					}
					else
					{
						Uri baseUri = null;
						try
						{
							if (this.BaseURI != null && this.BaseURI.Length > 0)
							{
								baseUri = new Uri(this.BaseURI);
							}
						}
						catch (UriFormatException)
						{
						}
						this.absUri = this.resolver.ResolveUri(baseUri, this.SystemId);
						this.uriString = ((!(this.absUri != null)) ? string.Empty : this.absUri.ToString());
					}
				}
				return this.uriString;
			}
		}

		public void Resolve()
		{
			if (this.ActualUri == string.Empty)
			{
				this.LoadFailed = true;
				this.LiteralEntityValue = string.Empty;
				return;
			}
			if (base.Root.ExternalResources.ContainsKey(this.ActualUri))
			{
				this.LiteralEntityValue = (string)base.Root.ExternalResources[this.ActualUri];
			}
			Stream stream = null;
			try
			{
				stream = (this.resolver.GetEntity(this.absUri, null, typeof(Stream)) as Stream);
				Mono.Xml2.XmlTextReader xmlTextReader = new Mono.Xml2.XmlTextReader(this.ActualUri, stream, base.Root.NameTable);
				this.LiteralEntityValue = xmlTextReader.GetRemainder().ReadToEnd();
				base.Root.ExternalResources.Add(this.ActualUri, this.LiteralEntityValue);
				if (base.Root.ExternalResources.Count > 256)
				{
					throw new InvalidOperationException("The total amount of external entities exceeded the allowed number.");
				}
			}
			catch (Exception)
			{
				this.LiteralEntityValue = string.Empty;
				this.LoadFailed = true;
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
			}
		}
	}
}
