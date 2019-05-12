using System;

namespace System.Security.Cryptography
{
	/// <summary>Determines the set of valid key sizes for the symmetric cryptographic algorithms.</summary>
	public sealed class KeySizes
	{
		private int _maxSize;

		private int _minSize;

		private int _skipSize;

		/// <summary>Initializes a new instance of the <see cref="T:System.Security.Cryptography.KeySizes" /> class with the specified key values.</summary>
		/// <param name="minSize">The minimum valid key size. </param>
		/// <param name="maxSize">The maximum valid key size. </param>
		/// <param name="skipSize">The interval between valid key sizes. </param>
		public KeySizes(int minSize, int maxSize, int skipSize)
		{
			this._maxSize = maxSize;
			this._minSize = minSize;
			this._skipSize = skipSize;
		}

		/// <summary>Specifies the maximum key size in bits.</summary>
		/// <returns>The maximum key size in bits.</returns>
		public int MaxSize
		{
			get
			{
				return this._maxSize;
			}
		}

		/// <summary>Specifies the minimum key size in bits.</summary>
		/// <returns>The minimum key size in bits.</returns>
		public int MinSize
		{
			get
			{
				return this._minSize;
			}
		}

		/// <summary>Specifies the interval between valid key sizes in bits.</summary>
		/// <returns>The interval between valid key sizes in bits.</returns>
		public int SkipSize
		{
			get
			{
				return this._skipSize;
			}
		}

		internal bool IsLegal(int keySize)
		{
			int num = keySize - this.MinSize;
			bool flag = num >= 0 && keySize <= this.MaxSize;
			return (this.SkipSize != 0) ? (flag && num % this.SkipSize == 0) : flag;
		}

		internal static bool IsLegalKeySize(KeySizes[] legalKeys, int size)
		{
			foreach (KeySizes keySizes in legalKeys)
			{
				if (keySizes.IsLegal(size))
				{
					return true;
				}
			}
			return false;
		}
	}
}
