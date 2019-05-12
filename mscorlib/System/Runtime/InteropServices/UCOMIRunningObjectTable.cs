using System;

namespace System.Runtime.InteropServices
{
	/// <summary>Use <see cref="T:System.Runtime.InteropServices.ComTypes.IRunningObjectTable" /> instead.</summary>
	[Obsolete]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("00000010-0000-0000-c000-000000000046")]
	[ComImport]
	public interface UCOMIRunningObjectTable
	{
		/// <summary>Registers that the supplied object has entered the running state.</summary>
		/// <param name="grfFlags">Specifies whether the Running Object Table's (ROT) reference to <paramref name="punkObject" /> is weak or strong, and controls access to the object through its entry in the ROT. </param>
		/// <param name="punkObject">Reference to the object being registered as running. </param>
		/// <param name="pmkObjectName">Reference to the moniker that identifies <paramref name="punkObject" />. </param>
		/// <param name="pdwRegister">Reference to a 32-bit value that can be used to identify this ROT entry in subsequent calls to <see cref="M:System.Runtime.InteropServices.UCOMIRunningObjectTable.Revoke(System.Int32)" /> or <see cref="M:System.Runtime.InteropServices.UCOMIRunningObjectTable.NoteChangeTime(System.Int32,System.Runtime.InteropServices.FILETIME@)" />. </param>
		void Register(int grfFlags, [MarshalAs(UnmanagedType.Interface)] object punkObject, UCOMIMoniker pmkObjectName, out int pdwRegister);

		/// <summary>Unregisters the specified object from the ROT.</summary>
		/// <param name="dwRegister">The ROT entry to revoke. </param>
		void Revoke(int dwRegister);

		/// <summary>Determines if the specified moniker is currently registered in the Running Object Table.</summary>
		/// <param name="pmkObjectName">Reference to the moniker to search for in the Running Object Table. </param>
		void IsRunning(UCOMIMoniker pmkObjectName);

		/// <summary>Returns the registered object if the supplied object name is registered as running.</summary>
		/// <param name="pmkObjectName">Reference to the moniker to search for in the ROT. </param>
		/// <param name="ppunkObject">On successful return, contains the requested running object. </param>
		void GetObject(UCOMIMoniker pmkObjectName, [MarshalAs(UnmanagedType.Interface)] out object ppunkObject);

		/// <summary>Makes a note of the time that a particular object has changed so IMoniker::GetTimeOfLastChange can report an appropriate change time.</summary>
		/// <param name="dwRegister">The ROT entry of the changed object. </param>
		/// <param name="pfiletime">Reference to the object's last change time. </param>
		void NoteChangeTime(int dwRegister, ref FILETIME pfiletime);

		/// <summary>Searches for this moniker in the ROT and reports the recorded time of change, if present.</summary>
		/// <param name="pmkObjectName">Reference to the moniker to search for in the ROT. </param>
		/// <param name="pfiletime">On successful return, contains the objects last change time. </param>
		void GetTimeOfLastChange(UCOMIMoniker pmkObjectName, out FILETIME pfiletime);

		/// <summary>Enumerates the objects currently registered as running.</summary>
		/// <param name="ppenumMoniker">On successful return, the new enumerator for the ROT. </param>
		void EnumRunning(out UCOMIEnumMoniker ppenumMoniker);
	}
}
