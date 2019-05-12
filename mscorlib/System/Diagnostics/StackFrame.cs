using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace System.Diagnostics
{
	/// <summary>Provides information about a <see cref="T:System.Diagnostics.StackFrame" />, which represents a function call on the call stack for the current thread.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	[MonoTODO("Serialized objects are not compatible with MS.NET")]
	[Serializable]
	public class StackFrame
	{
		/// <summary>Defines the value that is returned from the <see cref="M:System.Diagnostics.StackFrame.GetNativeOffset" /> or <see cref="M:System.Diagnostics.StackFrame.GetILOffset" /> method when the native or Microsoft intermediate language (MSIL) offset is unknown. This field is constant.</summary>
		/// <filterpriority>1</filterpriority>
		public const int OFFSET_UNKNOWN = -1;

		private int ilOffset = -1;

		private int nativeOffset = -1;

		private MethodBase methodBase;

		private string fileName;

		private int lineNumber;

		private int columnNumber;

		private string internalMethodName;

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class.</summary>
		public StackFrame()
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class, optionally capturing source information.</summary>
		/// <param name="fNeedFileInfo">true to capture the file name, line number, and column number of the stack frame; otherwise, false. </param>
		public StackFrame(bool fNeedFileInfo)
		{
			bool flag = StackFrame.get_frame_info(2, fNeedFileInfo, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class that corresponds to a frame above the current stack frame.</summary>
		/// <param name="skipFrames">The number of frames up the stack to skip. </param>
		public StackFrame(int skipFrames)
		{
			bool flag = StackFrame.get_frame_info(skipFrames + 2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class that corresponds to a frame above the current stack frame, optionally capturing source information.</summary>
		/// <param name="skipFrames">The number of frames up the stack to skip. </param>
		/// <param name="fNeedFileInfo">true to capture the file name, line number, and column number of the stack frame; otherwise, false. </param>
		public StackFrame(int skipFrames, bool fNeedFileInfo)
		{
			bool flag = StackFrame.get_frame_info(skipFrames + 2, fNeedFileInfo, out this.methodBase, out this.ilOffset, out this.nativeOffset, out this.fileName, out this.lineNumber, out this.columnNumber);
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class that contains only the given file name and line number.</summary>
		/// <param name="fileName">The file name. </param>
		/// <param name="lineNumber">The line number in the specified file. </param>
		public StackFrame(string fileName, int lineNumber)
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out fileName, out lineNumber, out this.columnNumber);
			this.fileName = fileName;
			this.lineNumber = lineNumber;
			this.columnNumber = 0;
		}

		/// <summary>Initializes a new instance of the <see cref="T:System.Diagnostics.StackFrame" /> class that contains only the given file name, line number, and column number.</summary>
		/// <param name="fileName">The file name. </param>
		/// <param name="lineNumber">The line number in the specified file. </param>
		/// <param name="colNumber">The column number in the specified file. </param>
		public StackFrame(string fileName, int lineNumber, int colNumber)
		{
			bool flag = StackFrame.get_frame_info(2, false, out this.methodBase, out this.ilOffset, out this.nativeOffset, out fileName, out lineNumber, out this.columnNumber);
			this.fileName = fileName;
			this.lineNumber = lineNumber;
			this.columnNumber = colNumber;
		}

		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool get_frame_info(int skip, bool needFileInfo, out MethodBase method, out int iloffset, out int native_offset, out string file, out int line, out int column);

		/// <summary>Gets the line number in the file that contains the code that is executing. This information is typically extracted from the debugging symbols for the executable.</summary>
		/// <returns>The file line number.-or- Zero if the file line number cannot be determined.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int GetFileLineNumber()
		{
			return this.lineNumber;
		}

		/// <summary>Gets the column number in the file that contains the code that is executing. This information is typically extracted from the debugging symbols for the executable.</summary>
		/// <returns>The file column number.-or- Zero if the file column number cannot be determined.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int GetFileColumnNumber()
		{
			return this.columnNumber;
		}

		/// <summary>Gets the file name that contains the code that is executing. This information is typically extracted from the debugging symbols for the executable.</summary>
		/// <returns>The file name.-or- null if the file name cannot be determined.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public virtual string GetFileName()
		{
			return this.fileName;
		}

		internal string GetSecureFileName()
		{
			string result = "<filename unknown>";
			if (this.fileName == null)
			{
				return result;
			}
			try
			{
				result = this.GetFileName();
			}
			catch (SecurityException)
			{
			}
			return result;
		}

		/// <summary>Gets the offset from the start of the Microsoft intermediate language (MSIL) code for the method that is executing. This offset might be an approximation depending on whether or not the just-in-time (JIT) compiler is generating debugging code. The generation of this debugging information is controlled by the <see cref="T:System.Diagnostics.DebuggableAttribute" />.</summary>
		/// <returns>The offset from the start of the MSIL code for the method that is executing.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int GetILOffset()
		{
			return this.ilOffset;
		}

		/// <summary>Gets the method in which the frame is executing.</summary>
		/// <returns>The method in which the frame is executing.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual MethodBase GetMethod()
		{
			return this.methodBase;
		}

		/// <summary>Gets the offset from the start of the native just-in-time (JIT)-compiled code for the method that is being executed. The generation of this debugging information is controlled by the <see cref="T:System.Diagnostics.DebuggableAttribute" /> class.</summary>
		/// <returns>The offset from the start of the JIT-compiled code for the method that is being executed.</returns>
		/// <filterpriority>2</filterpriority>
		public virtual int GetNativeOffset()
		{
			return this.nativeOffset;
		}

		internal string GetInternalMethodName()
		{
			return this.internalMethodName;
		}

		/// <summary>Builds a readable representation of the stack trace.</summary>
		/// <returns>A readable representation of the stack trace.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*" />
		/// </PermissionSet>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.methodBase == null)
			{
				stringBuilder.Append(Locale.GetText("<unknown method>"));
			}
			else
			{
				stringBuilder.Append(this.methodBase.Name);
			}
			stringBuilder.Append(Locale.GetText(" at "));
			if (this.ilOffset == -1)
			{
				stringBuilder.Append(Locale.GetText("<unknown offset>"));
			}
			else
			{
				stringBuilder.Append(Locale.GetText("offset "));
				stringBuilder.Append(this.ilOffset);
			}
			stringBuilder.Append(Locale.GetText(" in file:line:column "));
			stringBuilder.Append(this.GetSecureFileName());
			stringBuilder.AppendFormat(":{0}:{1}", this.lineNumber, this.columnNumber);
			return stringBuilder.ToString();
		}
	}
}
