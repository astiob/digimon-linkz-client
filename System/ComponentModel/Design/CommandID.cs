using System;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	/// <summary>Represents a unique command identifier that consists of a numeric command ID and a GUID menu group identifier.</summary>
	[ComVisible(true)]
	public class CommandID
	{
		private int cID;

		private Guid guid;

		/// <summary>Initializes a new instance of the <see cref="T:System.ComponentModel.Design.CommandID" /> class using the specified menu group GUID and command ID number.</summary>
		/// <param name="menuGroup">The GUID of the group that this menu command belongs to. </param>
		/// <param name="commandID">The numeric identifier of this menu command. </param>
		public CommandID(Guid menuGroup, int commandID)
		{
			this.cID = commandID;
			this.guid = menuGroup;
		}

		/// <summary>Gets the GUID of the menu group that the menu command identified by this <see cref="T:System.ComponentModel.Design.CommandID" /> belongs to.</summary>
		/// <returns>The GUID of the command group for this command.</returns>
		public virtual Guid Guid
		{
			get
			{
				return this.guid;
			}
		}

		/// <summary>Gets the numeric command ID.</summary>
		/// <returns>The command ID number.</returns>
		public virtual int ID
		{
			get
			{
				return this.cID;
			}
		}

		/// <summary>Determines whether two <see cref="T:System.ComponentModel.Design.CommandID" /> instances are equal.</summary>
		/// <returns>true if the specified object is equivalent to this one; otherwise, false.</returns>
		/// <param name="obj">The object to compare. </param>
		public override bool Equals(object obj)
		{
			return obj is CommandID && (obj == this || (((CommandID)obj).Guid.Equals(this.guid) && ((CommandID)obj).ID.Equals(this.cID)));
		}

		/// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
		public override int GetHashCode()
		{
			return this.guid.GetHashCode() ^ this.cID.GetHashCode();
		}

		/// <summary>Returns a <see cref="T:System.String" /> that represents the current object.</summary>
		/// <returns>A string that contains the command ID information, both the GUID and integer identifier. </returns>
		public override string ToString()
		{
			return this.guid.ToString() + " : " + this.cID.ToString();
		}
	}
}
