using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System
{
	/// <summary>Represents a type using an internal metadata token.</summary>
	/// <filterpriority>2</filterpriority>
	[MonoTODO("Serialization needs tests")]
	[ComVisible(true)]
	[Serializable]
	public struct RuntimeTypeHandle : ISerializable
	{
		private IntPtr value;

		internal RuntimeTypeHandle(IntPtr val)
		{
			this.value = val;
		}

		private RuntimeTypeHandle(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			MonoType monoType = (MonoType)info.GetValue("TypeObj", typeof(MonoType));
			this.value = monoType.TypeHandle.Value;
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException(Locale.GetText("Insufficient state."));
			}
		}

		/// <summary>Gets a handle to the type represented by this instance.</summary>
		/// <returns>A handle to the type represented by this instance.</returns>
		/// <filterpriority>2</filterpriority>
		public IntPtr Value
		{
			get
			{
				return this.value;
			}
		}

		/// <summary>Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with the data necessary to deserialize the type represented by the current instance.</summary>
		/// <param name="info">The object to be populated with serialization information. </param>
		/// <param name="context">(Reserved) The location where serialized data will be stored and retrieved. </param>
		/// <exception cref="T:System.ArgumentNullException">
		///   <paramref name="info" /> is null. </exception>
		/// <exception cref="T:System.Runtime.Serialization.SerializationException">
		///   <see cref="P:System.RuntimeTypeHandle.Value" /> is invalid. </exception>
		/// <filterpriority>2</filterpriority>
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.value == IntPtr.Zero)
			{
				throw new SerializationException("Object fields may not be properly initialized");
			}
			info.AddValue("TypeObj", Type.GetTypeHandle(this), typeof(MonoType));
		}

		/// <summary>Indicates whether the specified object is equal to the current <see cref="T:System.RuntimeTypeHandle" /> structure.</summary>
		/// <returns>true if <paramref name="obj" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is equal to the value of this instance; otherwise, false.</returns>
		/// <param name="obj">An object to compare to the current instance.</param>
		/// <filterpriority>2</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public override bool Equals(object obj)
		{
			return obj != null && base.GetType() == obj.GetType() && this.value == ((RuntimeTypeHandle)obj).Value;
		}

		/// <summary>Indicates whether the specified <see cref="T:System.RuntimeTypeHandle" /> structure is equal to the current <see cref="T:System.RuntimeTypeHandle" /> structure.</summary>
		/// <returns>true if the value of <paramref name="handle" /> is equal to the value of this instance; otherwise, false.</returns>
		/// <param name="handle">The <see cref="T:System.RuntimeTypeHandle" /> structure to compare to the current instance.</param>
		/// <filterpriority>2</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public bool Equals(RuntimeTypeHandle handle)
		{
			return this.value == handle.Value;
		}

		/// <summary>Returns the hash code for the current instance.</summary>
		/// <returns>A 32-bit signed integer hash code.</returns>
		/// <filterpriority>2</filterpriority>
		public override int GetHashCode()
		{
			return this.value.GetHashCode();
		}

		/// <summary>Gets a handle to the module that contains the type represented by the current instance.</summary>
		/// <returns>A <see cref="T:System.ModuleHandle" /> structure representing a handle to the module that contains the type represented by the current instance.</returns>
		/// <filterpriority>1</filterpriority>
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[CLSCompliant(false)]
		public ModuleHandle GetModuleHandle()
		{
			if (this.value == IntPtr.Zero)
			{
				throw new InvalidOperationException("Object fields may not be properly initialized");
			}
			return Type.GetTypeFromHandle(this).Module.ModuleHandle;
		}

		/// <summary>Indicates whether a <see cref="T:System.RuntimeTypeHandle" /> structure is equal to an object.</summary>
		/// <returns>true if <paramref name="right" /> is a <see cref="T:System.RuntimeTypeHandle" /> and is equal to <paramref name="left" />; otherwise, false.</returns>
		/// <param name="left">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="right" />.</param>
		/// <param name="right">An object to compare to <paramref name="left" />.</param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(RuntimeTypeHandle left, object right)
		{
			return right != null && right is RuntimeTypeHandle && left.Equals((RuntimeTypeHandle)right);
		}

		/// <summary>Indicates whether a <see cref="T:System.RuntimeTypeHandle" /> structure is not equal to an object.</summary>
		/// <returns>true if <paramref name="right" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is not equal to <paramref name="left" />; otherwise, false.</returns>
		/// <param name="left">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="right" />.</param>
		/// <param name="right">An object to compare to <paramref name="left" />.</param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(RuntimeTypeHandle left, object right)
		{
			return right == null || !(right is RuntimeTypeHandle) || !left.Equals((RuntimeTypeHandle)right);
		}

		/// <summary>Indicates whether an object and a <see cref="T:System.RuntimeTypeHandle" /> structure are equal.</summary>
		/// <returns>true if <paramref name="left" /> is a <see cref="T:System.RuntimeTypeHandle" /> structure and is equal to <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">An object to compare to <paramref name="right" />.</param>
		/// <param name="right">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="left" />.</param>
		/// <filterpriority>3</filterpriority>
		public static bool operator ==(object left, RuntimeTypeHandle right)
		{
			return left != null && left is RuntimeTypeHandle && ((RuntimeTypeHandle)left).Equals(right);
		}

		/// <summary>Indicates whether an object and a <see cref="T:System.RuntimeTypeHandle" /> structure are not equal.</summary>
		/// <returns>true if <paramref name="left" /> is a <see cref="T:System.RuntimeTypeHandle" /> and is not equal to <paramref name="right" />; otherwise, false.</returns>
		/// <param name="left">An object to compare to <paramref name="right" />.</param>
		/// <param name="right">A <see cref="T:System.RuntimeTypeHandle" /> structure to compare to <paramref name="left" />.</param>
		/// <filterpriority>3</filterpriority>
		public static bool operator !=(object left, RuntimeTypeHandle right)
		{
			return left == null || !(left is RuntimeTypeHandle) || !((RuntimeTypeHandle)left).Equals(right);
		}
	}
}
