using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Represents a collection of <see cref="T:System.Net.Mail.AlternateView" /> objects.</summary>
	public sealed class AlternateViewCollection : Collection<AlternateView>, IDisposable
	{
		internal AlternateViewCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.AlternateViewCollection" />.</summary>
		public void Dispose()
		{
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		protected override void InsertItem(int index, AlternateView item)
		{
			base.InsertItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		protected override void SetItem(int index, AlternateView item)
		{
			base.SetItem(index, item);
		}
	}
}
