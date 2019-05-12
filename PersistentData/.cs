using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

internal sealed class \uE00E
{
	private static string \uE000;

	private static \uE00E.\uE00F \uE001;

	static \uE00E()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		\uE00E.\uE001 = new \uE00E.\uE00F(\uE00E.\uE001);
		Stream uE = \uE011.\uE000(executingAssembly.GetManifestResourceStream(\uE00E.\uE001(~(~(~(~(~(-(~(~(~(--1832406290)) ^ 653457487))) ^ -1773154943)) ^ 578455328)))));
		\uE00E.\uE000 = new \uE00E.\uE010().\uE001(uE);
	}

	public static string \uE000(int \uE000)
	{
		return (string)((Hashtable)AppDomain.CurrentDomain.GetData(\uE00E.\uE000))[\uE000];
	}

	public static string \uE001(int \uE000)
	{
		char[] array = "#\u0005#\u000f\u0019".ToCharArray();
		int num = array.Length;
		while ((num -= -(~(-(-(~(-(-(-(778382748 ^ 778382749))))))))) >= -(~(~105163911) ^ 958578483 ^ 1063654324))
		{
			array[num] = (char)((int)array[num] ^ -(-(-(-(~(-(-(-(-1969265918 ^ -1063027371))))))) ^ 1245453856) ^ \uE000);
		}
		return new string(array);
	}

	private delegate string \uE00F(int \uE000);

	private sealed class \uE010
	{
		public MethodBuilder \uE000(TypeBuilder \uE000)
		{
			MethodAttributes methodAttributes = -(-(-(-(-(-(-(~((MethodAttributes)(-769774499) ^ (MethodAttributes)(-537923533)) ^ (MethodAttributes)812483646 ^ (MethodAttributes)845182711)) ^ (MethodAttributes)(-1132812790)) ^ (MethodAttributes)(-1283028936)))));
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
				int num = ~(~(~(-(-(-(~1053136508 ^ -1660910205 ^ 1547365499))))));
				for (;;)
				{
					num ^= (~(-(--1916880769)) ^ 1916880825);
					switch (num - (~(-(2100596655 ^ -180244674)) ^ -2005589847))
					{
					case 0:
					{
						BindingFlags bindingFlags;
						method = typeof(Stream).GetMethod(array[-(~(-134063063 ^ 1927997223) ^ -1964459262)], bindingFlags, null, new Type[~(~(-(-0)))], null);
						BindingFlags bindingFlags2;
						method2 = typeof(AppDomain).GetMethod(array[~(~(-(~(~(-(~(~(474527516 ^ 881079519))) ^ -475741720 ^ -2055171538) ^ 1323880457))))], bindingFlags2, null, new Type[~(-(~(-1312992461) ^ 1312992461))], null);
						Type typeFromHandle = typeof(AppDomain);
						string name = array[-(~(-(-(-663004635 ^ -1961568286))) ^ -690079293 ^ -2051951606)];
						BindingFlags bindingAttr = bindingFlags;
						Binder binder = null;
						Type[] array2 = new Type[-(~(~(~(~(-(~-451293141 ^ 1719444072 ^ -1429757807) ^ -837073701) ^ 2136226480) ^ 1062979207)) ^ 1481217473)];
						array2[~(-205271924 ^ -651522462) ^ -719930095] = typeof(string);
						array2[-(-(~(-(-(1670767222 ^ 2143826938))))) ^ -1895150929 ^ 1822975197] = typeof(object);
						method3 = typeFromHandle.GetMethod(name, bindingAttr, binder, array2, null);
						methodBuilder.SetReturnType(typeof(string));
						num = (~(-(-(-(~(~(~(-(~1261456775))))))) ^ -653770717) ^ 1841820192);
						continue;
					}
					case 1:
					{
						byte[] array3;
						array3[-(~(-(~(-(~(-1702352719 ^ 1653411669)))))) ^ -133879866] = (byte)((int)array3[-(-(~(~-2742243) ^ -274256977 ^ 275841939))] ^ (~(-(~(-966100628 ^ 2112439003))) ^ 1149027394));
						array3[~(-(~(~(~(~(~1346714562) ^ 1435148270 ^ 127409955) ^ -2094598713))) ^ 2123280660)] = (byte)((int)array3[-(~(-(--385280350 ^ -2115584571) ^ 1760537412))] ^ (-(~1346666275 ^ 153020762) ^ 1392574170 ^ 173697215));
						array3[-(~(-(~(~(~(~(-1180543877 ^ -1447698173 ^ -269989210)))))))] = (byte)((int)array3[-(~(-(~(~(-(~(~(~(~-1365635158)) ^ -1010118744)))) ^ -1955258220)) ^ -433725260)] ^ ~(-(-(~(-(~(-(~(-1381477282 ^ 609188374 ^ -1586166410 ^ 680667912)))))))));
						array3[-(~(~(-565852559 ^ 787615589 ^ -1006795064) ^ -860508160))] = (byte)((int)array3[-(-(~(~36)))] ^ ~(~-1831020532 ^ 632453013 ^ -1217482844));
						num = ~(-(~(-(~(-(-377804266 ^ 23159774))) ^ -400930369)));
						continue;
					}
					case 2:
					{
						byte[] array3;
						array3[~(-(~(-(-(~1382572252 ^ 454568094 ^ 1386712012) ^ 1590029183))) ^ 1158817991)] = (byte)((int)array3[~(-(~(-(~(~-2054553882 ^ 1836734490 ^ -386685225)))))] ^ (-(~(-(~(-(~-1049951975)) ^ -1901031373))) ^ -806068732 ^ -2144350420));
						array3[~(-(~(-(-(~(-(~(1984351372 ^ -1936228118))) ^ 1717381713 ^ -122963793))) ^ -1353327235 ^ -881720369))] = (byte)((int)array3[-(-(~(-(~(-(~(-(~-1873791373)) ^ 1873791399))))))] ^ (~(~-270556199 ^ 276547152) ^ -6003204));
						array3[-(~(-(--973877395)) ^ -141357177 ^ 1864933223 ^ 1565024167)] = (byte)((int)array3[~(-(~(-(~(-(~-925368336)))) ^ 925368353))] ^ -(~(-(-(-(~(~(-(-689424341))) ^ 1656016753 ^ -1268975845))))));
						array3[-(~(-(-(-(~(~-250758119 ^ -535022534) ^ -1113230748) ^ 74338567))) ^ 1872601621) ^ 951645824] = (byte)((int)array3[-(~(-834936441 ^ -1520699665)) ^ 1801459525] ^ -(~(~(-(~-104)))));
						num = (~(-(-(-1703822350 ^ 2131159676) ^ 161018516)) ^ 319839456);
						continue;
					}
					case 3:
					{
						byte[] array3;
						array3[-(~(~(-883734632 ^ 1962085858 ^ 688355802) ^ 1767427187))] = (byte)((int)array3[~(~(~(~1533251564 ^ -434889819) ^ -96689936)) ^ 1196110996] ^ (~(~(~(~606822219))) ^ 606822250));
						array3[~(-(~(-(313722711 ^ 1846478077))) ^ -1365134295) ^ -769804369] = (byte)((int)array3[-(-(-(~(~(~(~(1088101548 ^ 398114417))))))) ^ -1466016499] ^ (-(~-469101208 ^ 830467687) ^ -713688246));
						array3[-(-(-(-(~(~(-(-(~-879039259))))))) ^ 879039287)] = (byte)((int)array3[-(~(~(~(~(~271930816) ^ 271930862))))] ^ ~(-(~(344935560 ^ -410133615) ^ -551680626 ^ -790313030) ^ 1539288038 ^ -1613793131 ^ -948471338));
						array3[~(~(-(~(~(~(-(-(1386915853 ^ 1204696430)) ^ 907864939)) ^ -1625002946) ^ -1134814695)))] = (byte)((int)array3[~(-(-(~(~(~(-1190479956 ^ 882127241)) ^ 493617965) ^ -1863154376)))] ^ (-(-(~(-(~(-(-(-(398050161 ^ -1974618463))) ^ -1352920831)) ^ -824079852 ^ 523939982))) ^ -478978438));
						num = (-(-(~(-(-(~(-(~(--1869884242)))))) ^ 440233471)) ^ -450786390 ^ -1872179841);
						continue;
					}
					case 4:
					{
						Type typeFromHandle2 = typeof(string);
						string name2 = array[-(-(-(~6)))];
						BindingFlags bindingFlags;
						BindingFlags bindingAttr2 = bindingFlags;
						Binder binder2 = null;
						Type[] array4 = new Type[~(~(~(~(-(-(~(-1005056770 ^ 319565920) ^ -1647859458 ^ 86086239)))) ^ -320556728) ^ 1558792329)];
						array4[~(~(-(~(-(~-1730884765)))) ^ 1773438871 ^ -1920697320 ^ 2095329514)] = typeof(string);
						method4 = typeFromHandle2.GetMethod(name2, bindingAttr2, binder2, array4, null);
						for (;;)
						{
							int num2 = -(-(-(~(~(-(~(-(-1137215936 ^ -365181827) ^ 1942126399)) ^ 829913894 ^ -347715109)))));
							for (;;)
							{
								switch ((num2 ^ (-(~(-(-(-(~(-(-(--119676783)))))))) ^ 119676745)) - ~(-(-(~(-(~(~348365775 ^ -374122142) ^ 513803222 ^ 1967497463)) ^ 930096825)) ^ 1578795259))
								{
								case 0:
									num = (~(~(-(-(-(-(-1709732858 ^ 1538298239)))))) ^ -774182556 ^ 276690022);
									num2 = -(~(-(~(-(~(-(-(~(-(-0))))))))));
									continue;
								case 1:
								{
									Type type;
									method5 = type.GetMethod(array[-(~(~(~(~-938630671)) ^ -198322896)) ^ -422426615 ^ 621658943], bindingFlags, null, new Type[-(~(~(~-453250541 ^ 108488449)) ^ -28585173) ^ 482821690], null);
									num2 = ~(~(-(~(~(1265231405 ^ 1269543482)))) ^ -1235437668 ^ 1231101044);
									continue;
								}
								case 2:
								{
									Type type2 = Type.GetType(array[-(~-1435661974) ^ -1886721907 ^ -490937941 ^ -950304679]);
									string name3 = array[~(-(-(-(~(~(-(-9)))))))];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr3 = bindingFlags2;
									Binder binder3 = null;
									Type[] array5 = new Type[~(-(~(--1003480982 ^ -2046301501 ^ 1110962856)))];
									array5[-(~(~(~(--2021355120)) ^ 2124562981 ^ -782688890 ^ 679464492))] = typeof(int);
									method6 = type2.GetMethod(name3, bindingAttr3, binder3, array5, null);
									num2 = ~(-(~(-(--17))));
									continue;
								}
								case 3:
									method7 = typeof(Stream).GetMethod(array[~(~(-(-(1135581667 ^ 795308686)) ^ -85570890) ^ 1922941863 ^ 1667502612) ^ -2015924126], bindingFlags, null, new Type[-(-887226842 ^ -1020833562) ^ -940430813 ^ -832797585 ^ 1065038696 ^ -1055863708], null);
									num2 = (~(-(-(-268815184 ^ 1941701523) ^ 648146467 ^ 893306661 ^ 136658928)) ^ 2013519396);
									continue;
								case 4:
									goto IL_8F0;
								}
								break;
							}
						}
						IL_8F0:
						continue;
					}
					case 5:
					{
						byte[] array3;
						array3[~(-(-(-(~(-(~(-(264727925 ^ -929272939 ^ 1432423757))))))) ^ -1841689677)] = (byte)((int)array3[~(~(~(-(-(~(--1572146123))))) ^ -1212564685 ^ -368267039)] ^ -(-(-(-(-368093469))) ^ -368093484));
						array3[~(~(-(-(-(-(-(-(--1552383644)) ^ 1552383622))))))] = (array3[~(-(-(-(~(~(536448913 ^ -757443217) ^ -1161965918) ^ 417604431 ^ 1870280456))))] ^ ~(-(~(~(~(-90))))));
						array3[-(~(-(-(~(~(1029127724 ^ 839593427 ^ -819064904))) ^ 1698051688) ^ 507705542) ^ -1157180685)] = (byte)((int)array3[~(-(~(~(-(--1915562739) ^ 1607771955))) ^ -771329498)] ^ (-(~(~(-(--414573429 ^ -1379131135) ^ 1833050854 ^ -1214738487 ^ 1185083715))) ^ 687972084));
						array3[~(~(-(~(~(-(-(~-1663915958 ^ -1075478388 ^ -2045375978))) ^ -1524555061))))] = (byte)((int)array3[~(-(-98628000) ^ -66450002) ^ 644878450 ^ -2028967811 ^ -1485472798] ^ -(~(~(-(-(-(-(~(-1586228173 ^ 1456920073)))) ^ 1477396825) ^ 1685142687) ^ 103106475) ^ -839258085));
						num = ~(~(-(~(~(-(907139700 ^ -1674280382 ^ -1440393144))))));
						continue;
					}
					case 6:
					{
						BindingFlags bindingFlags;
						method8 = typeof(MemberInfo).GetMethod(array[~(-(692037195 ^ 2050607445 ^ 593657090 ^ 616305472) ^ -850469191 ^ -1718165536)], bindingFlags, null, new Type[~(-(-262078075 ^ -1124991018 ^ 1284570706))], null);
						for (;;)
						{
							int num3 = -(-(-(-(-(~(~(-(~-816775189 ^ -473357443)))) ^ 1768547915 ^ 1173371607))));
							for (;;)
							{
								num3 ^= (-(-(~(~(482649922 ^ 451685485)) ^ -146677748)) ^ -244749548);
								switch (num3 + ~(~(-(-(-(-(~1889031876 ^ 892183288) ^ -1747936208) ^ 765106098)))))
								{
								case 0:
								{
									Type typeFromHandle3 = typeof(Type);
									string name4 = array[-(-(~-1613970574 ^ 854963849)) ^ 1388743169];
									BindingFlags bindingFlags2;
									BindingFlags bindingAttr4 = bindingFlags2;
									Binder binder4 = null;
									Type[] array6 = new Type[-(~(~(-(-(~(~(-2137647441 ^ -121114615 ^ -249542515 ^ -318126756 ^ 248629617))))) ^ -1789501959))];
									array6[~(~(-(~(~(~(-(~(179383919 ^ -2142466335))))) ^ 1784555276)) ^ -2090600133) ^ 1673559743] = typeof(RuntimeTypeHandle);
									method9 = typeFromHandle3.GetMethod(name4, bindingAttr4, binder4, array6, null);
									num3 = -(-(-122641162) ^ 122641152);
									continue;
								}
								case 1:
									num = -(-1377462164 ^ 1674150036 ^ 835970820);
									num3 = -(-(-(-(-(774273731 ^ 774273743)))));
									continue;
								case 2:
									method10 = typeof(MemberInfo).GetMethod(array[~(~(-(-(~(835559376 ^ 1306115760)) ^ -2081694053)))], bindingFlags, null, new Type[~(-(-(~2058224878)) ^ 2058224878)], null);
									num3 = (~(-406083526 ^ -1116530635 ^ -972222341) ^ -1665818500);
									continue;
								case 3:
									method11 = typeof(Type).GetMethod(array[~(-(~(~(~(658716816 ^ -658716822)))))], bindingFlags, null, new Type[-(-(-(~966392777))) ^ 966392778], null);
									num3 = -(-(-(30481845 ^ 666407511 ^ -92023098) ^ -1293348267 ^ 1845709569));
									continue;
								case 4:
									goto IL_CE8;
								}
								break;
							}
						}
						IL_CE8:
						continue;
					}
					case 7:
					{
						byte[] array3;
						array3[-(~(~(~(-(-(~(~1271755442) ^ -1929330696 ^ -671257954)) ^ 192874980)) ^ 441429504))] = (byte)((int)array3[~(~(~(1928231465 ^ -566693726 ^ 1395225925)))] ^ ~(~(-(~(~910583529 ^ -1652834591 ^ -1614909177) ^ -881043750))));
						for (;;)
						{
							int num4 = -(-(~(-(-(~1369841946 ^ -2070818361)))) ^ 1989834596) ^ -1548908608;
							for (;;)
							{
								switch ((num4 ^ (-(~(~(~-2077965603 ^ 471501155 ^ 1064380217))) ^ -1487967054)) - -(~(~(~(~(~(-1923609202)) ^ 1813491903 ^ 1180384906 ^ -759617415 ^ 1974316029)))))
								{
								case 0:
								{
									string @string = Encoding.UTF8.GetString(array3);
									char[] array7 = new char[-(-(-983349786 ^ -983349785))];
									array7[~(-(-(-(-(~(~-1))))))] = (char)(~(char)(-(char)(-(char)(~(char)(1766782934 ^ 1825130164)) ^ 92778847)));
									array = @string.Split(array7);
									num4 = ~(-(-(~(~(~(-(-1841240961 ^ 1648016235)) ^ -2038223746))) ^ -168055822 ^ 2096965405));
									continue;
								}
								case 1:
								{
									string name5 = "?";
									MethodAttributes attributes = methodAttributes;
									Type typeFromHandle4 = typeof(string);
									Type[] array8 = new Type[~(-(-(-(1605313531 ^ 1582400926)) ^ 1620543575) ^ -179442042 ^ -1809530698)];
									array8[~(~(-(-(~(-(-1207119355 ^ 1504640794)) ^ -715763514 ^ 888438232))))] = typeof(Stream);
									methodBuilder = \uE000.DefineMethod(name5, attributes, typeFromHandle4, array8);
									num4 = (~(-(-(~(-1131291361 ^ -1834604335)))) ^ 775419318);
									continue;
								}
								case 2:
									num = -(~(~(-(~-700467810 ^ 700467729))));
									num4 = -(~(~(~(-(~124)))));
									continue;
								case 3:
									array3[-(-(-(-(~-189091201) ^ -313893207)) ^ -435219562)] = (byte)((int)array3[-(~(1615332896 ^ 1979039888 ^ 559811545 ^ -1719564423) ^ -343217163 ^ 1189650853)] ^ (~(~(~(-(~(-(~1199310070 ^ -273798446))) ^ -1143133972)) ^ -429318293) ^ -751318692 ^ -642896028));
									num4 = -(~(-(~(~(-(-(1791696318 ^ -550205447))))) ^ 1727277682) ^ 754208690);
									continue;
								case 4:
									goto IL_F71;
								}
								break;
							}
						}
						IL_F71:
						continue;
					}
					case 8:
					{
						byte[] array3;
						array3[~(-(-(-(-(~894007313 ^ 2047177744))) ^ 1330383908))] = (byte)((int)array3[~(-(~(777216701 ^ 373160813 ^ -946772471)))] ^ ~(~(~(~(~(-(~(~(-(-(~-658156310)))) ^ -627205581))) ^ -39374581))));
						array3[-(~(-470295315 ^ 1392567972)) ^ -234128630 ^ 1123828070] = (byte)((int)array3[-(~(-(~(-(~(-(-(677507366 ^ -1661987726 ^ -1265506953))))))))] ^ (-(~(-(~(-(~(-(-(--2120976099)) ^ -658156950) ^ 1972376149))) ^ 997072043)) ^ -397354375));
						array3[~(-(-(~716290892) ^ 2005804766)) ^ 1564450229] = (byte)((int)array3[-(-(~(-(-(-(~(-(~(~(-803598071 ^ -129528012))))))) ^ -1754265781)) ^ -1087530679)] ^ (~(~(~(-(~(-1636438503)) ^ -106281128 ^ 1651530568) ^ 1474747512) ^ -820631238) ^ 1654716647));
						array3[~(-(-(~-1441413551) ^ -1441413509))] = (byte)((int)array3[-(~(-(~(-(~(--37))))))] ^ -(-(-(~-785987661)) ^ -785987620));
						num = -(~(-(~(-(-(--852245062))) ^ 908110836 ^ 980701717)) ^ 1050198953);
						continue;
					}
					case 9:
					{
						Type typeFromHandle5 = typeof(BinaryReader);
						BindingFlags bindingFlags;
						BindingFlags bindingAttr5 = bindingFlags;
						Binder binder5 = null;
						Type[] array9 = new Type[~(~(-(-(--1))))];
						array9[~(~(-(-(~(~-805578659 ^ 2073474279)) ^ -278063633 ^ -215459842) ^ -1473761623))] = typeof(Stream);
						constructor = typeFromHandle5.GetConstructor(bindingAttr5, binder5, array9, null);
						for (;;)
						{
							int num5 = ~(~(~(~(-(-(~(~(-(933281458 ^ 216564483))))))))) ^ 994590135;
							for (;;)
							{
								num5 ^= -(~(-(~(-(~(~1658662954)) ^ 439209561) ^ 1107697770) ^ -1003859416 ^ 19100659));
								switch (num5 + ~(~(~(-(~(~61))))))
								{
								case 0:
									num = ~(~(-(~(~(~(~(~(~(~(1286089185 ^ 1633609180))))))))) ^ 771148862);
									num5 = -(~(~-3394625 ^ -1724995717 ^ 1726125262));
									continue;
								case 1:
									method12 = typeof(BinaryReader).GetMethod(array[~(-(~(-(-(-(-1938250065 ^ 1222927202)) ^ -1013922453 ^ 319166826 ^ 1519253086 ^ 254266813)) ^ -1486659012) ^ -423411686)], bindingFlags, null, new Type[-(-(-1193636794) ^ 1193636794)], null);
									num5 = (-(-(-(~(-(-(~(--2023762361) ^ 942021205 ^ -1405828890)) ^ 1428609533))) ^ -1667491855) ^ 621296899);
									continue;
								case 2:
									constructor2 = typeof(Hashtable).GetConstructor(bindingFlags, null, new Type[~(-(~(-(-869000248 ^ -1868826888 ^ 869780612) ^ 1958704511) ^ -14405334) ^ -1432409963 ^ 1316769136)], null);
									num5 = (~(~(~(~(-(-(~1214506366 ^ 2145185364) ^ 2057865221))) ^ 350722715)) ^ -577347292 ^ 341550307 ^ -1874994059);
									continue;
								case 3:
								{
									Type typeFromHandle6 = typeof(Hashtable);
									string name6 = array[~(~(-(~(~(~(-(~1973417393)))) ^ -2118482085 ^ 1644602053) ^ -1492762356 ^ 824545581))];
									BindingFlags bindingAttr6 = bindingFlags;
									Binder binder6 = null;
									Type[] array10 = new Type[-(-(-(-(-(~(~(-1788404214 ^ 1093018801)))) ^ 340683530) ^ 322102101) ^ 750931224)];
									array10[~(~(-(-(~(-840917129 ^ 809249082)))) ^ -486068714 ^ 2086866785 ^ -1656264507)] = typeof(object);
									array10[-(~(~(-(-(-(-(~860238727 ^ 2028081929))))) ^ 1138412719) ^ -1303918525) ^ -1170660254] = typeof(object);
									method13 = typeFromHandle6.GetMethod(name6, bindingAttr6, binder6, array10, null);
									num5 = (-(-(1165378350 ^ -1592404991)) ^ -961935921 ^ -583635686);
									continue;
								}
								case 4:
									goto IL_13C0;
								}
								break;
							}
						}
						IL_13C0:
						continue;
					}
					case 10:
					{
						BindingFlags bindingFlags = ~(-(-(-(~(-(~(BindingFlags)(-1198199103) ^ (BindingFlags)(-1714582365) ^ (BindingFlags)1498898323) ^ (BindingFlags)1290490947))) ^ (BindingFlags)(-352345984))) ^ (BindingFlags)568669945;
						BindingFlags bindingFlags2 = -(-(~(~(-(~(~(BindingFlags)1148194755))) ^ (BindingFlags)1131480551) ^ (BindingFlags)(-1822462830)) ^ (BindingFlags)1905760242 ^ (BindingFlags)439750274);
						byte[] array3 = Convert.FromBase64String("WT+9Y3IKPLFJDEszFxUCAUnrPg9zSXQ2JVQ0tT1rbhydfG9dBhVoNQRcMzcLTRQVQE47R2VZVHlwZUZyb21IYW4CbGU7Z2V0X05hbWU7SW5kZXhPZjtFeGl0O2dldF9GcmFtZUNvdW50O2dldF9MZW5ndGg7UmVhZFN0cmluZztBZGQ7Z2V0X1Bvc2l0aW9uO2dldF9DdXJyZW50RG9tYWluO1NldERhdGE7UnVudGltZU1ldGhvZDtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tUcmFjZTtTeXN0ZW0uRGlhZ25vc3RpY3MuU3RhY2tGcmFtZTszMzIyODtTeXN0ZW0uRW52aXJvbm1lbnQ=");
						array3[-(-(~(~(~(-(-(~(~(--1590802157 ^ 102117923))) ^ -788302832)))))) ^ 1983581985] = (byte)((int)array3[-(-(-(-(~(~(~1443713915 ^ 2059413952) ^ 830313320) ^ 1869214505) ^ -1638732766))) ^ -1687214551 ^ -2011484402] ^ -(~(-(-(~(-(~(-1723312202) ^ -782052374))) ^ -1210745923))));
						num = -(~(-(-(~-130233671 ^ 130233656))));
						continue;
					}
					case 11:
					{
						byte[] array3;
						array3[-(-(-(1123368821 ^ -1123368826)))] = (byte)((int)array3[-(~(-(-(~(-(~(-(-(~-968817422)))) ^ -1989631571))))) ^ 1615396393 ^ 794909561] ^ (~(~168614153 ^ -896746685 ^ 1801659779) ^ -1411168327));
						array3[-(~(~(~(-(-(-(--1690337481)) ^ 1612143247 ^ 199913058 ^ -944215042) ^ 1289211267)))) ^ 2074996136] = (byte)((int)array3[~143065867 ^ 1369641996 ^ 332119183 ^ -1257220999] ^ (-(~(-(-(-(-1133538636 ^ -1420844750 ^ -112111927 ^ 89479750))) ^ -1387487163) ^ 1830627028) ^ -729129455));
						array3[~(~(~(-(~(~(~236849235)) ^ 1214726280 ^ 1685240388 ^ -1630510864)) ^ 1126460800))] = (byte)((int)array3[~(~(-(1220623146 ^ 1441230157) ^ -489051754))] ^ -(-(-(~1934832702)) ^ 1934832726));
						array3[-(~(~(-(-(-(-(-(-(17700419 ^ 1250434284)))) ^ 329585200 ^ 2060321714))))) ^ -585964349] = (byte)((int)array3[~(~(~(-(-(~(-(415114876 ^ -914211472) ^ -305171784)))) ^ -1897891311 ^ 1307372621))] ^ ~(~(~(~(~789194004)) ^ 1948742420 ^ 533044044 ^ -1058566763) ^ -1980333210 ^ 442908477 ^ -396360869));
						num = (-(~(-(-(-1197884717)))) ^ -1197884763);
						continue;
					}
					case 12:
					{
						byte[] array3;
						array3[-(-(~(-(~(-(~(~-792435023))))))) ^ -792435030] = (byte)((int)array3[-(-(~(~-1241418854 ^ -1738781103 ^ -1195791918 ^ 328563183) ^ 867716006 ^ -2027953203 ^ 836126104))] ^ ~(-(-(-(-(~(-(~(~(~750304879))) ^ 750304795)))))));
						array3[~(-(~(~(~(-(--1391439703)) ^ 1391439697))))] = (byte)((int)array3[-(-(~(~(-(-(~(~(-(~1907713098) ^ 257339190)))) ^ 693692942)) ^ 1471881589))] ^ (~(~(--700599248 ^ 1664370845 ^ -1010481661)) ^ -188467881 ^ 2113368136));
						array3[~(-(~(-(-(~1425906852))))) ^ 1425906852] = (byte)((int)array3[-(-(--2015587729) ^ 2015587734)] ^ (~(-(-(-(~(-(~(~-1615307440))))) ^ 51873899)) ^ 2016691648 ^ -459567049));
						array3[~(-(-(-(-(765965805 ^ -765965798)))))] = (byte)((int)array3[-(-(1588526811 ^ -522931237) ^ -1099281160)] ^ (-(~(-(--1309001706))) ^ -1309001627));
						num = -(-(-(~(-(~(--1034595576) ^ 424959479) ^ 620687713))));
						continue;
					}
					case 13:
					{
						byte[] array3;
						array3[~(~(~(-(~(~(1366728429 ^ 1366728431))))))] = (byte)((int)array3[~(-(-1038828636) ^ -1038828638)] ^ (~(-(~(~(-(~(-(~(-537966691) ^ 756439343))))))) ^ -218554647));
						array3[~(-(~(~3)))] = (byte)((int)array3[~(-(-1483729593)) ^ -1483729596] ^ -(~(~1160924049) ^ -1160923994));
						array3[~(~(-(-(-(~(~(-(931765791 ^ -1686293894 ^ -1825004951)) ^ 610289087)))) ^ 464276406))] = (byte)((int)array3[~(~(~(-(~(~(~(-(-576481964 ^ -651806609)) ^ 1689378 ^ -1652201718 ^ 1745407051)))) ^ -250494629))] ^ ~(~(~703367906 ^ -1792214368 ^ -1968652113 ^ -797153334 ^ 434841853)));
						array3[-(~(-(-(~424044393)))) ^ -1452659543 ^ 1080908537 ^ -1012365685 ^ -870946744] = (byte)((int)array3[~(~(~(-(--1736560106 ^ -354036598) ^ -1428584600) ^ -1148905658) ^ -1674016395)] ^ ~(-(-(-(~1224185761) ^ -2095081077) ^ 873984470)));
						num = (-(~(~(-580200718 ^ -1560533246))) ^ -2140598676);
						continue;
					}
					case 14:
					{
						byte[] array3;
						array3[~(~922698166 ^ -168213666 ^ 1406016355 ^ 529187550 ^ -2028173762 ^ 140402038)] = (byte)((int)array3[~(-(-(-789851016 ^ -288344836))) ^ -1044111514] ^ -(~(~(-(~(~(~(--1061022383) ^ 66907314)) ^ -1019282976)))));
						array3[~(~(-734582299)) ^ -734582320] = (byte)((int)array3[~(-(-65823014 ^ -371887920 ^ -83682759 ^ -289016827))] ^ (~(~(-(~(~1542120847)))) ^ -1542120868));
						array3[~(~(-(--689391285 ^ -689391276)))] = (byte)((int)array3[-(-(-(~(~(-1980837409 ^ 1349313012))) ^ 645779402))] ^ -(-(-(-(~(-1345201059 ^ -1564278175) ^ -2066208811)) ^ 1983186541)));
						array3[~(~(~(~(-358832929 ^ -1939507532 ^ -1329606657 ^ 1582957661) ^ -2011424279)))] = (byte)((int)array3[-(~(-(-(-(~1350272123 ^ -1420184383 ^ 706223687)))) ^ -1754531165 ^ 741505721) ^ -324277736 ^ -2033695523] ^ -(-(~(~(-(~(~(~(~-615610100 ^ -1055779159)))) ^ 265800752) ^ -361452382))));
						num = (~(~(~-1143884077) ^ 1978319446 ^ 710450143 ^ 1233283841 ^ 1197108851) ^ 356783572);
						continue;
					}
					case 15:
					{
						byte[] array3;
						array3[-(-(~(-18)))] = (byte)((int)array3[~(~(~(~(-657928856 ^ -719190754 ^ 1512446495 ^ -1521888384 ^ -150265828)))) ^ 93200868] ^ ~(~(~(1487715892 ^ 11916540) ^ -1478060616)));
						array3[-(-(-(~(-303139296 ^ -303139279))))] = (byte)((int)array3[-1960051234 ^ -911832433 ^ -1151040520 ^ 390553943 ^ -290548244] ^ (~(-(~(~(~(-1744954228 ^ -1932096527 ^ 594954773))) ^ -1771413359) ^ -1879046471) ^ -1043643207));
						array3[~(-(-(-(~(~(--572717449 ^ -712859463) ^ 263382959) ^ -894295963) ^ 1889076993 ^ 1111071211)))] = (byte)((int)array3[~(-(~(~(-1123828863 ^ 552170727 ^ -1127219555 ^ 556085743))))] ^ (-(-(-(-2036137368) ^ -1624421000)) ^ -428756856));
						array3[-(-(-1996230680 ^ 1158192774 ^ 1961669485 ^ -1192771049))] = (byte)((int)array3[-(-(~(~(~(-(~(~(~(-972071330 ^ 1848765975))))) ^ -864295744 ^ 380718741)))) ^ -1928600076] ^ (~(~(-(~(~(~139950402)) ^ -2116889018 ^ 2065407845))) ^ -224513932));
						num = ~(-(~(~(~(-(~(~1452568111))) ^ 1452568154))));
						continue;
					}
					case 16:
					{
						Type type = Type.GetType(array[-(~(-(1471537017 ^ -1851128639))) ^ -765756602 ^ -340041954]);
						BindingFlags bindingFlags;
						constructor3 = type.GetConstructor(bindingFlags, null, new Type[-(~(-(~(-(-(-(~(--1969833048))) ^ -1950894267) ^ 18943200))))], null);
						Type type3 = type;
						string name7 = array[~(-(-(~(~(-(~(-(-(-545174398 ^ 545174396)))))))))];
						BindingFlags bindingAttr7 = bindingFlags;
						Binder binder7 = null;
						Type[] array11 = new Type[~(-(~(~(--222553993 ^ 548281047))) ^ -110033330) ^ -727909614];
						array11[~(~(-(~(~(1865617132 ^ -162548385) ^ 857951659) ^ -1049587972 ^ -695472609) ^ -174541537)) ^ -1566323491 ^ 358617287] = typeof(int);
						method14 = type3.GetMethod(name7, bindingAttr7, binder7, array11, null);
						method15 = Type.GetType(array[~(-(~(-(~(~1241250341 ^ -1067992415))))) ^ -1985199984]).GetMethod(array[-(~(-(-(~(~(~(-(~(1035772530 ^ 1282777298))))) ^ 828089480))) ^ -1083352618)], bindingFlags, null, new Type[-(-(~(-(~(~(-(-1878277928) ^ 1424458174))) ^ -524979448) ^ -1921666030 ^ 1456699791))], null);
						num = -(~(--627790920) ^ 627790925);
						continue;
					}
					case 17:
					{
						byte[] array3;
						array3[-(~(~(-(~(~(--25982358 ^ 303110034) ^ 1622499732) ^ -729038907))) ^ -1482441664)] = (byte)((int)array3[-(~(-(~(-(~(1425838880 ^ 715247779 ^ 1745303194 ^ -461721777))) ^ 716727180) ^ -1793257630)) ^ 1300608682] ^ -(~(-(-(-(-(~(~(~(1941262871 ^ 1555033805) ^ -1195950175)) ^ -985335660)))) ^ -1391033300)));
						array3[~(-(~(~(~-1701507433))) ^ 1701507441)] = (byte)((int)array3[-(-(~(~(~(-(-(~(~(-(~(~(--23))))))))))))] ^ (~(~(-1331993992 ^ 135081947)) ^ -549353024 ^ 1742193736));
						array3[-(~(-(~(~(-(-(-(~-1880544797))))) ^ 139406835) ^ -1432555558) ^ -758836187)] = (byte)((int)array3[~(~(-(-(~(~1172392565))))) ^ -177310653 ^ 1701029428 ^ -705914859] ^ ~(~(-467798224 ^ -760486055) ^ 917901339));
						array3[~(~(~(-(~(~(-(~(~-2107275229)) ^ -1781446153 ^ -1205290405)))))) ^ -1951198838 ^ -607087134] = (byte)((int)array3[~(-(-(-(-(-(-(~1032457249)))) ^ 1194428456))) ^ -246772140 ^ -1947137467] ^ -(-(~(~(~(-1077276485 ^ -513180775)))) ^ -1587768093));
						num = -(~(~(~(-(-(906298392 ^ 1107047874 ^ -1108738068 ^ 2083439336))) ^ -1237447464)));
						continue;
					}
					case 18:
					{
						byte[] array3;
						array3[~(~(-(~(~36341606 ^ -1732105672 ^ 1666041579 ^ 106599490))))] = (byte)((int)array3[-(~(-(-(~1856912013 ^ 607230237))) ^ 1507833664) ^ -322966727] ^ ~(-(-(-(-(~(911618797 ^ 1526257026) ^ -1419552122) ^ 1677662124))) ^ -1540183538));
						array3[~(~(-(~174078107) ^ -1774606839 ^ -294767399 ^ -1529883472) ^ -688357642)] = (byte)((int)array3[-(~(~(-(-(~(-(-(~(~-456049429))))) ^ -456049434))))] ^ -(-(-(~(-(--2041816227))) ^ -2041816208)));
						array3[-(-(~(~(~(~2099333464))) ^ 2099333459))] = (byte)((int)array3[-(~(~(~(-(-(-(-81015514 ^ 830620630 ^ -1377231286) ^ 1145225628)))))) ^ -587354416] ^ -(-(-511787923 ^ 883899535) ^ -707690329));
						array3[~(~(-(-(~(-(~(~(~-1886977207))) ^ -1075535456) ^ 528834752)) ^ 1491660948 ^ -1997419187))] = (byte)((int)array3[-(~(~(-(-(-(-(-(--1184989030 ^ -1681462994) ^ -1891230057))) ^ 339364058 ^ 1176223751))))] ^ ~(-(~(~(~(~(~(~(--400997180) ^ -315437199)) ^ 696499654)) ^ 831689608 ^ -490382760))));
						num = -(-(~(1331788938 ^ -1331789048)));
						continue;
					}
					case 19:
						goto IL_2098;
					}
					break;
				}
			}
			IL_2098:
			methodBuilder.DefineParameter(~(~(-(-(~(-(~-7737349 ^ 780482988 ^ 675071777)))) ^ 942803333) ^ 1056827148), ~(-(-(-(-(ParameterAttributes)2117086892 ^ (ParameterAttributes)(-1777079852))) ^ (ParameterAttributes)2025311910 ^ (ParameterAttributes)(-1065911591) ^ (ParameterAttributes)(-305980965)) ^ (ParameterAttributes)(-1187978326) ^ (ParameterAttributes)(-1668412905)) ^ (ParameterAttributes)1734442142, "a");
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
			ilgenerator.Emit(OpCodes.Stloc_S, -(-(-(~-1081427344))) ^ 1900721541 ^ -736751409 ^ 450240319);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(~(-(~-931279519 ^ -666016663 ^ 1621213155) ^ 1181430063) ^ -240054749) ^ 951375387));
			ilgenerator.Emit(OpCodes.Ldtoken, typeof(\uE00E));
			ilgenerator.Emit(OpCodes.Call, method9);
			ilgenerator.Emit(OpCodes.Callvirt, method10);
			ilgenerator.Emit(OpCodes.Callvirt, method4);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Bne_Un_S, label3);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(-(-(-2090421322 ^ 1189214270)) ^ 1397481706) ^ -1231326558 ^ -1378144310 ^ 198012833 ^ -1838335759 ^ -338469215);
			ilgenerator.Emit(OpCodes.Ldstr, array[-(-(-(-(~(~(~(~2130321977)))) ^ 1403692558 ^ -445691590 ^ -935342845)))]);
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
			ilgenerator.Emit(OpCodes.Stloc_S, -(-(~-1950769408)) ^ 1950769402);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Newobj, constructor);
			ilgenerator.Emit(OpCodes.Stloc_S, -(~(-(-11752607 ^ 429868846) ^ 1296738588)) ^ 1416018600);
			ilgenerator.Emit(OpCodes.Newobj, constructor2);
			ilgenerator.Emit(OpCodes.Stloc_S, -(~(~(~(~(~(--1573928133)) ^ 1885531900))) ^ -1925355398 ^ 7747134 ^ 1338731817 ^ -281878190));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(~(~(-(~(~-510361662) ^ -1650845308)) ^ 88887858 ^ 338751135 ^ -1798120991 ^ -106953972)));
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Stloc_S, ~(-(-(~(135448768 ^ 135448776)))));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(-(~(-(-(~(-(-304438856)) ^ 1137755490)) ^ 2121050398))))) ^ 798569012);
			ilgenerator.Emit(OpCodes.Ldc_I4_M1);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(-(-158414089 ^ 1958564485 ^ 992954516 ^ 1662672211)) ^ 2018227637)) ^ -1571997688);
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.Emit(OpCodes.Br_S, label5);
			ilgenerator.MarkLabel(label6);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(--490291236 ^ 490291235)));
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Conv_I4);
			ilgenerator.Emit(OpCodes.Ldc_I4, -(~(~(~(-(~(-(~718756287))) ^ -85310831)) ^ -367089158 ^ -307232411 ^ -1635100210)) ^ 1226435161);
			ilgenerator.Emit(OpCodes.Add);
			int arg = int.Parse(array[~(-(-(~(~(-(~(~(~1073654562) ^ 780308381) ^ 292804566) ^ -1045370)))))]);
			ilgenerator.Emit(OpCodes.Ldc_I4, arg);
			ilgenerator.Emit(OpCodes.Xor);
			ilgenerator.Emit(OpCodes.Box, typeof(int));
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(~(~(1692373480 ^ 1024904370)))) ^ 78646967 ^ -1291578466 ^ -293781387);
			ilgenerator.Emit(OpCodes.Callvirt, method12);
			ilgenerator.Emit(OpCodes.Callvirt, method13);
			ilgenerator.MarkLabel(label5);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Callvirt, method);
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(-(-(~-1593220032 ^ -1818523185)) ^ -946254071 ^ 183786876)));
			ilgenerator.Emit(OpCodes.Blt_S, label6);
			ilgenerator.Emit(OpCodes.Call, method2);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(~(-(~(~(~(-(--965907551)) ^ -598991995 ^ -1806332384)))) ^ -1904935422));
			ilgenerator.Emit(OpCodes.Ldloc_S, ~(~(289739294 ^ -101254140 ^ -639658544 ^ -407607434) ^ 1477222995) ^ 887885059 ^ -1170689045);
			ilgenerator.Emit(OpCodes.Callvirt, method3);
			ilgenerator.Emit(OpCodes.Ldloc_S, -(-(-(1719476455 ^ 347262075)) ^ -1926229148));
			ilgenerator.Emit(OpCodes.Ret);
			return methodBuilder;
		}

		public string \uE001(Stream \uE000)
		{
			TypeBuilder typeBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName
			{
				Name = "?"
			}, -(~(-(~(~(~(~((AssemblyBuilderAccess)(-1440686767) ^ (AssemblyBuilderAccess)(-57403513))))))) ^ (AssemblyBuilderAccess)1626015583 ^ (AssemblyBuilderAccess)(-912248203))).DefineDynamicModule("?").DefineType("?", -(-(~(-((TypeAttributes)1453638967 ^ (TypeAttributes)(-1569402721)))) ^ (TypeAttributes)187689561));
			this.\uE000(typeBuilder);
			Type type = typeBuilder.CreateType();
			string name = "?";
			BindingFlags invokeAttr = -(-(~(~(-(~(-(~((BindingFlags)1985932919 ^ (BindingFlags)(-1273696841) ^ (BindingFlags)945657586 ^ (BindingFlags)789699936))) ^ (BindingFlags)1629506006) ^ (BindingFlags)(-1272577890)))));
			Binder binder = null;
			object target = null;
			object[] array = new object[-(-(-(-(-(-(~-1889678107 ^ -1161548290 ^ 1229505739 ^ 171835473) ^ 493474651)))) ^ -1804007130)];
			array[~(~(-(-(-(~(-(~(~(-(1093617624 ^ 986765304))))))) ^ -2080346145)))] = \uE000;
			return (string)type.InvokeMember(name, invokeAttr, binder, target, array);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static \uE010()
		{
			for (;;)
			{
				int num = -(~(~(--1508944169 ^ -1356105690)) ^ -608394230 ^ -761755006);
				for (;;)
				{
					switch ((num ^ (~(~(-(~(-(~-1950316774 ^ -1589721781 ^ 1517814766))))) ^ -409589656 ^ -1760532079)) - -(~(-(~(~(-(-1815495055)) ^ -682354602) ^ 1151182353))))
					{
					case 0:
						num = (~(~(~(-(-(-(-(~-1824522144) ^ 1706822385) ^ -1227693907))) ^ 897859776)) ^ 1974119064);
						continue;
					case 1:
						num = ~(-(-(~(-(-(-177629019))) ^ -177629020)));
						continue;
					case 2:
						num = (~(-(~(-(-(~(--1157586012) ^ 1287239359))) ^ 92503916 ^ 1379416221)) ^ 1610460436);
						continue;
					case 3:
						num = ~(~(~-940719243) ^ 940719244);
						continue;
					case 4:
						num = -(~(~(2133817393 ^ -1836571770) ^ -1271542529 ^ -1971325290 ^ -9543263 ^ 1299993511) ^ -1644123103);
						continue;
					case 5:
						num = ~(-(~(-(-(-(--122))))));
						continue;
					case 6:
						num = (-(~(~(-(1864077800 ^ -2090983572 ^ 1965325455)))) ^ -1721634295);
						continue;
					case 7:
						num = (-(~(-(-(~(-1137881924 ^ -266819541) ^ -2002957097 ^ 1307867724)))) ^ 1990405623);
						continue;
					case 8:
						num = -(~(-(~(~(~(~(~-411586025)))) ^ -411586027)));
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
