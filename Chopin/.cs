using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

internal sealed class \uE01B
{
	private static string \uE000;

	private static \uE01B.\uE01C \uE001;

	static \uE01B()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		\uE01B.\uE001 = new \uE01B.\uE01C(\uE01B.\uE001);
		Stream uE = \uE01E.\uE000(executingAssembly.GetManifestResourceStream(\uE01B.\uE001(~(~(~(~(~863374433 ^ 433760972)))) ^ -715957934)));
		\uE01B.\uE000 = new \uE01B.\uE01D().\uE001(uE);
	}

	public static string \uE000(int \uE000)
	{
		return (string)((Hashtable)AppDomain.CurrentDomain.GetData(\uE01B.\uE000))[\uE000];
	}

	public static string \uE001(int \uE000)
	{
		char[] array = "êÝÅÜ".ToCharArray();
		int num = array.Length;
		while ((num -= (-(~(-1315611500)) ^ -1315611500)) >= -(-(-(-304467387 ^ 1814480964 ^ -2114127359))))
		{
			array[num] = (char)((int)array[num] ^ -(~(-(-(~(~(608612668 ^ -1255421911 ^ -1187046397 ^ 476735069)))) ^ -2038683753 ^ -1304157100)) ^ \uE000);
		}
		return new string(array);
	}

	private delegate string \uE01C(int \uE000);

	private sealed class \uE01D
	{
		public MethodBuilder \uE000(TypeBuilder \uE000)
		{
			MethodAttributes methodAttributes = -(-(~(MethodAttributes)1535325807 ^ (MethodAttributes)1074918150 ^ (MethodAttributes)(-462603776)));
			string[] array;
			MethodInfo method;
			MethodInfo method2;
			MethodInfo method3;
			MethodBuilder methodBuilder;
			MethodInfo method4;
			MethodInfo method5;
			MethodInfo method6;
			MethodInfo method7;
			MethodInfo method8;
			MethodInfo method9;
			MethodInfo method10;
			MethodInfo method11;
			ConstructorInfo constructor;
			MethodInfo method12;
			ConstructorInfo constructor2;
			MethodInfo method13;
			ConstructorInfo constructor3;
			MethodInfo method14;
			MethodInfo method15;
			for (;;)
			{
				int num = -(--1344613702 ^ -963143356 ^ 1766673284);
				for (;;)
				{
					num ^= (~(~(-(1057752370 ^ -730273194 ^ -649311619 ^ 1913107676)) ^ -1428105070) ^ 354432656);
					switch (num - -(-(~(~(-(~(~(-(~(~-1641562371)))) ^ 790045945))) ^ -1322214849)))
					{
					case 0:
					{
						BindingFlags bindingFlags;
						method = typeof(Stream).GetMethod(array[-(--347491615 ^ 1080706081 ^ -1179701459 ^ -621614303 ^ -746824146) ^ 453143785], bindingFlags, null, new Type[-(~(~(-(-0))))], null);
						BindingFlags bindingFlags2;
						method2 = typeof(AppDomain).GetMethod(array[~(-(~(~(~(~(~1499626902 ^ -684462438) ^ -1239173667) ^ 509991103)))) ^ -1117002111 ^ 1686365983], bindingFlags2, null, new Type[~(-(~(~(-(~(--1640675730)))))) ^ -1540463196 ^ -1243148667 ^ 1934423036 ^ 732702253 ^ 685967202], null);
						Type typeFromHandle = typeof(AppDomain);
						string name = array[~(~398945525 ^ 1888356775 ^ -589962027 ^ -1934734269 ^ 926076363)];
						BindingFlags bindingAttr = bindingFlags;
						Binder binder = null;
						Type[] array2 = new Type[-(-(-(~(-245922321 ^ -46109472)) ^ -1846194120) ^ -714096364 ^ 1217230898)];
						array2[~(-(~(762105456 ^ -653186642 ^ 2147172416 ^ 1954109536)))] = typeof(string);
						array2[~(-1423002042 ^ 706588009) ^ 705926150 ^ 1423929559] = typeof(object);
						method3 = typeFromHandle.GetMethod(name, bindingAttr, binder, array2, null);
						methodBuilder.SetReturnType(typeof(string));
						num = (~(-(~1844918108 ^ -1376433073) ^ -1800315578) ^ -1421026856);
						continue;
					}
					case 1:
					{
						byte[] array3;
						array3[~(~(-(~(-1485901650 ^ -1485901682))))] = (byte)((int)array3[~(-(-(-(-(-(~(~(-1647811187)) ^ -915886222)))) ^ -664052957)) ^ -302818472 ^ 1631111842] ^ ~(~(-(~(948539107 ^ 557427898) ^ 430973533))));
						array3[-(-(-(-(--2031272521 ^ -751958374))) ^ -2058775874 ^ 1544490444 ^ 1937398657)] = (byte)((int)array3[~(-(~(~1962501526) ^ 1962501557))] ^ ~(~(-(~(~-466964888)) ^ -731403598) ^ -106625058 ^ 907541739));
						array3[~(-(-(-(-(1779769059 ^ -795393653 ^ 671481877 ^ 1836818593)))))] = (byte)((int)array3[-(~(-(~(~(-(~-35))))))] ^ -(-(-(-1335002382 ^ -415814361) ^ -1465538541)));
						array3[-(~(~(~(-(~(-(-874862948 ^ -1857787724 ^ -1178929037))) ^ 1409051767))) ^ 1327998450)] = (byte)((int)array3[-(-(~-1593705282 ^ 1593705317))] ^ (-(-(~(~(~-1805944030)))) ^ 1805944032));
						num = ~(-(~(~(~1071986725 ^ -2097083758) ^ -1384338198 ^ -1809677741) ^ 2051400584));
						continue;
					}
					case 2:
					{
						byte[] array3;
						array3[~(-(--1881776801 ^ 1881776779))] = (byte)((int)array3[~(~(-966509155) ^ 1614163890 ^ -1504570874)] ^ (-(~(~(-(~(335545828 ^ 990268235) ^ -1697640884)) ^ -1569263682) ^ 444842538) ^ -220425581));
						array3[~(~(-(~(~(-(-(~(~1693965822))))))) ^ -1693965784)] = (byte)((int)array3[-(-(-(~1508552558 ^ 845351515 ^ 1804182812)))] ^ -(-(-(~(-(~(~(-1796387293))) ^ 927782221))) ^ 1549690084));
						array3[-(~(-(~(~(-(-1704889681 ^ -1704889723))))))] = (byte)((int)array3[~(~(-(-(-(-1200971018 ^ -1091049292)) ^ -993032958 ^ -754311956)) ^ -291033985)] ^ -(-(~1692329981 ^ -1692329920)));
						array3[~(-(-(-(-(~(-(-(--1396853292) ^ 1447182731)) ^ 1226914811)) ^ 1277243510)))] = (byte)((int)array3[~(1373225353 ^ -1149861399) ^ 1403972888 ^ 1191072427] ^ (~(~(~(~(-(~(-(-1382836891 ^ 868832997))))))) ^ -170635515 ^ -1804504093));
						num = ~(-(~924828731) ^ -1468143413 ^ 1620955917);
						continue;
					}
					case 3:
					{
						byte[] array3;
						array3[~(-(~(~(~(-(~(-49415980 ^ -1493796303) ^ -1543209675))))))] = (byte)((int)array3[-(-(-(-(-(-(946685493 ^ 2042015804)))))) ^ 1104934436] ^ -(-(~(-(-(~(-(~(~(-1403892604 ^ -218892158) ^ 478948847 ^ 2068165735)))))) ^ -963128749)));
						array3[-(-(-(~(~(-(~1820860634) ^ 239684314) ^ -391741255 ^ 1524408050) ^ 793021336)))] = (byte)((int)array3[-(~(-(~(--1860874880 ^ -1336665176)) ^ -557930236))] ^ ~(~(-(-(~(~1487786250 ^ 2092002748) ^ -1281281644)) ^ -1749185160)));
						array3[-(~(~(961161220 ^ -961161259)))] = (byte)((int)array3[~(-(~(~(~(~1086568199))) ^ -1384629247)) ^ -306476246] ^ (-(-(~(-(~(~(-(~(--1998934463))))) ^ 1045326848))) ^ -1296520550 ^ 158377311 ^ -224185629));
						array3[~(--1065888917) ^ -245695484 ^ 825044830] = (byte)((int)array3[~(-(~(~(-(-1791630747 ^ 1791630762)))))] ^ -(~(-(-(-(-(-(-891410193 ^ -1838924672 ^ 582211874)))))) ^ -2047462753));
						num = ~(~(-(-(~(--130421910)) ^ 1705176140 ^ -1650784420)));
						continue;
					}
					case 4:
					{
						Type typeFromHandle2 = typeof(string);
						string name2 = array[-(-(~(-(~(-2114821908 ^ -520913411)))) ^ -1627462934)];
						BindingFlags bindingFlags;
						BindingFlags bindingAttr2 = bindingFlags;
						Binder binder2 = null;
						Type[] array4 = new Type[~(~(~(-(-(-(949601008 ^ 1637423578) ^ 1924639644)) ^ -1163188495)) ^ -1647399372) ^ -811415441 ^ -1016057826];
						array4[~(~(~(--57326683) ^ 135243372 ^ -1722844674 ^ 1970722702) ^ 1446105983) ^ 1318230215] = typeof(string);
						method4 = typeFromHandle2.GetMethod(name2, bindingAttr2, binder2, array4, null);
						for (;;)
						{
							int num2 = ~(-(-(~(-546163009 ^ 1757298484) ^ -2039269938) ^ -1292444354)) ^ -2092310153;
							for (;;)
							{
								switch ((num2 ^ -(~(-(--1762775999 ^ -202633515 ^ 743757410) ^ 1230435535))) - ~(-(-(~(-(~-886075268)))) ^ -1338824477 ^ 2065479848))
								{
								case 0:
									num = (-(~(--1852624677)) ^ 1852624733);
									num2 = ~(~(~(-(--3))));
									continue;
								case 1:
								{
									Type type;
									method5 = type.GetMethod(array[-(~(~(~(-(-(~-1831317074) ^ 2089420783)) ^ 296593864)))], bindingFlags, null, new Type[-(~(~(-(-(-(~(~(~1423783197)) ^ 1167376050)) ^ -697350892) ^ -953758020)))], null);
									num2 = ~(-275742707 ^ -1755283141 ^ 1818753005 ^ -1554295155 ^ 1211326376);
									continue;
								}
								case 2:
								{
									Type type2 = Type.GetType(array[-(~(-(~-1433396057 ^ -1372292617)) ^ 541120988 ^ 618970270)]);
									string name3 = array[-(~(~(~(-(-(~915253625 ^ 893532039) ^ 934146939 ^ 1830859776)))) ^ -1801472207) ^ 841126214];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr3 = bindingFlags2;
									Binder binder3 = null;
									Type[] array5 = new Type[-(~(~(-(-1730213729)))) ^ -1730213730];
									array5[~(-(-(~(~(~-1292639218 ^ -1292842590))) ^ 290174315)) ^ 289970373] = typeof(int);
									method6 = type2.GetMethod(name3, bindingAttr3, binder3, array5, null);
									num2 = ~(~(~(~(-(-15)))));
									continue;
								}
								case 3:
									method7 = typeof(Stream).GetMethod(array[~(-(-(~(~1340644959)) ^ -1678640956 ^ 736532846))], bindingFlags, null, new Type[-(~(-1733957787 ^ 2107676796 ^ 970300253)) ^ -590289851], null);
									num2 = -(~(~-1982399007 ^ 1982398995));
									continue;
								case 4:
									goto IL_8D6;
								}
								break;
							}
						}
						IL_8D6:
						continue;
					}
					case 5:
					{
						byte[] array3;
						array3[-(~(-(-(-(~(~(-728342787 ^ 105470978))))) ^ 757136665))] = (byte)((int)array3[~(~(-(1870553102 ^ 1852985306)) ^ -17575883)] ^ ~(~(~(1746460801 ^ 1730341687 ^ 252036750 ^ 1980636829) ^ -4694915 ^ 1843725604 ^ 462648628)));
						array3[~(~(-(-(~(~(-(--825184522)) ^ -1193516390)) ^ 1654853666) ^ 900649664) ^ 1827496411 ^ 1307340619)] = (byte)((int)array3[~(-(64338569 ^ 153856957)) ^ 184422697] ^ ~(-(-(-(-644244341 ^ -1798751619)) ^ 16229192 ^ 1302791655)));
						array3[~(-(-(~(-536825295) ^ -536825302)))] = (byte)((int)array3[~(-(~(~(~(-(-(1412233404 ^ 1982658004 ^ -565831828) ^ -1177287197))) ^ -212878840 ^ -746792709) ^ -1705082127))] ^ -(--104835845 ^ -8516830 ^ -180658090 ^ -209371309));
						array3[~(-(-(-(~-1747466468)))) ^ 1747466494] = (byte)((int)array3[~(-(-(-(~(-(-(~(1955306097 ^ 32220749)))) ^ -2107786117 ^ 932477005) ^ -1062602731)))] ^ ~(~(~(~(-(-(-28055653 ^ 1846288502 ^ -1872771166)))))));
						num = ~(~(~(-(~(-(~(-459909610)))))) ^ 459909529);
						continue;
					}
					case 6:
					{
						BindingFlags bindingFlags;
						method8 = typeof(MemberInfo).GetMethod(array[~(-(-(480772539 ^ -480772538)))], bindingFlags, null, new Type[-(-(~(~(~(-(-(~(~(--1908001355) ^ 1421302768 ^ -1761660600 ^ 1276010252))))))))], null);
						for (;;)
						{
							int num3 = -(~(-(-(-(~(-(~-167144146)) ^ -137795906))))) ^ -29422490;
							for (;;)
							{
								num3 ^= -(-(-(-(-2092920406 ^ 1439667449)) ^ -695270044));
								switch (num3 + (~(-(~(-(~(~(1064019861 ^ 764778419 ^ -1250584099) ^ -2138444873)))) ^ 816282220) ^ 396703847))
								{
								case 0:
								{
									Type typeFromHandle3 = typeof(Type);
									string name4 = array[-(-(--1829532514 ^ 848930325 ^ 704308817 ^ 2087065) ^ 1987111866)];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr4 = bindingFlags2;
									Binder binder4 = null;
									Type[] array6 = new Type[-(-(~(-(-(-(~(~(~(-1751790176)))))) ^ -18606158))) ^ -1769069075];
									array6[~(-(-(-(-(~2048934151 ^ -1537542779) ^ -2076802181 ^ -1275157843)) ^ 1657445715 ^ -1955067897))] = typeof(RuntimeTypeHandle);
									method9 = typeFromHandle3.GetMethod(name4, bindingAttr4, binder4, array6, null);
									num3 = (-1673218202 ^ -1985554531 ^ -73922588 ^ -1137904867 ^ 610931471 ^ -230518897 ^ 2072115060);
									continue;
								}
								case 1:
									num = -(~(-(-(-(~-720430768)))) ^ -720430766);
									num3 = (~(-(~(~562614735 ^ 2134045830))) ^ -1589257540);
									continue;
								case 2:
									method10 = typeof(MemberInfo).GetMethod(array[-(-(~(-608061743 ^ -2002879595)) ^ -1398787393)], bindingFlags, null, new Type[-(~(~(--1372303279))) ^ -1372303279], null);
									num3 = (~(-(~(-(-1654988743) ^ 107339845 ^ -1370194317)) ^ 1551841924) ^ -1762935938);
									continue;
								case 3:
									method11 = typeof(Type).GetMethod(array[~(~(-(~(-(~(~(-1420138834 ^ 1757858388) ^ -1860506675) ^ 216954260))))) ^ -1583887014], bindingFlags, null, new Type[-(-(-(~(-(~(--624261341))) ^ 1541579929 ^ 432890807))) ^ -301608268 ^ -1994429115], null);
									num3 = (-(1290024580 ^ 678410357 ^ 373220933) ^ 1924491460);
									continue;
								case 4:
									goto IL_D05;
								}
								break;
							}
						}
						IL_D05:
						continue;
					}
					case 7:
					{
						byte[] array3;
						array3[~(~(~(~1622181504 ^ 1622181553)))] = (byte)((int)array3[~(-(~(-(~350829656) ^ -350829676)))] ^ -(-(~(~(~(~(-(~121067616) ^ -513934845 ^ -1396154448 ^ -1122997638 ^ 73509714 ^ -204260143)))))));
						for (;;)
						{
							int num4 = -(~(~(~-416333402 ^ 990026286) ^ -1657271150 ^ 1091950434));
							for (;;)
							{
								switch ((num4 ^ ~(~(~(-(-435099377 ^ -1483795622 ^ 261310440 ^ -416898496))) ^ 1352505472 ^ -105412794)) - ~(-(~(~(~(-(~(-(~(-(-(-395353608 ^ 1472074438 ^ 1076792450))))))))))))
								{
								case 0:
								{
									string @string = Encoding.UTF8.GetString(array3);
									char[] array7 = new char[~(-(~(1919763311 ^ -282593953 ^ -1658236896)) ^ -6546961)];
									array7[-(~(--1870652440)) ^ -936719651 ^ -1487589692] = (char)(-(char)(-1353809033 ^ 986320811 ^ -1170206768 ^ 1458087900 ^ -2032936171));
									array = @string.Split(array7);
									num4 = (~(~(~(~(-(-(~(--181452726))) ^ -2026959489) ^ 809356483))) ^ 1111327118);
									continue;
								}
								case 1:
								{
									string name5 = "?";
									MethodAttributes attributes = methodAttributes;
									Type typeFromHandle4 = typeof(string);
									Type[] array8 = new Type[~(~(~(~(~(-1014633294 ^ 779553670 ^ -1285047931)) ^ 1259870831) ^ 182761871 ^ -527039825))];
									array8[-(-1641570457) ^ -183557749 ^ -1115861781 ^ -848558347 ^ -456783092] = typeof(Stream);
									methodBuilder = \uE000.DefineMethod(name5, attributes, typeFromHandle4, array8);
									num4 = (-(-(-(~877731555 ^ 2121423725))) ^ 1243825655);
									continue;
								}
								case 2:
									num = ~(-(~(-(--114))));
									num4 = ~(~(~(-127)));
									continue;
								case 3:
									array3[~(~(-(-308748268) ^ 274928640) ^ 33819564)] = (byte)((int)array3[~(-(~(-(792685463 ^ -482843013))) ^ -871952982)] ^ -(-(~(--946209348) ^ -1011605215 ^ -2123946048 ^ -2058683588)));
									num4 = -(-(-(~(1717156925 ^ 1706097660 ^ 1452896096 ^ 707133816)) ^ 2136353184));
									continue;
								case 4:
									goto IL_F82;
								}
								break;
							}
						}
						IL_F82:
						continue;
					}
					case 8:
					{
						byte[] array3;
						array3[-(~(-(~(~(~(~(-(-1329348544 ^ -848870030) ^ 504074844 ^ -1126178459))) ^ -546245077))))] = (byte)((int)array3[~(~(~(-(-(-(~(-497263012 ^ -839698406 ^ -296372799))) ^ 517893086)) ^ 551241600))] ^ ~(~(-(-(~(-(-1569204615 ^ 1583434178)))) ^ -1184008722) ^ 1165748858));
						array3[~(~(-(~(~-1697003261))) ^ 1697003227)] = (byte)((int)array3[-(-(~(~(--207702075))) ^ 207702047)] ^ (~(~(--1093357644 ^ -9278775)) ^ 704295854 ^ -1750890204));
						array3[-(-(-(~(-(~-992464499) ^ 976306671 ^ -449379389 ^ -632457570)) ^ -1047329510))] = (byte)((int)array3[~(-(~(~(-(~(~206934757 ^ 662186788)) ^ 527080235 ^ -1006358266)) ^ 548670425)) ^ 1903158719 ^ 1583809106] ^ ~(-(~(-240050308 ^ -571923936) ^ 1109426925) ^ -1853407203));
						array3[-(-(-(-(~(~(-(~(-(~26709214))))))) ^ 223993664)) ^ 91272528 ^ 163505880] = (byte)((int)array3[~(-(~(~(~(~(-182428143))) ^ -1393796288))) ^ 1506542968] ^ -(-(~(-(~(-(-1281437902 ^ -1040841490)))) ^ 1430823264 ^ -1629413538 ^ 147481927 ^ -1324674853)));
						num = -(~(-(-(-(~(~(-347618137 ^ -934829492)) ^ 1382232826)) ^ -765400178) ^ -854106529 ^ -1846862273));
						continue;
					}
					case 9:
					{
						Type typeFromHandle5 = typeof(BinaryReader);
						BindingFlags bindingFlags;
						BindingFlags bindingAttr5 = bindingFlags;
						Binder binder5 = null;
						Type[] array9 = new Type[-(-(~(~(-221308552 ^ 1744086531)))) ^ -1791246470];
						array9[~(-(-(-(-624701708)))) ^ -897230121 ^ 273060900] = typeof(Stream);
						constructor = typeFromHandle5.GetConstructor(bindingAttr5, binder5, array9, null);
						for (;;)
						{
							int num5 = ~(-(~(-(--1702254574) ^ -1746933677 ^ 1677916672 ^ 611538803))) ^ 304075311 ^ 99498693 ^ -306401469 ^ -2011585990 ^ 1064503463;
							for (;;)
							{
								num5 ^= -(-(-875370182 ^ -875370236));
								switch (num5 + ~(~(-(~(~(~(-(-729387239 ^ 136250204))))) ^ 593924992)))
								{
								case 0:
									num = ~(-(~(-(-(-(~(-(-(~-1904274350))))) ^ 547342555) ^ 1425626067) ^ -98988198));
									num5 = -(-(~(-(-(-(-(-(~(-1599331557 ^ 1097755997 ^ 1351317344 ^ -610637202))) ^ 1792234304))))));
									continue;
								case 1:
									method12 = typeof(BinaryReader).GetMethod(array[~(~(~(2132503508 ^ 814052227 ^ 1697364495)) ^ -1638778693 ^ 1259889943)], bindingFlags, null, new Type[-(-(~(1799268833 ^ -1336913294 ^ -1346834446 ^ -906059617 ^ 1355240006)) ^ 46923151) ^ 282846410], null);
									num5 = -(~(-(-(-(-(-(~603254248) ^ -1858796462))) ^ -1448459546)) ^ -460041051);
									continue;
								case 2:
									constructor2 = typeof(Hashtable).GetConstructor(bindingFlags, null, new Type[~(-(~(~448253125 ^ 1314331941))) ^ 283538683 ^ 1141286180], null);
									num5 = (~(~(--1756654170)) ^ -2040939035 ^ 286402628);
									continue;
								case 3:
								{
									Type typeFromHandle6 = typeof(Hashtable);
									string name6 = array[~(~(-(-(~(-(~(~-799952043 ^ -303566011)))) ^ 1035354117)))];
									BindingFlags bindingAttr6 = bindingFlags;
									Binder binder6 = null;
									Type[] array10 = new Type[-(~(-(-(-(-(~(~223274992) ^ -179750458)) ^ 2100686483)))) ^ -2033500325 ^ 66654207];
									array10[-(~(-(-(-(~1315355373)) ^ 1261776665 ^ -114625888) ^ 59152554))] = typeof(object);
									array10[-(-(-(~549422838))) ^ 549422838] = typeof(object);
									method13 = typeFromHandle6.GetMethod(name6, bindingAttr6, binder6, array10, null);
									num5 = ~(~(-(-(-(~(~(-1740857088 ^ 636785049)) ^ 666124991) ^ -1703120352))));
									continue;
								}
								case 4:
									goto IL_140C;
								}
								break;
							}
						}
						IL_140C:
						continue;
					}
					case 10:
					{
						BindingFlags bindingFlags = ~(~(~(-(-(BindingFlags)481086581 ^ (BindingFlags)(-1344544085)))) ^ (BindingFlags)1284034859);
						BindingFlags bindingFlags2 = -(~(-(-(-(~(-(-(~(BindingFlags)1590926254 ^ (BindingFlags)(-889793698) ^ (BindingFlags)1148992586 ^ (BindingFlags)799443827))))))));
						byte[] array3 = Convert.FromBase64String("WT/CY3IKPLtJDEszFx0CAUkxPg9zSXT2JVQ0vz1rbhzifG9dBhVoNQxcMzcLTRT4QE47R2V0VHldZUZyb21IYQhkbGU7Z2V0X05hbWU7SW5kZXhPZjtFeGl0O2dldF9GcmFtZUNvdW50O2dldF9MZW5ndGg7UmVhZFN0cmluZztBZGQ7Z2V0X1Bvc2l0aW9uO2dldF9DdXJyZW50RG9tYWluO1NldERhdGE7UnVudGltZU1ldGhvZDtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tUcmFjZTtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tGcmFtZTsxMTQyODtTeXN0ZW0uRW52aXJvbm1lbnQ=");
						array3[~(~(-(~(~(-(-(-1753651355 ^ -1091751795))) ^ 1063723436 ^ -351038971))) ^ -2077018042) ^ 621999724 ^ -1556163179] = (byte)((int)array3[-(~(-(-(-910999046)))) ^ -1640707497 ^ 1468500908] ^ (~(-(~(-(927958862 ^ 1626789770)) ^ -1788460538 ^ -299417902 ^ -1029871188)) ^ -295352924));
						num = (-(~(-(-(~-1912799203)))) ^ 265146000 ^ 2110730508);
						continue;
					}
					case 11:
					{
						byte[] array3;
						array3[~(~(~(-(-(~(-(~(~(-(-(-1872303010 ^ 1814637235)))) ^ 61880606)))))))] = (byte)((int)array3[~(~(-(1813751730 ^ -1813751743)))] ^ (~(~(-(~(~(~(~(--667766092))) ^ 731977235 ^ -122178342)))) ^ 1101908421 ^ -2012194146 ^ -1030126760));
						array3[~(-(-1544644732 ^ 646009441 ^ -1050192719 ^ 1141412187))] = (byte)((int)array3[-(-(~(~(--14))))] ^ ~(~(~(~(~(~(-(-(1790714188 ^ 1944705675))) ^ -783391484)) ^ -523348849 ^ -755419828 ^ -97714314))));
						array3[-(-(~(~(~(2144784115 ^ 2049295024 ^ 1809852727)))) ^ 534807125 ^ -1911793455)] = (byte)((int)array3[-(-(~-455343710 ^ 533086822) ^ -1006090315 ^ 1666901542) ^ -1548280411] ^ (-(~(~(-(-(-(~(-(~(-(~-1986781559))))) ^ -1763607497))))) ^ -527786708));
						array3[-(-(-(-(-(-1214672801 ^ 1191095988) ^ 244891909))))] = (byte)((int)array3[-(~(~(-(-(-(~-1606936633) ^ -842879506))) ^ -1129432012 ^ -574004023) ^ -211673813)] ^ -(~(~(-(~(~(~-474106742)) ^ 104165379 ^ -1450289396 ^ 1177784531)) ^ 897536953) ^ 1682283501 ^ -1527761703));
						num = (~(-(~(-(~762210263) ^ -1842078606 ^ 1117840193 ^ 1780431892))) ^ -1901529669 ^ -1834759537 ^ 1947297210);
						continue;
					}
					case 12:
					{
						byte[] array3;
						array3[~(~(-(~(-(~(-(-1822261684)) ^ 496351445))) ^ 1896364390))] = (byte)((int)array3[-(-(-(-(~(~(-1785402282)) ^ -1598876232))) ^ 891812329)] ^ (~(~(-(-2036590599 ^ 2090596778 ^ -1906884403)) ^ 77683577 ^ -603478980) ^ 1393143372));
						array3[-(~(~(-(~(-(-1994462640) ^ -1344338231) ^ -2074864865))) ^ 2070784323) ^ -699529544 ^ 263606370] = (byte)((int)array3[~(~(-(-(-(~(-1817376756) ^ -1469021285)) ^ 878260177)) ^ 260351041)] ^ -(~(-(-1259495880 ^ 261892633) ^ 1150176143)));
						array3[~(~(~(-(-1662138561)))) ^ -1662138567] = (byte)((int)array3[-(-(~(-(269830479 ^ -2128880480)))) ^ -1861315095] ^ -(-(~(-(-862933437 ^ 1548512840) ^ -1565238868) ^ 627413034) ^ 996608119 ^ 745144609));
						array3[-(-(-918978065 ^ 275436538 ^ 1729675713)) ^ -1102343204] = (byte)((int)array3[~(-(-(~(~(-(~(2027469834 ^ -624257856 ^ -1392167355)) ^ -253173401)))))] ^ -(-(~(-(~1439542159) ^ 1414416052 ^ -55370142) ^ 47123659)));
						num = -(-(-(-595977996 ^ -1445090766) ^ -2146514717 ^ -828399996) ^ -993453267);
						continue;
					}
					case 13:
					{
						byte[] array3;
						array3[~(-2142920147 ^ 1414690424 ^ 501382800 ^ 906646843)] = (array3[~(-(~(-(1049479441 ^ 602123795 ^ 815861118)) ^ 1142140240)) ^ 1776076587] ^ -(~(-(~88))));
						array3[~(-(~(~(~(~(~(-(--1439534487)))))))) ^ 387174345 ^ 394708749 ^ -1194489186 ^ -308998195] = (byte)((int)array3[-(~(-(~(~(-(-(-(~(-1169926673) ^ -1962933960)))))))) ^ -826561749] ^ (~(-(-(~1518470427)) ^ -428686775) ^ -1125070876));
						array3[~(-(~(-(~(~(~-684152099) ^ -650825691) ^ -235772670))))] = (byte)((int)array3[~(-(-(~(--3))))] ^ (~(-(-1863849695) ^ -397874976 ^ 1491890714) ^ 541291519));
						array3[-(-(-(~(-(-(1080673025 ^ -1008507797 ^ -798628618))))) ^ 887928500 ^ -1366864544 ^ 37143579 ^ -297402234 ^ 636779734)] = (byte)((int)array3[~(~(~(-(-(-(~(~(~(~(--1554713167)))))) ^ 1554713162))))] ^ (~(-(~(~(-(~(~(~(-1450225004))) ^ 726460389))))) ^ 1810215116 ^ -383384131));
						num = ~(~(-(-(-(-(~(-(-1912667196) ^ 112854840 ^ 1801327481)) ^ 1226368345))) ^ -1459359067));
						continue;
					}
					case 14:
					{
						byte[] array3;
						array3[-(-(-(~(~(~(~(-2095094631) ^ -474687421)) ^ -32350720)) ^ 1631984955))] = (byte)((int)array3[~(~(-(~(-(-645664981 ^ 1844338925) ^ 1995308871 ^ -1035429152 ^ -1072616557)) ^ 1059470866))] ^ -(~(-(~(~-1)))));
						array3[-(-(~(-(-(~(-(-(--418974922) ^ -525938947)) ^ -127942131)))))] = (byte)((int)array3[~(-(~(-(~(~(-(~(~(~-947339905)) ^ -947339962)))))))] ^ ~(~(~(~(~(~(~(~(-1525135548 ^ 1922445849))))) ^ 1479251954 ^ -1808831100)) ^ 462065414));
						array3[-(~(-(~(-(~(~(~(~-333213960) ^ 1438370484 ^ -63873532)))) ^ -1212571467 ^ 233784592)))] = (byte)((int)array3[~(~(~(-(~(-(--2056499618 ^ 2021033909)) ^ 1104273876 ^ 1500028537)) ^ -638883078 ^ -1011408033))] ^ -(-(~(~(~(~(-(-(~(--1479636684) ^ -1156256212))) ^ 484075364))))));
						array3[-(~(~(-(-(~(~(1848914436 ^ -1195173667 ^ 1843633883 ^ 962339473))))) ^ 2093638125) ^ 24971422)] = (byte)((int)array3[~(~(-(~(-(~(-(--241922836)) ^ 1172599651 ^ -1495621609) ^ -1799865634 ^ -2045614760))))] ^ -(-(~(-(~(-829129137)) ^ 1418784359)) ^ 591703053 ^ 913573401 ^ 1892402534));
						num = ~(-(~(-1873050129 ^ -746843747 ^ -1126667383)));
						continue;
					}
					case 15:
					{
						byte[] array3;
						array3[-(~(-902457981 ^ 950414558 ^ -225199795))] = (byte)((int)array3[~(-(~(-(-(~(~(~(-1215754650) ^ -148632881)) ^ -751716269))) ^ -610294279 ^ 1208162577))] ^ (-(~-134608335 ^ 1125925834) ^ -1259976279));
						array3[-(~(~532388020 ^ -965311701)) ^ -1381522870 ^ 845003796 ^ -18875971 ^ 1974698904 ^ 781741102 ^ -675458201 ^ -880689343] = (byte)((int)array3[-(-(~(~(~(-1631852075)))) ^ 1631852088)] ^ -(-(-(~-1368838804) ^ 198531991 ^ -1514375425)));
						array3[~(~(-(-(-186450820 ^ -186450833))))] = (byte)((int)array3[~(-(-(~(~(-2101985200 ^ 542348850 ^ -1735264338 ^ -1791775525) ^ -1632905830 ^ 382549667) ^ 486184307 ^ -13135105 ^ 991141454)))] ^ ~(~(-1559812567 ^ 306407312 ^ -1320926767)));
						array3[~(-(-(-(-(~276336379) ^ 1239226049))) ^ 1504024616)] = (byte)((int)array3[-(-(-(~(-(~(~(~(-(201435235 ^ -1346188449) ^ 1893603118 ^ -951199735)))) ^ -340831241))))] ^ (~(-(~(-(~1537552152)) ^ 654170365)) ^ 2063787306 ^ -106566362));
						num = -(-(-(-(-(~(1772072462 ^ -342693367))))) ^ -2113059723);
						continue;
					}
					case 16:
					{
						Type type = Type.GetType(array[~(-(~(~(-(~(-(-1764553900) ^ 642170109) ^ 63686839)) ^ 1285652725)))]);
						BindingFlags bindingFlags;
						constructor3 = type.GetConstructor(bindingFlags, null, new Type[~(-(~(-(-(~(~(1842078599 ^ -552863205 ^ 1831378986 ^ -1508332999 ^ -808964263 ^ 321563171)) ^ 1525508363)))))], null);
						Type type3 = type;
						string name7 = array[~(-(-(~(-(-(~1910953691) ^ 850917283 ^ 687844562)) ^ 953457698)) ^ -1259659163) ^ 664948529 ^ 1070367013];
						BindingFlags bindingAttr7 = bindingFlags;
						Binder binder7 = null;
						Type[] array11 = new Type[-(-(-(433559832 ^ -433559833)))];
						array11[~(-(-(860224874 ^ 1745564383 ^ -1531891638)))] = typeof(int);
						method14 = type3.GetMethod(name7, bindingAttr7, binder7, array11, null);
						method15 = Type.GetType(array[-(~(~(~(~1121212475) ^ -1121212459)))]).GetMethod(array[~(~(--470982030)) ^ 470982031], bindingFlags, null, new Type[-(~(~(~272060854) ^ 222523522) ^ -494149941)], null);
						num = ~(~(~(~(-(~(-(-1941001942)) ^ -236489599 ^ -1788794284 ^ 389482502)))));
						continue;
					}
					case 17:
					{
						byte[] array3;
						array3[~(~(-(~(-(-(~(~(-(-(-1363423938 ^ 2068628448))))))) ^ -1251414447)) ^ 1621090437)] = (byte)((int)array3[-(-(-(~(~(~(-(-20)))))))] ^ -(-(661243280 ^ -26977313 ^ 1326117230 ^ -1777897700)));
						array3[~(-(~(~(~(-(1610484467 ^ 1541041452 ^ 1149768219 ^ 1085044188))))))] = (byte)((int)array3[-(-(-(-(~(~(~(~-993754095))) ^ -993754105))))] ^ ~(-(-(-(-(--37530675)) ^ 1881426127 ^ -1914234072))));
						array3[~(-(~(~(-(-847730565 ^ 847730579)))))] = (byte)((int)array3[~(-(-499371126) ^ -2095194483) ^ 1629613331] ^ (~(-(~(-(~(309786202 ^ -927441598) ^ -989695705)) ^ -1735648625) ^ 1856447307 ^ 261283456) ^ 2134852626 ^ 1275718735 ^ 716990987));
						array3[-(~(-(~(~(1176092553 ^ 2127602673)) ^ -1732205084) ^ 938294133 ^ 59489698 ^ -1479666118)) ^ -866199918] = (byte)((int)array3[~(~(~(~(~(-(~(~(~(-(-1028296573 ^ 728851435)) ^ -373025922))))))))] ^ (-(~(~(~(-(-1591241069 ^ 41284550) ^ -1797096432)) ^ 794023276)) ^ -417582184));
						num = (-(~(~(-(-961892382 ^ 1976589305 ^ -1710020494 ^ -1314993599) ^ -1468235372 ^ -306645176))) ^ -583835397);
						continue;
					}
					case 18:
					{
						byte[] array3;
						array3[~(-(-(~(~-1496563041)))) ^ 1496563049] = (byte)((int)array3[-(-(~(~(~-1721621446))) ^ -1403725906) ^ -892793758] ^ -(~(-(-(~(-(-178578059 ^ 43481985)) ^ -137599303)))));
						array3[-(-(--1221183305)) ^ 302711830 ^ 1522713429] = (byte)((int)array3[-(-(-(~-1149965522) ^ -1344015850 ^ -1159083777)) ^ -1367420468] ^ ~(-(~(-(-(-(~-49)))))));
						array3[-(~(~(-(-(~(-(--1695536665) ^ 886362780)))) ^ -1373308047))] = (byte)((int)array3[-(~(-(~(-(~(-(684877556 ^ 2073297873 ^ 250897439 ^ 1228906190))))))) ^ -344825852] ^ (~(-(-(--1596308866) ^ -1714196757)) ^ -328352069 ^ -714829208));
						array3[~(~(-(~-1287575751)) ^ -869447496 ^ 2137812878)] = (byte)((int)array3[-(-(~(-(~(~(~(-(-(~339979953)))))) ^ 216000420))) ^ 412895512] ^ -(-(-679044383 ^ -178219178 ^ -981995212) ^ -409862949));
						num = ~(~(-(-(-(~(-(~(-(82992695 ^ -794631074 ^ -1213477622) ^ -1677424410))))))));
						continue;
					}
					case 19:
						goto IL_2155;
					}
					break;
				}
			}
			IL_2155:
			methodBuilder.DefineParameter(~(~(~(~(~(~-24423913) ^ 607855631) ^ 154456515 ^ 1231170998)) ^ -1696167316), -(-(~(~(-(-((ParameterAttributes)1699178940 ^ (ParameterAttributes)(-1127585241)))))) ^ (ParameterAttributes)645069925), "a");
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.DeclareLocal(typeof(StackTrace));
			ilgenerator.DeclareLocal(typeof(int));
			ilgenerator.DeclareLocal(typeof(StackFrame));
			ilgenerator.DeclareLocal(typeof(Type));
			ilgenerator.DeclareLocal(typeof(string));
			ilgenerator.DeclareLocal(typeof(long));
			ilgenerator.DeclareLocal(typeof(BinaryReader));
			ilgenerator.DeclareLocal(typeof(Hashtable));
			ilgenerator.DeclareLocal(typeof(string));
			Label label = ilgenerator.DefineLabel();
			Label label2 = ilgenerator.DefineLabel();
			Label label3 = ilgenerator.DefineLabel();
			Label label4 = ilgenerator.DefineLabel();
			Label label5 = ilgenerator.DefineLabel();
			Label label6 = ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Newobj, constructor3);
			ilgenerator.Emit(OpCodes.Stloc_0);
			ilgenerator.Emit(OpCodes.Ldc_I4_8);
			ilgenerator.Emit(OpCodes.Stloc_1);
			ilgenerator.Emit(OpCodes.Br_S, label);
			ilgenerator.MarkLabel(label4);
			ilgenerator.Emit(OpCodes.Ldloc_0);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Callvirt, method14);
			ilgenerator.Emit(OpCodes.Stloc_2);
			ilgenerator.Emit(OpCodes.Ldloc_2);
			ilgenerator.Emit(OpCodes.Callvirt, method15);
			ilgenerator.Emit(OpCodes.Callvirt, method8);
			ilgenerator.Emit(OpCodes.Stloc_3);
			ilgenerator.Emit(OpCodes.Ldloc_3);
			ilgenerator.Emit(OpCodes.Brfalse_S, label2);
			ilgenerator.Emit(OpCodes.Ldloc_3);
			ilgenerator.Emit(OpCodes.Callvirt, method11);
			ilgenerator.Emit(OpCodes.Stloc_S, -(~(~(-(~(~(~(~(-657104418)) ^ -826516689 ^ -1325274819 ^ -1486302264)))))));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(~(~(-(--1886330840) ^ -1885258713 ^ -638134599)))) ^ 641267019));
			ilgenerator.Emit(OpCodes.Ldtoken, typeof(\uE01B));
			ilgenerator.Emit(OpCodes.Call, method9);
			ilgenerator.Emit(OpCodes.Callvirt, method10);
			ilgenerator.Emit(OpCodes.Callvirt, method4);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Bne_Un_S, label3);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-613023077 ^ -19765070 ^ 631532077)));
			ilgenerator.Emit(OpCodes.Ldstr, array[~(~(~(~(~(~1402266291 ^ -1646171222) ^ 1072517028 ^ 721674099)))) ^ -627369506]);
			ilgenerator.Emit(OpCodes.Callvirt, method4);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Beq_S, label2);
			ilgenerator.Emit(OpCodes.Ldc_I4_0);
			ilgenerator.Emit(OpCodes.Call, method6);
			ilgenerator.MarkLabel(label3);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Ldc_I4_1);
			ilgenerator.Emit(OpCodes.Add);
			ilgenerator.Emit(OpCodes.Stloc_1);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldloc_1);
			ilgenerator.Emit(OpCodes.Ldloc_0);
			ilgenerator.Emit(OpCodes.Callvirt, method5);
			ilgenerator.Emit(OpCodes.Blt_S, label4);
			ilgenerator.MarkLabel(label2);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method7);
			ilgenerator.Emit(OpCodes.Stloc_S, ~(-(~(~(~(-(-1913021438)))) ^ 748237072)) ^ -1587499755);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Stloc_S, ~(-(-(~(~(-(-228759918 ^ -491978074) ^ -858028592) ^ -1260414478)) ^ -1701533767 ^ 636578044) ^ -678316208));
			ilgenerator.Emit(OpCodes.Newobj, constructor2);
			ilgenerator.Emit(OpCodes.Stloc_S, -(~(~(-(~(-380775487 ^ 1480358650 ^ -1370772512 ^ -813632768 ^ 1445400613) ^ -1529394641) ^ -1849845280) ^ -1845651161)) ^ -571124500);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(~(-(~(~(-(~(782393057 ^ -1845023978)))))) ^ -1130007055)));
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Stloc_S, -(-(-(-(~(-1980459797 ^ 2129690824 ^ -1583888301 ^ 434788840))) ^ 411189711 ^ -1476117600)));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-606022313 ^ 244968120) ^ -242513753 ^ -619886416);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(~(~(-(-(~(~(~908471885) ^ 908471882)))))));
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.Emit(OpCodes.Br_S, label5);
			ilgenerator.MarkLabel(label6);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-1162153463) ^ -1862816800 ^ -771166966 ^ 129649435);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Conv_I4);
			ilgenerator.Emit(OpCodes.Ldc_I4, ~(~(-(~(-(114004143 ^ 321108999) ^ 1754008364 ^ -2103715756)))));
			ilgenerator.Emit(OpCodes.Add);
			int arg = int.Parse(array[-(-(~(--765369157) ^ -765369175))]);
			ilgenerator.Emit(OpCodes.Ldc_I4, arg);
			ilgenerator.Emit(OpCodes.Xor);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(~(-(~(~(~(~-557453467) ^ 943802612))))) ^ 427513974);
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.MarkLabel(label5);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(~(~(-218983119 ^ -202186629 ^ -44825373)) ^ -61574740)));
			ilgenerator.Emit(OpCodes.Blt_S, label6);
			ilgenerator.Emit(OpCodes.Call, method2);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(-(~(-(-(~(~(~-1918212017))) ^ -1495940565) ^ 729786995))))));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(~(~1522334231 ^ -1522334226)));
			ilgenerator.Emit(OpCodes.Callvirt, method3);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(~(--923502130 ^ 2119346458 ^ 94789074 ^ 1060325023))) ^ 382688603 ^ -1694722359);
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		public string \uE001(Stream \uE000)
		{
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName
			{
				Name = "?"
			}, -(~(-(-(~(~(AssemblyBuilderAccess)0)))))).DefineDynamicModule("?").DefineType("?", ~(~(-(-(-(-(TypeAttributes)502782403))) ^ (TypeAttributes)1643906918 ^ (TypeAttributes)(-1918850363) ^ (TypeAttributes)(-240421792))));
			this.\uE000(typeBuilder);
			Type type = typeBuilder.CreateType();
			string name = "?";
			BindingFlags invokeAttr = -(-(-(-(~(BindingFlags)(-1596138594)) ^ (BindingFlags)(-276988462) ^ (BindingFlags)(-1895632826)) ^ (BindingFlags)1063037165));
			Binder binder = null;
			object target = null;
			object[] array = new object[~(~(~(-2)))];
			array[~(-(-1653409764 ^ 597960688) ^ -1093214229)] = \uE000;
			return (string)type.InvokeMember(name, invokeAttr, binder, target, array);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static \uE01D()
		{
			for (;;)
			{
				int num = -(-(~(~(~(~(~(~(~282722298)))) ^ -1921499685)) ^ 1650343335));
				for (;;)
				{
					switch ((num ^ (-(-(-(~(-(-(~1896969950) ^ 6760464))) ^ -2140088829)) ^ 251229961)) - (-(~(~(-(-(-801364130 ^ -1547753704 ^ 923585606 ^ 1317664324))))) ^ -168180347))
					{
					case 0:
						num = -(-(~(-(~(-(~(-(~(~(--235202421))))) ^ 235202312)))));
						continue;
					case 1:
						num = (~(~(132294438 ^ 665320655)) ^ 541416424);
						continue;
					case 2:
						num = -(~(-(-(-(-(-(-696569543 ^ -921198516)))))) ^ -527208817);
						continue;
					case 3:
						num = -(-(~-1384209611 ^ 1384209612));
						continue;
					case 4:
						num = (-(-(~(--1016826095) ^ 1842317774) ^ -289773145) ^ 1074894716);
						continue;
					case 5:
						num = (~(~(-(-(-(-(~(~(--144826440 ^ -376294769)))))))) ^ -1372313497 ^ -7911174 ^ -1333743070);
						continue;
					case 6:
						num = ~(~(~(-(--3))));
						continue;
					case 7:
						num = -(~(-(~(~1839687071)) ^ -1839687069));
						continue;
					case 8:
						num = ~(~(-(~(-(~(-(~(-1775296746 ^ -417162151 ^ 1896710990))))))));
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
