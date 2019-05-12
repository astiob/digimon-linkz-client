using PersistentData;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security;

internal class \uE00B : \uE00A
{
	public CryptoHelper cryptoHelper;

	public byte[] persistentData;

	[CompilerGenerated]
	private string \uE000;

	public string Path { get; set; }

	public void \uE000()
	{
		if (File.Exists(this.Path))
		{
			try
			{
				byte[] data = File.ReadAllBytes(this.Path);
				data = this.cryptoHelper.DecryptWithDES(data);
				this.persistentData = data;
				goto IL_68;
			}
			catch (Exception ex)
			{
				this.isException = true;
				this.typeException = ex.GetType();
				goto IL_68;
			}
		}
		this.isException = true;
		this.typeException = typeof(Exception);
		IL_68:
		this.isFinished = true;
	}

	public void \uE001()
	{
		try
		{
			byte[] array = this.persistentData;
			array = this.cryptoHelper.EncryptWithDES(array);
			File.WriteAllBytes(this.Path, array);
		}
		catch (IOException ex)
		{
			this.isException = true;
			this.typeException = ex.GetType();
		}
		catch (UnauthorizedAccessException ex2)
		{
			this.isException = true;
			this.typeException = ex2.GetType();
		}
		catch (SecurityException ex3)
		{
			this.isException = true;
			this.typeException = ex3.GetType();
		}
		catch (Exception ex4)
		{
			this.isException = true;
			this.typeException = ex4.GetType();
		}
		this.isFinished = true;
	}
}
