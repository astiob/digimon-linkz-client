using System;
using System.Collections;

namespace System.IO.IsolatedStorage
{
	internal class IsolatedStorageFileEnumerator : IEnumerator
	{
		private IsolatedStorageScope _scope;

		private string[] _storages;

		private int _pos;

		public IsolatedStorageFileEnumerator(IsolatedStorageScope scope, string root)
		{
			this._scope = scope;
			if (Directory.Exists(root))
			{
				this._storages = Directory.GetDirectories(root, "d.*");
			}
			this._pos = -1;
		}

		public object Current
		{
			get
			{
				if (this._pos < 0 || this._storages == null || this._pos >= this._storages.Length)
				{
					return null;
				}
				return new IsolatedStorageFile(this._scope, this._storages[this._pos]);
			}
		}

		public bool MoveNext()
		{
			return this._storages != null && ++this._pos < this._storages.Length;
		}

		public void Reset()
		{
			this._pos = -1;
		}
	}
}
