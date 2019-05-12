using System;
using System.Net.Sockets;

namespace System.Net
{
	/// <summary>Stores serialized information from <see cref="T:System.Net.EndPoint" /> derived classes.</summary>
	public class SocketAddress
	{
		private byte[] data;

		/// <summary>Creates a new instance of the <see cref="T:System.Net.SocketAddress" /> class using the specified address family and buffer size.</summary>
		/// <param name="family">An <see cref="T:System.Net.Sockets.AddressFamily" /> enumerated value. </param>
		/// <param name="size">The number of bytes to allocate for the underlying buffer. </param>
		/// <exception cref="T:System.ArgumentOutOfRangeException">
		///   <paramref name="size" /> is less than 2. These 2 bytes are needed to store <paramref name="family" />. </exception>
		public SocketAddress(System.Net.Sockets.AddressFamily family, int size)
		{
			if (size < 2)
			{
				throw new ArgumentOutOfRangeException("size is too small");
			}
			this.data = new byte[size];
			this.data[0] = (byte)family;
			this.data[1] = (byte)(family >> 8);
		}

		/// <summary>Creates a new instance of the <see cref="T:System.Net.SocketAddress" /> class for the given address family.</summary>
		/// <param name="family">An <see cref="T:System.Net.Sockets.AddressFamily" /> enumerated value. </param>
		public SocketAddress(System.Net.Sockets.AddressFamily family) : this(family, 32)
		{
		}

		/// <summary>Gets the <see cref="T:System.Net.Sockets.AddressFamily" /> enumerated value of the current <see cref="T:System.Net.SocketAddress" />.</summary>
		/// <returns>One of the <see cref="T:System.Net.Sockets.AddressFamily" /> enumerated values.</returns>
		public System.Net.Sockets.AddressFamily Family
		{
			get
			{
				return (System.Net.Sockets.AddressFamily)((int)this.data[0] + ((int)this.data[1] << 8));
			}
		}

		/// <summary>Gets the underlying buffer size of the <see cref="T:System.Net.SocketAddress" />.</summary>
		/// <returns>The underlying buffer size of the <see cref="T:System.Net.SocketAddress" />.</returns>
		public int Size
		{
			get
			{
				return this.data.Length;
			}
		}

		/// <summary>Gets or sets the specified index element in the underlying buffer.</summary>
		/// <returns>The value of the specified index element in the underlying buffer.</returns>
		/// <param name="offset">The array index element of the desired information. </param>
		/// <exception cref="T:System.IndexOutOfRangeException">The specified index does not exist in the buffer. </exception>
		public byte this[int offset]
		{
			get
			{
				return this.data[offset];
			}
			set
			{
				this.data[offset] = value;
			}
		}

		/// <summary>Returns information about the socket address.</summary>
		/// <returns>A string that contains information about the <see cref="T:System.Net.SocketAddress" />.</returns>
		/// <PermissionSet>
		///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode" />
		/// </PermissionSet>
		public override string ToString()
		{
			string text = ((System.Net.Sockets.AddressFamily)this.data[0]).ToString();
			int num = this.data.Length;
			string text2 = string.Concat(new object[]
			{
				text,
				":",
				num,
				":{"
			});
			for (int i = 2; i < num; i++)
			{
				int num2 = (int)this.data[i];
				text2 += num2;
				if (i < num - 1)
				{
					text2 += ",";
				}
			}
			return text2 + "}";
		}

		/// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Net.SocketAddress" /> instance.</summary>
		/// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
		/// <param name="comparand">The specified <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Net.SocketAddress" /> instance.</param>
		public override bool Equals(object obj)
		{
			SocketAddress socketAddress = obj as SocketAddress;
			if (socketAddress != null && socketAddress.data.Length == this.data.Length)
			{
				byte[] array = socketAddress.data;
				for (int i = 0; i < this.data.Length; i++)
				{
					if (array[i] != this.data[i])
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		/// <summary>Serves as a hash function for a particular type, suitable for use in hashing algorithms and data structures like a hash table.</summary>
		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			int num = 0;
			for (int i = 0; i < this.data.Length; i++)
			{
				num += (int)this.data[i] + i;
			}
			return num;
		}
	}
}
