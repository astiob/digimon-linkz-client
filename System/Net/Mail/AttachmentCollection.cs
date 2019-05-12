using System;
using System.Collections.ObjectModel;

namespace System.Net.Mail
{
	/// <summary>Stores attachments to be sent as part of an e-mail message.</summary>
	public sealed class AttachmentCollection : Collection<Attachment>, IDisposable
	{
		internal AttachmentCollection()
		{
		}

		/// <summary>Releases all resources used by the <see cref="T:System.Net.Mail.AttachmentCollection" />. </summary>
		public void Dispose()
		{
			for (int i = 0; i < this.Count; i++)
			{
				this[i].Dispose();
			}
		}

		protected override void ClearItems()
		{
			base.ClearItems();
		}

		protected override void InsertItem(int index, Attachment item)
		{
			base.InsertItem(index, item);
		}

		protected override void RemoveItem(int index)
		{
			base.RemoveItem(index);
		}

		protected override void SetItem(int index, Attachment item)
		{
			base.SetItem(index, item);
		}
	}
}
