using System;
using System.Collections.ObjectModel;
using System.Text;

namespace System.Net.Mail
{
	/// <summary>Store e-mail addresses that are associated with an e-mail message.</summary>
	public class MailAddressCollection : Collection<MailAddress>
	{
		/// <summary>Add a list of e-mail addresses to the collection.</summary>
		/// <param name="addresses">The e-mail addresses to add to the <see cref="T:System.Net.Mail.MailAddressCollection" />. Multiple e-mail addresses must be separated with a comma character (","). </param>
		/// <exception cref="T:System.ArgumentNullException">The<paramref name=" addresses" /> parameter is null.</exception>
		/// <exception cref="T:System.ArgumentException">The<paramref name=" addresses" /> parameter is an empty string.</exception>
		/// <exception cref="T:System.FormatException">The<paramref name=" addresses" /> parameter contains an e-mail address that is invalid or not supported. </exception>
		public void Add(string addresses)
		{
			foreach (string address in addresses.Split(new char[]
			{
				','
			}))
			{
				this.Add(new MailAddress(address));
			}
		}

		/// <summary>Inserts an e-mail address into the <see cref="T:System.Net.Mail.MailAddressCollection" />, at the specified location.</summary>
		/// <param name="index">The location at which to insert the e-mail address that is specified by <paramref name="item" />.</param>
		/// <param name="item">The e-mail address to be inserted into the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">The<paramref name=" item" /> parameter is null.</exception>
		protected override void InsertItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException();
			}
			base.InsertItem(index, item);
		}

		/// <summary>Replaces the element at the specified index.</summary>
		/// <param name="index">The index of the e-mail address element to be replaced.</param>
		/// <param name="item">An e-mail address that will replace the element in the collection.</param>
		/// <exception cref="T:System.ArgumentNullException">The<paramref name=" item" /> parameter is null.</exception>
		protected override void SetItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException();
			}
			base.SetItem(index, item);
		}

		/// <summary>Returns a string representation of the e-mail addresses in this <see cref="T:System.Net.Mail.MailAddressCollection" /> object.</summary>
		/// <returns>A <see cref="T:System.String" /> containing the e-mail addresses in this collection.</returns>
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.Count; i++)
			{
				if (i > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(this[i].ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
