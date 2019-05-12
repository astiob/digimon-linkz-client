using System;
using System.Collections.Generic;

namespace Mono.Xml
{
	internal class DictionaryBase : List<KeyValuePair<string, DTDNode>>
	{
		public IEnumerable<DTDNode> Values
		{
			get
			{
				foreach (KeyValuePair<string, DTDNode> p in this)
				{
					yield return p.Value;
				}
				yield break;
			}
		}
	}
}
