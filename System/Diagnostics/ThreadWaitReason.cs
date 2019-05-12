using System;

namespace System.Diagnostics
{
	/// <summary>Specifies the reason a thread is waiting.</summary>
	/// <filterpriority>2</filterpriority>
	public enum ThreadWaitReason
	{
		/// <summary>The thread is waiting for event pair high.</summary>
		EventPairHigh = 7,
		/// <summary>The thread is waiting for event pair low.</summary>
		EventPairLow,
		/// <summary>Thread execution is delayed.</summary>
		ExecutionDelay = 4,
		/// <summary>The thread is waiting for the scheduler.</summary>
		Executive = 0,
		/// <summary>The thread is waiting for a free virtual memory page.</summary>
		FreePage,
		/// <summary>The thread is waiting for a local procedure call to arrive.</summary>
		LpcReceive = 9,
		/// <summary>The thread is waiting for reply to a local procedure call to arrive.</summary>
		LpcReply,
		/// <summary>The thread is waiting for a virtual memory page to arrive in memory.</summary>
		PageIn = 2,
		/// <summary>The thread is waiting for a virtual memory page to be written to disk.</summary>
		PageOut = 12,
		/// <summary>Thread execution is suspended.</summary>
		Suspended = 5,
		/// <summary>The thread is waiting for system allocation.</summary>
		SystemAllocation = 3,
		/// <summary>The thread is waiting for an unknown reason.</summary>
		Unknown = 13,
		/// <summary>The thread is waiting for a user request.</summary>
		UserRequest = 6,
		/// <summary>The thread is waiting for the system to allocate virtual memory.</summary>
		VirtualMemory = 11
	}
}
