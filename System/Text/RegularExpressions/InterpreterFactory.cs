using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	internal class InterpreterFactory : IMachineFactory
	{
		private IDictionary mapping;

		private ushort[] pattern;

		private string[] namesMapping;

		private int gap;

		public InterpreterFactory(ushort[] pattern)
		{
			this.pattern = pattern;
		}

		public IMachine NewInstance()
		{
			return new Interpreter(this.pattern);
		}

		public int GroupCount
		{
			get
			{
				return (int)this.pattern[1];
			}
		}

		public int Gap
		{
			get
			{
				return this.gap;
			}
			set
			{
				this.gap = value;
			}
		}

		public IDictionary Mapping
		{
			get
			{
				return this.mapping;
			}
			set
			{
				this.mapping = value;
			}
		}

		public string[] NamesMapping
		{
			get
			{
				return this.namesMapping;
			}
			set
			{
				this.namesMapping = value;
			}
		}
	}
}
