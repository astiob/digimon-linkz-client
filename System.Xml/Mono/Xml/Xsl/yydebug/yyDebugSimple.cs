using System;

namespace Mono.Xml.Xsl.yydebug
{
	internal class yyDebugSimple : yyDebug
	{
		private void println(string s)
		{
			Console.Error.WriteLine(s);
		}

		public void push(int state, object value)
		{
			this.println(string.Concat(new object[]
			{
				"push\tstate ",
				state,
				"\tvalue ",
				value
			}));
		}

		public void lex(int state, int token, string name, object value)
		{
			this.println(string.Concat(new object[]
			{
				"lex\tstate ",
				state,
				"\treading ",
				name,
				"\tvalue ",
				value
			}));
		}

		public void shift(int from, int to, int errorFlag)
		{
			switch (errorFlag)
			{
			case 0:
			case 1:
			case 2:
				this.println(string.Concat(new object[]
				{
					"shift\tfrom state ",
					from,
					" to ",
					to,
					"\t",
					errorFlag,
					" left to recover"
				}));
				break;
			case 3:
				this.println(string.Concat(new object[]
				{
					"shift\tfrom state ",
					from,
					" to ",
					to,
					"\ton error"
				}));
				break;
			default:
				this.println(string.Concat(new object[]
				{
					"shift\tfrom state ",
					from,
					" to ",
					to
				}));
				break;
			}
		}

		public void pop(int state)
		{
			this.println("pop\tstate " + state + "\ton error");
		}

		public void discard(int state, int token, string name, object value)
		{
			this.println(string.Concat(new object[]
			{
				"discard\tstate ",
				state,
				"\ttoken ",
				name,
				"\tvalue ",
				value
			}));
		}

		public void reduce(int from, int to, int rule, string text, int len)
		{
			this.println(string.Concat(new object[]
			{
				"reduce\tstate ",
				from,
				"\tuncover ",
				to,
				"\trule (",
				rule,
				") ",
				text
			}));
		}

		public void shift(int from, int to)
		{
			this.println(string.Concat(new object[]
			{
				"goto\tfrom state ",
				from,
				" to ",
				to
			}));
		}

		public void accept(object value)
		{
			this.println("accept\tvalue " + value);
		}

		public void error(string message)
		{
			this.println("error\t" + message);
		}

		public void reject()
		{
			this.println("reject");
		}
	}
}
