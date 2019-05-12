using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Stores linked resources to be sent as part of an e-mail message.</summary>
	public sealed class LinkedResourceCollection : Collection<LinkedResource>, IDisposable
	{
		internal LinkedResourceCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.LinkedResourceCollection" />.</summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		protected override void InsertItem(int index, LinkedResource item)
		{
			base.InsertItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		protected override void SetItem(int index, LinkedResource item)
		{
			base.SetItem(index, item);
		}
	}
}
