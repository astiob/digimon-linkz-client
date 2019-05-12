using System;

namespace System.IO
{
	[Flags]
	internal enum InotifyMask : uint
	{
		Access = 1u,
		Modify = 2u,
		Attrib = 4u,
		CloseWrite = 8u,
		CloseNoWrite = 16u,
		Open = 32u,
		MovedFrom = 64u,
		MovedTo = 128u,
		Create = 256u,
		Delete = 512u,
		DeleteSelf = 1024u,
		MoveSelf = 2048u,
		BaseEvents = 4095u,
		Umount = 8192u,
		Overflow = 16384u,
		Ignored = 32768u,
		OnlyDir = 16777216u,
		DontFollow = 33554432u,
		AddMask = 536870912u,
		Directory = 1073741824u,
		OneShot = 2147483648u
	}
}
