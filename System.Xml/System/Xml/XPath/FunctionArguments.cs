using System;
using System.Collections;

namespace System.Xml.XPath
{
	internal class FunctionArguments
	{
		protected Expression _arg;

		protected FunctionArguments _tail;

		public FunctionArguments(Expression arg, FunctionArguments tail)
		{
			this._arg = arg;
			this._tail = tail;
		}

		public Expression Arg
		{
			get
			{
				return this._arg;
			}
		}

		public FunctionArguments Tail
		{
			get
			{
				return this._tail;
			}
		}

		public void ToArrayList(ArrayList a)
		{
			FunctionArguments functionArguments = this;
			do
			{
				a.Add(functionArguments._arg);
				functionArguments = functionArguments._tail;
			}
			while (functionArguments != null);
		}
	}
}
