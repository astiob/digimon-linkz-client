using System;
using System.IO;

internal class \uE014 : Stream
{
	private const int \uE000 = 8192;

	private string \uE001;

	private MemoryStream \uE002;

	private \uE007 \uE003;

	private bool \uE004;

	public override bool CanRead
	{
		get
		{
			return this.\uE002.CanRead;
		}
	}

	public override bool CanSeek
	{
		get
		{
			return this.\uE002.CanSeek;
		}
	}

	public override bool CanWrite
	{
		get
		{
			return this.\uE002.CanWrite;
		}
	}

	public override long Length
	{
		get
		{
			return this.\uE002.Length;
		}
	}

	public override long Position
	{
		get
		{
			return this.\uE002.Position;
		}
		set
		{
			this.\uE002.Position = value;
		}
	}

	public \uE014(string \uE018) : this(\uE018, 8192, null, null)
	{
	}

	public \uE014(string \uE019, \uE007 \uE01A) : this(\uE019, 8192, \uE01A, \uE01A)
	{
	}

	public \uE014(string \uE01B, int \uE01C, \uE007 \uE01D, \uE007 \uE01E)
	{
		this.\uE003 = \uE01E;
		this.\uE001 = \uE01B;
		this.\uE002 = new MemoryStream(\uE01C);
		if (!File.Exists(\uE01B))
		{
			return;
		}
		try
		{
			byte[] array = File.ReadAllBytes(\uE01B);
			if (\uE01D != null)
			{
				array = \uE01D.\uE005(array);
			}
			this.\uE002.Write(array, 0, array.Length);
			this.\uE002.Seek(0L, SeekOrigin.Begin);
		}
		catch
		{
		}
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		return this.\uE002.Read(buffer, offset, count);
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		return this.\uE002.Seek(offset, origin);
	}

	public override void SetLength(long value)
	{
		this.\uE002.SetLength(value);
		this.\uE004 = true;
	}

	public override void Write(byte[] buffer, int offset, int count)
	{
		this.\uE002.Write(buffer, offset, count);
		this.\uE004 = true;
	}

	public override void Flush()
	{
		if (!this.\uE004)
		{
			return;
		}
		this.\uE004 = false;
		using (FileStream fileStream = new FileStream(this.\uE001, FileMode.Create))
		{
			byte[] array = this.\uE002.GetBuffer();
			if (this.\uE003 != null)
			{
				array = this.\uE003.\uE003(array, 0, (int)this.\uE002.Length);
			}
			fileStream.Write(array, 0, array.Length);
		}
		byte[] buffer = this.\uE002.GetBuffer();
		Array.Clear(buffer, 0, buffer.Length);
		this.\uE002.Seek(0L, SeekOrigin.Begin);
		this.\uE002.SetLength(0L);
	}
}
