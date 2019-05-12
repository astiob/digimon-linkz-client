using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

[Obfuscation(Feature = "dead code", Exclude = true, StripAfterObfuscation = false)]
internal class \uE01A
{
	private static byte[] \uE005 = new byte[4];

	private static byte[] \uE006;

	static \uE01A()
	{
		for (;;)
		{
			int num = \uE01A.\uE00D(49);
			for (;;)
			{
				switch (num)
				{
				case 0:
					\uE01A.\uE006 = new byte[4];
					num = 3;
					continue;
				case 1:
					\uE01A.\uE005[1] = (\uE01A.\uE006[1] = 83);
					num = \uE01A.\uE00D(52);
					continue;
				case 2:
					\uE01A.\uE006[3] = 50;
					num = 6;
					continue;
				case 3:
					\uE01A.\uE005[0] = (\uE01A.\uE006[0] = 82);
					num = 1;
					continue;
				case 4:
					\uE01A.\uE005[2] = (\uE01A.\uE006[2] = 65);
					num = 5;
					continue;
				case 5:
					\uE01A.\uE005[3] = 49;
					num = 2;
					continue;
				case 6:
					return;
				}
				break;
			}
		}
	}

	public static Stream \uE007(Stream \uE071)
	{
		BinaryReader binaryReader = new BinaryReader(\uE071);
		DESCryptoServiceProvider descryptoServiceProvider = new DESCryptoServiceProvider();
		int num = (int)binaryReader.ReadUInt16();
		byte[] array = new byte[num];
		for (;;)
		{
			int num2 = \uE020.\uE001(43);
			for (;;)
			{
				int num3;
				switch (num2)
				{
				case 0:
					if (num3 != 0)
					{
						num2 = 2;
						continue;
					}
					goto IL_8F;
				case 1:
				{
					byte[] array2 = new byte[4];
					num2 = 4;
					continue;
				}
				case 2:
				{
					IL_4C:
					byte[] array2;
					array[num3] ^= array2[num3 % 4];
					num2 = 6;
					continue;
				}
				case 3:
					num3 = 0;
					num2 = 0;
					continue;
				case 4:
				{
					byte[] array2;
					binaryReader.Read(array2, 0, 4);
					num2 = 3;
					continue;
				}
				case 5:
					binaryReader.Read(array, 0, num);
					num2 = 1;
					continue;
				case 6:
					num3++;
					goto IL_8F;
				}
				break;
				IL_8F:
				if (num3 >= num)
				{
					goto Block_1;
				}
				goto IL_4C;
			}
		}
		Block_1:
		BinaryReader binaryReader2 = new BinaryReader(new MemoryStream(array, false));
		int num5;
		byte[] array3;
		bool flag;
		byte[] array4;
		bool flag2;
		bool flag3;
		RSACryptoServiceProvider rsacryptoServiceProvider;
		for (;;)
		{
			int num4 = \uE020.\uE001(45);
			for (;;)
			{
				switch (num4)
				{
				case 0:
					array3 = new byte[num5];
					num4 = 10;
					continue;
				case 1:
					flag = binaryReader2.ReadBoolean();
					num4 = 5;
					continue;
				case 2:
				{
					int num6;
					binaryReader2.Read(array4, 0, num6);
					num4 = 1;
					continue;
				}
				case 3:
				{
					int num6 = binaryReader2.ReadInt32();
					num4 = 8;
					continue;
				}
				case 4:
					flag2 = binaryReader2.ReadBoolean();
					num4 = 3;
					continue;
				case 5:
					num5 = (int)binaryReader2.ReadByte();
					num4 = 0;
					continue;
				case 6:
					flag3 = binaryReader2.ReadBoolean();
					num4 = 4;
					continue;
				case 7:
					binaryReader2.ReadString();
					num4 = 6;
					continue;
				case 8:
				{
					int num6;
					array4 = new byte[num6];
					num4 = 2;
					continue;
				}
				case 9:
					if (flag)
					{
						num4 = \uE020.\uE001(53);
						continue;
					}
					goto IL_19B;
				case 10:
					rsacryptoServiceProvider = null;
					num4 = 9;
					continue;
				case 11:
					goto IL_198;
				}
				break;
			}
		}
		IL_198:
		bool flag4 = true;
		goto IL_19E;
		IL_19B:
		flag4 = false;
		IL_19E:
		if (!flag4)
		{
			byte[] publicKey = Assembly.GetExecutingAssembly().GetName().GetPublicKey();
			if (publicKey == null || publicKey.Length != 160)
			{
				throw new InvalidOperationException();
			}
			Buffer.BlockCopy(publicKey, 12, array3, 0, num5);
			for (;;)
			{
				int num7 = \uE01A.\uE00D(46);
				for (;;)
				{
					switch (num7)
					{
					case 0:
						rsacryptoServiceProvider = new RSACryptoServiceProvider();
						num7 = 2;
						continue;
					case 1:
					{
						byte[] array5 = array3;
						int num8 = 5;
						array5[num8] |= 128;
						num7 = 0;
						continue;
					}
					case 2:
						goto IL_222;
					}
					break;
				}
			}
			IL_222:
			rsacryptoServiceProvider.ImportParameters(\uE01A.\uE00A(publicKey));
		}
		if (flag3)
		{
			if (flag)
			{
				binaryReader2.Read(array3, 0, num5);
			}
			int num9 = (int)binaryReader2.ReadByte();
			byte[] array6 = new byte[num9];
			binaryReader2.Read(array6, 0, num9);
			descryptoServiceProvider.IV = array6;
			descryptoServiceProvider.Key = array3;
		}
		MemoryStream memoryStream = new MemoryStream();
		if (flag3)
		{
			using (CryptoStream cryptoStream = new CryptoStream(binaryReader.BaseStream, descryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Read))
			{
				if (flag2)
				{
					\uE020.\uE000(cryptoStream, memoryStream);
					goto IL_309;
				}
				\uE01A.\uE009(cryptoStream, memoryStream);
				goto IL_309;
			}
		}
		if (flag2)
		{
			\uE020.\uE000(binaryReader.BaseStream, memoryStream);
		}
		else
		{
			\uE01A.\uE009(binaryReader.BaseStream, memoryStream);
		}
		IL_309:
		if (rsacryptoServiceProvider != null)
		{
			memoryStream.Position = 0L;
			if (!\uE01A.\uE00C(rsacryptoServiceProvider, memoryStream, array4))
			{
				throw new InvalidOperationException();
			}
		}
		memoryStream.Position = 0L;
		return memoryStream;
	}

	private static byte[] \uE008(byte[] \uE072, int \uE073, int \uE074)
	{
		if (\uE072 != null)
		{
			if (\uE072.Length >= \uE073 + \uE074)
			{
				byte[] array = new byte[\uE074];
				Array.Copy(\uE072, \uE073, array, 0, \uE074);
				return array;
			}
		}
		return null;
	}

	private static void \uE009(Stream \uE075, Stream \uE076)
	{
		byte[] array = new byte[4096];
		for (;;)
		{
			int num = \uE075.Read(array, 0, array.Length);
			if (num <= 0)
			{
				break;
			}
			\uE076.Write(array, 0, num);
		}
	}

	private static RSAParameters \uE00A(byte[] \uE077)
	{
		bool flag = \uE077.Length == 160;
		if (flag && !\uE01A.\uE00B(\uE077, \uE01A.\uE005, 20))
		{
			return default(RSAParameters);
		}
		if (!flag && !\uE01A.\uE00B(\uE077, \uE01A.\uE006, 8))
		{
			return default(RSAParameters);
		}
		RSAParameters rsaparameters = default(RSAParameters);
		int num = flag ? 20 : 8;
		int num3;
		for (;;)
		{
			int num2 = \uE020.\uE001(62);
			for (;;)
			{
				switch (num2)
				{
				case 0:
					if (flag)
					{
						num2 = 9;
						continue;
					}
					goto IL_150;
				case 1:
					num3 = 128;
					num2 = 6;
					continue;
				case 2:
					num += num3;
					num2 = \uE01A.\uE00D(46);
					continue;
				case 3:
					num3 = 4;
					num2 = 7;
					continue;
				case 4:
					Array.Reverse(rsaparameters.Modulus);
					num2 = 0;
					continue;
				case 5:
					Array.Reverse(rsaparameters.Exponent);
					num2 = 2;
					continue;
				case 6:
					rsaparameters.Modulus = \uE01A.\uE008(\uE077, num, num3);
					num2 = 4;
					continue;
				case 7:
					rsaparameters.Exponent = \uE01A.\uE008(\uE077, num, num3);
					num2 = 5;
					continue;
				case 8:
					num += 8;
					num2 = 3;
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
			int num4 = \uE01A.\uE00D(42);
			for (;;)
			{
				switch (num4)
				{
				case 0:
					num3 = 64;
					num4 = 2;
					continue;
				case 1:
					num += num3;
					num4 = 11;
					continue;
				case 2:
					rsaparameters.DQ = \uE01A.\uE008(\uE077, num, num3);
					num4 = 14;
					continue;
				case 3:
					num3 = 64;
					num4 = 6;
					continue;
				case 4:
					rsaparameters.Q = \uE01A.\uE008(\uE077, num, num3);
					num4 = 9;
					continue;
				case 5:
					num += num3;
					num4 = 0;
					continue;
				case 6:
					rsaparameters.P = \uE01A.\uE008(\uE077, num, num3);
					num4 = 8;
					continue;
				case 7:
					num += num3;
					num4 = 10;
					continue;
				case 8:
					Array.Reverse(rsaparameters.P);
					num4 = 20;
					continue;
				case 9:
					Array.Reverse(rsaparameters.Q);
					num4 = 1;
					continue;
				case 10:
					num3 = 128;
					num4 = 21;
					continue;
				case 11:
					num3 = 64;
					num4 = 12;
					continue;
				case 12:
					rsaparameters.DP = \uE01A.\uE008(\uE077, num, num3);
					num4 = 15;
					continue;
				case 13:
					rsaparameters.InverseQ = \uE01A.\uE008(\uE077, num, num3);
					num4 = 17;
					continue;
				case 14:
					Array.Reverse(rsaparameters.DQ);
					num4 = 18;
					continue;
				case 15:
					Array.Reverse(rsaparameters.DP);
					num4 = 5;
					continue;
				case 16:
					num3 = 64;
					num4 = 4;
					continue;
				case 17:
					Array.Reverse(rsaparameters.InverseQ);
					num4 = \uE020.\uE001(45);
					continue;
				case 18:
					num += num3;
					num4 = 19;
					continue;
				case 19:
					num3 = 64;
					num4 = 13;
					continue;
				case 20:
					num += num3;
					num4 = 16;
					continue;
				case 21:
					rsaparameters.D = \uE01A.\uE008(\uE077, num, num3);
					num4 = 22;
					continue;
				case 22:
					Array.Reverse(rsaparameters.D);
					num4 = 23;
					continue;
				case 23:
					return rsaparameters;
				}
				break;
			}
		}
		return rsaparameters;
	}

	private static bool \uE00B(byte[] \uE078, byte[] \uE079, int \uE07A)
	{
		int num = 0;
		for (;;)
		{
			int num2 = \uE01A.\uE00D(46);
			for (;;)
			{
				switch (num2)
				{
				case 0:
					IL_12:
					if (\uE078[num + \uE07A] != \uE079[num])
					{
						num2 = 2;
						continue;
					}
					num++;
					goto IL_33;
				case 1:
					if (num != 0)
					{
						num2 = 0;
						continue;
					}
					goto IL_33;
				case 2:
					return false;
				}
				break;
				IL_33:
				if (num >= \uE079.Length)
				{
					return true;
				}
				goto IL_12;
			}
		}
		return false;
	}

	private static bool \uE00C(RSACryptoServiceProvider \uE07B, Stream \uE07C, byte[] \uE07D)
	{
		byte[] rgbHash = new SHA1CryptoServiceProvider().ComputeHash(\uE07C);
		return \uE07B.VerifyHash(rgbHash, null, \uE07D);
	}

	internal static int \uE00D(int \uE07E)
	{
		switch (\uE07E - ~(~(42 ^ -2147483648) ^ -2147483648))
		{
		case 0:
			return -(~(-(~1 ^ int.MinValue) ^ int.MinValue));
		case 4:
			return -(-(-int.MaxValue) ^ int.MinValue) ^ int.MinValue ^ int.MinValue;
		case 5:
			return ~(~(~(~-2147483633)) ^ int.MinValue);
		case 6:
			return -(~(-(-(~-2)))) ^ int.MinValue ^ int.MinValue;
		case 7:
			return -(~-1 ^ int.MinValue) ^ int.MinValue;
		case 9:
			return ~(~(~(~(--17))) ^ int.MinValue) ^ int.MinValue;
		case 10:
			return -(-(-(~(~(~-2147483645)) ^ int.MinValue)));
		}
		return ~(~(~(~(~0))));
	}
}
