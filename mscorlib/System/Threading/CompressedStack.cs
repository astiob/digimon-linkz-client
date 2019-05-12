using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System.Threading
{
	/// <summary>Provides methods for setting and capturing the compressed stack on the current thread. This class cannot be inherited. </summary>
	/// <filterpriority>2</filterpriority>
	[Serializable]
	public sealed class CompressedStack : ISerializable
	{
		private ArrayList _list;

		internal CompressedStack(int length)
		{
			if (length > 0)
			{
				this._list = new ArrayList(length);
			}
		}

		internal CompressedStack(CompressedStack cs)
		{
			if (cs != null && cs._list != null)
			{
				this._list = (ArrayList)cs._list.Clone();
			}
		}

		/// <summary>Creates a copy of the current compressed stack.</summary>
		/// <returns>A <see cref="T:System.Threading.CompressedStack" /> object representing the current compressed stack.</returns>
		/// <filterpriority>2</filterpriority>
		[ComVisible(false)]
		public CompressedStack CreateCopy()
		{
			return new CompressedStack(this);
		}

		/// <summary>Captures the compressed stack from the current thread.</summary>
		/// <returns>A <see cref="T:System.Threading.CompressedStack" /> object.</returns>
		/// <filterpriority>1</filterpriority>
		public static CompressedStack Capture()
		{
			CompressedStack compressedStack = new CompressedStack(0);
			compressedStack._list = SecurityFrame.GetStack(1);
			CompressedStack compressedStack2 = Thread.CurrentThread.GetCompressedStack();
			if (compressedStack2 != null)
			{
				for (int i = 0; i < compressedStack2._list.Count; i++)
				{
					compressedStack._list.Add(compressedStack2._list[i]);
				}
			}
			return compressedStack;
		}

		/// <summary>Gets the compressed stack for the current thread.</summary>
		/// <returns>A <see cref="T:System.Threading.CompressedStack" /> for the current thread.</returns>
		/// <exception cref="T:System.Security.SecurityException">A caller in the call chain does not have permission to access unmanaged code.-or-The request for <see cref="T:System.Security.Permissions.StrongNameIdentityPermission" /> failed.</exception>
		/// <filterpriority>1</filterpriority>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		///   <IPermission class="System.Security.Permissions.StrongNameIdentityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PublicKeyBlob="00000000000000000400000000000000" />
		/// </PermissionSet>
		public static CompressedStack GetCompressedStack()
		{
			CompressedStack compressedStack = Thread.CurrentThread.GetCompressedStack();
			if (compressedStack == null)
			{
				compressedStack = CompressedStack.Capture();
			}
			else
			{
				CompressedStack compressedStack2 = CompressedStack.Capture();
				for (int i = 0; i < compressedStack2._list.Count; i++)
				{
					compressedStack._list.Add(compressedStack2._list[i]);
				}
			}
			return compressedStack;
		}

		/// <summary>Sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object with the logical context information needed to recreate an instance of this execution context.</summary>
		/// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> object to be populated with serialization information. </param>
		/// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> structure representing the destination context of the serialization. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <filterpriority>1</filterpriority>
		[MonoTODO("incomplete")]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
		}

		/// <summary>Runs a method in the specified compressed stack on the current thread.</summary>
		/// <param name="compressedStack">The <see cref="T:System.Threading.CompressedStack" /> to set.</param>
		/// <param name="callback">A <see cref="T:System.Threading.ContextCallback" /> that represents the method to be run in the specified security context.</param>
		/// <param name="state">The object to be passed to the callback method.</param>
		/// <exception cref="T:System.ArgumentException">
		///   <paramref name="compressedStack" /> is null.</exception>
		public static void Run(CompressedStack compressedStack, ContextCallback callback, object state)
		{
			if (compressedStack == null)
			{
				throw new ArgumentException("compressedStack");
			}
			Thread currentThread = Thread.CurrentThread;
			CompressedStack compressedStack2 = null;
			try
			{
				compressedStack2 = currentThread.GetCompressedStack();
				currentThread.SetCompressedStack(compressedStack);
				callback(state);
			}
			finally
			{
				if (compressedStack2 != null)
				{
					currentThread.SetCompressedStack(compressedStack2);
				}
			}
		}

		internal bool Equals(CompressedStack cs)
		{
			if (this.IsEmpty())
			{
				return cs.IsEmpty();
			}
			if (cs.IsEmpty())
			{
				return false;
			}
			if (this._list.Count != cs._list.Count)
			{
				return false;
			}
			for (int i = 0; i < this._list.Count; i++)
			{
				SecurityFrame securityFrame = (SecurityFrame)this._list[i];
				SecurityFrame sf = (SecurityFrame)cs._list[i];
				if (!securityFrame.Equals(sf))
				{
					return false;
				}
			}
			return true;
		}

		internal bool IsEmpty()
		{
			return this._list == null || this._list.Count == 0;
		}

		internal IList List
		{
			get
			{
				return this._list;
			}
		}
	}
}
