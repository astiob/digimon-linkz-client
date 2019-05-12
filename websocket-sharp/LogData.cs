using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace WebSocketSharp
{
	public class LogData
	{
		private StackFrame _caller;

		private DateTime _date;

		private LogLevel _level;

		private string _message;

		internal LogData(LogLevel level, StackFrame caller, string message)
		{
			this._level = level;
			this._caller = caller;
			this._message = (message ?? string.Empty);
			this._date = DateTime.Now;
		}

		public StackFrame Caller
		{
			get
			{
				return this._caller;
			}
		}

		public DateTime Date
		{
			get
			{
				return this._date;
			}
		}

		public LogLevel Level
		{
			get
			{
				return this._level;
			}
		}

		public string Message
		{
			get
			{
				return this._message;
			}
		}

		public override string ToString()
		{
			string text = string.Format("{0}|{1,-5}|", this._date, this._level);
			MethodBase method = this._caller.GetMethod();
			Type declaringType = method.DeclaringType;
			string arg = string.Format("{0}{1}.{2}|", text, declaringType.Name, method.Name);
			string[] array = this._message.Replace("\r\n", "\n").TrimEnd(new char[]
			{
				'\n'
			}).Split(new char[]
			{
				'\n'
			});
			if (array.Length <= 1)
			{
				return string.Format("{0}{1}", arg, this._message);
			}
			StringBuilder stringBuilder = new StringBuilder(string.Format("{0}{1}\n", arg, array[0]), 64);
			int length = text.Length;
			string format = string.Format("{{0,{0}}}{{1}}\n", length);
			for (int i = 1; i < array.Length; i++)
			{
				stringBuilder.AppendFormat(format, "", array[i]);
			}
			stringBuilder.Length--;
			return stringBuilder.ToString();
		}
	}
}
