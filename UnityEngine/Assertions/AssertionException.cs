using System;

namespace UnityEngine.Assertions
{
	/// <summary>
	///   <para>An exception that is thrown on a failure. Assertions.Assert._raiseExceptions needs to be set to true.</para>
	/// </summary>
	public class AssertionException : Exception
	{
		private string m_UserMessage;

		public AssertionException(string message, string userMessage) : base(message)
		{
			this.m_UserMessage = userMessage;
		}

		public override string Message
		{
			get
			{
				string text = base.Message;
				if (this.m_UserMessage != null)
				{
					text = text + '\n' + this.m_UserMessage;
				}
				return text;
			}
		}
	}
}
