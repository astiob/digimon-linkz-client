using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

[Obfuscation(Feature = "dead code", Exclude = true, StripAfterObfuscation = false)]
internal class \uE01E
{
	private static byte[] \uE000 = new byte[4];

	private static byte[] \uE001;

	static \uE01E()
	{
		for (;;)
		{
			int num = \uE01E.\uE006(93);
			for (;;)
			{
				switch (num)
				{
				case 0:
					\uE01E.\uE000[2] = (\uE01E.\uE001[2] = 65);
					num = \uE01E.\uE006(90);
					continue;
				case 1:
					\uE01E.\uE000[3] = 49;
					num = 4;
					continue;
				case 2:
					\uE01E.\uE001 = new byte[4];
					num = 5;
					continue;
				case 3:
					\uE01E.\uE000[1] = (\uE01E.\uE001[1] = 83);
					num = 0;
					continue;
				case 4:
					\uE01E.\uE001[3] = 50;
					num = 6;
					continue;
				case 5:
					\uE01E.\uE000[0] = (\uE01E.\uE001[0] = 82);
					num = 3;
					continue;
				case 6:
					return;
				}
				break;
			}
		}
	}

	public static MemoryStream \uE000(Stream \uE000)
	{
		BinaryReader binaryReader = new BinaryReader(\uE000);
		DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
		int num = (int)binaryReader.ReadUInt16();
		byte[] array = new byte[num];
		for (;;)
		{
			int num2 = \uE01E.\uE006(93);
			for (;;)
			{
				int num3;
				switch (num2)
				{
				case 0:
					num3 = 0;
					num2 = \uE01E.\uE006(90);
					continue;
				case 1:
					if (num3 != 0)
					{
						num2 = 4;
						continue;
					}
					goto IL_95;
				case 2:
					binaryReader.Read(array, 0, num);
					num2 = 5;
					continue;
				case 3:
				{
					byte[] array2;
					binaryReader.Read(array2, 0, 4);
					num2 = 0;
					continue;
				}
				case 4:
				{
					IL_6C:
					byte[] array2;
					array[num3] ^= array2[num3 % 4];
					num2 = 6;
					continue;
				}
				case 5:
				{
					byte[] array2 = new byte[4];
					num2 = 3;
					continue;
				}
				case 6:
					num3++;
					goto IL_95;
				}
				break;
				IL_95:
				if (num3 >= num)
				{
					goto Block_2;
				}
				goto IL_6C;
			}
		}
		Block_2:
		BinaryReader binaryReader2 = new BinaryReader(new MemoryStream(array, false));
		byte[] array3;
		int num6;
		byte[] array4;
		bool flag;
		bool flag2;
		RSACryptoServiceProvider rsacryptoServiceProvider;
		bool flag3;
		for (;;)
		{
			int num4 = \uE01E.\uE006(89);
			for (;;)
			{
				switch (num4)
				{
				case 0:
				{
					int num5 = binaryReader2.ReadInt32();
					num4 = 1;
					continue;
				}
				case 1:
				{
					int num5;
					array3 = new byte[num5];
					num4 = 4;
					continue;
				}
				case 2:
					array4 = new byte[num6];
					num4 = 7;
					continue;
				case 3:
					flag = binaryReader2.ReadBoolean();
					num4 = 0;
					continue;
				case 4:
				{
					int num5;
					binaryReader2.Read(array3, 0, num5);
					num4 = 9;
					continue;
				}
				case 5:
					num6 = (int)binaryReader2.ReadByte();
					num4 = 2;
					continue;
				case 6:
					flag2 = binaryReader2.ReadBoolean();
					num4 = 3;
					continue;
				case 7:
					rsacryptoServiceProvider = null;
					num4 = 8;
					continue;
				case 8:
					if (flag3)
					{
						num4 = \uE026.\uE001(86);
						continue;
					}
					goto IL_19D;
				case 9:
					flag3 = binaryReader2.ReadBoolean();
					num4 = 5;
					continue;
				case 10:
					binaryReader2.ReadString();
					num4 = 6;
					continue;
				case 11:
					goto IL_19A;
				}
				break;
			}
		}
		IL_19A:
		bool flag4 = true;
		goto IL_1A0;
		IL_19D:
		flag4 = false;
		IL_1A0:
		if (!flag4)
		{
			byte[] publicKey = Assembly.GetExecutingAssembly().GetName().GetPublicKey();
			if (publicKey == null || publicKey.Length != 160)
			{
				throw new InvalidOperationException();
			}
			Buffer.BlockCopy(publicKey, 12, array4, 0, num6);
			for (;;)
			{
				int num7 = \uE01E.\uE006(92);
				for (;;)
				{
					switch (num7)
					{
					case 0:
					{
						byte[] array5 = array4;
						int num8 = 5;
						array5[num8] |= 128;
						num7 = 1;
						continue;
					}
					case 1:
						rsacryptoServiceProvider = new RSACryptoServiceProvider();
						num7 = 2;
						continue;
					case 2:
						goto IL_224;
					}
					break;
				}
			}
			IL_224:
			rsacryptoServiceProvider.ImportParameters(\uE01E.\uE003(publicKey));
		}
		if (flag2)
		{
			if (flag3)
			{
				binaryReader2.Read(array4, 0, num6);
			}
			int num9 = (int)binaryReader2.ReadByte();
			byte[] array6 = new byte[num9];
			binaryReader2.Read(array6, 0, num9);
			descryptoServiceProvider.IV = array6;
			descryptoServiceProvider.Key = array4;
		}
		MemoryStream memoryStream = new MemoryStream();
		if (flag2)
		{
			using (CryptoStream cryptoStream = new CryptoStream(binaryReader.BaseStream, descryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read))
			{
				if (flag)
				{
					\uE026.\uE000(cryptoStream, memoryStream);
					goto IL_30B;
				}
				\uE01E.\uE002(cryptoStream, memoryStream);
				goto IL_30B;
			}
		}
		if (flag)
		{
			\uE026.\uE000(binaryReader.BaseStream, memoryStream);
		}
		else
		{
			\uE01E.\uE002(binaryReader.BaseStream, memoryStream);
		}
		IL_30B:
		if (rsacryptoServiceProvider != null)
		{
			memoryStream.Position = 0L;
			if (!\uE01E.\uE005(rsacryptoServiceProvider, memoryStream, array3))
			{
				throw new InvalidOperationException();
			}
		}
		memoryStream.Position = 0L;
		return memoryStream;
	}

	private static byte[] \uE001(byte[] \uE000, int \uE001, int \uE002)
	{
		if (\uE000 != null)
		{
			if (\uE000.Length >= \uE001 + \uE002)
			{
				byte[] array = new byte[\uE002];
				Array.Copy(\uE000, \uE001, array, 0, \uE002);
				return array;
			}
		}
		return null;
	}

	private static void \uE002(Stream \uE000, Stream \uE001)
	{
		byte[] array = new byte[4096];
		for (;;)
		{
			int num = \uE000.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			\uE001.Write(array, 0, num);
		}
	}

	private static RSAParameters \uE003(byte[] \uE000)
	{
		bool flag = \uE000.Length == 160;
		if (flag && !\uE01E.\uE004(\uE000, \uE01E.\uE000, 20))
		{
			return default(RSAParameters);
		}
		if (!flag && !\uE01E.\uE004(\uE000, \uE01E.\uE001, 8))
		{
			return default(RSAParameters);
		}
		RSAParameters rsaparameters = default(RSAParameters);
		int num = flag ? 20 : 8;
		int num3;
		for (;;)
		{
			int num2 = \uE026.\uE001(87);
			for (;;)
			{
				switch (num2)
				{
				case 0:
					Array.Reverse(rsaparameters.Exponent);
					num2 = 2;
					continue;
				case 1:
					rsaparameters.Modulus = \uE01E.\uE001(\uE000, num, num3);
					num2 = \uE026.\uE001(88);
					continue;
				case 2:
					num += num3;
					num2 = 7;
					continue;
				case 3:
					if (flag)
					{
						num2 = 9;
						continue;
					}
					goto IL_150;
				case 4:
					Array.Reverse(rsaparameters.Modulus);
					num2 = 3;
					continue;
				case 5:
					num3 = 4;
					num2 = 8;
					continue;
				case 6:
					num += 8;
					num2 = 5;
					continue;
				case 7:
					num3 = 128;
					num2 = 1;
					continue;
				case 8:
					rsaparameters.Exponent = \uE01E.\uE001(\uE000, num, num3);
					num2 = 0;
					continue;
				case 9:
					goto IL_14D;
				}
				break;
			}
		}
		IL_14D:
		bool flag2 = false;
		goto IL_153;
		IL_150:
		flag2 = true;
		IL_153:
		if (!flag2)
		{
			return rsaparameters;
		}
		num += num3;
		for (;;)
		{
			int num4 = \uE026.\uE001(91);
			for (;;)
			{
				switch (num4)
				{
				case 0:
					Array.Reverse(rsaparameters.InverseQ);
					num4 = 22;
					continue;
				case 1:
					rsaparameters.DP = \uE01E.\uE001(\uE000, num, num3);
					num4 = 8;
					continue;
				case 2:
					Array.Reverse(rsaparameters.Q);
					num4 = 19;
					continue;
				case 3:
					rsaparameters.P = \uE01E.\uE001(\uE000, num, num3);
					num4 = 21;
					continue;
				case 4:
					num += num3;
					num4 = 12;
					continue;
				case 5:
					num += num3;
					num4 = 17;
					continue;
				case 6:
					rsaparameters.DQ = \uE01E.\uE001(\uE000, num, num3);
					num4 = 20;
					continue;
				case 7:
					num3 = 128;
					num4 = 15;
					continue;
				case 8:
					Array.Reverse(rsaparameters.DP);
					num4 = 16;
					continue;
				case 9:
					Array.Reverse(rsaparameters.D);
					num4 = 23;
					continue;
				case 10:
					num3 = 64;
					num4 = 6;
					continue;
				case 11:
					num3 = 64;
					num4 = 1;
					continue;
				case 12:
					num3 = 64;
					num4 = 13;
					continue;
				case 13:
					rsaparameters.Q = \uE01E.\uE001(\uE000, num, num3);
					num4 = \uE01E.\uE006(93);
					continue;
				case 14:
					rsaparameters.InverseQ = \uE01E.\uE001(\uE000, num, num3);
					num4 = 0;
					continue;
				case 15:
					rsaparameters.D = \uE01E.\uE001(\uE000, num, num3);
					num4 = 9;
					continue;
				case 16:
					num += num3;
					num4 = 10;
					continue;
				case 17:
					num3 = 64;
					num4 = 14;
					continue;
				case 18:
					num3 = 64;
					num4 = 3;
					continue;
				case 19:
					num += num3;
					num4 = 11;
					continue;
				case 20:
					Array.Reverse(rsaparameters.DQ);
					num4 = 5;
					continue;
				case 21:
					Array.Reverse(rsaparameters.P);
					num4 = 4;
					continue;
				case 22:
					num += num3;
					num4 = 7;
					continue;
				case 23:
					return rsaparameters;
				}
				break;
			}
		}
		return rsaparameters;
	}

	private static bool \uE004(byte[] \uE000, byte[] \uE001, int \uE002)
	{
		int num = 0;
		for (;;)
		{
			int num2 = \uE01E.\uE006(92);
			for (;;)
			{
				switch (num2)
				{
				case 0:
					if (num != 0)
					{
						num2 = 1;
						continue;
					}
					goto IL_33;
				case 1:
					IL_19:
					if (\uE000[num + \uE002] != \uE001[num])
					{
						num2 = 2;
						continue;
					}
					num++;
					goto IL_33;
				case 2:
					return false;
				}
				break;
				IL_33:
				if (num >= \uE001.Length)
				{
					return true;
				}
				goto IL_19;
			}
		}
		return false;
	}

	private static bool \uE005(RSACryptoServiceProvider \uE000, Stream \uE001, byte[] \uE002)
	{
		byte[] rgbHash = new SHA1CryptoServiceProvider().ComputeHash(\uE001);
		return \uE000.VerifyHash(rgbHash, null, \uE002);
	}

	internal static int \uE006(int \uE000)
	{
		switch (\uE000 - -(-(~1129906944 ^ -1129907034)))
		{
		case 0:
			return -(~(~(1389397944 ^ 1658424322)) ^ -805899188);
		case 1:
			return -(~(1942746433 ^ 770038162) ^ 1580084435);
		case 3:
			return -(-(-(~-1)));
		case 4:
			return -(-(-1844670117) ^ -1844670117);
		case 5:
			return -(~-1122463986) ^ -1621359574 ^ 574917414;
		}
		return -(~(-(-(~1020400643)) ^ 389943043) ^ 736893697);
	}
}
