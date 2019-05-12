using System;

namespace Mono.Xml.XPath.yydebug
{
	internal interface yyDebug
	{
		void push(int state, object value);

		void lex(int state, int token, string name, object value);

		void shift(int from, int to, int errorFlag);

		void pop(int state);

		void discard(int state, int token, string name, object value);

		void reduce(int from, int to, int rule, string text, int len);

		void shift(int from, int to);

		void accept(object value);

		void error(string message);

		void reject();
	}
}
