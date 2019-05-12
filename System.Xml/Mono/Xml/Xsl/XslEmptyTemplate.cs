using System;
using System.Collections;

namespace Mono.Xml.Xsl
{
	internal class XslEmptyTemplate : XslTemplate
	{
		private static XslEmptyTemplate instance = new XslEmptyTemplate();

		private XslEmptyTemplate() : base(null)
		{
		}

		public static XslTemplate Instance
		{
			get
			{
				return XslEmptyTemplate.instance;
			}
		}

		public override void Evaluate(XslTransformProcessor p, Hashtable withParams)
		{
		}
	}
}
