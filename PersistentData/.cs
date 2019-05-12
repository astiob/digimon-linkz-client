using PersistentData;
using System;

internal sealed class \uE00C : \uE00A
{
	public CryptoHelper cryptoHelper;

	public byte[] serializeData;

	public byte[] deserializeData;

	public void \uE000()
	{
		try
		{
			this.deserializeData = this.cryptoHelper.DecryptWithDES(this.serializeData);
		}
		catch (Exception ex)
		{
			this.isException = true;
			this.typeException = ex.GetType();
		}
		this.isFinished = true;
	}
}
