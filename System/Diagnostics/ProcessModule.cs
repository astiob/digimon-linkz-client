using System;
using System.ComponentModel;

namespace System.Diagnostics
{
	/// <summary>Represents a.dll or .exe file that is loaded into a particular process.</summary>
	/// <filterpriority>2</filterpriority>
	[System.ComponentModel.Designer("System.Diagnostics.Design.ProcessModuleDesigner, System.Design, Version=2.0.5.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	public class ProcessModule : System.ComponentModel.Component
	{
		private IntPtr baseaddr;

		private IntPtr entryaddr;

		private string filename;

		private FileVersionInfo version_info;

		private int memory_size;

		private string modulename;

		internal ProcessModule(IntPtr baseaddr, IntPtr entryaddr, string filename, FileVersionInfo version_info, int memory_size, string modulename)
		{
			this.baseaddr = baseaddr;
			this.entryaddr = entryaddr;
			this.filename = filename;
			this.version_info = version_info;
			this.memory_size = memory_size;
			this.modulename = modulename;
		}

		/// <summary>Gets the memory address where the module was loaded.</summary>
		/// <returns>The load address of the module.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The base memory address of this module")]
		public IntPtr BaseAddress
		{
			get
			{
				return this.baseaddr;
			}
		}

		/// <summary>Gets the memory address for the function that runs when the system loads and runs the module.</summary>
		/// <returns>The entry point of the module.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
		/// </PermissionSet>
		[MonitoringDescription("The base memory address of the entry point of this module")]
		public IntPtr EntryPointAddress
		{
			get
			{
				return this.entryaddr;
			}
		}

		/// <summary>Gets the full path to the module.</summary>
		/// <returns>The fully qualified path that defines the location of the module.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The file name of this module")]
		public string FileName
		{
			get
			{
				return this.filename;
			}
		}

		/// <summary>Gets version information about the module.</summary>
		/// <returns>A <see cref="T:System.Diagnostics.FileVersionInfo" /> that contains the module's version information.</returns>
		/// <filterpriority>2</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
		/// </PermissionSet>
		[System.ComponentModel.Browsable(false)]
		public FileVersionInfo FileVersionInfo
		{
			get
			{
				return this.version_info;
			}
		}

		/// <summary>Gets the amount of memory that is required to load the module.</summary>
		/// <returns>The size, in bytes, of the memory that the module occupies.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The memory needed by this module")]
		public int ModuleMemorySize
		{
			get
			{
				return this.memory_size;
			}
		}

		/// <summary>Gets the name of the process module.</summary>
		/// <returns>The name of the module.</returns>
		/// <filterpriority>2</filterpriority>
		[MonitoringDescription("The name of this module")]
		public string ModuleName
		{
			get
			{
				return this.modulename;
			}
		}

		/// <summary>Converts the name of the module to a string.</summary>
		/// <returns>The value of the <see cref="P:System.Diagnostics.ProcessModule.ModuleName" /> property.</returns>
		/// <filterpriority>2</filterpriority>
		public override string ToString()
		{
			return this.ModuleName;
		}
	}
}
