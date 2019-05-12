using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Diagnostics
{
	/// <summary>Provides the default output methods and behavior for tracing.</summary>
	/// <filterpriority>1</filterpriority>
	public class DefaultTraceListener : TraceListener
	{
		private const string ConsoleOutTrace = "Console.Out";

		private const string ConsoleErrorTrace = "Console.Error";

		private static readonly bool OnWin32 = Path.DirectorySeparatorChar == '\\';

		private static readonly string MonoTracePrefix;

		private static readonly string MonoTraceFile;

		private string logFileName;

		private bool assertUiEnabled;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.DefaultTraceListener" /> class with "Default" as its <see cref="P:System.Diagnostics.TraceListener.Name" /> property value.</summary>
		public DefaultTraceListener() : base("Default")
		{
		}

		static DefaultTraceListener()
		{
			if (!DefaultTraceListener.OnWin32)
			{
				string environmentVariable = Environment.GetEnvironmentVariable("MONO_TRACE_LISTENER");
				if (environmentVariable != null)
				{
					string monoTraceFile;
					string monoTracePrefix;
					if (environmentVariable.StartsWith("Console.Out"))
					{
						monoTraceFile = "Console.Out";
						monoTracePrefix = DefaultTraceListener.GetPrefix(environmentVariable, "Console.Out");
					}
					else if (environmentVariable.StartsWith("Console.Error"))
					{
						monoTraceFile = "Console.Error";
						monoTracePrefix = DefaultTraceListener.GetPrefix(environmentVariable, "Console.Error");
					}
					else
					{
						monoTraceFile = environmentVariable;
						monoTracePrefix = string.Empty;
					}
					DefaultTraceListener.MonoTraceFile = monoTraceFile;
					DefaultTraceListener.MonoTracePrefix = monoTracePrefix;
				}
			}
		}

		private static string GetPrefix(string var, string target)
		{
			if (var.Length > target.Length)
			{
				return var.Substring(target.Length + 1);
			}
			return string.Empty;
		}

		/// <summary>Gets or sets a value indicating whether the application is running in user-interface mode.</summary>
		/// <returns>true if user-interface mode is enabled; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public bool AssertUiEnabled
		{
			get
			{
				return this.assertUiEnabled;
			}
			set
			{
				this.assertUiEnabled = value;
			}
		}

		/// <summary>Gets or sets the name of a log file to write trace or debug messages to.</summary>
		/// <returns>The name of a log file to write trace or debug messages to.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[MonoTODO]
		public string LogFileName
		{
			get
			{
				return this.logFileName;
			}
			set
			{
				this.logFileName = value;
			}
		}

		/// <summary>Emits or displays a message and a stack trace for an assertion that always fails.</summary>
		/// <param name="message">The message to emit or display. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Fail(string message)
		{
			base.Fail(message);
		}

		/// <summary>Emits or displays detailed messages and a stack trace for an assertion that always fails.</summary>
		/// <param name="message">The message to emit or display. </param>
		/// <param name="detailMessage">The detailed message to emit or display. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		///   <IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		public override void Fail(string message, string detailMessage)
		{
			base.Fail(message, detailMessage);
			if (this.ProcessUI(message, detailMessage) == DefaultTraceListener.DialogResult.Abort)
			{
				try
				{
					Thread.CurrentThread.Abort();
				}
				catch (MethodAccessException)
				{
				}
			}
			this.WriteLine(new StackTrace().ToString());
		}

		private DefaultTraceListener.DialogResult ProcessUI(string message, string detailMessage)
		{
			if (!this.AssertUiEnabled)
			{
				return DefaultTraceListener.DialogResult.None;
			}
			object obj;
			MethodInfo method;
			try
			{
				Assembly assembly = Assembly.Load("System.Windows.Forms, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
				if (assembly == null)
				{
					return DefaultTraceListener.DialogResult.None;
				}
				Type type = assembly.GetType("System.Windows.Forms.MessageBoxButtons");
				obj = Enum.Parse(type, "AbortRetryIgnore");
				method = assembly.GetType("System.Windows.Forms.MessageBox").GetMethod("Show", new Type[]
				{
					typeof(string),
					typeof(string),
					type
				});
			}
			catch
			{
				return DefaultTraceListener.DialogResult.None;
			}
			if (method == null || obj == null)
			{
				return DefaultTraceListener.DialogResult.None;
			}
			string text = string.Format("Assertion Failed: {0} to quit, {1} to debug, {2} to continue", "Abort", "Retry", "Ignore");
			string text2 = string.Format("{0}{1}{2}{1}{1}{3}", new object[]
			{
				message,
				Environment.NewLine,
				detailMessage,
				new StackTrace()
			});
			string text3 = method.Invoke(null, new object[]
			{
				text2,
				text,
				obj
			}).ToString();
			if (text3 != null)
			{
				if (DefaultTraceListener.<>f__switch$map0 == null)
				{
					DefaultTraceListener.<>f__switch$map0 = new Dictionary<string, int>(2)
					{
						{
							"Ignore",
							0
						},
						{
							"Abort",
							1
						}
					};
				}
				int num;
				if (DefaultTraceListener.<>f__switch$map0.TryGetValue(text3, out num))
				{
					if (num == 0)
					{
						return DefaultTraceListener.DialogResult.Ignore;
					}
					if (num == 1)
					{
						return DefaultTraceListener.DialogResult.Abort;
					}
				}
			}
			return DefaultTraceListener.DialogResult.Retry;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void WriteWindowsDebugString(string message);

		private void WriteDebugString(string message)
		{
			if (DefaultTraceListener.OnWin32)
			{
				DefaultTraceListener.WriteWindowsDebugString(message);
			}
			else
			{
				this.WriteMonoTrace(message);
			}
		}

		private void WriteMonoTrace(string message)
		{
			string monoTraceFile = DefaultTraceListener.MonoTraceFile;
			if (monoTraceFile != null)
			{
				if (DefaultTraceListener.<>f__switch$map1 == null)
				{
					DefaultTraceListener.<>f__switch$map1 = new Dictionary<string, int>(2)
					{
						{
							"Console.Out",
							0
						},
						{
							"Console.Error",
							1
						}
					};
				}
				int num;
				if (DefaultTraceListener.<>f__switch$map1.TryGetValue(monoTraceFile, out num))
				{
					if (num == 0)
					{
						Console.Out.Write(message);
						return;
					}
					if (num == 1)
					{
						Console.Error.Write(message);
						return;
					}
				}
			}
			this.WriteLogFile(message, DefaultTraceListener.MonoTraceFile);
		}

		private void WritePrefix()
		{
			if (!DefaultTraceListener.OnWin32)
			{
				this.WriteMonoTrace(DefaultTraceListener.MonoTracePrefix);
			}
		}

		private void WriteImpl(string message)
		{
			if (base.NeedIndent)
			{
				this.WriteIndent();
				this.WritePrefix();
			}
			this.WriteDebugString(message);
			if (Debugger.IsLogging())
			{
				Debugger.Log(0, null, message);
			}
			this.WriteLogFile(message, this.LogFileName);
		}

		private void WriteLogFile(string message, string logFile)
		{
			try
			{
				this.WriteLogFileImpl(message, logFile);
			}
			catch (MethodAccessException)
			{
			}
		}

		private void WriteLogFileImpl(string message, string logFile)
		{
			if (logFile != null && logFile.Length != 0)
			{
				FileInfo fileInfo = new FileInfo(logFile);
				StreamWriter streamWriter = null;
				try
				{
					if (fileInfo.Exists)
					{
						streamWriter = fileInfo.AppendText();
					}
					else
					{
						streamWriter = fileInfo.CreateText();
					}
				}
				catch
				{
					return;
				}
				using (streamWriter)
				{
					streamWriter.Write(message);
					streamWriter.Flush();
				}
			}
		}

		/// <summary>Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" /> method.</summary>
		/// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" />. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public override void Write(string message)
		{
			this.WriteImpl(message);
		}

		/// <summary>Writes the output to the OutputDebugString function and to the <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" /> method, followed by a carriage return and line feed (\r\n).</summary>
		/// <param name="message">The message to write to OutputDebugString and <see cref="M:System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)" />. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public override void WriteLine(string message)
		{
			string message2 = message + Environment.NewLine;
			this.WriteImpl(message2);
			base.NeedIndent = true;
		}

		private enum DialogResult
		{
			None,
			Retry,
			Ignore,
			Abort
		}
	}
}
