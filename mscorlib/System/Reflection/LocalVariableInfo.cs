using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	/// <summary>Discovers the attributes of a local variable and provides access to local variable metadata.</summary>
	[ComVisible(true)]
	public class LocalVariableInfo
	{
		internal Type type;

		internal bool is_pinned;

		internal ushort position;

		internal LocalVariableInfo()
		{
		}

		/// <summary>Gets a <see cref="T:System.Boolean" /> value indicating whether the object referred to by the local variable is pinned in memory.</summary>
		/// <returns>true if the object referred to by the variable is pinned in memory; otherwise, false.</returns>
		public virtual bool IsPinned
		{
			get
			{
				return this.is_pinned;
			}
		}

		/// <summary>Gets the index of the local variable within the method body.</summary>
		/// <returns>An integer value that represents the order of declaration of the local variable within the method body.</returns>
		public virtual int LocalIndex
		{
			get
			{
				return (int)this.position;
			}
		}

		/// <summary>Gets the type of the local variable.</summary>
		/// <returns>A <see cref="T:System.Type" /> object that represents the type of the local variable.</returns>
		public virtual Type LocalType
		{
			get
			{
				return this.type;
			}
		}

		/// <summary>Returns a user-readable string that describes the local variable.</summary>
		/// <returns>A string that displays information about the local variable, including the type name, index, and pinned status.</returns>
		public override string ToString()
		{
			if (this.is_pinned)
			{
				return string.Format("{0} ({1}) (pinned)", this.type, this.position);
			}
			return string.Format("{0} ({1})", this.type, this.position);
		}
	}
}
