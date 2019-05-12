using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	/// <summary>Defines the lock that implements single-writer/multiple-reader semantics. This is a value type.</summary>
	/// <filterpriority>2</filterpriority>
	[ComVisible(true)]
	public struct LockCookie
	{
		internal int ThreadId;

		internal int ReaderLocks;

		internal int WriterLocks;

		internal LockCookie(int thread_id)
		{
			this.ThreadId = thread_id;
			this.ReaderLocks = 0;
			this.WriterLocks = 0;
		}

		internal LockCookie(int thread_id, int reader_locks, int writer_locks)
		{
			this.ThreadId = thread_id;
			this.ReaderLocks = reader_locks;
			this.WriterLocks = writer_locks;
		}

		/// <summary>Returns the hash code for this instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		/// <summary>Indicates whether the current instance is equal to the specified <see cref="T:System.Threading.LockCookie" />.</summary>
		/// <returns>true if <paramref name="obj" /> is equal to the value of the current instance; otherwise, false.</returns>
		/// <param name="obj">The <see cref="T:System.Threading.LockCookie" /> to compare to the current instance.</param>
		public bool Equals(LockCookie obj)
		{
			return this.ThreadId == obj.ThreadId && this.ReaderLocks == obj.ReaderLocks && this.WriterLocks == obj.WriterLocks;
		}

		/// <summary>Indicates whether a specified object is a <see cref="T:System.Threading.LockCookie" /> and is equal to the current instance.</summary>
		/// <returns>true if the value of <paramref name="obj" /> is equal to the value of the current instance; otherwise, false.</returns>
		/// <param name="obj">The object to compare to the current instance.</param>
		public override bool Equals(object obj)
		{
			return obj is LockCookie && obj.Equals(this);
		}

		/// <summary>Indicates whether two <see cref="T:System.Threading.LockCookie" /> structures are equal.</summary>
		/// <returns>true if <paramref name="a" /> is equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="a" />.</param>
		public static bool operator ==(LockCookie a, LockCookie b)
		{
			return a.Equals(b);
		}

		/// <summary>Indicates whether two <see cref="T:System.Threading.LockCookie" /> structures are not equal.</summary>
		/// <returns>true if <paramref name="a" /> is not equal to <paramref name="b" />; otherwise, false.</returns>
		/// <param name="a">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="b" />.</param>
		/// <param name="b">The <see cref="T:System.Threading.LockCookie" /> to compare to <paramref name="a" />.</param>
		public static bool operator !=(LockCookie a, LockCookie b)
		{
			return !a.Equals(b);
		}
	}
}
