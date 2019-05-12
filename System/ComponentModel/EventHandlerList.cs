using System;

namespace System.ComponentModel
{
	/// <summary>Provides a simple list of delegates. This class cannot be inherited.</summary>
	public sealed class EventHandlerList : IDisposable
	{
		private ListEntry entries;

		private Delegate null_entry;

		/// <summary>Gets or sets the delegate for the specified object.</summary>
		/// <returns>The delegate for the specified key, or null if a delegate does not exist.</returns>
		/// <param name="key">An object to find in the list. </param>
		public Delegate this[object key]
		{
			get
			{
				if (key == null)
				{
					return this.null_entry;
				}
				ListEntry listEntry = this.FindEntry(key);
				if (listEntry != null)
				{
					return listEntry.value;
				}
				return null;
			}
			set
			{
				this.AddHandler(key, value);
			}
		}

		/// <summary>Adds a delegate to the list.</summary>
		/// <param name="key">The object that owns the event. </param>
		/// <param name="value">The delegate to add to the list. </param>
		public void AddHandler(object key, Delegate value)
		{
			if (key == null)
			{
				this.null_entry = Delegate.Combine(this.null_entry, value);
				return;
			}
			ListEntry listEntry = this.FindEntry(key);
			if (listEntry == null)
			{
				listEntry = new ListEntry();
				listEntry.key = key;
				listEntry.value = null;
				listEntry.next = this.entries;
				this.entries = listEntry;
			}
			listEntry.value = Delegate.Combine(listEntry.value, value);
		}

		/// <summary>Adds a list of delegates to the current list.</summary>
		/// <param name="listToAddFrom">The list to add.</param>
		public void AddHandlers(EventHandlerList listToAddFrom)
		{
			if (listToAddFrom == null)
			{
				return;
			}
			for (ListEntry next = listToAddFrom.entries; next != null; next = next.next)
			{
				this.AddHandler(next.key, next.value);
			}
		}

		/// <summary>Removes a delegate from the list.</summary>
		/// <param name="key">The object that owns the event. </param>
		/// <param name="value">The delegate to remove from the list. </param>
		public void RemoveHandler(object key, Delegate value)
		{
			if (key == null)
			{
				this.null_entry = Delegate.Remove(this.null_entry, value);
				return;
			}
			ListEntry listEntry = this.FindEntry(key);
			if (listEntry == null)
			{
				return;
			}
			listEntry.value = Delegate.Remove(listEntry.value, value);
		}

		/// <summary>Disposes the delegate list.</summary>
		public void Dispose()
		{
			this.entries = null;
		}

		private ListEntry FindEntry(object key)
		{
			for (ListEntry next = this.entries; next != null; next = next.next)
			{
				if (next.key == key)
				{
					return next;
				}
			}
			return null;
		}
	}
}
