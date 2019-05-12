using System;
using System.Diagnostics;

namespace OAuth
{
	[DebuggerDisplay("{Name}:{Value}")]
	[Serializable]
	public class WebParameter
	{
		public WebParameter(string name, string value)
		{
			this.Name = name;
			this.Value = value;
		}

		public string Value { get; set; }

		public string Name { get; private set; }
	}
}
