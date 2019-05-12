using System;
using TS;

namespace AdventureScene
{
	public abstract class BaseCommand
	{
		protected ScriptEngine scriptEngine;

		public int commandAddress;

		public bool continueAnalyze;

		protected void SetWaitScriptEngine(bool isWaitStatus)
		{
			this.scriptEngine.SetStatus(isWaitStatus, ScriptEngine.Status.EXTERNAL_COMMAND);
		}

		protected void ResumeScriptEngine()
		{
			this.scriptEngine.Resume(this.commandAddress);
		}

		public abstract string GetCommandName();

		public virtual bool GetParameter(string[] commandParams)
		{
			return true;
		}

		public virtual bool RunScriptCommand()
		{
			return true;
		}

		public virtual bool UpdateCommand()
		{
			return true;
		}

		public virtual void OnPauseAction()
		{
		}

		public virtual void OnResumeAction()
		{
		}
	}
}
