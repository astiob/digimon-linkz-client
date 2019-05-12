using System;

namespace System.ComponentModel
{
	/// <summary>Defines the mechanism for querying the object for changes and resetting of the changed status.</summary>
	public interface IChangeTracking
	{
		/// <summary>Gets the object's changed status.</summary>
		/// <returns>true if the object’s content has changed since the last call to <see cref="M:System.ComponentModel.IChangeTracking.AcceptChanges" />; otherwise, false.</returns>
		bool IsChanged { get; }

		/// <summary>Resets the object’s state to unchanged by accepting the modifications.</summary>
		void AcceptChanges();
	}
}
