using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

internal sealed class \uE019
{
	private static string \uE002;

	private static \uE019.\uE000 \uE003;

	static \uE019()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		\uE019.\uE003 = new \uE019.\uE000(\uE019.\uE001);
		Stream uE = \uE01A.\uE007(executingAssembly.GetManifestResourceStream(\uE019.\uE003(-(-(-(~(~0)))))));
		\uE019.\uE002 = new \uE019.\uE001().\uE001(uE);
	}

	public static string \uE000(int \uE06A)
	{
		return (string)((Hashtable)AppDomain.CurrentDomain.GetData(\uE019.\uE002))[\uE06A];
	}

	public static string \uE001(int \uE06B)
	{
		char[] array = "#&+3".ToCharArray();
		int num = array.Length;
		while ((num -= (-(-(-(-(~(~(~-2 ^ -2147483648 ^ -2147483648)))))) ^ -2147483648 ^ -2147483648)) >= (-(-(-0)) ^ -2147483648 ^ -2147483648))
		{
			array[num] = (char)((int)array[num] ^ -(-(-(-(-(-(-(2147483548 ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue) ^ \uE06B);
		}
		return new string(array);
	}

	private delegate string \uE000(int \uE06E);

	private sealed class \uE001
	{
		public MethodBuilder \uE000(TypeBuilder \uE06F)
		{
			MethodAttributes attributes = ~(~(~(-(-(MethodAttributes)2147483497) ^ (MethodAttributes)(-2147483648))));
			MethodInfo method;
			string[] array3;
			MethodInfo method2;
			MethodInfo method3;
			MethodInfo method4;
			MethodInfo method5;
			MethodInfo method6;
			MethodInfo method7;
			MethodBuilder methodBuilder;
			Type type;
			ConstructorInfo constructor;
			ConstructorInfo constructor2;
			ConstructorInfo constructor3;
			MethodInfo method8;
			MethodInfo method9;
			MethodInfo method10;
			MethodInfo method11;
			MethodInfo method12;
			MethodInfo method13;
			for (;;)
			{
				int num = ~(-(-(-(-(~(-(-(~-5) ^ int.MinValue)))) ^ int.MinValue)));
				for (;;)
				{
					num ^= -(~(~(~(~(~(~(--2147483630 ^ int.MinValue) ^ int.MinValue))))) ^ int.MinValue);
					switch (num - -(-(-(-(~2147483628 ^ -2147483648 ^ -2147483648 ^ -2147483648 ^ -2147483648 ^ -2147483648)))))
					{
					case 0:
					{
						byte[] array;
						array[~(~(~-48 ^ int.MinValue)) ^ int.MinValue] = (byte)((int)array[~(~47 ^ int.MinValue ^ int.MinValue)] ^ (~(~198 ^ int.MinValue) ^ int.MinValue));
						array[-(-(~2147483599 ^ int.MinValue))] = (byte)((int)array[~(~(-2147483600) ^ int.MinValue)] ^ -(-(-(--2147483603) ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						array[~(~(~(~(~(-(-(~(-(-49))))) ^ int.MinValue)) ^ int.MinValue))] = (byte)((int)array[~(~(~(-(-(-50 ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue ^ int.MinValue)] ^ ~(-44 ^ int.MinValue ^ int.MinValue));
						array[~(-(-(~(~(~(-(-(--66) ^ int.MinValue ^ int.MinValue) ^ int.MinValue))) ^ int.MinValue)))] = (byte)((int)array[-(-(~(-(~(-(~2147483579)))))) ^ int.MinValue] ^ -(~(~(~(-(~-2147483548 ^ int.MinValue))))));
						num = (~(-(~(-52 ^ int.MinValue))) ^ int.MinValue);
						continue;
					}
					case 1:
					{
						byte[] array;
						array[~(~(-(~38 ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue)] = (byte)((int)array[~(-(-(-(~(~40 ^ int.MinValue) ^ int.MinValue))))] ^ -(-(-(-(-2147483568 ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue)));
						array[~(-(~(~(-(~(~(2147483607 ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue))] = (byte)((int)array[-(-(~(-41 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue ^ int.MinValue] ^ (~(-(-(-(-(-(~2147483415) ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue));
						array[~(-(~(-(~(-(~-45 ^ int.MinValue)))) ^ int.MinValue))] = (byte)((int)array[~(-(~(-43 ^ int.MinValue))) ^ int.MinValue] ^ -(~(~(~(~(~(-(-2 ^ int.MinValue) ^ int.MinValue))) ^ int.MinValue)) ^ int.MinValue));
						array[~(~(~(-(-(-43) ^ int.MinValue))) ^ int.MinValue)] = (byte)((int)array[-(-(-(~41 ^ int.MinValue ^ int.MinValue)))] ^ ~(-(-(-(-(~(~(2147483530 ^ int.MinValue ^ int.MinValue)))) ^ int.MinValue))));
						num = ~(~(~(~(~(-(~(-(~(--2147483597)) ^ int.MinValue))))) ^ int.MinValue ^ int.MinValue));
						continue;
					}
					case 2:
					{
						Type typeFromHandle = typeof(string);
						string name = "IndexOf";
						BindingFlags bindingAttr = ~(~(~(-(BindingFlags)(-2147483595))) ^ (BindingFlags)(-2147483648));
						Binder binder = null;
						Type[] array2 = new Type[~(-(~(-(--4 ^ int.MinValue) ^ int.MinValue)))];
						array2[~(-(~(~(~(-(~-3))))))] = typeof(string);
						array2[-(-(--1) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue] = typeof(StringComparison);
						method = typeFromHandle.GetMethod(name, bindingAttr, binder, array2, null);
						for (;;)
						{
							int num2 = -(~(-2147483637) ^ int.MinValue);
							for (;;)
							{
								switch ((num2 ^ -(~(~(~(19 ^ -2147483648)) ^ -2147483648))) - ~(~(~(-24))))
								{
								case 0:
									num = (~(~(~(~(-(-(~(-52)) ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue ^ int.MinValue);
									num2 = ~(~(-(~(-(~(~-14))) ^ int.MinValue ^ int.MinValue)));
									continue;
								case 1:
								{
									Type typeFromHandle2 = typeof(Environment);
									string name2 = array3[-(~(~(2147483640 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue)];
									BindingFlags bindingAttr2 = -(~((BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648))) ^ (BindingFlags)(-2147483648);
									Binder binder2 = null;
									Type[] array4 = new Type[-(-(-2147483647 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue)];
									array4[-(-(~(~0)))] = typeof(int);
									method2 = typeFromHandle2.GetMethod(name2, bindingAttr2, binder2, array4, null);
									num2 = (~(-(~(~(~(~(~(-(~-16))))) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue) ^ int.MinValue);
									continue;
								}
								case 2:
									method3 = typeof(StackTrace).GetMethod(array3[-(-(~(-(~(-(-2147483637 ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue)], -(~(-(-(-(~(-((BindingFlags)2147483598 ^ (BindingFlags)(-2147483648))))) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648)))) ^ (BindingFlags)(-2147483648), null, new Type[-(-(~(~(~(-(~-2 ^ int.MinValue)) ^ int.MinValue))))], null);
									num2 = -(~(-(-(-(~(-(-(12 ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue))) ^ int.MinValue));
									continue;
								case 3:
									method4 = typeof(Stream).GetMethod(array3[~(~(~(2147483637 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue], ~(-(-(~(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648)), null, new Type[~(~(-(~(-(~(-(-2147483646 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue))], null);
									num2 = -(-(~2147483644) ^ int.MinValue);
									continue;
								case 4:
									goto IL_6FF;
								}
								break;
							}
						}
						IL_6FF:
						continue;
					}
					case 3:
					{
						byte[] array = Convert.FromBase64String("WT8NY3IKPDBJDEszF4ICAUlTPg9zSXQ9JVQ0ND1rbhwtfG9dBhVoNZNcMzcLTRSnQE47R2VZVHlwZUZyb21IYW5kCmU7Z2V0X05hbWU7SW5kZXhPZjtFeGl0O2dldF9GcmFtZUNvdW50O2dldF9MZW5ndGg7UmVhZFN0cmluZztBZGQ7Z2V0X1Bvc2l0aW9uO2dldF9DdXJyZW50RG9tYWluO1NldERhdGE7UnVudGltZU1ldGhvZDtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tUcmFjZTtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tGcmFtZTsxMTY5MjtTeXN0ZW0uRW52aXJvbm1lbnQ7ZGU0ZG90O1NpbXBsZUFzc2VtYmx5RXhwbG9yZXI7YmFiZWx2bTtzbW9rZXRlc3Q=");
						array[~(-(~(-(~(~-2147483646 ^ int.MinValue)))) ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue] = (byte)((int)array[-(-0 ^ int.MinValue ^ int.MinValue)] ^ -(-(~(~(~(-(-(-31 ^ int.MinValue))))) ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						array[-(-(-int.MaxValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)] = (array[~(-2 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue] ^ -(-(-(-(-(-90))))));
						array[-(-(~(-(-(-3)) ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue] = (byte)((int)array[-(-2 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue] ^ (~(~(-(-(-(~(120 ^ int.MinValue ^ int.MinValue) ^ int.MinValue) ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue));
						num = ~(~(-(-(~(~(~(~(~2147483642) ^ int.MinValue)))))));
						continue;
					}
					case 4:
					{
						byte[] array;
						array[~(-(-(-(-2147483644 ^ int.MinValue ^ int.MinValue ^ int.MinValue))))] = (byte)((int)array[~(-(-(-4))) ^ int.MinValue ^ int.MinValue] ^ -(~(~(~(~(-37 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue)));
						array[~(-(-(~(~(2147483643 ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue))] = (byte)((int)array[-(~(-(-(~-4) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)))] ^ (-(-(-(-0) ^ int.MinValue)) ^ int.MinValue));
						array[~(~(-(2147483643 ^ int.MinValue ^ int.MinValue))) ^ int.MinValue] = (byte)((int)array[-(-(~(~(5 ^ int.MinValue) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue] ^ (-(-(--107 ^ int.MinValue)) ^ int.MinValue));
						array[-(~-2147483643 ^ int.MinValue ^ int.MinValue) ^ int.MinValue] = (byte)((int)array[-(-(-(-(~(--2147483641)))) ^ int.MinValue ^ int.MinValue ^ int.MinValue)] ^ -(-(~(-(-(-(-(~(~(~(-2147483567))))))))) ^ int.MinValue));
						num = ~(-(-(~-2147483638)) ^ int.MinValue);
						continue;
					}
					case 5:
					{
						byte[] array;
						array[~(~(~(-(~(-9)))))] = (byte)((int)array[~(-(-2147483640 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue] ^ -(~(-(2147483564 ^ int.MinValue))));
						array[~(-(~(~(~(-(-(-(10 ^ int.MinValue ^ int.MinValue)))) ^ int.MinValue) ^ int.MinValue)))] = (array[-(~(~(~(~2147483640) ^ int.MinValue)))] ^ ~(~(~(~114))));
						array[-(-(-(-(~(-10 ^ int.MinValue))))) ^ int.MinValue] = (array[~(~(-(~(~(2147483639 ^ int.MinValue))) ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue)] ^ ~(-(-(-76))));
						array[-(-(~(-(~(~(~(~(-(2147483637 ^ int.MinValue))) ^ int.MinValue) ^ int.MinValue)))))] = (byte)((int)array[-(~(-(-(~(-(-2147483638 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue)] ^ ~(~(-(~(-2147483603 ^ int.MinValue)))));
						num = (~(-(-(~(-(~(-(--2147483640) ^ int.MinValue))))) ^ int.MinValue) ^ int.MinValue);
						continue;
					}
					case 6:
					{
						byte[] array;
						array[-(~(26 ^ int.MinValue ^ int.MinValue))] = (byte)((int)array[~(~(~(~(-2147483621 ^ int.MinValue))))] ^ -(-(-2147483563 ^ int.MinValue)));
						array[~(-(-(~(-(~(~(~(-2147483621))))))) ^ int.MinValue)] = (byte)((int)array[~(--2147483619 ^ int.MinValue ^ int.MinValue ^ int.MinValue)] ^ -(~(-(~(~(-(-(-(~(-(-2147483569 ^ int.MinValue))))))) ^ int.MinValue ^ int.MinValue))));
						array[~(-(-(-(~(-(~(-32)))))))] = (byte)((int)array[-(~(-(-(-(~-2147483621))) ^ int.MinValue))] ^ -(-(~(-(~(~(~(~(~-4)))) ^ int.MinValue) ^ int.MinValue))));
						array[-(-(-(~(~(-53 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue))))] = (byte)((int)array[~(~(~(~(~(-(-(~-2147483595)) ^ int.MinValue))) ^ int.MinValue)) ^ int.MinValue] ^ ~(-(46 ^ int.MinValue) ^ int.MinValue));
						num = ~(~(-(~(--48))) ^ int.MinValue ^ int.MinValue);
						continue;
					}
					case 7:
					{
						byte[] array;
						array[-(-(--19 ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue] = (byte)((int)array[-(-(~(-(-(-20 ^ int.MinValue))))) ^ int.MinValue] ^ (~(-(-(~(~(--2147483543))))) ^ int.MinValue));
						array[~(-(-(-(--21))))] = (byte)((int)array[-(-(~(--2147483627 ^ int.MinValue)))] ^ ~(-(~(-(-(~(~(2147483624 ^ int.MinValue))))))));
						array[-(~(-(-(~2147483627 ^ int.MinValue))))] = (byte)((int)array[-(~(-(-(~(-(~2147483626))))) ^ int.MinValue ^ int.MinValue ^ int.MinValue)] ^ (-(~(-(-(~(-(~(~(~-62 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue))))) ^ int.MinValue));
						array[-(-(~(~22 ^ int.MinValue ^ int.MinValue)))] = (byte)((int)array[-(~(--21 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue)] ^ (-(~(~(-(~(-(~(~(~2147483603 ^ int.MinValue))))))) ^ int.MinValue) ^ int.MinValue));
						num = (~(~(~(-16 ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue);
						continue;
					}
					case 8:
					{
						byte[] array;
						array[-(-(-(-(--11))))] = (byte)((int)array[-(-(-(~-2147483638)) ^ int.MinValue ^ int.MinValue) ^ int.MinValue] ^ (-(~70 ^ int.MinValue) ^ int.MinValue));
						array[-(-(~-13 ^ int.MinValue) ^ int.MinValue)] = (byte)((int)array[-(-(-(-(-(~(~(~(~(-(~-13))))))))))] ^ (~(2147483557 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						array[-(-(-(~(-(~11 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue] = (byte)((int)array[-(-(-(2147483635 ^ int.MinValue)))] ^ -(-(-(~(-(-(-2147483418 ^ int.MinValue)))))));
						array[~(-(-(~(~(-(-(~(-(--2147483634))))) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue)] = (byte)((int)array[-(~(-(~(-(~(~(-(-(-(-2147483636 ^ int.MinValue))))) ^ int.MinValue)) ^ int.MinValue)))] ^ -(-(~(~(-(~-2147483531))) ^ int.MinValue)));
						num = ~(~(~(~(-(-(~(~(-2147483636 ^ int.MinValue))))))) ^ int.MinValue ^ int.MinValue);
						continue;
					}
					case 9:
					{
						method5 = typeof(Stream).GetMethod(array3[-(~(~(~(-(~(~2147483636))))) ^ int.MinValue)], ~(-(~(-(-(BindingFlags)2147483594 ^ (BindingFlags)(-2147483648))))), null, new Type[~(-(-(~(~(~(-0)) ^ int.MinValue)) ^ int.MinValue))], null);
						method6 = typeof(AppDomain).GetMethod(array3[-(-(-(-(-(~(~(2147483634 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue))))))], -(-(-(~((BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ^ (BindingFlags)(-2147483648))))) ^ (BindingFlags)(-2147483648), null, new Type[~(~(-(-(int.MinValue ^ int.MinValue))))], null);
						Type typeFromHandle3 = typeof(AppDomain);
						string name3 = array3[-(~(~(-(-(~14)))))];
						BindingFlags bindingAttr3 = -(~(-(~(-(~(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.NonPublic)))) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648);
						Binder binder3 = null;
						Type[] array5 = new Type[~(~(~(-(-(-3)) ^ int.MinValue ^ int.MinValue)))];
						array5[~(~(0 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue))] = typeof(string);
						array5[-(~(~(-(-(~(-(~(-(-2147483647 ^ int.MinValue ^ int.MinValue) ^ int.MinValue))))))))] = typeof(object);
						method7 = typeFromHandle3.GetMethod(name3, bindingAttr3, binder3, array5, null);
						methodBuilder.SetReturnType(typeof(string));
						num = -(-(-(-(~(-(-(2147483592 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue);
						continue;
					}
					case 10:
					{
						byte[] array;
						array[~(~(-(-(~(-(~(~(-(~(--23))))))))))] = (byte)((int)array[~(-(-2147483624 ^ int.MinValue) ^ int.MinValue ^ int.MinValue)] ^ -(-(-(~(-(-(-(-(-(2147483528 ^ int.MinValue)))))))) ^ int.MinValue ^ int.MinValue));
						array[~(~(-(-(~(~(-2147483624 ^ int.MinValue ^ int.MinValue)))))) ^ int.MinValue] = (byte)((int)array[-(~(~(~23))) ^ int.MinValue ^ int.MinValue] ^ -(2147483584 ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						array[-(-(-(~(-(-(-(2147483624 ^ int.MinValue))) ^ int.MinValue) ^ int.MinValue) ^ int.MinValue) ^ int.MinValue)] = (byte)((int)array[-(~(~(-25 ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue] ^ ~(-(-(~(~(2147483592 ^ int.MinValue))))));
						array[-(-(-(-(-(-26) ^ int.MinValue)) ^ int.MinValue))] = (byte)((int)array[~(-(-(-(~(-28 ^ int.MinValue) ^ int.MinValue) ^ int.MinValue ^ int.MinValue)))] ^ (-(-(--88) ^ int.MinValue) ^ int.MinValue));
						num = (-(~(-(~(-(-9 ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue ^ int.MinValue);
						continue;
					}
					case 11:
					{
						byte[] array;
						array[~(~(-2147483633 ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue ^ int.MinValue] = (byte)((int)array[~(~(-(-(~(-16 ^ int.MinValue ^ int.MinValue) ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue))] ^ ~(~(~(-106) ^ int.MinValue ^ int.MinValue)));
						array[-(-(~(~(-(~15 ^ int.MinValue ^ int.MinValue ^ int.MinValue))) ^ int.MinValue))] = (byte)((int)array[~(~(-(~(~(~(-(~-2147483634) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue)) ^ int.MinValue ^ int.MinValue))] ^ (~(-(--39 ^ int.MinValue)) ^ int.MinValue));
						array[-(~(-(-(-(~(-(-(-2147483633 ^ int.MinValue))) ^ int.MinValue) ^ int.MinValue) ^ int.MinValue) ^ int.MinValue))] = (byte)((int)array[~(-18) ^ int.MinValue ^ int.MinValue] ^ ~(~(-(--2147483593)) ^ int.MinValue));
						array[~(~(~(-(-(-19 ^ int.MinValue))))) ^ int.MinValue] = (byte)((int)array[-(~(~(-(~(2147483629 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue ^ int.MinValue) ^ int.MinValue))] ^ ~(~(~(2147483642 ^ int.MinValue))));
						num = -(-(~(-(~(-(-(~(-(-9 ^ int.MinValue ^ int.MinValue)))))) ^ int.MinValue ^ int.MinValue)));
						continue;
					}
					case 12:
					{
						byte[] array;
						array[~(-(~(-(~(~(~2147483610) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue))) ^ int.MinValue] = (byte)((int)array[-(-(-(~-2147483614 ^ int.MinValue)))] ^ -(~(-(~(~(-(~2147483592)) ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue));
						array[~(~(~(--2147483611 ^ int.MinValue)))] = (byte)((int)array[-(-(~(-(~(-(~2147483609)) ^ int.MinValue))) ^ int.MinValue ^ int.MinValue)] ^ ~(-(-(-(~(~(~(--2147483585)))) ^ int.MinValue))));
						array[~(-(~(-(-(-(--39 ^ int.MinValue)))) ^ int.MinValue))] = (byte)((int)array[~(-(-(~(~2147483610) ^ int.MinValue)))] ^ (-(~(-(~-2147483604 ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue));
						array[~(-(~(-(-(-(~(~(~(~(-2147483608 ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue))))))] = (byte)((int)array[-2147483610 ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue] ^ ~(~(--15) ^ int.MinValue ^ int.MinValue));
						num = ~(~(-(~(~(~(--5))) ^ int.MinValue) ^ int.MinValue));
						continue;
					}
					case 13:
					{
						byte[] array;
						string @string = Encoding.UTF8.GetString(array);
						char[] array6 = new char[-(~(-(-(-0 ^ int.MinValue ^ int.MinValue ^ int.MinValue)))) ^ int.MinValue];
						array6[-(~(~(-(-int.MinValue) ^ int.MinValue)))] = (char)(~(char)(-(char)(-(char)(~(char)(-(char)-59))) ^ int.MinValue) ^ int.MinValue);
						array3 = @string.Split(array6);
						methodBuilder = \uE06F.DefineMethod("?", attributes);
						type = Type.GetType(array3[~(-18 ^ int.MinValue) ^ int.MinValue]);
						constructor = type.GetConstructor(~(-(-(~(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)))), null, new Type[~(-(-(-(--1 ^ int.MinValue ^ int.MinValue))))], null);
						num = (~(-(-(-(~(2147483592 ^ int.MinValue ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue);
						continue;
					}
					case 14:
					{
						Type typeFromHandle4 = typeof(BinaryReader);
						BindingFlags bindingAttr4 = -(~(-(-(-(-(-(~(BindingFlags)(-2147483598)))))) ^ (BindingFlags)(-2147483648)));
						Binder binder4 = null;
						Type[] array7 = new Type[~(-(~(~(~(~(-(-(~(~(~(~-2147483646))) ^ int.MinValue))))))))];
						array7[~(~(~(~(-(~int.MaxValue ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue)] = typeof(Stream);
						constructor2 = typeFromHandle4.GetConstructor(bindingAttr4, binder4, array7, null);
						for (;;)
						{
							int num3 = -(~(~(~(2147483615 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue;
							for (;;)
							{
								num3 ^= -(~(-(-(~(~(-(-(-2147483630 ^ int.MinValue ^ int.MinValue) ^ int.MinValue)))))));
								switch (num3 + -(-(13 ^ -2147483648 ^ -2147483648)))
								{
								case 0:
									constructor3 = typeof(Hashtable).GetConstructor(-(~(~(-(BindingFlags)(-2147483596)) ^ (BindingFlags)(-2147483648))), null, new Type[~(~(-(~(--2147483647 ^ int.MinValue))))], null);
									num3 = (-(-(-(-(~(--25 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue);
									continue;
								case 1:
								{
									Type typeFromHandle5 = typeof(Hashtable);
									string name4 = array3[~(~(~(~(-(-12 ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue) ^ int.MinValue))) ^ int.MinValue];
									BindingFlags bindingAttr5 = -(-(~(~(~(~(~(~(~(~(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)) ^ (BindingFlags)(-2147483648)))))))) ^ (BindingFlags)(-2147483648));
									Binder binder5 = null;
									Type[] array8 = new Type[-(~(~(-(~(-(-(-3)))) ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue)];
									array8[~(-(-(~(-(-(~int.MaxValue ^ int.MinValue ^ int.MinValue) ^ int.MinValue)))))] = typeof(object);
									array8[~(-(-(-(~(-3))))) ^ int.MinValue ^ int.MinValue] = typeof(object);
									method8 = typeFromHandle5.GetMethod(name4, bindingAttr5, binder5, array8, null);
									num3 = (-(~(-(-(~(~(~(~(~27)))))) ^ int.MinValue)) ^ int.MinValue);
									continue;
								}
								case 2:
									method9 = typeof(BinaryReader).GetMethod(array3[-(~(~(~(~(-(-2147483637))))) ^ int.MinValue)], -(-(~(~(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648))) ^ (BindingFlags)(-2147483648), null, new Type[-(-(~(-1 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue], null);
									num3 = ~(~(-25 ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue);
									continue;
								case 3:
									num = (-(-(-(~(-(~(-2147483636 ^ int.MinValue)))))) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue);
									num3 = -(-(-(~(-(-(-(-(-(-(--2147483619))))))))) ^ int.MinValue);
									continue;
								case 4:
									goto IL_1B5D;
								}
								break;
							}
						}
						IL_1B5D:
						continue;
					}
					case 15:
					{
						byte[] array;
						array[-(~(~(~42) ^ int.MinValue ^ int.MinValue) ^ int.MinValue) ^ int.MinValue] = (byte)((int)array[~(-(~(~(-(-(-2147483604)) ^ int.MinValue))))] ^ ~(~(~(-(-(-67 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue))));
						array[~(-(-(-(~(-(-2147483602))) ^ int.MinValue)))] = (byte)((int)array[-(~(~-44) ^ int.MinValue ^ int.MinValue)] ^ (~(~103 ^ int.MinValue) ^ int.MinValue));
						array[-(~(~(-(-(-45) ^ int.MinValue))) ^ int.MinValue)] = (byte)((int)array[~(~(~(-(-(-(~(2147483601 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue] ^ -(~(~(~(~(-(~(-(~(-35 ^ int.MinValue)))) ^ int.MinValue ^ int.MinValue ^ int.MinValue))))));
						array[-(-(--46) ^ int.MinValue) ^ int.MinValue] = (byte)((int)array[-(~(~(~(~(~(~(~45))) ^ int.MinValue ^ int.MinValue) ^ int.MinValue))) ^ int.MinValue] ^ -(-(-(-(~(-(-(-(~(-(92 ^ int.MinValue))))))) ^ int.MinValue))));
						num = ~(~(~(~-2147483647) ^ int.MinValue));
						continue;
					}
					case 16:
					{
						byte[] array;
						array[-(~(-(~(~(-(~(2147483617 ^ int.MinValue) ^ int.MinValue) ^ int.MinValue)))))] = (byte)((int)array[-(-(-(~(~(~(~(-(-(~(-(-30 ^ int.MinValue) ^ int.MinValue))))))))))] ^ (-(~(-(--2147483526))) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue));
						array[~(~(-(-(~2147483615 ^ int.MinValue))))] = (byte)((int)array[-(-(~(-(-(2147483615 ^ int.MinValue)) ^ int.MinValue))) ^ int.MinValue] ^ -(-(~(~(~(~121 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue)) ^ int.MinValue));
						array[~(-(-(~(~2147483614)) ^ int.MinValue ^ int.MinValue ^ int.MinValue))] = (byte)((int)array[-(~(-(~(~(-(-(-(~-33))))) ^ int.MinValue ^ int.MinValue)))] ^ ~(~(-(~4 ^ int.MinValue) ^ int.MinValue)));
						array[~(-(~(~(-(~(34 ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue)] = (byte)((int)array[-(-(-(-(~(-(~(-(~(~36))))) ^ int.MinValue)))) ^ int.MinValue] ^ ~(~(~(-(-(~(~-32)))))));
						num = -(~(~(-(13 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue);
						continue;
					}
					case 17:
					{
						Type type2 = type;
						string name5 = array3[-(~(-(-(-(-(~(-(-(~(-1 ^ int.MinValue) ^ int.MinValue)))))))))];
						BindingFlags bindingAttr6 = ~(~(-(-(-(~(-(-((BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648))) ^ (BindingFlags)(-2147483648)))))) ^ (BindingFlags)(-2147483648);
						Binder binder6 = null;
						Type[] array9 = new Type[~(-(~(-3 ^ int.MinValue ^ int.MinValue)))];
						array9[-(-(-(-(-(~(~(~(~(-(~(-1 ^ int.MinValue))))))))))) ^ int.MinValue] = typeof(int);
						method10 = type2.GetMethod(name5, bindingAttr6, binder6, array9, null);
						for (;;)
						{
							int num4 = -(~(~(-5 ^ int.MinValue)) ^ int.MinValue);
							for (;;)
							{
								switch ((num4 ^ (~(--2147483636 ^ -2147483648) ^ -2147483648 ^ -2147483648)) - -(~(~(~(~(~(~(-(-(-13 ^ -2147483648) ^ -2147483648)))) ^ -2147483648))) ^ -2147483648))
								{
								case 0:
									method11 = typeof(MemberInfo).GetMethod(array3[-(-(~(-(-2147483645 ^ int.MinValue))))], ~(~(-(~(~(~(~(-(-(~(BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic))) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648))))))), null, new Type[~(~(~(~(~(~(-0)))))) ^ int.MinValue ^ int.MinValue], null);
									num4 = -(~(-(~(-(--2147483623) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue);
									continue;
								case 1:
									method12 = Type.GetType(array3[~(~(~(~(~(-(~-20) ^ int.MinValue))))) ^ int.MinValue]).GetMethod(array3[~(-(-(~(-(-(-(~(int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue))))))) ^ int.MinValue ^ int.MinValue)], -(-(BindingFlags)(-2147483596) ^ (BindingFlags)(-2147483648)) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648), null, new Type[-(~(-(~(~(~(2147483646 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue))))], null);
									num4 = (-(-(--6)) ^ int.MinValue ^ int.MinValue);
									continue;
								case 2:
									num = (-(~(~(~(~(~(~(~(~2147483641)))))))) ^ int.MinValue);
									num4 = -(-(~-27 ^ int.MinValue ^ int.MinValue));
									continue;
								case 3:
									method13 = typeof(Type).GetMethod(array3[~(-(-(-(~(-(-(~(~-6))))) ^ int.MinValue)) ^ int.MinValue)], ~(-(-(-(-(~((BindingFlags)(-2147483596) ^ (BindingFlags)(-2147483648))))))), null, new Type[-(~(-(1 ^ int.MinValue))) ^ int.MinValue], null);
									num4 = -(-(-(-(-2147483644 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue));
									continue;
								case 4:
									goto IL_21BC;
								}
								break;
							}
						}
						IL_21BC:
						continue;
					}
					case 18:
						goto IL_21C1;
					}
					break;
				}
			}
			IL_21C1:
			MethodBuilder methodBuilder2 = methodBuilder;
			Type[] array10 = new Type[~(-(~2147483645 ^ int.MinValue))];
			array10[~(-(-(~(~int.MaxValue ^ int.MinValue))) ^ int.MinValue) ^ int.MinValue] = typeof(Stream);
			methodBuilder2.SetParameters(array10);
			for (;;)
			{
				int num5 = ~(~(~(-1) ^ int.MinValue ^ int.MinValue));
				for (;;)
				{
					switch ((num5 ^ ~(~(~(~(~(-(-(~(~2147483632))))) ^ -2147483648)))) - ~(-(-(~(-(--2147483633 ^ -2147483648)))) ^ -2147483648 ^ -2147483648))
					{
					case 0:
						methodBuilder.DefineParameter(-(~(-(0 ^ int.MinValue)) ^ int.MinValue), -(-ParameterAttributes.None ^ (ParameterAttributes)(-2147483648)) ^ (ParameterAttributes)(-2147483648), "a");
						num5 = -(-(~(-(~(~(~(-(~(2147483614 ^ int.MinValue))) ^ int.MinValue) ^ int.MinValue)))));
						continue;
					case 1:
					{
						ILGenerator ilgenerator = methodBuilder.GetILGenerator();
						ilgenerator.DeclareLocal(type);
						ilgenerator.DeclareLocal(typeof(long));
						ilgenerator.DeclareLocal(typeof(BinaryReader));
						ilgenerator.DeclareLocal(typeof(Hashtable));
						ilgenerator.DeclareLocal(typeof(string));
						ilgenerator.DeclareLocal(typeof(int));
						ilgenerator.DeclareLocal(typeof(Type));
						ilgenerator.DeclareLocal(typeof(string));
						Label label = ilgenerator.DefineLabel();
						Label label2 = ilgenerator.DefineLabel();
						Label label3 = ilgenerator.DefineLabel();
						Label label4 = ilgenerator.DefineLabel();
						Label label5 = ilgenerator.DefineLabel();
						Label label6 = ilgenerator.DefineLabel();
						Label label7 = ilgenerator.DefineLabel();
						ilgenerator.Emit(OpCodes.Newobj, constructor);
						ilgenerator.Emit(OpCodes.Stloc_0);
						ilgenerator.Emit(OpCodes.Ldc_I4_0);
						ilgenerator.Emit(OpCodes.Stloc_S, ~(~(~(~-2147483643 ^ int.MinValue) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue)) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Br, label);
						ilgenerator.MarkLabel(label5);
						ilgenerator.Emit(OpCodes.Ldloc_0);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(-(-2147483643 ^ int.MinValue ^ int.MinValue)))) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Callvirt, method10);
						ilgenerator.Emit(OpCodes.Callvirt, method12);
						ilgenerator.Emit(OpCodes.Callvirt, method11);
						ilgenerator.Emit(OpCodes.Stloc_S, -(~(~(~(-(-(~(~(-(-5 ^ int.MinValue) ^ int.MinValue)))))))));
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(~(-(-(-(~(-(9 ^ int.MinValue ^ int.MinValue)))))))));
						ilgenerator.Emit(OpCodes.Brfalse, label2);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(~(-8)) ^ int.MinValue) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Callvirt, method13);
						ilgenerator.Emit(OpCodes.Stloc_S, -(-(-(-(~(-(~(--2147483639 ^ int.MinValue) ^ int.MinValue)))))) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-(~7)) ^ int.MinValue) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldstr, array3[-(-(~(~(-(-21 ^ int.MinValue ^ int.MinValue))) ^ int.MinValue ^ int.MinValue))]);
						ilgenerator.Emit(OpCodes.Ldc_I4_5);
						ilgenerator.Emit(OpCodes.Callvirt, method);
						ilgenerator.Emit(OpCodes.Ldc_I4_M1);
						ilgenerator.Emit(OpCodes.Bne_Un, label3);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-(~(~2147483640)) ^ int.MinValue ^ int.MinValue) ^ int.MinValue));
						ilgenerator.Emit(OpCodes.Ldstr, array3[-(~(-(-(-(~(--20))))))]);
						ilgenerator.Emit(OpCodes.Ldc_I4_5);
						ilgenerator.Emit(OpCodes.Callvirt, method);
						ilgenerator.Emit(OpCodes.Ldc_I4_M1);
						ilgenerator.Emit(OpCodes.Bne_Un, label3);
						ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-(~(~(--2147483641)) ^ int.MinValue) ^ int.MinValue)) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldstr, array3[~(-(-(-(~(~(-(-(24 ^ int.MinValue))) ^ int.MinValue)))))]);
						ilgenerator.Emit(OpCodes.Ldc_I4_5);
						ilgenerator.Emit(OpCodes.Callvirt, method);
						ilgenerator.Emit(OpCodes.Ldc_I4_M1);
						ilgenerator.Emit(OpCodes.Bne_Un, label3);
						ilgenerator.Emit(OpCodes.Ldloc_S, -(~(-(-6 ^ int.MinValue ^ int.MinValue))));
						ilgenerator.Emit(OpCodes.Ldstr, array3[-(-(~(2147483623 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue)]);
						ilgenerator.Emit(OpCodes.Ldc_I4_5);
						ilgenerator.Emit(OpCodes.Callvirt, method);
						ilgenerator.Emit(OpCodes.Ldc_I4_M1);
						ilgenerator.Emit(OpCodes.Beq, label4);
						ilgenerator.MarkLabel(label3);
						ilgenerator.Emit(OpCodes.Ldc_I4_0);
						ilgenerator.Emit(OpCodes.Call, method2);
						ilgenerator.MarkLabel(label4);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-(-(-(-6 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue ^ int.MinValue ^ int.MinValue))) ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldc_I4_1);
						ilgenerator.Emit(OpCodes.Add);
						ilgenerator.Emit(OpCodes.Stloc_S, ~(~(~(~(-(-(-(~(-2147483644 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue))))) ^ int.MinValue) ^ int.MinValue);
						ilgenerator.MarkLabel(label);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-2147483642 ^ int.MinValue)) ^ int.MinValue ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldloc_0);
						ilgenerator.Emit(OpCodes.Callvirt, method3);
						ilgenerator.Emit(OpCodes.Blt, label5);
						ilgenerator.MarkLabel(label2);
						ilgenerator.Emit(OpCodes.Ldarg_0);
						ilgenerator.Emit(OpCodes.Callvirt, method4);
						ilgenerator.Emit(OpCodes.Stloc_1);
						ilgenerator.Emit(OpCodes.Ldarg_0);
						ilgenerator.Emit(OpCodes.Newobj, constructor2);
						ilgenerator.Emit(OpCodes.Stloc_2);
						ilgenerator.Emit(OpCodes.Newobj, constructor3);
						ilgenerator.Emit(OpCodes.Stloc_3);
						ilgenerator.Emit(OpCodes.Ldloc_2);
						ilgenerator.Emit(OpCodes.Callvirt, method9);
						ilgenerator.Emit(OpCodes.Stloc_S, ~(~(~-5)) ^ int.MinValue ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ldloc_3);
						ilgenerator.Emit(OpCodes.Ldc_I4_M1);
						ilgenerator.Emit(OpCodes.Box, typeof(int));
						ilgenerator.Emit(OpCodes.Ldloc_S, -(~(~(-(~(~(~(~-2147483644) ^ int.MinValue) ^ int.MinValue)))) ^ int.MinValue));
						ilgenerator.Emit(OpCodes.Callvirt, method8);
						ilgenerator.Emit(OpCodes.Br, label6);
						ilgenerator.MarkLabel(label7);
						ilgenerator.Emit(OpCodes.Ldloc_3);
						ilgenerator.Emit(OpCodes.Ldarg_0);
						ilgenerator.Emit(OpCodes.Callvirt, method5);
						ilgenerator.Emit(OpCodes.Conv_I4);
						ilgenerator.Emit(OpCodes.Ldc_I4, -(~(~(~(~-43 ^ int.MinValue ^ int.MinValue)))));
						ilgenerator.Emit(OpCodes.Add);
						int arg = int.Parse(array3[~(~(-(~(-(~(-(~16) ^ int.MinValue))) ^ int.MinValue)))]);
						ilgenerator.Emit(OpCodes.Ldc_I4, arg);
						ilgenerator.Emit(OpCodes.Xor);
						ilgenerator.Emit(OpCodes.Box, typeof(int));
						ilgenerator.Emit(OpCodes.Ldloc_2);
						ilgenerator.Emit(OpCodes.Callvirt, method9);
						ilgenerator.Emit(OpCodes.Callvirt, method8);
						ilgenerator.MarkLabel(label6);
						ilgenerator.Emit(OpCodes.Ldarg_0);
						ilgenerator.Emit(OpCodes.Callvirt, method5);
						ilgenerator.Emit(OpCodes.Ldloc_1);
						ilgenerator.Emit(OpCodes.Blt, label7);
						ilgenerator.Emit(OpCodes.Call, method6);
						ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(~(-(-(-(--2147483645) ^ int.MinValue))))) ^ int.MinValue ^ int.MinValue));
						ilgenerator.Emit(OpCodes.Ldloc_3);
						ilgenerator.Emit(OpCodes.Callvirt, method7);
						ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-(-(~(-(-(-(-2147483643))))))) ^ int.MinValue) ^ int.MinValue ^ int.MinValue);
						ilgenerator.Emit(OpCodes.Ret);
						num5 = (-(-(-(~(~2147483618) ^ int.MinValue ^ int.MinValue))) ^ int.MinValue);
						continue;
					}
					case 2:
						return methodBuilder;
					}
					break;
				}
			}
			return methodBuilder;
		}

		public string \uE001(Stream \uE070)
		{
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName
			{
				Name = "?"
			}, ~(-(-(-(-(~(-(-(-(~(~(~(AssemblyBuilderAccess)(-2147483648)))))))))))) ^ (AssemblyBuilderAccess)(-2147483648)).DefineDynamicModule("?").DefineType("?", ~(~(~(~(-(-(-(TypeAttributes)(-2147483648)) ^ (TypeAttributes)(-2147483648)))))));
			this.\uE000(typeBuilder);
			Type type = typeBuilder.CreateType();
			string name = "?";
			BindingFlags invokeAttr = -(-(-(~((BindingFlags)(-2147483369) ^ (BindingFlags)(-2147483648))) ^ (BindingFlags)(-2147483648) ^ (BindingFlags)(-2147483648)));
			Binder binder = null;
			object target = null;
			object[] array = new object[-(~(-(-(-0))))];
			array[~(~(~(-(-(~int.MinValue))))) ^ int.MinValue] = \uE070;
			return (string)type.InvokeMember(name, invokeAttr, binder, target, array);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static \uE001()
		{
			for (;;)
			{
				int num = ~(-(-2147483621 ^ int.MinValue) ^ int.MinValue) ^ int.MinValue;
				for (;;)
				{
					switch ((num ^ ~(-(~(~(~(-(~2147483632)) ^ -2147483648 ^ -2147483648 ^ -2147483648))))) - -(-(~(~(-(~-2147483626)))) ^ -2147483648))
					{
					case 0:
						num = -(~(-(-(22 ^ int.MinValue ^ int.MinValue))));
						continue;
					case 1:
						num = -(~(18 ^ int.MinValue) ^ int.MinValue);
						continue;
					case 2:
						num = (~(~(-(~(~(~(~(--2147483631)) ^ int.MinValue) ^ int.MinValue)))) ^ int.MinValue);
						continue;
					case 3:
						num = ~(-(-(~(-(~(-(~(~2147483627 ^ int.MinValue ^ int.MinValue ^ int.MinValue))))))));
						continue;
					case 4:
						num = (-(~(-(~(~(-(-(~19 ^ int.MinValue)))) ^ int.MinValue)) ^ int.MinValue) ^ int.MinValue);
						continue;
					case 5:
						num = ~(-(-(2147483602 ^ int.MinValue) ^ int.MinValue) ^ int.MinValue);
						continue;
					case 6:
						num = ~(~(-(~(-(-(19 ^ int.MinValue)) ^ int.MinValue)) ^ int.MinValue ^ int.MinValue) ^ int.MinValue ^ int.MinValue);
						continue;
					case 7:
						num = ~(~(-(~(~(-(-(~(17 ^ int.MinValue ^ int.MinValue)) ^ int.MinValue))) ^ int.MinValue)));
						continue;
					case 8:
						num = (-(~(-2147483633)) ^ int.MinValue);
						continue;
					case 9:
						return;
					}
					break;
				}
			}
		}
	}
}
