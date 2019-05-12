using System;
using System.Reflection;

namespace System.Diagnostics
{
	/// <summary>Provides a set of methods and properties that help you trace the execution of your code. This class cannot be inherited.</summary>
	/// <filterpriority>1</filterpriority>
	public sealed class Trace
	{
		private Trace()
		{
		}

		/// <summary>Refreshes the trace configuration data.</summary>
		/// <filterpriority>1</filterpriority>
		[MonoNotSupported("")]
		public static void Refresh()
		{
			throw new NotImplementedException();
		}

		/// <summary>Gets or sets whether <see cref="M:System.Diagnostics.Trace.Flush" /> should be called on the <see cref="P:System.Diagnostics.Trace.Listeners" /> after every write.</summary>
		/// <returns>true if <see cref="M:System.Diagnostics.Trace.Flush" /> is called on the <see cref="P:System.Diagnostics.Trace.Listeners" /> after every write; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static bool AutoFlush
		{
			get
			{
				return TraceImpl.AutoFlush;
			}
			set
			{
				TraceImpl.AutoFlush = value;
			}
		}

		/// <summary>Gets or sets the indent level.</summary>
		/// <returns>The indent level. The default is zero.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static int IndentLevel
		{
			get
			{
				return TraceImpl.IndentLevel;
			}
			set
			{
				TraceImpl.IndentLevel = value;
			}
		}

		/// <summary>Gets or sets the number of spaces in an indent.</summary>
		/// <returns>The number of spaces in an indent. The default is four.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		public static int IndentSize
		{
			get
			{
				return TraceImpl.IndentSize;
			}
			set
			{
				TraceImpl.IndentSize = value;
			}
		}

		/// <summary>Gets the collection of listeners that is monitoring the trace output.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.TraceListenerCollection" /> that represents a collection of type <see cref="T:System.Diagnostics.TraceListener" /> monitoring the trace output.</returns>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		public static TraceListenerCollection Listeners
		{
			get
			{
				return TraceImpl.Listeners;
			}
		}

		/// <summary>Gets the correlation manager for the thread for this trace.</summary>
		/// <returns>The <see cref="T:System.Diagnostics.CorrelationManager" /> object associated with the thread for this trace.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public static CorrelationManager CorrelationManager
		{
			get
			{
				return TraceImpl.CorrelationManager;
			}
		}

		/// <summary>Gets or sets a value indicating whether the global lock should be used.  </summary>
		/// <returns>true if the global lock is to be used; otherwise, false. The default is true.</returns>
		public static bool UseGlobalLock
		{
			get
			{
				return TraceImpl.UseGlobalLock;
			}
			set
			{
				TraceImpl.UseGlobalLock = value;
			}
		}

		/// <summary>Checks for a condition; if the condition is false, displays a message box that shows the call stack.</summary>
		/// <param name="condition">The conditional expression to evaluate. If the condition is true, a failure message is not sent and the message box is not displayed.</param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Assert(bool condition)
		{
			TraceImpl.Assert(condition);
		}

		/// <summary>Checks for a condition; if the condition is false, outputs a specified message and displays a message box that shows the call stack.</summary>
		/// <param name="condition">The conditional expression to evaluate. If the condition is true, the specified message is not sent and the message box is not displayed.  </param>
		/// <param name="message">The message to send to the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message)
		{
			TraceImpl.Assert(condition, message);
		}

		/// <summary>Checks for a condition; if the condition is false, outputs two specified messages and displays a message box that shows the call stack.</summary>
		/// <param name="condition">The conditional expression to evaluate. If the condition is true, the specified messages are not sent and the message box is not displayed.  </param>
		/// <param name="message">The message to send to the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection. </param>
		/// <param name="detailMessage">The detailed message to send to the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Assert(bool condition, string message, string detailMessage)
		{
			TraceImpl.Assert(condition, message, detailMessage);
		}

		/// <summary>Flushes the output buffer, and then closes the <see cref="P:System.Diagnostics.Trace.Listeners" />.</summary>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Close()
		{
			TraceImpl.Close();
		}

		/// <summary>Emits the specified error message.</summary>
		/// <param name="message">A message to emit. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Fail(string message)
		{
			TraceImpl.Fail(message);
		}

		/// <summary>Emits an error message, and a detailed error message.</summary>
		/// <param name="message">A message to emit. </param>
		/// <param name="detailMessage">A detailed message to emit. </param>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Fail(string message, string detailMessage)
		{
			TraceImpl.Fail(message, detailMessage);
		}

		/// <summary>Flushes the output buffer, and causes buffered data to be written to the <see cref="P:System.Diagnostics.Trace.Listeners" />.</summary>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Flush()
		{
			TraceImpl.Flush();
		}

		/// <summary>Increases the current <see cref="P:System.Diagnostics.Trace.IndentLevel" /> by one.</summary>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Indent()
		{
			TraceImpl.Indent();
		}

		/// <summary>Decreases the current <see cref="P:System.Diagnostics.Trace.IndentLevel" /> by one.</summary>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Unindent()
		{
			TraceImpl.Unindent();
		}

		/// <summary>Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Write(object value)
		{
			TraceImpl.Write(value);
		}

		/// <summary>Writes a message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="message">A message to write. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Write(string message)
		{
			TraceImpl.Write(message);
		}

		/// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Write(object value, string category)
		{
			TraceImpl.Write(value, category);
		}

		/// <summary>Writes a category name and a message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="message">A message to write. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void Write(string message, string category)
		{
			TraceImpl.Write(message, category);
		}

		/// <summary>Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value)
		{
			TraceImpl.WriteIf(condition, value);
		}

		/// <summary>Writes a message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="message">A message to write. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message)
		{
			TraceImpl.WriteIf(condition, message);
		}

		/// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, object value, string category)
		{
			TraceImpl.WriteIf(condition, value, category);
		}

		/// <summary>Writes a category name and message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="message">A message to write. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteIf(bool condition, string message, string category)
		{
			TraceImpl.WriteIf(condition, message, category);
		}

		/// <summary>Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLine(object value)
		{
			TraceImpl.WriteLine(value);
		}

		/// <summary>Writes a message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="message">A message to write. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLine(string message)
		{
			TraceImpl.WriteLine(message);
		}

		/// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLine(object value, string category)
		{
			TraceImpl.WriteLine(value, category);
		}

		/// <summary>Writes a category name and message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection.</summary>
		/// <param name="message">A message to write. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLine(string message, string category)
		{
			TraceImpl.WriteLine(message, category);
		}

		/// <summary>Writes the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value)
		{
			TraceImpl.WriteLineIf(condition, value);
		}

		/// <summary>Writes a message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="message">A message to write. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message)
		{
			TraceImpl.WriteLineIf(condition, message);
		}

		/// <summary>Writes a category name and the value of the object's <see cref="M:System.Object.ToString" /> method to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="value">An <see cref="T:System.Object" /> whose name is sent to the <see cref="P:System.Diagnostics.Trace.Listeners" />. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, object value, string category)
		{
			TraceImpl.WriteLineIf(condition, value, category);
		}

		/// <summary>Writes a category name and message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection if a condition is true.</summary>
		/// <param name="condition">true to cause a message to be written; otherwise, false. </param>
		/// <param name="message">A message to write. </param>
		/// <param name="category">A category name used to organize the output. </param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void WriteLineIf(bool condition, string message, string category)
		{
			TraceImpl.WriteLineIf(condition, message, category);
		}

		private static void DoTrace(string kind, Assembly report, string message)
		{
			string arg = string.Empty;
			try
			{
				arg = report.Location;
			}
			catch (MethodAccessException)
			{
			}
			TraceImpl.WriteLine(string.Format("{0} {1} : 0 : {2}", arg, kind, message));
		}

		/// <summary>Writes an error message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified message.</summary>
		/// <param name="message">The informative message to write.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceError(string message)
		{
			Trace.DoTrace("Error", Assembly.GetCallingAssembly(), message);
		}

		/// <summary>Writes an error message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified array of objects and formatting information.</summary>
		/// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args" /> array.</param>
		/// <param name="args">An object array containing zero or more objects to format.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceError(string message, params object[] args)
		{
			Trace.DoTrace("Error", Assembly.GetCallingAssembly(), string.Format(message, args));
		}

		/// <summary>Writes an informational message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified message.</summary>
		/// <param name="message">The informative message to write.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceInformation(string message)
		{
			Trace.DoTrace("Information", Assembly.GetCallingAssembly(), message);
		}

		/// <summary>Writes an informational message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified array of objects and formatting information.</summary>
		/// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args" /> array.</param>
		/// <param name="args">An object array containing zero or more objects to format.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceInformation(string message, params object[] args)
		{
			Trace.DoTrace("Information", Assembly.GetCallingAssembly(), string.Format(message, args));
		}

		/// <summary>Writes a warning message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified message.</summary>
		/// <param name="message">The informative message to write.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceWarning(string message)
		{
			Trace.DoTrace("Warning", Assembly.GetCallingAssembly(), message);
		}

		/// <summary>Writes a warning message to the trace listeners in the <see cref="P:System.Diagnostics.Trace.Listeners" /> collection using the specified array of objects and formatting information.</summary>
		/// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args" /> array.</param>
		/// <param name="args">An object array containing zero or more objects to format.</param>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[Conditional("TRACE")]
		public static void TraceWarning(string message, params object[] args)
		{
			Trace.DoTrace("Warning", Assembly.GetCallingAssembly(), string.Format(message, args));
		}
	}
}
